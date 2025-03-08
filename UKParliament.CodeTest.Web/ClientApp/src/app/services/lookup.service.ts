import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { LookupItem } from '../models/lookup-item';
import { Subject } from 'rxjs';
import { LookupItemsEnum } from '../models/lookup-items-enum';

/**
 * This service keeps track of the LookupItems available for the main filters.
 * The flow is as  follows: 
 * - You get the `lookUpItem` subject and subscribe to it on the client.
 * - When you want to look it up you call `getLookUpItems`
 * - This internally will make a call IF values are not already present
 * - The resulting call will update the subjects sending updates to its subscribers.
 */
@Injectable({
  providedIn: 'root'
})
export class LookupService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  private payBands: LookupItem[] = [];
  private departments: LookupItem[] = [];
  private employeeTypes: LookupItem[] = [];

  payBandsSubject = new Subject<LookupItem[]>();
  departmentsSubject = new Subject<LookupItem[]>();
  employeeTypesSubject = new Subject<LookupItem[]>();

  public lookUpItem(item: LookupItemsEnum) {
    switch (item) {
      case LookupItemsEnum.Department:
        return this.departmentsSubject;

      case LookupItemsEnum.EmployeeType:
        return this.employeeTypesSubject;

      case LookupItemsEnum.PayBand:
        return this.payBandsSubject;
    }
  }

  private populateLookupItem(item: LookupItemsEnum, newItems: LookupItem[]) {
    switch (item) {
      case LookupItemsEnum.Department:
        this.departments = newItems;
        this.departmentsSubject.next(this.departments);
        break;

      case LookupItemsEnum.EmployeeType:
        this.employeeTypes = newItems;
        this.employeeTypesSubject.next(this.employeeTypes);
        break;

      case LookupItemsEnum.PayBand:
        this.payBands = newItems;
        this.payBandsSubject.next(this.payBands);
        break;
    }
  }

  private getLookupItem(item: LookupItemsEnum) {
    switch (item) {
      case LookupItemsEnum.Department:
        return this.departments;

      case LookupItemsEnum.EmployeeType:
        return this.employeeTypes;

      case LookupItemsEnum.PayBand:
        return this.payBands;
    }
  }

  public getLookupItems(item: LookupItemsEnum, forceRefresh = false) {
    const loadedItem = this.getLookupItem(item);
    if (loadedItem.length === 0 || forceRefresh) {
      this.http.get<LookupItem[]>(this.baseUrl + `api/lookup/search?item=${item.toString()}`).subscribe(e => {
        this.populateLookupItem(item, e);
      });

      return true;
    }

    this.populateLookupItem(item, loadedItem);
    return false;
  }
}
