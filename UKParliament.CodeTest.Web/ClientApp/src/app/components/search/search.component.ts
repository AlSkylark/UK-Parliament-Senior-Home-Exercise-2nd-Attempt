import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { TextboxComponent } from "../inputs/textbox/textbox.component";
import { FilterSelectComponent } from "../inputs/filter-select/filter-select.component";
import { CommonModule } from '@angular/common';
import { LookupItemsEnum } from 'src/app/models/lookup-items-enum';
import { SearchRequest } from 'src/app/models/search-request';
import { FilterService } from 'src/app/services/filter.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { ButtonComponent } from "../inputs/button/button.component";
import { EditorService } from 'src/app/services/editor.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [TextboxComponent, FilterSelectComponent, CommonModule, ButtonComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent implements OnInit, OnDestroy {

  editorIsOpen = false;
  editorInCreateMode = false;

  filterSubscription: Subscription;
  editorSubscription: Subscription;
  constructor(private filterService: FilterService, private employeeService: EmployeeService, private editorService: EditorService) {
    this.filterSubscription = this.filterService.filtersSubject.subscribe(f => this.filters = f);

    this.editorSubscription = this.editorService.$editorOpen.subscribe(isOpen => {

      this.editorIsOpen = isOpen;
      this.editorInCreateMode = this.editorIsOpen && this.employeeService.activeEmployee === null;

      if (!isOpen &&
        (this.filters.employeeType != ""
          || this.filters.payBand != ""
          || this.filters.department != "")) {
        this.showFilters = true;
      }

    });
  }


  ngOnInit(): void {
    this.employeeService.fetchEmployees();
  }

  ngOnDestroy(): void {
    this.filterSubscription.unsubscribe();
    this.editorSubscription.unsubscribe();
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

  openEditorForCreate() {
    this.employeeService.unsetEmployee();
    this.editorService.openEditor()
  }
}
