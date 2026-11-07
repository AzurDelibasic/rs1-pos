import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreatedResponse } from '../../core/models/created-response';
import {
  CloseShiftCommand,
  GetCurrentShiftQueryDto,
  ListShiftsRequest,
  ListShiftsResponse,
  ListWaitersRequest,
  ListWaitersResponse,
  OpenShiftCommand
} from './shifts-api.model';

@Injectable({
  providedIn: 'root',
})
export class ShiftsApiService {
  private readonly baseUrl = `${environment.apiUrl}/Shifts`;
  private http = inject(HttpClient);

  list(request?: ListShiftsRequest): Observable<ListShiftsResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListShiftsResponse>(this.baseUrl, { params });
  }

  listWaiters(request?: ListWaitersRequest): Observable<ListWaitersResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListWaitersResponse>(`${this.baseUrl}/waiters`, { params });
  }

  getCurrent(): Observable<GetCurrentShiftQueryDto | null> {
    return this.http.get<GetCurrentShiftQueryDto | null>(`${this.baseUrl}/current`);
  }

  open(payload: OpenShiftCommand): Observable<CreatedResponse> {
    return this.http.post<CreatedResponse>(this.baseUrl, payload);
  }

  close(id: number, payload: CloseShiftCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/close`, payload);
  }
}
