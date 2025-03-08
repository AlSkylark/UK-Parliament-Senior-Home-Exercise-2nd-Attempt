import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-skip',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './skip.component.html',
  styleUrl: './skip.component.scss'
})
export class SkipComponent {

  skipToMain(id: string) {
    let element = document.getElementById(id);
    element!.setAttribute('tabindex', '-1') // You can set tabindex in HTML too than in JS
    element!.focus()
  }
}
