import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface Alert {
  message: string,
  isError: boolean
}

@Injectable({
  providedIn: 'root'
})
export class EditorAlertService {

  $alerts = new Subject<Alert>();

  sendAlert(message: string, isError = false) {
    this.$alerts.next({ message, isError });
  }
}
