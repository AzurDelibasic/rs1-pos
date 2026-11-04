export enum TableStatusType {
  Free = 1,
  Occupied = 2,
  Reserved = 3
}

export interface GetFloorPlanQueryDto {
  regions: GetFloorPlanQueryDtoRegion[];
}

export interface GetFloorPlanQueryDtoRegion {
  id: number;
  name: string;
  tables: GetFloorPlanQueryDtoTable[];
}

export interface GetFloorPlanQueryDtoTable {
  id: number;
  label: string;
  seats: number;
  status: TableStatusType;
}
