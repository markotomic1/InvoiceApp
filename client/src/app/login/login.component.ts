import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthEvent, OAuthService } from 'angular-oauth2-oidc';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  private eventsSubscription: Subscription = new Subscription();
  constructor(
    private oauthService: OAuthService,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService
  ) {}
  ngOnDestroy(): void {
    this.eventsSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.eventsSubscription = this.oauthService.events.subscribe({
      next: (event: OAuthEvent) => {
        if (event.type !== 'token_received') return;
        if (this.oauthService.hasValidAccessToken()) {
          this.authService.login();
        }
      },
    });
  }

  login() {
    if (this.oauthService.hasValidAccessToken()) {
      this.toastr.info('Vec ste prijavljeni!');
      return;
    }
    this.oauthService.initCodeFlow();
  }

  navigateToHome() {
    this.router.navigateByUrl('/home');
  }
}
