import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
const host = environment.host;
const localInstalacaoRoute = environment.apiPaths.localInstalacao.filtrar;
@Injectable({
  providedIn: 'root'
})
export class ImportacaoService {

  constructor(private http: HttpClient) { }

  getLocalInstalacao(): Observable<any> {
    const path = `${host}${localInstalacaoRoute}`;

    return this.http.get(path);
  }
  getModeloImportacaoAmbiente() {
    const path = host + environment.apiPaths.importacao.getModeloAmbiente();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  getModeloImportacaoAtividade() {
    const path = host + environment.apiPaths.importacao.getModeloAtividade();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  public async inserirPlanilhaAmbiente(arquivo) {
    const path = host + environment.apiPaths.importacao.inserirPlanilhaAmbiente(arquivo);
    return this.http
    .put(path, arquivo)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  public async inserirPlanilhaAtividade(arquivo) {
    const path = host + environment.apiPaths.importacao.inserirPlanilhaAtividade(arquivo);
    return this.http
    .put(path, arquivo)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async listarTodasNRs() {
    const path = host + environment.apiPaths.invAmbiente.listarTodasNRs();

    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
 async getAllSeveridades() {
    const path = host + environment.apiPaths.importacao.getAllSeveridades();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });

  }
  async getAllProbabilidades() {
    const path = host + environment.apiPaths.importacao.getAllProbabilidades();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getAllRiscos() {
    const path = host + environment.apiPaths.importacao.getAllRiscos();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getAllSistemasOperacionais() {
    const path = host + environment.apiPaths.importacao.getAllSistemasOperacionais();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });

  }
  async getAllDisciplina() {
    const path = host + environment.apiPaths.importacao.getAllDisciplina();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getAllAtividadePadrao() {
    const path = host + environment.apiPaths.importacao.getAllAtividadePadrao();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getAllPesos() {
    const path = host + environment.apiPaths.importacao.getAllPesos();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });

  }
  async getAllPerfilCatalogo() {
    const path = host + environment.apiPaths.importacao.getAllPerfilCatalogo();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async exportarDadosAmbiente(filtros) {
    const path = host + environment.apiPaths.importacao.exportarDadosAmbiente(filtros);
    return this.http
    .put(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async exportarDadosAtividade(filtros) {
    const path = host + environment.apiPaths.importacao.exportarDadosAtividade(filtros);
    return this.http
    .put(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  async exportarDadosAPR(filtros) {
    const path = host + environment.apiPaths.importacao.exportarDadosAPR(filtros);
    return this.http
    .put(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }



  /*async enviarFiltrosAmbiente(filtros) {
    const path = host + environment.apiPaths.importacao.enviarFiltrosAmbiente(filtros);
    return this.http
    .post(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async enviarFiltrosAtividade(filtros) {
    const path = host + environment.apiPaths.importacao.enviarFiltrosAtividade(filtros);
    return this.http
    .post(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }*/
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

export class ArquivoModelo {

  public arquivoBase64: string;
  public EightIDUsuarioModificador: string;

  constructor(arquivoBase64: string, EightIDUsuarioModificador: string) {
    this.arquivoBase64 = arquivoBase64;
    this.EightIDUsuarioModificador = EightIDUsuarioModificador;
  }

}
