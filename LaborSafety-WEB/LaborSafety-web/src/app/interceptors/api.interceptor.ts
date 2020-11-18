import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';

import { Observable, throwError as observableThrowError, of } from 'rxjs';
import { catchError, tap, } from 'rxjs/operators';
import { ToasterService } from 'angular2-toaster';
import { appToaster } from 'src/app/configs/app-toaster.config';
import { Injectable } from '@angular/core';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { LoaderService } from 'src/app/core/services/loader.service';
import { MatDialog } from '@angular/material';
import { LoginModalComponent } from '../modules/auth/login/login-modal/login-modal.component';
import { MenuService } from '../core/services/menu.service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  isOpen = false;

  constructor(
    private toasterService: ToasterService,
    private authService: AuthenticationService,
    public loaderService: LoaderService,
    public dialogRef: MatDialog,
    private menuService: MenuService
  ) {
    this.isOpen = false;
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!req.url.includes('ListarPorNivel?filtro=')) {
      this.loaderService.show();
    }

    let httpParameters: any = {};
    httpParameters = {
      setHeaders: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
        'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, PUT',
      }
    };

    const request = req.clone(httpParameters);

    return next.handle(request).pipe(
      tap(evt => {
        if (evt instanceof HttpResponse) {
          if (evt.status === 200) {

            if (req.method !== 'GET') {
              let tittle = evt.body.tittle
                ? evt.body.tittle
                : appToaster.successHead;
              if (req.url.includes('Login')) {
                tittle = appToaster.loginSucess;
              }
              if ((evt.body.errorMessage === 'Erro ao inserir Inventário de Ambiente') ||
              (evt.url.includes('InventarioAmbiente/Inserir') && evt.body.Data.status === false)) {
                if ((evt.body.errorMessage === 'Erro ao inserir Inventário de Ambiente')) {
                  this.toasterService.pop('error', 'Erro', evt.body.errorMessage);
                }
              } else if (evt.url.includes('ImportarPlanilha') && evt.body.Data.status === false) {
                  this.toasterService.pop('error', 'Erro', '');
              } else if (!evt.url.includes('LocalInstalacao/ListarPorNivel')
                  && !evt.url.includes('InventarioAtividade/CalcularRiscoTotalTela')
                  && !evt.url.includes('InventarioAmbiente/CalcularRiscoTotalTela') &&
                  !evt.url.includes('Pessoa/ListarPorCodigos') &&
                  !evt.url.includes('OperacaoApr/ListarPorId')) {
                    this.toasterService.pop('success', tittle, evt.body.Message);
              }
            }
          }

          this.loaderService.hide();
        }

        return evt;
      }),
      catchError((err: any) => {
        let httpResponse = false;
        if (err instanceof HttpErrorResponse) {
          httpResponse = true;
        }

        this.loaderService.hide();
        this.handleAuthError(err, req, httpResponse);

        return of(err);
      })
    );
  }

  handleAuthError(err: HttpErrorResponse, req: HttpRequest<any>, httpResponse = true) {
    let msgInterceptor: string;
    if (httpResponse) {
      switch (err.status) {
        case 200:
          msgInterceptor = 'Operação realizada com sucesso';
          break;
        case 401:
          localStorage.clear();
          this.loginModal();
          msgInterceptor = 'Favor efetuar o login para continuar';
          break;
        case 403:
          msgInterceptor = 'Você não possui permissão para executar esta ação';
          this.authService.logout();
          this.loginModal();
          break;
        case 404:
          msgInterceptor = 'Dados não encontrados';
          break;
        case 500:
          msgInterceptor = 'Erro no servidor';
          break;
        case 501:
          msgInterceptor = 'Erro no servidor';
          break;
        default:
          msgInterceptor = 'Ocorreu um erro.';
          break;
      }
      let tittleError: any;
      let msgError: any;


      if (req.url.includes('login')) {
        if (!err.error.errorMessage && err.error.error && err.error.error !== '') {
          const errorParse = JSON.parse(err.error.error);
          if (errorParse && errorParse.errorMessage) {
            err.error.errorMessage = errorParse.errorMessage;
            err.error.errorDetails = errorParse.errorDetails;
          }
        }

        tittleError = 'Erro ao tentar Logar.';
      } else {
        tittleError = err.error.errorMessage
          ? err.error.errorMessage
          : (err.error.Message ? err.error.Message : appToaster.errorHead);
      }

      msgError = err.error.errorDetails
        ? err.error.errorDetails
        : msgInterceptor;

      this.toasterService.pop('error', tittleError, msgError);
    } else {
      this.toasterService.pop('error', '', err.message);
    }

    throw Error;
  }

  loginModal() {
    if (this.isOpen) {
      return;
    }

    this.isOpen = true;
    const dialog = this.dialogRef.open(LoginModalComponent);
    dialog.afterClosed().subscribe(x => {
      this.isOpen = false;
      if (this.menuService.verifyMenuChange()) {
        window.location.reload();
      }
    });
  }
}
