import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy } from '@angular/core';
import { EmployeeViewModel } from 'src/app/models/employee-view-model';
import { Resource } from 'src/app/models/resource';
import { EmployeeService } from 'src/app/services/employee.service';
import { ButtonComponent } from "../inputs/button/button.component";
import { EditorService } from 'src/app/services/editor.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss'
})
export class CardComponent implements OnDestroy {
  @Input({ required: true })
  employee!: Resource<EmployeeViewModel>;

  @Input()
  selected = false;

  editorIsOpen = false;

  editorSubscription: Subscription;
  constructor(private employeeService: EmployeeService, private editorService: EditorService) {
    this.editorSubscription = this.editorService.$editorOpen.subscribe(val => this.editorIsOpen = val);
  }

  ngOnDestroy(): void {
    this.editorSubscription.unsubscribe();
  }

  openEmployeeProfile() {
    this.openProfile("self");
  }

  openManagerProfile(event: Event) {
    event.preventDefault();
    this.openProfile("manager");
  }

  openProfile(type: string) {
    const link = this.employee.links.find(l => l.rel === type)?.href;
    if (link) {
      this.employeeService.selectEmployee(link, () => this.editorService.openEditor());
    }
  }
}
