import { EPI } from './EPI.model';
import { Frequencia } from './frequencia.model';
import { Severidade } from './severidade.model';
import { Risco } from './risco.model';
import { ContraMedidaRisco } from './contramedidarisco.model';
export class RiscoMapeadoModelo {
  id?: string;
   Risco: Risco;
  Severidade: Severidade;
  Frequencia: Frequencia;
  FonteGeradora: string;
  Procedimentos: string;
  Grau: number;
  Contramedidas: string;
  EPI: EPI[];

  constructor(r: Risco, s: Severidade, f: Frequencia,
              fg: string, p: string, g: number, c: string, e: EPI[]) {
    this.Risco = r;
    this.Severidade = s;
    this.Frequencia = f;
    this.FonteGeradora = fg;
    this.Procedimentos = p;
    this.Grau = g;
    this.Contramedidas = c;
    this.EPI = e;
  }
  setRisco(risco: Risco) {
    this.Risco = risco;
  }
  setSeveridade(severidade: Severidade) {
    this.Severidade = severidade;
  }
  setFrequencia(frequencia: Frequencia) {
    this.Frequencia = frequencia;
  }
}
