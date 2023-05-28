import { IFormatter } from './formatter';
import { TableFormatter } from './formatters/table';

export default class FormatterFactory {
  static getFormatter(type: string): IFormatter {
    switch (type.toLocaleLowerCase()) {
      case 'table':
        return new TableFormatter();
      default:
        return new TableFormatter();
    }
  }
}
