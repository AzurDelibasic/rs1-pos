import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreatedResponse } from '../../core/models/created-response';
import {
  CreateCashRegisterCommand,
  GetCashRegisterByIdQueryDto,
  ListCashRegistersRequest,
  ListCashRegistersResponse,
  UpdateCashRegisterCommand
} from './cash-registers-api.model';

@Injectable({
  providedIn: 'root',
})
export class CashRegistersApiService {
  private readonly baseUrl = `${environment.apiUrl}/CashRegisters`;
  private http = inject(HttpClient);

  list(request?: ListCashRegistersRequest): Observable<ListCashRegistersResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListCashRegistersResponse>(this.baseUrl, { params });
  }

  getById(id: number): Observable<GetCashRegisterByIdQueryDto> {
    return this.http.get<GetCashRegisterByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateCashRegisterCommand): Observable<CreatedResponse> {
    return this.http.post<CreatedResponse>(this.baseUrl, payload);
  }

  update(id: number, payload: UpdateCashRegisterCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  activate(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/activate`, {});
  }

  deactivate(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/deactivate`, {});
  }
}
