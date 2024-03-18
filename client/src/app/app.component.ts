import { Component, OnInit } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';

const authConfig: AuthConfig = {
  issuer: 'http://localhost:5001',
  redirectUri: window.location.origin + '/',

  clientId: 'spa',

  responseType: 'code',

  scope: 'offline_access',

  showDebugInformation: true,
  logoutUrl: '/',
  requireHttps: false,
};

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private oauthService: OAuthService) {
    this.oauthService.configure(authConfig);
  }
  ngOnInit(): void {
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }
}
