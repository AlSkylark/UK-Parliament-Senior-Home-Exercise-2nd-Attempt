import { Component } from '@angular/core';
import { NavigationComponent } from "../navigation/navigation.component";
import { EmployeeService } from 'src/app/services/employee.service';
import { EditorService } from 'src/app/services/editor.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NavigationComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  constructor(private employeeService: EmployeeService, private editorService: EditorService) { }

  toMainPage() {
    this.editorService.closeEditor();
    this.employeeService.unsetEmployee();
  }

}
