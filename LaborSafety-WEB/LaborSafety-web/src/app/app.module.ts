import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { ToasterModule } from 'angular2-toaster';
import { CookieService } from 'ngx-cookie-service';
import { LoaderService } from './core/services/loader.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoaderComponent } from './shared/components/loader/loader.component';
import { MatProgressSpinnerModule, MatDialogModule } from '@angular/material';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LoginModalModule } from './modules/auth/login/login-modal/login-modal.module';
import { LoginInternalModule } from './modules/auth/login/login-internal/login-internal.module';
import { APP_BASE_HREF } from '@angular/common';
import { PageNotFoundComponent } from './shared/components/page-notfound/page-notfound.component';

@NgModule({
  declarations: [AppComponent, LoaderComponent, PageNotFoundComponent],
  exports: [LoaderComponent, PageNotFoundComponent],
  imports: [
    BrowserModule,
    CoreModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    FlexLayoutModule,
    ToasterModule.forRoot(),
    LoginModalModule,
    LoginInternalModule,
    MatDialogModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true
    },
    CookieService,
    LoaderService,
    { provide: APP_BASE_HREF, useValue: window['base-href'] }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
