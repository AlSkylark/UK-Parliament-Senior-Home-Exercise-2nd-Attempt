import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BaseInputComponent } from '../base-input.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-textbox',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './textbox.component.html',
  styleUrl: './textbox.component.scss'
})
export class TextboxComponent extends BaseInputComponent<string> {

  @Input()
  isPassword = false;

  onInput(event: Event): void {
    const inputValue = (event.target as HTMLInputElement).value;
    this.valueChange.emit(inputValue);
    this.errorService.resetErrors();
  }
}
