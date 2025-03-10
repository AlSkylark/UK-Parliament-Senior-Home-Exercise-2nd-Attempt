import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form-section',
  standalone: true,
  imports: [],
  templateUrl: './form-section.component.html',
  styleUrl: './form-section.component.scss'
})
export class FormSectionComponent {
  @Input({ required: true })
  title!: string;
}
