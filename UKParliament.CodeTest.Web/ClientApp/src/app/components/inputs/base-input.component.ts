import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ValidationError } from 'src/app/models/errors/validation-error';
import { ErrorService } from 'src/app/services/error.service';


@Component({
    template: ''
})
export abstract class BaseComponent<T> {
    @Input({ required: true })
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
    value?: T | null;

    @Output()
    valueChange = new EventEmitter<T>();

    abstract onInput(event: Event): void;

    validationErrors: ValidationError[] = [];
    validationMessages: string[] = [];

    constructor(protected errorService: ErrorService) {
        this.errorService.errorsSubject.subscribe(errors => {
            this.validationErrors = errors;
            this.isValid = !this.validationErrors
                .some(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase());
            this.validationMessages = this.validationErrors
                .filter(v => v.propertyName.toLocaleUpperCase() == this.id.toLocaleUpperCase())
                .map(v => v.errorMessage);
        });
    }
}