import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { of as observableOf } from 'rxjs';

import { CookieService } from 'ngx-cookie-service';

import { environment } from '../../../environments/environment';

import { Login } from '../../shared/models/login.model';
import { AdministracaoPerfilModelo } from 'src/app/shared/models/AdministracaoPerfilModel';
import { ToasterService } from 'angular2-toaster';
import { Router } from '@angular/router';
const credentialsKey = 'currentUser';
const host = environment.host;
@Injectable()
export class AuthenticationService {
  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private toasterService: ToasterService,
    private router: Router) { }
  requiredPermissions = new Map();
  remember: boolean;


  async getPerfil(codPerfil) {
    const path = host + environment.apiPaths.permissaoPerfil.getPerfil(codPerfil);
    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });

  }
  async getListaTelasEFuncionalidadesPorPerfil(codPerfil) {
    const path = host + environment.apiPaths.permissaoPerfil.getListaTelasEFuncionalidadesPorPerfil(codPerfil);
    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  login(loginData: Login): Promise<any> {

    const path = `${environment.loginHost}${environment.apiPaths.login}`;
    const grantType = 'password';
    const username = loginData.id;
    const password = loginData.password;
    const model = {
      grant_type: grantType,
      username: username,
      password: password,
    };
   // const body = `grant_type=${grantType}&username=${username}&password=${password}`;
    const body = new HttpParams()
    .set('grant_type', grantType)
    .set('username', username)
    .set('password', password);

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      })
    };

    const tokenApp = this.getAppToken();
    if (tokenApp) {
      httpOptions.headers['Authorization-App'] = tokenApp;
    }
    return this.http
      .post(path, body, httpOptions)
      .toPromise()
      .then((login: any) => {
        this.storageInfo(loginData, null, login);
      });
  }

  logout(): Observable<boolean> {
    sessionStorage.removeItem(credentialsKey);
    localStorage.removeItem(credentialsKey);
    sessionStorage.removeItem('perfil');
    this.cookieService.deleteAll();

    return observableOf(true);
  }

  getUserInfo(): Observable<any> {
    const savedCredentials = this.getSavedCredentials();
    return observableOf(savedCredentials);
  }

  storageInfo(loginData, data, login) {
    this.remember = loginData.remember;
    const storage = loginData.remember ? localStorage : sessionStorage;
    const user = {
      role: 'client',
      user: loginData.id,
      permissions: data == null ? data : data.Data.funcionalidades,
      token: login.access_token,
      area: 1
    };
    storage.setItem(credentialsKey, JSON.stringify(user));
  }

  isLogin() {
    if (
      localStorage.getItem(credentialsKey) ||
      sessionStorage.getItem(credentialsKey)
    ) {
      return true;
    }
    return false;
  }

  getUser() {
    const savedCredentials = this.getSavedCredentials();
    return savedCredentials && savedCredentials.user;
  }

  getToken() {
    const savedCredentials = this.getSavedCredentials();
    return savedCredentials && savedCredentials.token;
  }

  getAppToken() {
    return '2BD7C39EE335AB8FC46737E7CC175';
  }

  getUserRole(): Observable<any> {
    const savedCredentials = this.getSavedCredentials();
    return observableOf(savedCredentials.role);
  }

  setArea(CodArea: number) {
    const savedCredentials = this.getSavedCredentials();
    savedCredentials.area = CodArea;
    const storage = this.remember ? localStorage : sessionStorage;
    storage.setItem(credentialsKey, JSON.stringify(savedCredentials));
  }

  getArea(): number {
    const savedCredentials = this.getSavedCredentials();
    return savedCredentials.area;
  }

  getUserType() {
    const savedCredentials = this.getSavedCredentials();
    if (this.isLogin()) {
      return savedCredentials.role;
    } else {
      return false;
    }
  }

  private getSavedCredentials() {
    const savedCredentials =
      sessionStorage.getItem(credentialsKey) ||
      localStorage.getItem(credentialsKey);
    return JSON.parse(savedCredentials);
  }

  getListOfPermissions() {
    const storage = JSON.parse(
      sessionStorage.getItem(credentialsKey) ||
      localStorage.getItem(credentialsKey)
    );
    if (storage) {
      return storage.permissions;
    } else {
      return undefined;
    }
  }

  getUrlRedirect(): string {
    const funcionalidades: AdministracaoPerfilModelo[] = this.getListOfPermissions();
    if (!funcionalidades || funcionalidades.length === 0) {
      this.toasterService.pop('error', 'Acesso não autorizado', 'Voce não possui acesso liberado a nenhuma funcionalidade.');
      this.router.navigateByUrl('/auth/login');
    }
    const funcRedirecionamento: number = funcionalidades[0].CodFuncionalidade;
    let returnUrl = '';
    this.getListOfRequiredPermissions().forEach((value, key) => {
      if (value === funcRedirecionamento) {

        returnUrl = key;
      }
    });

    return returnUrl;
  }

  hasPermissionInThisRoute(state) {
    if (this.getListOfRequiredPermissions().has(state.url)) {
      const requiredPermission = this.getListOfRequiredPermissions().get(state.url);
      if (this.getListOfPermissions().filter(x => x.CodFuncionalidade === requiredPermission).length > 0) {
        return true;
      } else {
        return false;
      }
    } else {
      const telaCadastro = new RegExp('/criar$').test(state.url);
      const urlBase = state.url.substring(0, state.url.lastIndexOf('/'));
      if (telaCadastro && this.getListOfRequiredPermissions().has(urlBase)) {
        const requiredPermission = this.getListOfRequiredPermissions().get(urlBase);
        if (this.getListOfPermissions().filter(x => x.CodFuncionalidade === requiredPermission && x.Edicao).length > 0) {
          return true;
        } else {
          return false;
        }
      } else {
        return true;
      }
    }
  }

  getListOfRequiredPermissions() {
    if (this.requiredPermissions && this.requiredPermissions.size > 0) {
      return this.requiredPermissions;
    } else {
      this.initPermissions();
      return this.requiredPermissions;
    }
  }

  initPermissions() {
    this.requiredPermissions = new Map();
    this.requiredPermissions.set('/client/dashboard', 1);
    this.requiredPermissions.set('/client/perfil', 2);
    this.requiredPermissions.set('/client/perfil/gestao', 2);
    this.requiredPermissions.set('/client/inventarioAtividade', 3);
    this.requiredPermissions.set('/client/inventarioAtividade/gestao', 3);
  }
  getPerfilCom8id(id) {
    const path = host + environment.apiPaths.permissaoPerfil.getPerfilCom8id(id);
    return this.http
    .post(path, id)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

/*  initPermissions() {
    this.requiredPermissions = new Map();
    this.requiredPermissions.set('/client/dashboard', 1);
    this.requiredPermissions.set('/client/perfil', 2);
    this.requiredPermissions.set('/client/perfil/gestao', 2);
    this.requiredPermissions.set('/client/veiculos', 12);
    this.requiredPermissions.set('/client/veiculos/listagem', 12);
    this.requiredPermissions.set('/client/macro-ponto', 3);
    this.requiredPermissions.set('/client/areas-responsaveis/listagem', 4);
    this.requiredPermissions.set('/client/areas-responsaveis', 4);
    this.requiredPermissions.set('/client/nao-conformidade', 5);
    this.requiredPermissions.set('/client/rotas', 6);
    this.requiredPermissions.set('/client/rotas/listagem', 6);
    this.requiredPermissions.set('/client/ponto-descarga', 7);
    this.requiredPermissions.set('/client/ponto-descarga/listagem', 7);
    this.requiredPermissions.set('/client/prioridades', 8);
    this.requiredPermissions.set('/client/tipo-movimento', 9);
    this.requiredPermissions.set('/client/coletas', 10);
    this.requiredPermissions.set('/client/coletas-pendentes', 11);
    this.requiredPermissions.set('/client/configuracao', 14);
    this.requiredPermissions.set('/client/usuarios', 16);
    this.requiredPermissions.set('/client/usuarios/listagem', 16);
    this.requiredPermissions.set('/client/tipo-veiculo', 15);
    this.requiredPermissions.set('/client/tipo-veiculo/listagem', 15);
  }*/
}
