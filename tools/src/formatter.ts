import { Result } from './models';

export interface IFormatter {
  render(results: Result[]): string;
}
