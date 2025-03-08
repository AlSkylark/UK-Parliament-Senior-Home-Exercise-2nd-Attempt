import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { EmployeeViewModel } from '../models/employee-view-model';
import { FilterService } from './filter.service';
import { Resource } from '../models/resource';
import { ResourceCollection } from '../models/resource-collection';
import { ErrorService } from './error.service';
import { ErrorBag } from '../models/errors/error-bag';

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  constructor(
    private http: HttpClient, @Inject('BASE_URL')
    private baseUrl: string,
    private filterService: FilterService,
    private errorService: ErrorService) { }

  createEmployeeLink?: string;
  employeeListSubject = new Subject<ResourceCollection<Resource<EmployeeViewModel>>>();

  activeEmployee: Resource<EmployeeViewModel> | null = null;
  employeeSubject = new BehaviorSubject<Resource<EmployeeViewModel> | null>(null);

  public fetchEmployees() {
    const filters = this.filterService.getCurrentFilters();
    const params = new URLSearchParams(filters);
    this.http.get<Resource<ResourceCollection<Resource<EmployeeViewModel>>>>(`${this.baseUrl}api/employee?${params}`).subscribe(employees => {
      this.createEmployeeLink = employees.links.find(l => l.rel = "self")?.href;
      this.employeeListSubject.next(employees.data);
    })
  }

  public requestManager(url: string) {
    return this.http.get<Resource<EmployeeViewModel>>(url);
  }

  public selectEmployee(url: string) {
    this.http.get<Resource<EmployeeViewModel>>(url).subscribe(resource => {
      this.activeEmployee = resource;
      this.employeeSubject.next(this.activeEmployee);
    })
  }

  public openEditorForCreate() {
    const newEmployee: Resource<EmployeeViewModel> = {
      data: {
        address: {},
        manager: {}
      },
      links: []
    }

    this.activeEmployee = newEmployee;
    this.employeeSubject.next(newEmployee);
  }

  public closeEmployeeEditor() {
    this.activeEmployee = null;
    this.employeeSubject.next(null);
  }

  public createEmployee(employee: EmployeeViewModel) {
    if (!this.createEmployeeLink) {
      return;
    }

    this.sanitiseDates(employee);

    const toSend = this.cleanEmpty(employee);

    this.http.post<Resource<EmployeeViewModel>>(this.createEmployeeLink, toSend).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.employeeSubject.next(this.activeEmployee);
      },
      error: error => {
        const errorResp = error as HttpErrorResponse;
        console.log(error);
        this.errorService.displayErrors(errorResp.error as ErrorBag);
      }
    })
  }

  public deactivateEmployee(url: string, employee: EmployeeViewModel) {
    employee.dateLeft = this.DateOnly("", true);

    this.updateEmployee(url, employee);
  }

  public updateEmployee(url: string, employee: EmployeeViewModel) {
    this.sanitiseDates(employee);

    this.http.put<Resource<EmployeeViewModel>>(url, employee).subscribe({
      next: result => {
        this.activeEmployee = result;
        this.employeeSubject.next(this.activeEmployee);
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
    employee.dateJoined = this.DateOnly(employee?.dateJoined);
    employee.doB = this.DateOnly(employee?.doB);

    if (employee.dateLeft !== null && (employee?.dateLeft?.length ?? 0) > 0) {
      employee.dateLeft = this.DateOnly(employee?.dateLeft);
    }
  }

  private DateOnly(date?: string, createNew = false) {
    if (!date) {
      return;
    }
    if (createNew) {
      return new Date().toISOString().split("T")[0];
    }

    return new Date(date!).toISOString().split("T")[0];
  }

  private cleanEmpty(obj: Object): any {
    let clean: any = {}
    for (const entry in obj) {
      let val = obj[entry as keyof Object];
      if (val instanceof Object) {
        const recursed = this.cleanEmpty(val);
        if (Object.keys(recursed).length === 0) {
          continue;
        }
      }

      if (val) {
        clean[entry] = val;
      }
    }
    return clean;
  }
}

