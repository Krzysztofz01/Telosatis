import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '@core/api/api-config';
import { CreateTokenDto, ListTokensOrderKey, TokenCreatedDto, TokenDto, TokenRootAdminCreatedDto } from './tokens.models';
import { ApiResponse } from '@core/api/api-response';
import { unwrap } from '@core/api/unwrap';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TokensService {
  private http = inject(HttpClient);
  private baseUrl = `${inject(API_BASE_URL)}/api/tokens`

  createToken(dto: CreateTokenDto): Observable<TokenCreatedDto> {
    return this.http
      .post<ApiResponse<TokenCreatedDto>>(this.baseUrl, dto)
      .pipe(unwrap());
  }

  listTokens(orderKey: ListTokensOrderKey | null = null, orderDescending: boolean | null = null): Observable<Array<TokenDto>> {
    const params = new HttpParams();

    if (orderKey !== null) {
      params.append('k', orderKey);
    }

    if (orderDescending !== null) {
      params.append('d', orderDescending);
    }

    return this.http
      .get<ApiResponse<Array<TokenDto>>>(this.baseUrl, { params })
      .pipe(unwrap());
  }

  deleteToken(tokenId: string): Observable<string> {
    return this.http
      .delete<ApiResponse<string>>(`${this.baseUrl}/${tokenId}`)
      .pipe(unwrap());
  }

  isRootAdminCreated(): Observable<TokenRootAdminCreatedDto> {
    return this.http
      .get<ApiResponse<TokenRootAdminCreatedDto>>(`${this.baseUrl}/onboarding`)
      .pipe(unwrap());
  }

  createRootAdminToken(): Observable<TokenCreatedDto> {
    return this.http
      .post<ApiResponse<TokenCreatedDto>>(`${this.http}/onboarding`, {})
      .pipe(unwrap());
  }
}
