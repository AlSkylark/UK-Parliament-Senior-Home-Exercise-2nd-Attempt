import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FilterSelectComponent } from '../filter-select/filter-select.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ErrorService } from 'src/app/services/error.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss'
})
export class DropdownComponent extends FilterSelectComponent implements OnDestroy {

  @Input({ required: true })
  label!: string;

  @Input()
  showLabel = true;

  @Input()
  disabled = false;

  lookupSubscription: Subscription | undefined;

  ngOnInit(): void {
    this.lookupSubscription = this.lookupService.lookUpItem(this.itemToLook).subscribe(item => {
      this.itemList = item;
      this.loading = false;
    })
    this.getLookupItems();
  }

  ngOnDestroy(): void {
    this.lookupSubscription?.unsubscribe();
  }

}
