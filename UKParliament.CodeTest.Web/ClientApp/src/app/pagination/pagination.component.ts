import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Pagination } from '../models/pagination';
import { EmployeeService } from '../services/employee.service';
import { FilterService } from '../services/filter.service';
import { SearchRequest } from '../models/search-request';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from "../components/inputs/button/button.component";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [FormsModule, ButtonComponent],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent implements OnInit, OnDestroy {
  @Input({ required: true })
  pagination?: Pagination;

  currentFilters: SearchRequest = new SearchRequest();

  filterSubscription: Subscription | undefined;

  constructor(private employeeService: EmployeeService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.filterSubscription = this.filterService.filtersSubject.subscribe(f => this.currentFilters = f);
  }

  ngOnDestroy(): void {
    this.filterSubscription?.unsubscribe();
  }

  goNext() {
    if (!this.pagination) {
      return;
    }

    if (this.pagination.currentPage !== this.pagination.finalPage) {
      const page = (this.pagination.currentPage + 1).toString();
      this.currentFilters.page = page.toString();
      this.goTo();
    }
  }

  goPrev() {
    if (!this.pagination) {
      return;
    }

    if (this.pagination.currentPage !== 1) {
      const page = (this.pagination.currentPage - 1).toString();
      this.currentFilters.page = page.toString();
      this.goTo();
    }
  }

  goTo() {
    if (this.pagination) {
      this.filterService.updatePagination(this.currentFilters);
      this.employeeService.fetchEmployees();
    }
  }

  createRange(number: number) {
    return new Array(number).fill(0)
      .map((n, index) => index + 1);
  }

}
