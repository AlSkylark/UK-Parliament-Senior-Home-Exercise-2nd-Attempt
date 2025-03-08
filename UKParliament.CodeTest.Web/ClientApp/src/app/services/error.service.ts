import { Injectable } from '@angular/core';
import { ValidationError } from '../models/errors/validation-error';
import { Subject } from 'rxjs';
import { ErrorBag } from '../models/errors/error-bag';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  constructor() { }

  currentErrors: ValidationError[] = [];
  errorsSubject = new Subject<ValidationError[]>();

  public displayErrors(errorBag: ErrorBag | null) {
    if (errorBag) {
      this.currentErrors = errorBag.errors;
      this.errorsSubject.next(errorBag.errors);
      return;
    }

    this.errorsSubject.next([]);
  }

  public resetErrors() {
    if (this.currentErrors.length > 0) {
      this.errorsSubject.next([]);
    }
  }

}
