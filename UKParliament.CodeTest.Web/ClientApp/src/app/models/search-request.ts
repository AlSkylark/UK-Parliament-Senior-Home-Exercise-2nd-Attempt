import { EmployeeTypeEnum } from "./employee-type-enum"

export class SearchRequest {
    textSearch = "";
    employeeType: EmployeeTypeEnum | "" = "";
    payBand = "";
    department = "";
    limit = "20";
    page = "1";
}