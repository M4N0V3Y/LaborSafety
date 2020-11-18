import { Injectable, Injector } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from 'src/app/core/authentication/authentication.service';

@Injectable()
export class HttpTokenInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const headersConfig = {};

    const token = this.authenticationService.getToken();
    if (token) {
      headersConfig['Authorization'] = `Bearer ${token}`;
    }

    const tokenApp = this.authenticationService.getAppToken();
    if (tokenApp) {
      headersConfig['Authorization-App'] = tokenApp;
    }

    const request = req.clone({ setHeaders: headersConfig });
    return next.handle(request);
  }
}
