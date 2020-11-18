import {
  Injectable
} from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild
} from '@angular/router';

import {
  AuthenticationService
} from '../authentication/authentication.service';
import {
  ToasterService
} from 'angular2-toaster';
import { AdministracaoPerfilModelo } from 'src/app/shared/models/AdministracaoPerfilModel';

@Injectable()
export class ClientAuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private toasterService: ToasterService,
    private authenticationService: AuthenticationService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.chekUser(route, state);
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.chekUser(route, state);
  }

  private chekUser(route, state): boolean {
    const userType = this.authenticationService.getUserType();
    const isLogin = this.authenticationService.isLogin();
    const hasPermissionInThisRoute = this.authenticationService.hasPermissionInThisRoute(state);
    if (userType === 'client' && isLogin && hasPermissionInThisRoute) {
      return true;
    } else if (isLogin) {
      this.toasterService.pop('error', 'Acesso não autorizado', 'Voce não possui acesso a esta funcionalidade.');

      const funcionalidades: AdministracaoPerfilModelo[] = this.authenticationService.getListOfPermissions();
      if (funcionalidades.length === 0) {
        this.toasterService.pop('error', 'Acesso não autorizado', 'Voce não possui acesso liberado a nenhuma funcionalidade.');
        this.router.navigateByUrl('/auth/login');
      }

      const funcRedirecionamento: number = funcionalidades[0].CodFuncionalidade;
      let returnUrl = '';
      this.authenticationService.getListOfRequiredPermissions().forEach((value, key) => {
        if (value === funcRedirecionamento) {
          returnUrl = key;
        }
      });

      this.router.navigate([returnUrl]);
      return false;
    } else {
      this.router.navigate(['/auth/login'], {
        queryParams: {
          returnUrl: state.url
        }
      });
      return false;
    }
  }
}
