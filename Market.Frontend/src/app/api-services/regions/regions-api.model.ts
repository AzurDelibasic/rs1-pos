import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export class ListRegionsRequest extends BasePagedQuery {
  search?: string | null;
  onlyActive?: boolean | null;
}

export interface ListRegionsQueryDto {
  id: number;
  name: string;
  sortOrder: number;
  isActive: boolean;
}

export interface GetRegionByIdQueryDto {
  id: number;
  name: string;
  sortOrder: number;
  isActive: boolean;
}

export type ListRegionsResponse = PageResult<ListRegionsQueryDto>;

export interface CreateRegionCommand {
  name: string;
  sortOrder: number;
  isActive: boolean;
}

export interface UpdateRegionCommand {
  name: string;
  sortOrder: number;
  isActive: boolean;
}