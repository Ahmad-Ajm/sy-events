export type StringValues = string | string[];

export interface StringSegment {
  buffer?: string;
  offset: number;
  length: number;
  value?: string;
  hasValue: boolean;
  item?: string;
}
