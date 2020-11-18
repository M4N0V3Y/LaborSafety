import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Tela } from 'src/app/shared/models/tela.model';
const host = environment.host;
const homepageRoute = environment.apiPaths.homepage;

@Injectable({
  providedIn: 'root'
})
export class HomepageService {

  constructor(private http: HttpClient) { }
  async getAll(): Promise<Tela[]> {
    const path = host + homepageRoute.getAll;

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Tela[]))
      .toPromise();
    return data;
  }
  getListaTelasEFuncionalidadesPorPerfil(codPerfil) {
    const path = host + environment.apiPaths.permissaoPerfil.getListaTelasEFuncionalidadesPorPerfil(codPerfil);
    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
}
