import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export class ListTablesRequest extends BasePagedQuery {
  search?: string | null;
  regionId?: number | null;
  onlyActive?: boolean | null;
}

export interface ListTablesQueryDto {
  id: number;
  label: string;
  seats: number;
  regionId: number;
  regionName: string;
  isActive: boolean;
}

export interface GetTableByIdQueryDto {
  id: number;
  label: string;
  seats: number;
  regionId: number;
  regionName: string;
  isActive: boolean;
}

export type ListTablesResponse = PageResult<ListTablesQueryDto>;

export interface CreateTableCommand {
  label: string;
  seats: number;
  regionId: number;
  isActive: boolean;
}

export interface UpdateTableCommand {
  label: string;
  seats: number;
  regionId: number;
  isActive: boolean;
}
