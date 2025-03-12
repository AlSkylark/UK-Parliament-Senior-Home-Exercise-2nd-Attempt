import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { EmployeeViewModel } from '../models/employee-view-model';
import { FilterService } from './filter.service';
import { Resource } from '../models/resource';
import { ResourceCollection } from '../models/resource-collection';
import { ErrorService } from './error.service';
import { ErrorBag } from '../models/errors/error-bag';
import { Utilities } from '../utilities/utilities';
import { EditorAlertService } from './editor-alert.service';
import { WarningsService } from './warnings.service';
import { ManagerViewModel } from '../models/manager-view-model';

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL')
    private baseUrl: string,
    private filterService: FilterService,
    private errorService: ErrorService,
    private alertService: EditorAlertService,
    private warningsService: WarningsService) { }

  createEmployeeLink?: string;
  $employeeList = new Subject<ResourceCollection<Resource<EmployeeViewModel>>>();

  activeEmployee: Resource<EmployeeViewModel> | null = null;
  $activeEmployee = new BehaviorSubject<Resource<EmployeeViewModel> | null>(null);

  public fetchEmployees() {
    const filters = this.filterService.getCurrentFilters();
    const params = new URLSearchParams(filters);
    this.http.get<Resource<ResourceCollection<Resource<EmployeeViewModel>>>>(`${this.baseUrl}api/employee?${params}`).subscribe(employees => {
      this.createEmployeeLink = employees.links.find(l => l.rel = "self")?.href;
      this.$employeeList.next(employees.data);
    })
  }

  public requestManager(url: string) {
    return this.http.get<Resource<ManagerViewModel>>(url);
  }

  public selectEmployee(url: string, callback: Function | null = null) {
    this.http.get<Resource<EmployeeViewModel | ManagerViewModel>>(url).subscribe(resource => {
      this.activeEmployee = resource;
      this.$activeEmployee.next(this.activeEmployee);
      this.warningsService.displayErrors(this.activeEmployee.data.irregularities);

      if (callback) {
        callback();
      }
    });
  }

  public unsetEmployee() {
    this.activeEmployee = null;
    this.$activeEmployee.next(this.activeEmployee);
  }

  public createEmployee(employee: EmployeeViewModel) {
    if (!this.createEmployeeLink) {
      return;
    }

    const toSend = Utilities.CleanEmptyObjects(employee);
    this.sanitiseDates(toSend);

    this.http.post<Resource<EmployeeViewModel>>(this.createEmployeeLink, toSend).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.$activeEmployee.next(this.activeEmployee);
        this.warningsService.displayErrors(this.activeEmployee.data.irregularities);
        this.fetchEmployees();
        this.alertService.sendAlert("✔️ Employee created successfully!");
      },
      error: error => {
        const errorResp = error as HttpErrorResponse;
        console.log(error);
        this.errorService.displayErrors(errorResp.error as ErrorBag);
        this.alertService.sendAlert("❌ Unable to create employee", true);
      }
    })
  }

  public deactivateEmployee(url: string, employee: EmployeeViewModel) {
    employee.dateLeft = Utilities.DateOnly();

    this.updateEmployee(url, employee);
  }

  public reactivateEmployee(url: string, employee: EmployeeViewModel) {
    employee.dateLeft = undefined;

    this.updateEmployee(url, employee);
  }

  public updateEmployee(url: string, employee: EmployeeViewModel) {
    Utilities.CleanNullString(employee);
    this.sanitiseDates(employee);

    this.http.put<Resource<EmployeeViewModel>>(url, employee).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.$activeEmployee.next(this.activeEmployee);
        this.warningsService.displayErrors(this.activeEmployee.data.irregularities);
        this.fetchEmployees();
        this.alertService.sendAlert("✔️ Employee saved successfully!");
      },
      error: error => {
        const errorResp = error as HttpErrorResponse;
        console.log(error);
        this.errorService.displayErrors(errorResp.error as ErrorBag);
        this.alertService.sendAlert("❌ Unable to save employee", true);
      }
    });
  }

  public deleteEmployee(url: string) {
    this.http.delete<void>(url).subscribe({
      next: () => {
        this.fetchEmployees();
      },
      error: e => {
        console.log(e);
      }
    });
  }

  private sanitiseDates(employee: EmployeeViewModel) {
    if (employee?.dateJoined) {
      employee.dateJoined = Utilities.DateOnly(employee.dateJoined);
    }

    if (employee?.doB) {
      employee.doB = Utilities.DateOnly(employee.doB);
    }

    if (employee?.dateLeft !== null && (employee?.dateLeft?.length ?? 0) > 0) {
      employee.dateLeft = Utilities.DateOnly(employee.dateLeft);
    }
  }
}

