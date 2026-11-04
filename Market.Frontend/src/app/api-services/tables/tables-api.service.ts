import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreatedResponse } from '../../core/models/created-response';
import {
  CreateTableCommand,
  GetTableByIdQueryDto,
  ListTablesRequest,
  ListTablesResponse,
  UpdateTableCommand
} from './tables-api.model';

@Injectable({
  providedIn: 'root',
})
export class TablesApiService {
  private readonly baseUrl = `${environment.apiUrl}/Tables`;
  private http = inject(HttpClient);

  list(request?: ListTablesRequest): Observable<ListTablesResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListTablesResponse>(this.baseUrl, { params });
  }

  getById(id: number): Observable<GetTableByIdQueryDto> {
    return this.http.get<GetTableByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateTableCommand): Observable<CreatedResponse> {
    return this.http.post<CreatedResponse>(this.baseUrl, payload);
  }

  update(id: number, payload: UpdateTableCommand): Observable<void> {
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
