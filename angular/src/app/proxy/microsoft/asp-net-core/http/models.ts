import type { StringValues } from '../../extensions/primitives/models';

export interface IFormFile {
  contentType?: string;
  contentDisposition?: string;
  headers: Record<string, StringValues>;
  length: number;
  name?: string;
  fileName?: string;
}
