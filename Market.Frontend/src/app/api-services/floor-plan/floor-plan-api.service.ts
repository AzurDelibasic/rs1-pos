import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetFloorPlanQueryDto } from './floor-plan-api.model';

@Injectable({
  providedIn: 'root',
})
export class FloorPlanApiService {
  private readonly baseUrl = `${environment.apiUrl}/FloorPlan`;
  private http = inject(HttpClient);

  get(): Observable<GetFloorPlanQueryDto> {
    return this.http.get<GetFloorPlanQueryDto>(this.baseUrl);
  }
}
