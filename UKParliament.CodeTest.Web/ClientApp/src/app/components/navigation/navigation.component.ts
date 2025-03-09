import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent {



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
  constructor(private userService: UserService) {
    this.username = this.userService.getUser() ?? "No username";
  }

  openDropdown() {
    this.isOpen = !this.isOpen;
  }

  signOut() {
    this.userService.signOut();
  }

}
