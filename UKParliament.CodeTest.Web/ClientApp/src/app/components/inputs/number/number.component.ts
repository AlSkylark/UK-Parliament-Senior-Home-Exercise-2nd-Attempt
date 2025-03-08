import { Component } from '@angular/core';
import { BaseComponent } from '../base-input.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-number',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './number.component.html',
  styleUrl: './number.component.scss'
})
export class NumberComponent extends BaseComponent<number> {

  onInput(event: Event): void {
    const inputValue = (event.target as HTMLInputElement).value;
    this.valueChange.emit(parseFloat(inputValue));
    this.errorService.resetErrors();
  }

}
