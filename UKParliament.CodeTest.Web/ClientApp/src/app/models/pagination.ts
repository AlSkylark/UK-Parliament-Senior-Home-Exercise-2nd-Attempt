export interface Pagination {
    total: number,
    perPage: number,
    currentPage: number,
    finalPage: number,
    firstPageUrl?: string,
    finalPageUrl?: string,
    nextPageUrl?: string,
    prevPageUrl?: string,
    path?: string,
    from: number,
    to: number,
}