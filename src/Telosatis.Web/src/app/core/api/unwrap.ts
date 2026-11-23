import { catchError, map, MonoTypeOperatorFunction, OperatorFunction, pipe, throwError } from "rxjs";
import { ApiResponse, ApiResponseError } from "./api-response";
import { HttpErrorResponse } from "@angular/common/http";

export function isEnvelope(body: unknown): body is ApiResponse<unknown> {
  return typeof body === 'object' && body !== null && 'statusCode' in body && 'error' in body;
}

export function normalizeHttpError<T>(): MonoTypeOperatorFunction<T> {
  return catchError((err: unknown) => {
    if (err instanceof HttpErrorResponse && isEnvelope(err.error) && err.error.error) {
      return throwError(() => new ApiResponseError(err.error.statusCode, err.error.error!));
    }
    return throwError(() => err);
  });
}

export function throwIfEnvelopeError(body: unknown): void {
  if (isEnvelope(body) && body.error) {
    throw new ApiResponseError(body.statusCode, body.error);
  }
}

export function unwrap<T>(): OperatorFunction<ApiResponse<T>, T> {
  return pipe(
    map(res => {
      throwIfEnvelopeError(res);
      return res.data as T;
    }),
    normalizeHttpError(),
  );
}

export function unwrapEmpty(): OperatorFunction<unknown, void> {
  return pipe(
    map(body => {
      throwIfEnvelopeError(body);
    }),
    normalizeHttpError(),
  );
}