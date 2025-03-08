import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TextboxComponent } from "../inputs/textbox/textbox.component";
import { FilterSelectComponent } from "../inputs/filter-select/filter-select.component";
import { CommonModule } from '@angular/common';
import { LookupItemsEnum } from 'src/app/models/lookup-items-enum';
import { SearchRequest } from 'src/app/models/search-request';
import { FilterService } from 'src/app/services/filter.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { ButtonComponent } from "../inputs/button/button.component";

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [TextboxComponent, FilterSelectComponent, CommonModule, ButtonComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent implements OnInit {

  editorIsOpen = false;

  constructor(private filterService: FilterService, private employeeService: EmployeeService) { }
  ngOnInit(): void {
    this.filterService.filtersSubject.subscribe(f => this.filters = f);
    this.employeeService.employeeSubject.subscribe(e => this.editorIsOpen = !!e);
    this.employeeService.fetchEmployees();
  }

  filters: SearchRequest = new SearchRequest();

  showFilters = false;


  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  public get lookupItems(): typeof LookupItemsEnum {
    return LookupItemsEnum;
  }

  searchRequestUpdated() {
    this.filterService.updateFilters(this.filters);
  }

  resetFilter() {
    this.filterService.resetFilters();
  }

  search() {
    this.employeeService.fetchEmployees();
  }
}
