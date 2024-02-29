import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();
  constructor(private oauthService: OAuthService) {}

  login() {
    this.isLoggedInSubject.next(true);
  }
  logout() {
    this.isLoggedInSubject.next(false);
  }

  getToken(): string | null {
    return this.oauthService.getAccessToken();
  }

  setToken(token: string): void {
    sessionStorage.setItem('access_token', token);
  }

  removeToken(): void {
    this.oauthService.logOut();
  }
}
