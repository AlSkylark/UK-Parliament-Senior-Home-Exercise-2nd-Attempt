import { Injectable } from '@angular/core';
import { SearchRequest } from '../models/search-request';
import { Subject } from 'rxjs';
import { Pagination } from '../models/pagination';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  constructor() { }

  private currentFilters: SearchRequest = new SearchRequest();

  filtersSubject = new Subject<SearchRequest>();

  public getCurrentFilters() {
    return { ...this.currentFilters };
  }

  public updateFilters({ department, employeeType, payBand, textSearch }: SearchRequest) {
    this.currentFilters.department = department;
    this.currentFilters.employeeType = employeeType;
    this.currentFilters.payBand = payBand;
    this.currentFilters.textSearch = textSearch;
    this.resetPagination();

    this.filtersSubject.next(this.currentFilters);
  }

  public updatePagination({ page, limit }: SearchRequest) {
    this.currentFilters.page = page;
    this.currentFilters.limit = limit;

    this.filtersSubject.next(this.currentFilters);
  }

  public resetFilters() {
    this.currentFilters.department = "";
    this.currentFilters.employeeType = "";
    this.currentFilters.payBand = "";
    this.currentFilters.textSearch = "";

    this.filtersSubject.next(this.currentFilters);
  }

  public resetPagination() {
    this.currentFilters.page = "1";
    this.currentFilters.limit = "20";
  }
}
