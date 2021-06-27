export interface IPagination {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export class PaginatedItems<T> {
    items: T;
    pagination: IPagination;
}
