import { EmployeeViewModel } from "./employee-view-model";
import { Resource } from "./resource";

export interface ManagerViewModel extends EmployeeViewModel {
    employees: Resource<EmployeeViewModel>[]
}