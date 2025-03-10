import { Component, ElementRef, HostListener, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ThemeService } from 'src/app/services/theme.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnDestroy {

  @HostListener('document:click', ['$event'])
  clickout(event: Event) {
    if (!this.menuRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }

  @ViewChild("userMenu")
  menuRef!: ElementRef;

  isOpen = false;
  username: string;

  theme!: string;
  themeSubscription: Subscription;
  constructor(private userService: UserService, private themeService: ThemeService) {
    this.username = this.userService.getUser() ?? "No username";
    this.themeSubscription = this.themeService.$theme.subscribe(t => this.theme = t);
  }

  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }

  openDropdown() {
    this.isOpen = !this.isOpen;
  }

  signOut() {
    this.userService.signOut();
  }

  changeTheme() {
    this.themeService.changeTheme();
  }

}
