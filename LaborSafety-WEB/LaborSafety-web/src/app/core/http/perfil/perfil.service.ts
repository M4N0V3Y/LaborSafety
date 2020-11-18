import { Perfil } from 'src/app/shared/models/perfil.model';
import { Tela } from 'src/app/shared/models/tela.model';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Funcionalidade } from 'src/app/shared/models/funcionalidades.model';
import { PerfilFuncionalidade} from 'src/app/shared/models/PerfilFuncionalidade.model';
import { AdministracaoPerfilModelo } from 'src/app/shared/models/AdministracaoPerfilModel';
import { of } from 'rxjs';
import { NONE_TYPE } from '@angular/compiler/src/output/output_ast';

const host = environment.host;

@Injectable({
  providedIn: 'root'
})
export class PerfilService {
  constructor(private http: HttpClient) {}

  async getPerfilFuncionalidade(id: number): Promise<AdministracaoPerfilModelo[]> {
    const path = host + environment.apiPaths.perfil_funcionalidades.get(id);
    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as AdministracaoPerfilModelo[]))
      .toPromise();
    return data;
  }
  async getPerfis(): Promise<Perfil[]> {
    const path = host + environment.apiPaths.perfil.getAll;
    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Perfil[]))
      .toPromise();
    return data;
  }

  async getListarTelaEFuncionalidadesPorPerfilETela(codPerfil: number, codTela: number): Promise<Tela> {
    const path = host + environment.apiPaths.perfil.getListarTelaEFuncionalidadesPorPerfilETela(codPerfil, codTela);
    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Tela))
      .toPromise();
    return data;
  }

  async getListarListaTelasEFuncionalidadesPorPerfil(codPerfil: number): Promise<Tela[]> {
    const path = host + environment.apiPaths.perfil.getListarListaTelasEFuncionalidadesPorPerfil(codPerfil);
    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Tela[]))
      .toPromise();
    return data;
  }

  async getFuncionalidades(): Promise<Funcionalidade[]> {
    const path = host + environment.apiPaths.funcionalidade.getAll;
    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Funcionalidade[]))
      .toPromise();
    return data;
  }


  async atualizarPerfil(perfil: any): Promise<boolean> {
    const path = host + environment.apiPaths.perfil.put(perfil);
    const data = await this.http
      .post(path, perfil)
      .pipe(map((res: any) => res.Data as boolean))
      .toPromise();
    return data;
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
}
