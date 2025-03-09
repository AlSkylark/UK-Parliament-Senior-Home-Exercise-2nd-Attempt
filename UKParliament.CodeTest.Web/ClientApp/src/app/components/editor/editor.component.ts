import { Component, OnDestroy } from '@angular/core';
import { FormSectionComponent } from "../inputs/form-section/form-section.component";
import { EmployeeService } from 'src/app/services/employee.service';
import { TextboxComponent } from "../inputs/textbox/textbox.component";
import { EmployeeViewModel } from 'src/app/models/employee-view-model';
import { Resource } from 'src/app/models/resource';
import { DatePickerComponent } from "../inputs/date-picker/date-picker.component";
import { CommonModule } from '@angular/common';
import { ButtonComponent } from "../inputs/button/button.component";
import { LookupItemsEnum } from 'src/app/models/lookup-items-enum';
import { DropdownComponent } from "../inputs/dropdown/dropdown.component";
import { NumberComponent } from "../inputs/number/number.component";
import { CardComponent } from "../card/card.component";
import { Link } from 'src/app/models/link';
import { ErrorService } from 'src/app/services/error.service';
import { Subscription } from 'rxjs';
import { EditorService } from 'src/app/services/editor.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-editor',
  standalone: true,
  imports: [FormSectionComponent, TextboxComponent, DatePickerComponent, CommonModule, ButtonComponent, DropdownComponent, NumberComponent, CardComponent],
  templateUrl: './editor.component.html',
  styleUrl: './editor.component.scss'
})
export class EditorComponent implements OnDestroy {
  selectedEmployee!: Resource<EmployeeViewModel>;
  manager: Resource<EmployeeViewModel> | null = null;

  nonReactiveName?: string | null = null;
  link?: Link | null = null;

  hasErrors = false;

  createModeEnabled = false;

  employeeSubscription: Subscription;
  errorsSubscription: Subscription;

  constructor(
    private editorService: EditorService,
    private employeeService: EmployeeService,
    private errorService: ErrorService) {

    this.setNewEmployee();

    this.employeeSubscription = this.employeeService.$activeEmployee.subscribe(employee => {
      if (!employee) {
        this.setNewEmployee();
        this.resetInitialVars();
        return;
      }

      this.createModeEnabled = !employee?.data.id;
      this.selectedEmployee = employee;

      this.loadInitialVars();
      this.loadManager();
      this.errorService.resetErrors();
    });

    this.errorsSubscription = this.errorService.$errors.subscribe(e => this.hasErrors = e.length > 0);
  }

  ngOnDestroy(): void {
    this.employeeSubscription.unsubscribe();
    this.errorsSubscription.unsubscribe();
  }

  setNewEmployee() {
    const newEmployee: Resource<EmployeeViewModel> = {
      data: {
        address: {},
        manager: {}
      },
      links: []
    }
    this.selectedEmployee = newEmployee;
    this.createModeEnabled = true;
  }

  resetInitialVars() {
    this.manager = null;
    this.nonReactiveName = null;
    this.link = null;
  }

  loadInitialVars() {
    if (!this.selectedEmployee) {
      return;
    }

    this.nonReactiveName = this.selectedEmployee.data.firstName;

    this.link = this.selectedEmployee.links.find(l => l.rel === "self");
  }

  loadManager() {
    if (!this.selectedEmployee) {
      return;
    }

    const managerLink = this.selectedEmployee.links.find(l => l.rel === 'manager');
    if (managerLink) {
      this.employeeService.requestManager(managerLink.href)
        .subscribe(m => this.manager = m);
    } else {
      this.manager = null;
    }
  }

  createEmployee() {
    if (!this.selectedEmployee) {
      return;
    }

    this.employeeService.createEmployee(this.selectedEmployee.data);
  }

  saveEmployee() {
    if (!this.selectedEmployee || !this.link) {
      return;
    }

    this.employeeService.updateEmployee(this.link.href, this.selectedEmployee.data);
  }

  deleteEmployee() {
    if (!this.selectedEmployee || !this.link) {
      return;
    }

    this.employeeService.deleteEmployee(this.link.href);
    this.closeEditor();
  }

  deactivateEmployee() {
    if (!this.selectedEmployee || !this.link) {
      return;
    }

    this.employeeService.deactivateEmployee(this.link.href, this.selectedEmployee.data);
  }

  reactivateEmployee() {
    if (!this.selectedEmployee || !this.link) {
      return;
    }

    this.employeeService.reactivateEmployee(this.link.href, this.selectedEmployee.data);
  }

  closeEditor() {
    this.editorService.closeEditor();
    this.employeeService.unsetEmployee();
  }

  public get lookupItems(): typeof LookupItemsEnum {
    return LookupItemsEnum;
  }
}
