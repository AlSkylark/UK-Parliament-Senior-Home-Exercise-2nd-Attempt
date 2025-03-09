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

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL')
    private baseUrl: string,
    private filterService: FilterService,
    private errorService: ErrorService) { }

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
    return this.http.get<Resource<EmployeeViewModel>>(url);
  }

  public selectEmployee(url: string, callback: Function | null = null) {
    this.http.get<Resource<EmployeeViewModel>>(url).subscribe(resource => {
      this.activeEmployee = resource;
      this.$activeEmployee.next(this.activeEmployee);

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

    this.sanitiseDates(employee);

    const toSend = Utilities.CleanEmptyObjects(employee);

    this.http.post<Resource<EmployeeViewModel>>(this.createEmployeeLink, toSend).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.$activeEmployee.next(this.activeEmployee);
      },
      error: error => {
        const errorResp = error as HttpErrorResponse;
        console.log(error);
        this.errorService.displayErrors(errorResp.error as ErrorBag);
      }
    })
  }

  public deactivateEmployee(url: string, employee: EmployeeViewModel) {
    employee.dateLeft = Utilities.DateOnly();

    this.updateEmployee(url, employee);
  }

  public reactivateEmployee(url: string, employee: EmployeeViewModel) {
    employee.dateLeft = "";

    this.updateEmployee(url, employee);
  }

  public updateEmployee(url: string, employee: EmployeeViewModel) {
    this.sanitiseDates(employee);

    this.http.put<Resource<EmployeeViewModel>>(url, employee).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.$activeEmployee.next(this.activeEmployee);
        this.fetchEmployees();
      },
      error: error => {
        const errorResp = error as HttpErrorResponse;
        console.log(error);
        this.errorService.displayErrors(errorResp.error as ErrorBag);
      }
    })
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
    employee.dateJoined = Utilities.DateOnly(employee?.dateJoined);
    employee.doB = Utilities.DateOnly(employee?.doB);

    if (employee.dateLeft !== null && (employee?.dateLeft?.length ?? 0) > 0) {
      employee.dateLeft = Utilities.DateOnly(employee?.dateLeft);
    }
  }
}

