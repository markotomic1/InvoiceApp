import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const oauth2Service = inject(OAuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);
  const authService = inject(AuthService);

  if (oauth2Service.hasValidAccessToken()) {
    authService.login();
    return true;
  } else {
    toastr.warning('Ova radnja nije moguca.', 'Niste prijavljeni!');
    router.navigateByUrl('/');
    return false;
  }
};
