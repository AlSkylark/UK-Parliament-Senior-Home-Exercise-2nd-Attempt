import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  theme: string;
  $theme: BehaviorSubject<string>;

  constructor() {
    const darkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    this.theme = darkMode ? "dark" : "light";
    this.$theme = new BehaviorSubject<string>(this.theme);
  }

  changeTheme() {
    this.theme = this.theme === "dark" ? "light" : "dark";
    this.$theme.next(this.theme);
  }
}
