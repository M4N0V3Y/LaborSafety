using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IRascunhoAprPersistencia
    {
        void InserirRascunhoApr(RascunhoAprModelo rascunhoAprModelo);
        void Inserir(RascunhoAprModelo rascunhoAprModelo, DB_APRPTEntities entities);
    }
}
