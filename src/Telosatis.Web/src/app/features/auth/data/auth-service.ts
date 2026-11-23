import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '@core/api/api-config';
import { AuthenticatedDto, AuthenticateDto } from './auth.models';
import { Observable } from 'rxjs';
import { ApiResponse } from '@core/api/api-response';
import { unwrap } from '@core/api/unwrap';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = `${inject(API_BASE_URL)}/api/auth`

  authenticate(dto: AuthenticateDto): Observable<AuthenticatedDto> {
    return this.http
      .post<ApiResponse<AuthenticatedDto>>(this.baseUrl, dto)
      .pipe(unwrap());
  }

  reauthenticate(): Observable<AuthenticatedDto> {
    return this.http
      .post<ApiResponse<AuthenticatedDto>>(`${this.baseUrl}/re`, {})
      .pipe(unwrap());
  }
}
