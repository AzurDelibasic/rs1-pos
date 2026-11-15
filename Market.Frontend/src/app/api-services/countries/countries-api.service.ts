import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreatedResponse } from '../../core/models/created-response';
import {
  CreateCountryCommand,
  GetCountryByIdQueryDto,
  ListCountriesRequest,
  ListCountriesResponse,
  UpdateCountryCommand
} from './countries-api.model';

@Injectable({
  providedIn: 'root',
})
export class CountriesApiService {
  private readonly baseUrl = `${environment.apiUrl}/Countries`;
  private http = inject(HttpClient);

  list(request?: ListCountriesRequest): Observable<ListCountriesResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListCountriesResponse>(this.baseUrl, { params });
  }

  getById(id: number): Observable<GetCountryByIdQueryDto> {
    return this.http.get<GetCountryByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateCountryCommand): Observable<CreatedResponse> {
    return this.http.post<CreatedResponse>(this.baseUrl, payload);
  }

  update(id: number, payload: UpdateCountryCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
