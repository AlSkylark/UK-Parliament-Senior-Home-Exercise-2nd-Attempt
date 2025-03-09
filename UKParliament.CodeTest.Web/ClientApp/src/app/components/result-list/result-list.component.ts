import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { EmployeeViewModel } from 'src/app/models/employee-view-model';
import { Resource } from 'src/app/models/resource';
import { ResourceCollection } from 'src/app/models/resource-collection';
import { EmployeeService } from 'src/app/services/employee.service';
import { CardComponent } from "../card/card.component";
import { PaginationComponent } from "../../pagination/pagination.component";
import { EditorService } from 'src/app/services/editor.service';

@Component({
  selector: 'app-result-list',
  standalone: true,
  imports: [CommonModule, CardComponent, PaginationComponent],
  templateUrl: './result-list.component.html',
  styleUrl: './result-list.component.scss'
})
export class ResultListComponent {

  editorIsOpen = false;
  employeeCollection: ResourceCollection<Resource<EmployeeViewModel>> | undefined;
  selectedEmployee: Resource<EmployeeViewModel> | null = null;

  constructor(private employeeService: EmployeeService, private editorService: EditorService) {
    this.employeeService.$employeeList.subscribe(collection => this.employeeCollection = collection);
    this.employeeService.$activeEmployee.subscribe(e => {
      this.selectedEmployee = e;
    });
    this.editorService.$editorOpen.subscribe(val => this.editorIsOpen = val);
  }

}
