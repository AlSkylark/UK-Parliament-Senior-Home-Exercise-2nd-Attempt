import { Component, Input, OnInit, Output } from '@angular/core';
import { HeaderComponent } from "../header/header.component";
import { SearchComponent } from "../search/search.component";
import { FooterComponent } from "../footer/footer.component";
import { EditorComponent } from "../editor/editor.component";
import { ResultListComponent } from "../result-list/result-list.component";
import { CommonModule } from '@angular/common';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HeaderComponent, SearchComponent, FooterComponent, EditorComponent, ResultListComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  editorOpen = false;

  constructor(private employeeService: EmployeeService) {
    this.employeeService.employeeSubject.subscribe(e => this.editorOpen = (e !== null))
  }
}
