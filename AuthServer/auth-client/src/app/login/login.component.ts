import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.development';
import { AuthService } from '../_services/auth.service';
import { AxiosResponse } from 'axios';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  baseUrl = environment.baseUrl;
  actionUrl: string | undefined;
  constructor(
    private router: Router,
    private authService: AuthService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {}
  ngOnInit(): void {
    this.initializeLoginForm();
    const url = this.router.url.split('=')[1];
    this.actionUrl = url;
  }
  navigateToRegister() {
    if (!this.actionUrl) {
      this.toastr.error('Greska!');
      return;
    }
    this.router.navigateByUrl('/register?returnUrl=' + this.actionUrl);
  }

  initializeLoginForm() {
    this.loginForm = this.fb.group({
      email: new FormControl<string>('', {
        validators: [Validators.required, Validators.email],
      }),
      lozinka: new FormControl<string>('', {
        validators: [Validators.required],
      }),
    });
  }

  async login() {
    if (!this.actionUrl) {
      console.log('error no action url');
      this.toastr.error('Greska!');
      return;
    }

    if (!this.loginForm.valid) {
      this.toastr.error('Molimo vas unesite validne podatke!');
      return;
    }
    try {
      const formData = this.loginForm.value;

      //prijava sa kredencijalima
      const response = await this.authService.login(formData, this.actionUrl);

      //autorizacija i dobijanje access tokena
      const redirectResponse = await this.authService.getAuthorized(
        response.data.url,
        response.data.token
      );
      window.location.href = redirectResponse.data.url;
    } catch (error: any) {
      if (error?.response?.status === 400 || error?.response?.status === 401)
        this.toastr.error('Niste unijeli ispravne kredencijale');
      console.error(error);
    }
  }
}
