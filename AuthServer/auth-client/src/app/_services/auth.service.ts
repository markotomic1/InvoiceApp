import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { AxiosService } from './axios.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.baseUrl;

  constructor(private axiosService: AxiosService) {}

  login(data: any, resultUrl: string) {
    return this.axiosService.instance.post<{ url: string; token: string }>(
      this.baseUrl + '/auth/login?returnUrl=' + resultUrl,
      data
    );
  }
  register(data: any, resultUrl: string) {
    return this.axiosService.instance.post<{ url: string; token: string }>(
      this.baseUrl + '/auth/register?returnUrl=' + resultUrl,
      data
    );
  }

  getAuthorized(url: string, token: string) {
    return this.axiosService.instance.get(url, {
      headers: { Authorization: 'Bearer ' + token },
    });
  }
}
