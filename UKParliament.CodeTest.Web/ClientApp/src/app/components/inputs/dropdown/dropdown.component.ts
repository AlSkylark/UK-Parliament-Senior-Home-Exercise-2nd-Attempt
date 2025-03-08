import { Component, Input, OnInit } from '@angular/core';
import { FilterSelectComponent } from '../filter-select/filter-select.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss'
})
export class DropdownComponent extends FilterSelectComponent {
  @Input({ required: true })
  label!: string;

  @Input()
  showLabel = true;

  @Input()
  disabled = false;

  ngOnInit(): void {
    this.lookupService.lookUpItem(this.itemToLook).subscribe(item => {
      this.itemList = item;
      this.loading = false;
    })
    this.getLookupItems()
  }

}
