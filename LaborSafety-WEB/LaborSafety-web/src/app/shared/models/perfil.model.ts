import { Tela } from './tela.model';

export class Perfil {
  CodPerfil: number;
  Nome: string;
  GrupoAD: string;
  TelaSelecionada: Tela;

  constructor(codPerfil: number, nome: string, grupoAd: string, lista: Tela) {
    this.CodPerfil = codPerfil;
    this.Nome = nome;
    this.GrupoAD = grupoAd;
    this.TelaSelecionada = lista;
  }
}
