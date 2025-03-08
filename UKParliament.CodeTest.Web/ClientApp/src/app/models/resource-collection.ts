import { Pagination } from "./pagination";

export interface ResourceCollection<T> {
    results: T[],
    pagination: Pagination
}