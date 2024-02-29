import axios, { AxiosInstance, InternalAxiosRequestConfig } from 'axios';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AxiosService {
  private axiosInstance: AxiosInstance;

  constructor(
    private oauthService: OAuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.axiosInstance = axios.create({
      baseURL: environment.baseUrl,
    });

    this.axiosInstance.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        // provjera da li je token validan
        const accessToken = this.oauthService.getAccessToken();
        return this.ensureAccessTokenIsValid(accessToken)
          .then(() => {
            if (accessToken) {
              config.headers.Authorization = `Bearer ${accessToken}`;
            }

            return config;
          })
          .catch((error) => {
            console.error('Error ensuring access token is valid:', error);
            throw error;
          });
      },
      (error) => {
        return Promise.reject(error);
      }
    );
  }

  get instance() {
    return this.axiosInstance;
  }

  private async ensureAccessTokenIsValid(token: string) {
    const accessTokenExpiration = this.getExpirationDate(token);
    const currentTime = new Date();

    if (accessTokenExpiration && accessTokenExpiration < currentTime) {
      try {
        // await this.oauthService.refreshToken();
        this.oauthService.logOut();
        this.toastr.warning(
          'Molimo vas da se ponovo prijavite',
          'Sessija istekla'
        );
        this.router.navigateByUrl('/');
      } catch (error) {
        console.error('Error refreshing token:', error);
      }
    }
  }

  private getExpirationDate(token: string): Date | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      if (payload && payload.exp) {
        return new Date(payload.exp * 1000);
      }
    } catch (error) {
      console.error('Error parsing token payload:', error);
    }
    return null;
  }
}
