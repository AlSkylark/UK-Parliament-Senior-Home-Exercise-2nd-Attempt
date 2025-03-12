import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ValidationError } from 'src/app/models/errors/validation-error';
import { LookupItem } from 'src/app/models/lookup-item';
import { LookupItemsEnum } from 'src/app/models/lookup-items-enum';
import { ErrorService } from 'src/app/services/error.service';
import { LookupService } from 'src/app/services/lookup.service';

import { WarningsService } from 'src/app/services/warnings.service';
import { BaseComponent } from '../base-component.component';

@Component({
  selector: 'app-filter-select',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './filter-select.component.html',
  styleUrl: './filter-select.component.scss'
})
export class FilterSelectComponent extends BaseComponent implements OnInit, OnDestroy {
  @Input({ required: true })
  text!: string;

  @Input({ required: true })
  itemToLook!: LookupItemsEnum;

  itemList: LookupItem[] = [];

  @Input()
  value?: any;

  @Output()
  valueChange = new EventEmitter<any | undefined>();

  loading = false;

  lookUpSubscription: Subscription | undefined;

  constructor(protected lookupService: LookupService, protected errorService: ErrorService, private warningService: WarningsService) {
    super(errorService, warningService);
  }

  ngOnInit(): void {
    this.lookUpSubscription = this.lookupService.lookUpItem(this.itemToLook).subscribe(item => {
      this.itemList = item;
      this.loading = false;
    })
  }

  ngOnDestroy(): void {
    this.lookUpSubscription?.unsubscribe();
  }

  protected getLookupItems() {
    this.loading = this.lookupService.getLookupItems(this.itemToLook);
  }

  updateValue() {
    this.valueChange.next(this.value);
    this.errorService.resetErrors();
  }
}
