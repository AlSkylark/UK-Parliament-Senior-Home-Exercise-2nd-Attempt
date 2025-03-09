import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EditorService {

  $editorOpen = new BehaviorSubject<boolean>(false);

  openEditor() {
    this.$editorOpen.next(true);
  }

  closeEditor() {
    this.$editorOpen.next(false);
  }
}
