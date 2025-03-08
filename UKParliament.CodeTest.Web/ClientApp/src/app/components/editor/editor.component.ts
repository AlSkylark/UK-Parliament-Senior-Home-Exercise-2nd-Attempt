import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
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
import { ValidationError } from 'src/app/models/errors/validation-error';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-editor',
  standalone: true,
  imports: [FormSectionComponent, TextboxComponent, DatePickerComponent, CommonModule, ButtonComponent, DropdownComponent, NumberComponent, CardComponent],
  templateUrl: './editor.component.html',
  styleUrl: './editor.component.scss'
})
export class EditorComponent implements OnDestroy {

  selectedEmployee: Resource<EmployeeViewModel> | null = null;
  manager: Resource<EmployeeViewModel> | null = null;

  nonReactiveName?: string | null = null;
  link?: Link | null = null;

  createModeEnabled = false;
  subscriber: Subscription;

  constructor(private employeeService: EmployeeService, private errorService: ErrorService) {
    this.subscriber = this.employeeService.employeeSubject.subscribe(employee => {
      this.createModeEnabled = false;
      if (!employee?.data.id) {
        this.createModeEnabled = true;
      }

      if (this.selectedEmployee?.data.id !== employee?.data.id) {
        this.resetInitialVars();
      }

      this.selectedEmployee = employee;

      this.loadInitialVars();
      this.errorService.resetErrors();
    });
  }
  ngOnDestroy(): void {
    this.subscriber.unsubscribe();
  }

  resetInitialVars() {
    this.manager = null;
    this.nonReactiveName = null;
    this.link = null;
  }

  loadInitialVars() {
    if (!this.nonReactiveName) {
      this.nonReactiveName = this.selectedEmployee?.data.firstName;
    }

    if (!this.link) {
      this.link = this.selectedEmployee?.links.find(l => l.rel === "self");
    }

    const managerLink = this.selectedEmployee?.links.find(l => l.rel === 'manager');
    if (managerLink) {
      this.employeeService.requestManager(managerLink.href)
        .subscribe(m => this.manager = m);
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

    this.employeeService.updateEmployee(this.link!.href, this.selectedEmployee.data);
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

  closeEditor() {
    this.employeeService.closeEmployeeEditor();
  }

  public get lookupItems(): typeof LookupItemsEnum {
    return LookupItemsEnum;
  }
}
