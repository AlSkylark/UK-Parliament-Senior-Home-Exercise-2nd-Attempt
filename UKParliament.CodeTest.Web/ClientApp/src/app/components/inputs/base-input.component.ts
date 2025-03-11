import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { BaseComponent } from './base-component.component';


@Component({
    template: ''
})
export abstract class BaseInputComponent<T> extends BaseComponent {
    @Input()
    value?: T | null;

    @Output()
    valueChange = new EventEmitter<T>();

    abstract onInput(event: Event): void;
}