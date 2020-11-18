import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
const host = environment.host;
@Injectable({
  providedIn: 'root'
})

export class AprServiceService {

  constructor(private http: HttpClient) { }

  async gerarLog(ids) {
    const path = host + environment.apiPaths.apr.gerarLog(ids);

    return this.http
    .put(path, ids)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  async getBase64(numeroDeSerie) {
    const path = host + environment.apiPaths.apr.getBase64(numeroDeSerie);
    return this.http
    .put(path, numeroDeSerie)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async deletePessoa(id) {
    const path = host + environment.apiPaths.pessoas.deletePessoa(id);
    return this.http
    .delete(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  async editarPessoa(pessoa) {
    const path = host + environment.apiPaths.pessoas.editarPessoa(pessoa);
    return this.http
    .put(path, pessoa)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  async listarPessoaPorCPF(CPF) {
    const path = host + environment.apiPaths.pessoas.listarPessoaPorCPF(CPF);
    return this.http
    .get(path)
    .pipe()
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
  async inserirPessoa(pessoa) {
    const path = host + environment.apiPaths.pessoas.inserirPessoa(pessoa);
    return this.http
    .put(path, pessoa)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async calcularRiscoGeral(modelCalculo) {
    const path = host + environment.apiPaths.apr.calcularRiscoGeral(modelCalculo);
    return this.http
    .post(path, modelCalculo)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async gerarTodosLogs(){
    const path = host + environment.apiPaths.apr.getAllLogs();
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data; 
      });
  }
  async getModeloAPR() {
    const path = host + environment.apiPaths.apr.getModeloAPR();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }


  async verificaOrdemManutencao(ordemManutencao,codApr) {
    const path = host + environment.apiPaths.apr.verificaOrdemManutencao(ordemManutencao,codApr);
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async inserirAPR(APRModelo) {
    const path = host + environment.apiPaths.apr.inserirAPR(APRModelo);
    return this.http
    .put(path, APRModelo)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async editarAPR(APRModelo) {
    const path = host + environment.apiPaths.apr.editarAPR(APRModelo);
    return this.http
    .put(path, APRModelo)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async listarAPR(FiltroAPR) {
    const path = host + environment.apiPaths.apr.listarAPR(FiltroAPR);
    return this.http
    .post(path, FiltroAPR)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getAPRPorID(id) {
    const path = host + environment.apiPaths.apr.getAPRPorId(id);
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getOperacoesPorIDs(ids) {
    const path = host + environment.apiPaths.operacao.getOperacoesPorIDs(ids);
    return this.http
    .post(path, ids)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async getPessoasPorIDs(ids) {
    const path = host + environment.apiPaths.pessoas.getPessoasPorIds(ids);
    return this.http
    .post(path, ids)
    .pipe()
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
  async getAllPessoas() {
    const path = host + environment.apiPaths.pessoas.getAllPessoas();
    return this.http
    .get(path)
    .pipe()
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

}
