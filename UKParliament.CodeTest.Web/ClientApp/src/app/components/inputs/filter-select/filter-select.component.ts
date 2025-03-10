import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ValidationError } from 'src/app/models/errors/validation-error';
import { LookupItem } from 'src/app/models/lookup-item';
import { LookupItemsEnum } from 'src/app/models/lookup-items-enum';
import { ErrorService } from 'src/app/services/error.service';
import { LookupService } from 'src/app/services/lookup.service';

@Component({
  selector: 'app-filter-select',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './filter-select.component.html',
  styleUrl: './filter-select.component.scss'
})
export class FilterSelectComponent implements OnInit, OnDestroy {
  @Input({ required: true })
  text!: string;

  @Input({ required: true })
  id!: string;

  @Input({ required: true })
  itemToLook!: LookupItemsEnum;

  @Input()
  isValid = true;

  itemList: LookupItem[] = [];

  @Input()
  value?: string;

  @Output()
  valueChange = new EventEmitter<string | undefined>();

  loading = false;

  validationErrors: ValidationError[] = [];
  validationMessages: string[] = [];

  errorSubscription: Subscription;
  lookUpSubscription: Subscription | undefined;

  constructor(protected lookupService: LookupService, private errorService: ErrorService) {
    this.errorSubscription = this.errorService.$errors.subscribe(errors => {
      this.validationErrors = errors;
      this.isValid = !this.validationErrors.some(v => v.propertyName == this.id);
      this.validationMessages = this.validationErrors.filter(v => v.propertyName == this.id).map(v => v.errorMessage);
    });
  }

  ngOnInit(): void {
    this.lookUpSubscription = this.lookupService.lookUpItem(this.itemToLook).subscribe(item => {
      this.itemList = item;
      this.loading = false;
    })
  }

  ngOnDestroy(): void {
    this.errorSubscription.unsubscribe();
    this.lookUpSubscription?.unsubscribe();
  }

  getLookupItems() {
    this.loading = this.lookupService.getLookupItems(this.itemToLook);
  }

  updateValue() {
    this.valueChange.next(this.value);
    this.errorService.resetErrors();
  }
}
