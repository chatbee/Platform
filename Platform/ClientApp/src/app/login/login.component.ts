import { map, take } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from './../core/services/auth.service';
import { Component, OnDestroy } from '@angular/core';
import { AuthenticationModel } from '../models/authenticationModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnDestroy {
  password: string;
  email: string;
  subscription: any;
  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}
  errorMessage = '';
  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
  redirect() {
    const returnUrl =
      this.route.snapshot.queryParamMap.get('returnUrl') || 'admin';
    return this.router.navigate([returnUrl]).catch(e => {
      console.error(e);
    });
  }

  login() {
    this.subscription = this.authService
      .login(new AuthenticationModel(this.email, this.password))
      .pipe(
        map(res => {
          if (res.token) {
            this.redirect();
          } else {
            this.errorMessage = res;
          }
          return res;
        }),
        take(1)
      )
      .subscribe(r => {});
  }
}
