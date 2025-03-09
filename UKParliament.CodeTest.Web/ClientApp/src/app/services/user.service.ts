import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  currentUser: string | null = null;
  $currentUser = new BehaviorSubject<string | null>(null);

  public signOut() {
    this.currentUser = null;
    this.$currentUser.next(this.currentUser);
  }

  public signIn(login: string) {
    this.currentUser = login;
    this.$currentUser.next(this.currentUser);
  }

  public getUser() {
    return this.currentUser;
  }

}
