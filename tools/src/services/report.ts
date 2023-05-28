import { Result } from '../models';

export default class ReportService {
  public parseOutput(content: string, competitor: string): Array<Result> {
    return content
      .split(/\r?\n/)
      .map((line) => {
        const match = line
          .trim()
          .match(/^(?<passes>\d+)\s*;\s*(?<duration>\d+([.]\d+)?)\s*;\s*$/);

        // Moving to the next record if the line doesn't match
        if (!match) return;

        return {
          competitor: competitor.toLowerCase(),
          passes: +match.groups!['passes'],
          duration: +match.groups!['duration']
        } as Result;
      })
      .filter((item): item is Result => !!item);
  }
}
