import { Component, OnDestroy } from '@angular/core';
import { HeaderComponent } from "../header/header.component";
import { SearchComponent } from "../search/search.component";
import { FooterComponent } from "../footer/footer.component";
import { EditorComponent } from "../editor/editor.component";
import { ResultListComponent } from "../result-list/result-list.component";
import { CommonModule } from '@angular/common';
import { EditorService } from 'src/app/services/editor.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HeaderComponent, SearchComponent, FooterComponent, EditorComponent, ResultListComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnDestroy {
  editorOpen = false;

  editorSubscription: Subscription;
  constructor(private editorService: EditorService) {
    this.editorSubscription = this.editorService.$editorOpen.subscribe(val => this.editorOpen = val);
  }

  ngOnDestroy(): void {
    this.editorSubscription.unsubscribe();
  }


}
