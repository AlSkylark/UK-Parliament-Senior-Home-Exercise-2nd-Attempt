import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { DashboardComponent } from "../dashboard/dashboard.component";
import { LoginComponent } from "../login/login.component";
import { ThemeService } from 'src/app/services/theme.service';
import { SkipComponent } from "../skip/skip.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [DashboardComponent, LoginComponent, SkipComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent implements OnDestroy {

  currentUser: string | null = null;
  theme: string = "light";

  userSubscription: Subscription;
  themeSubscription: Subscription;
  constructor(private userService: UserService, private themeService: ThemeService) {
    this.userSubscription = this.userService.$currentUser.subscribe(u => this.currentUser = u);
    this.themeSubscription = this.themeService.$theme.subscribe(t => this.theme = t);
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.themeSubscription.unsubscribe();
  }

}
