import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ToasterService } from 'angular2-toaster';

import { AuthenticationService } from '../../authentication/authentication.service';

import { appToaster } from '../../../configs/app-toaster.config';
import { MatSidenav } from '@angular/material';

@Component({
  selector: 'app-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  @Input() snav: MatSidenav;
  @Input() title: string;

  user: any;
  tipoUser: any;
  textoToolTip: any;
  appSettings: any;
  userMenu = [{ title: 'Profile' }, { title: 'Log out' }];

  constructor(
    private router: Router,

    private toasterService: ToasterService,
    private authenticationService: AuthenticationService,
  ) {
  }

  async ngOnInit() {
    this.tipoUser =  await this.authenticationService.getPerfil(window.sessionStorage.getItem('perfil'));
    this.textoToolTip = 'Server name: ' + this.tipoUser.ServerName + '\n' + ' DataBase Name: ' + this.tipoUser.DatabaseName;
    this.user = this.authenticationService.getUser();
  }

  onContecxtItemSelection(title) {
    if (title === 'Log out') {
      this.logout();
    }
  }

  toggleSidebar(): boolean {
    return false;
  }

  goToHome() { }



  logout() {
    this.authenticationService.logout().subscribe((res) => {
      const logOutURL = '/auth/login';
      this.router.navigate([logOutURL], { replaceUrl: true });
      this.toasterService.pop('success', appToaster.successHead, appToaster.logoutSucess);
    });
  }

  snavToggle() {
    this.snav.toggle();
  }
}
