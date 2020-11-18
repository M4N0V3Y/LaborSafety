import { AdministracaoPerfilModelo } from './AdministracaoPerfilModel';


export class PerfilFuncionalidade {
    Id: number;
    Funcionalidades?: AdministracaoPerfilModelo[];
    constructor(Id?: number) {
        this.Id = Id;
      }
  }
