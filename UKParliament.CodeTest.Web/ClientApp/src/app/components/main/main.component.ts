import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { DashboardComponent } from "../dashboard/dashboard.component";
import { LoginComponent } from "../login/login.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [DashboardComponent, LoginComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent implements OnDestroy {

  currentUser: string | null = null;

  userSubscription: Subscription;
  constructor(private userService: UserService) {
    this.userSubscription = this.userService.$currentUser.subscribe(u => this.currentUser = u);
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

}
