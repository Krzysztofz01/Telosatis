import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { filter, map, Observable, switchMap } from 'rxjs';
import { FileInfoDto, ListDirectoryOrderKey, MoveFileDto, UploadState } from './files.models';
import { API_BASE_URL } from '@core/api/api-config';
import { normalizeHttpError, throwIfEnvelopeError, unwrap, unwrapEmpty } from '@core/api/unwrap';
import { ApiResponse } from '@core/api/api-response';
import { ApiFile } from '@core/api/api-file';
import { fileNameFromHeaders, normalizeBlobError } from '@core/api/file-utils';
import { sha256Hex } from '@core/files/sha256';
import { SHA256_CHECKSUM_HEADER } from '@core/api/headers';

@Injectable({
  providedIn: 'root',
})
export class FilesService {
  private http = inject(HttpClient);
  private baseUrl = `${inject(API_BASE_URL)}/api/files`

  deleteFile(filePath: string): Observable<void> {
    const params = new HttpParams({ fromObject: {
      'p': filePath
    }});

    return this.http
      .delete<void>(this.baseUrl, { params })
      .pipe(unwrapEmpty());
  }

  listDirectory(directoryPath: string, orderKey: ListDirectoryOrderKey | null = null, orderDescending: boolean | null = null) : Observable<Array<FileInfoDto>> {
    const params = new HttpParams({ fromObject: {
      'p': directoryPath
    }});

    if (orderKey !== null) {
      params.append('k', orderKey);
    }

    if (orderDescending !== null) {
      params.append('d', orderDescending)
    }

    return this.http
      .get<ApiResponse<Array<FileInfoDto>>>(this.baseUrl, { params })
      .pipe(unwrap());
  }

  moveFile(dto: MoveFileDto) : Observable<FileInfoDto> {
    return this.http
      .post<ApiResponse<FileInfoDto>>(`${this.baseUrl}/move`, dto)
      .pipe(unwrap());
  }

  createDirectory(directoryPath: string) : Observable<FileInfoDto> {
    const params = new HttpParams({ fromObject: {
      'p': directoryPath
    }});

    return this.http
      .post<ApiResponse<FileInfoDto>>(`${this.baseUrl}/directory`, { params })
      .pipe(unwrap());
  } 

  downloadFile(filePath: string) : Observable<ApiFile> {
    const params = new HttpParams({ fromObject: {
      'p': filePath
    }});

    return this.http
      .get(`${this.baseUrl}/download`, { params, responseType: 'blob', observe: 'response' })
      .pipe(
        map(res => ({
          blob: res.body!,
          fileName: fileNameFromHeaders(res.headers),
        })),
        normalizeBlobError(),
      );
  }

  uploadFile(file: File, directoryPath: string) : Observable<UploadState<FileInfoDto>> {
    return new Observable<UploadState<FileInfoDto>>(subscriber => {
      const abort = new AbortController();

      const hashing$ = new Observable<string>(hashSubscriber => {
        sha256Hex(file, progress => subscriber.next({ status: 'hashing', percent: progress}), abort.signal)
          .then(hash => { hashSubscriber.next(hash); hashSubscriber.complete() })
          .catch(err => hashSubscriber.error(err));
      });

      const sub = hashing$.pipe(
        switchMap(sha256 => {
          const params = new HttpParams({ fromObject: {
            'p': directoryPath,
          }});

          const form = new FormData();
          form.append('file', file, file.name);

          return this.http.post<ApiResponse<FileInfoDto>>(`${this.baseUrl}/upload`, form, {
            params: params,
            reportProgress: true,
            observe: 'events',
            headers: { [SHA256_CHECKSUM_HEADER]: sha256 }
          });
        }),
        filter(e => e.type === HttpEventType.UploadProgress || e.type === HttpEventType.Response),
        map((e): UploadState<FileInfoDto> => {
          if (e.type == HttpEventType.UploadProgress) {
            return { status: 'uploading', percent: e.total ? Math.round((100 * e.total) / e.total) : 0 }
          }

          throwIfEnvelopeError(e.body);
          return { status: 'done', data: e.body!.data! };
        }),
        normalizeHttpError(),
      )
      .subscribe(subscriber);

      return () => {
        abort.abort();
        sub.unsubscribe();
      }
    });
  }
}
