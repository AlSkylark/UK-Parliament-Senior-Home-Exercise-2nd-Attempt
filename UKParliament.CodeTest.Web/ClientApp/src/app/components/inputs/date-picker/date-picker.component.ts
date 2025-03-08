import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BaseComponent } from '../base-input.component';
import { CommonModule } from '@angular/common';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss'
})
export class DatePickerComponent extends BaseComponent<string> {

  onInput(event: Event): void {
    const inputValue = (event.target as HTMLInputElement).value;
    const date = new Date(inputValue);
    this.valueChange.emit(date.toISOString());
    this.errorService.resetErrors();
  }
}
