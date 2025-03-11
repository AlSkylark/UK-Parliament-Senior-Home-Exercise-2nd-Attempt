import { Injectable } from '@angular/core';
import { ValidationError } from '../models/errors/validation-error';
import { Subject } from 'rxjs';
import { ErrorBag } from '../models/errors/error-bag';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  constructor() { }

  errors: ValidationError[] = [];
  $errors = new Subject<ValidationError[]>();

  public displayErrors(errorBag: ErrorBag | null | undefined) {
    if (errorBag) {
      this.errors = errorBag.errors;
      this.$errors.next(errorBag.errors);
      return;
    }

    this.$errors.next([]);
  }

  public resetErrors() {
    if (this.errors.length > 0) {
      this.$errors.next([]);
    }
  }

}
