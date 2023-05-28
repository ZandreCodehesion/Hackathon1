import glob from 'glob';
import path from 'path';
import fs from 'fs';

import { Command } from 'commander';
import { uname } from 'node-uname';

import ReportService from '../services/report';
import DockerService from '../services/docker';
import { Result } from '../models';
import { TableFormatter } from '../formatters/table';

const reportService = new ReportService();
const dockerService = new DockerService();

const ARCHITECTURES: { [arch: string]: string } = {
  x86_64: 'amd64',
  aarch64: 'arm64',
  arm64: 'arm64'
};

export const command = new Command('benchmark')
  .requiredOption('-d, --directory <directory>', 'Implementation directory')
  .option('-f, --formatter <type>', 'Output formatter', 'table')
  .option('-o, --output-file <file>', 'Write output to given file')
  .option(
    '-u, --unconfined',
    'Run with seccomp:unconfined (native performance for interpreted languages)'
  )
  .action(async (args) => {
    const directory = path.resolve(args.directory as string);
    const unconfined = args.unconfined === true;

    console.info(`Unconfined mode: ${unconfined}`);

    // Making sure the outputs directory exists
    if (!fs.existsSync(directory)) {
      console.error(`Directory ${directory} does not exist!`);
      return;
    }

    // Search for all Dockerfiles within the base directory
    const files = glob.sync(path.join(directory, '**/Dockerfile'));
    if (!files.length) {
      console.error('No implementations have been found!');
      return;
    }

    // Collect runtime options for docker images
    let options: Array<string> = [];
    if (unconfined) {
      options.push('--security-opt seccomp=unconfined');
    }

    // Determine architecture
    const architecture = ARCHITECTURES[uname().machine] || 'amd64';
    console.info(`Detected architecture: ${architecture}`);

    // Processing files
    const results = new Array<Result>();
    files.forEach((file) => {
      const solutionDirectory = path.dirname(file);
      const [implementation] = solutionDirectory.split(path.sep).slice(-1);
      const imageName = `${implementation}`.toLocaleLowerCase();

      if (fs.existsSync(path.join(solutionDirectory, 'build-no'))) {
        console.warn(`[${implementation}] Skipping due to disabled build!`);
        return;
      }

      // NOTE: If any arch-* files are present then check if the current architecture
      // is present among the files, if not we skip this build...
      const archFiles = glob.sync(path.join(solutionDirectory, 'arch-*'));
      if (archFiles.length) {
        const hasArch = archFiles
          .map((file) => path.basename(file).replace('arch-', ''))
          .includes(architecture);

        if (!hasArch) {
          console.warn(
            `[${implementation}] Skipping due to architecture mismatch!`
          );
          return;
        }
      }

      // Build the Docker image for the current solution
      try {
        console.info(`[${implementation}] Building...`);
        dockerService.buildImage(solutionDirectory, imageName);
      } catch (e) {
        console.error(`[${implementation}] Failed building solution!`);
        return;
      }

      // Run the benchmark container and retrieve output from it
      let output = '';
      try {
        console.info(`[${implementation}] Running...`);
        output = dockerService.runContainer(imageName, options);
      } catch (err: any) {
        console.warn(
          `[${implementation}] Exited with abnormal code: ${err.status}. Results might be partial...`
        );
        output = err.output
          .filter((block: Buffer | null) => block !== null)
          .map((block: Buffer) => block.toString('utf8'))
          .join('');
      } finally {
        const localResults = reportService.parseOutput(output, implementation);
        results.push(...localResults);
      }
    });

    if (!results.length) {
      console.error('No data was collected!');
      return;
    }

    // Convert report to the correct format and print everything out.
    const formatter = new TableFormatter();
    const output = formatter.render(results);
    console.log(output);
  });
