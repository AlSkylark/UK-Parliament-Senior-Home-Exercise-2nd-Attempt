import { EmployeeViewModel } from "./employee-view-model";

export interface ManagerViewModel extends EmployeeViewModel {
    employees: EmployeeViewModel[]
}