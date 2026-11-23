import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { MonoTypeOperatorFunction, catchError, from, switchMap, throwError } from 'rxjs';
import { ApiResponseError } from './api-response';
import { ApiFile } from './api-file';
import { isEnvelope } from './unwrap';

// TODO: Optimization
export function fileNameFromHeaders(headers: HttpHeaders, fallback = 'download'): string {
  const cd = headers.get('Content-Disposition');
  if (!cd) return fallback;

  const utf8 = /filename\*\s*=\s*UTF-8''([^;]+)/i.exec(cd);
  if (utf8) return decodeURIComponent(utf8[1].trim());

  const plain = /filename\s*=\s*"?([^";]+)"?/i.exec(cd);
  return plain ? plain[1].trim() : fallback;
}

export function saveFile(file: ApiFile): void {
  const url = URL.createObjectURL(file.blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = file.fileName;
  a.click();
  setTimeout(() => URL.revokeObjectURL(url));
}

export function normalizeBlobError<T>(): MonoTypeOperatorFunction<T> {
  return catchError((err: unknown) => {
    if (!(err instanceof HttpErrorResponse) || !(err.error instanceof Blob)) {
      return throwError(() => err);
    }
    return from(err.error.text()).pipe(
      switchMap(text => {
        let body: unknown;
        try { body = JSON.parse(text); } catch { return throwError(() => err); }
        return isEnvelope(body) && body.error
          ? throwError(() => new ApiResponseError(body.statusCode, body.error!))
          : throwError(() => err);
      }),
    );
  });
}