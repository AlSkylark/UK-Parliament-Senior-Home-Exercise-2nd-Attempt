import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { EditorAlertService } from 'src/app/services/editor-alert.service';
import { animate, state, style, transition, trigger } from '@angular/animations'

@Component({
  selector: 'app-editor-alert',
  standalone: true,
  imports: [],
  templateUrl: './editor-alert.component.html',
  styleUrl: './editor-alert.component.scss',
  animations: [
    trigger('popInAndOut', [
      transition(":enter", [style({ bottom: '-5%' }),
      animate('300ms ease-out', style({ bottom: '0' }))]
      ),
      transition(":leave", [style({ bottom: '0' }),
      animate('300ms ease-out', style({ bottom: '-5%' }))]
      ),
    ]),
  ]
})
export class EditorAlertComponent {

  display = false;
  message: string = "";
  isError: boolean = false;

  alertSubscription: Subscription;
  constructor(private alertService: EditorAlertService) {
    this.alertSubscription = this.alertService.$alerts.subscribe(alert => {
      this.display = true;
      this.message = alert.message;
      this.isError = alert.isError;

      setTimeout(() => this.display = false, 2000);
    });
  }
}
