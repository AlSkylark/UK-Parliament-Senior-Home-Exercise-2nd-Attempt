import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BaseInputComponent } from '../base-input.component';
import { CommonModule } from '@angular/common';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss'
})
export class DatePickerComponent extends BaseInputComponent<string> {

  onInput(event: Event): void {
    const target = (event.target as HTMLInputElement);
    const inputValue = target.value;
    if (!inputValue) {
      return;
    }

    this.valueChange.emit(inputValue);
    this.errorService.resetErrors();
  }
}
