import { Link } from "./link";

export interface Resource<T> {
    data: T,
    links: Link[]
}