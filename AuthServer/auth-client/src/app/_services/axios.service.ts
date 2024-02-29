import axios, { AxiosInstance, AxiosResponse } from 'axios';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AxiosService {
  private axiosInstance: AxiosInstance;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: environment.baseUrl,
    });
  }

  get instance() {
    return this.axiosInstance;
  }
}
