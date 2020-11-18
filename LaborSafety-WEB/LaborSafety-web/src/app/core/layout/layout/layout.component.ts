import { MatSidenav } from '@angular/material';
import {
  Component,
  OnDestroy,
  ChangeDetectionStrategy,
  ViewChild,
  OnInit,
  AfterViewInit
} from '@angular/core';
import { AuthenticationService } from '../../authentication/authentication.service';
import { MENU_ITEMS, MenuItem } from 'src/app/modules/client/client-pages-menu';
import { ActivatedRoute } from '@angular/router';
import { AdministracaoPerfilModelo } from 'src/app/shared/models/AdministracaoPerfilModel';
import { ScrollService } from '../../services/scroll.service';
import { MenuService } from '../../services/menu.service';
import { Tela } from 'src/app/shared/models/tela.model';


@Component({
  selector: 'app-layout',
  changeDetection: ChangeDetectionStrategy.Default,
  styleUrls: ['./layout.component.scss'],
  templateUrl: './layout.component.html'
})
export class LayoutComponent implements OnDestroy, OnInit, AfterViewInit {

  @ViewChild('snav') sideNav: MatSidenav;
  menu: MenuItem[] = MENU_ITEMS;
  menuComPermissoesDeTela: MenuItem[] = [];
  private alive = true;
  title = '';
  currentTheme: string;
  @ViewChild('scrollEl') scrollEl: any;
  permissions = Array<AdministracaoPerfilModelo>();
  constructor(
    private route: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private scrollService: ScrollService,
    private menuService: MenuService
  ) {
  }
 async ngOnInit() {
   this.menuComPermissoesDeTela.push(this.menu[0]);
   const telasComAcesso = [];
   await this.authenticationService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
    .then((lista: Tela[]) => {
      lista.forEach((telas) => {
        telasComAcesso.push(telas.CodTela);
      });
    });


   this.menuService.setCurrentMenuClick('');
   this.menu.map((itemSideMenu) => {
      if (telasComAcesso.includes(itemSideMenu.CodTela)) {
        this.menuComPermissoesDeTela.push(itemSideMenu);
      }

    });

   this.permissions = this.authenticationService.getListOfPermissions();
   this.title = this.route.snapshot.firstChild.data.title;
  }

  ngAfterViewInit(): void {
    this.scrollService.setScroll(this.scrollEl.elementRef);
  }
  verificaTela(menu: MenuItem, tela) {
    switch (menu.CodTela) {
      case 1:
        break;

    }
  }
  ngOnDestroy() {
    this.alive = false;
  }
  getPermissions() {
    if (this.permissions) {
      return this.permissions;
    } else {
      return [];
    }
  }

  shouldShow(requiredPermission) {
    return (this.getPermissions().filter(x => x.CodFuncionalidade === requiredPermission).length > 0);
  }

  menuClick(name: string) {
    this.menuService.setCurrentMenuClick(name);
  }
}
