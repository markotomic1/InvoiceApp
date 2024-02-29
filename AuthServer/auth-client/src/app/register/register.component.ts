import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AxiosResponse } from 'axios';
import { AuthService } from '../_services/auth.service';
import { environment } from 'src/environments/environment.development';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup = new FormGroup({});
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

  navigateToLogin() {
    if (!this.actionUrl) {
      this.toastr.error('Greska!');
      return;
    }
    this.router.navigateByUrl('/login?returnUrl=' + this.actionUrl);
  }

  initializeLoginForm() {
    this.registerForm = this.fb.group({
      ime: new FormControl<string>('', { validators: [Validators.required] }),
      prezime: new FormControl<string>('', {
        validators: [Validators.required],
      }),
      brojTelefona: new FormControl<string>('', {
        validators: [Validators.required],
      }),
      email: new FormControl<string>('', {
        validators: [Validators.required, Validators.email],
      }),
      lozinka: new FormControl<string>('', {
        validators: [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(12),
        ],
      }),
      potvrdaLozinke: new FormControl<string>('', {
        validators: [Validators.required, this.matchValues('lozinka')],
      }),
    });

    this.registerForm.get('lozinka')?.valueChanges.subscribe({
      next: () =>
        this.registerForm.get('potvrdaLozinke')?.updateValueAndValidity(),
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : {
            noMatching: true,
          };
    };
  }

  async register() {
    if (!this.actionUrl) {
      this.toastr.error('Greska!');
      console.error('error no action url');
      return;
    }

    if (this.registerForm.get('potvrdaLozinke')?.hasError('noMatching')) {
      this.toastr.error('Lozinke moraju biti iste!');
      return;
    }
    if (!this.registerForm.valid) {
      this.toastr.error('Molimo vas unesite validne podatke!');
      return;
    }
    try {
      const formData = this.registerForm.value;
      const response = await this.authService.register(
        formData,
        this.actionUrl
      );

      // const redirectResponse = await this.authService.getAuthorized(
      //   response.data.url,
      //   response.data.token
      // );
      // window.location.href = redirectResponse.data.url;
      this.navigateToLogin();
    } catch (error) {
      console.log(error);
    }
  }
}
