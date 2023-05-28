import { Table } from 'console-table-printer';
import { Result } from '../models';
import { IFormatter } from '../formatter';

export class TableFormatter implements IFormatter {
  render(results: Result[]): string {
    const output = new Array<string>();

    output.push(this.printResults('Codehesion Hackathon', results));
    return output.join('\n');
  }

  private printResults(title: string, data: Result[]): string {
    const table = new Table({
      title,
      columns: [
        {
          name: 'index',
          title: 'Index',
          alignment: 'center',
          color: 'blue'
        },
        {
          name: 'implementation',
          title: 'Compeditor',
          alignment: 'left'
        },
        { name: 'passes', title: 'Passes', alignment: 'center' },
        { name: 'duration', title: 'Duration', alignment: 'center' },
        {
          name: 'passesPerSecond',
          title: 'Passes/Second',
          alignment: 'center'
        }
      ]
    });

    table.addRows(
      data
        .map((value) => {
          return {
            ...value,
            passesPerSecond: value.passes / value.duration
          };
        })
        .sort((a, b) => b.passesPerSecond - a.passesPerSecond)
        .map((value, index) => {
          return {
            index: index + 1,
            implementation: value.competitor,
            passes: value.passes,
            duration: value.duration.toFixed(5),
            passesPerSecond: value.passesPerSecond.toFixed(5)
          };
        })
    );

    return table.render();
  }
}
