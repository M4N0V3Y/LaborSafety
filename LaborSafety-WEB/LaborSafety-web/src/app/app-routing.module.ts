import { NgModule } from '@angular/core';
import {
  RouterModule,
  Routes,
  PreloadAllModules
} from '@angular/router';
import { PageNotFoundComponent } from './shared/components/page-notfound/page-notfound.component';

const app_routes: Routes = [
  { path: 'auth', loadChildren: './modules/auth/auth.module#AuthModule' },
  { path: 'browser-error', loadChildren: './modules/auth/browser-error/browser-error.module#BrowserErrorModule'  },
  {
    path: 'client',
    loadChildren: './modules/client/client.module#ClientModule'
  },
  { path: '', pathMatch: 'full', redirectTo: '/auth/login' },
  { path: '**', redirectTo: 'notfound' },
  { path: 'notfound', component: PageNotFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(app_routes, {
      preloadingStrategy: PreloadAllModules,
      onSameUrlNavigation: 'reload'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
