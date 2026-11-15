import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetAppSettingsQueryDto, UpdateAppSettingsCommand } from './app-settings-api.model';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsApiService {
  private readonly baseUrl = `${environment.apiUrl}/AppSettings`;
  private http = inject(HttpClient);

  get(): Observable<GetAppSettingsQueryDto> {
    return this.http.get<GetAppSettingsQueryDto>(this.baseUrl);
  }

  update(payload: UpdateAppSettingsCommand): Observable<void> {
    return this.http.put<void>(this.baseUrl, payload);
  }
}
