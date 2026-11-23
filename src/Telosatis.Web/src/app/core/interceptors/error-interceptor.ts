import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        console.error("FIXME: Not implemented - 401 handling")
      } else if (err.status >= 500 || err.status === 0) {
        console.error("FIXME: Not implemented - 5xx handling")
      }

      return throwError(() => err);
    })
  )
};
