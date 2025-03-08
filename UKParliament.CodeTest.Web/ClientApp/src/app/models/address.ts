import { BaseViewModel } from "./base-view-model";

export interface Address extends BaseViewModel {
    address1?: string,
    address2?: string,
    postcode?: string,
    county?: string
}