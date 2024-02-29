import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {
  constructor(
    private oauthService: OAuthService,
    public authService: AuthService
  ) {}
  ngOnInit(): void {}

  logout() {
    this.authService.logout();
    this.oauthService.logOut();
  }
}
