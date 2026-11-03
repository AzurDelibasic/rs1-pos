import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreatedResponse } from '../../core/models/created-response';
import {
  CreateRegionCommand,
  GetRegionByIdQueryDto,
  ListRegionsRequest,
  ListRegionsResponse,
  UpdateRegionCommand
} from './regions-api.model';

@Injectable({
  providedIn: 'root',
})
export class RegionsApiService {
  private readonly baseUrl = `${environment.apiUrl}/Regions`;
  private http = inject(HttpClient);

  list(request?: ListRegionsRequest): Observable<ListRegionsResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListRegionsResponse>(this.baseUrl, { params });
  }

  getById(id: number): Observable<GetRegionByIdQueryDto> {
    return this.http.get<GetRegionByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateRegionCommand): Observable<CreatedResponse> {
    return this.http.post<CreatedResponse>(this.baseUrl, payload);
  }

  update(id: number, payload: UpdateRegionCommand): Observable<void> {
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
