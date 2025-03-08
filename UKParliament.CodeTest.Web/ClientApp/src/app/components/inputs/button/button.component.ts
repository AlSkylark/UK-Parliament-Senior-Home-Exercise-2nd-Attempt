import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  @Input({ required: true })
  type: "button" | "menu" | "submit" | "reset" = 'button';

  @Input()
  disabled = false;

  @Input()
  clickFunction?: Function;

  @Input()
  ariaLabel = "";

  @Output()
  click = new EventEmitter<void>();

  activateButton() {
    this.click.next()
  }
}
