import { Component, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ValidationError } from 'src/app/models/errors/validation-error';
import { ErrorService } from 'src/app/services/error.service';
import { WarningsService } from 'src/app/services/warnings.service';

@Component({
  template: ''
})
export abstract class BaseComponent implements OnDestroy {
  @Input()
  label!: string;

  @Input({ required: true })
  id!: string;

  @Input()
  showLabel = true;

  @Input()
  autopopulate = "";

  @Input()
  placeholder = "";

  @Input()
  disabled = false;

  @Input()
  isValid = true;

  @Input()
  hasWarnings = false;

  validationErrors: ValidationError[] = [];
  validationMessages: string[] = [];

  warningErrors: ValidationError[] = [];
  warningMessages: string[] = [];

  errorSubscription: Subscription;
  warningsSubscription: Subscription;
  constructor(protected errorService: ErrorService, private warningsService: WarningsService) {
    this.errorSubscription = this.errorService.$errors.subscribe(errors => {
      this.validationErrors = errors;
      this.isValid = !this.validationErrors
        .some(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase());
      this.validationMessages = this.validationErrors
        .filter(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase())
        .map(v => v.errorMessage);
    });

    this.warningsSubscription = this.warningsService.$errors.subscribe(warnings => {
      this.warningErrors = warnings;
      this.hasWarnings = !!this.warningErrors
        .some(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase());
      this.warningMessages = this.warningErrors
        .filter(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase())
        .map(v => v.errorMessage);
    })
  }

  ngOnDestroy(): void {
    this.errorSubscription.unsubscribe();
  }
}
