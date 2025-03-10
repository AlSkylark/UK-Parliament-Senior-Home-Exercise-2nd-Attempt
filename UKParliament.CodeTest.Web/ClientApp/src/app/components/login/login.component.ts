import { Component } from '@angular/core';
import { TextboxComponent } from "../inputs/textbox/textbox.component";
import { ButtonComponent } from "../inputs/button/button.component";
import { UserService } from 'src/app/services/user.service';
import { ErrorService } from 'src/app/services/error.service';
import { ErrorBag } from 'src/app/models/errors/error-bag';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [TextboxComponent, ButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  username = "";

  constructor(private userService: UserService, private errorService: ErrorService) { }

  logIn(e: Event) {
    e.preventDefault();
    if (this.username.length === 0) {
      const errors: ErrorBag = {
        errors: [{ propertyName: "login", errorMessage: "Login has to be at least 1 character long.", attemptedValue: "", customState: "", errorCode: "", severity: 1 }],
        isValid: false
      }
      this.errorService.displayErrors(errors);
    }

    this.userService.signIn(this.username);
  }
}
