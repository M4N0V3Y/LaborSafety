using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IOperacaoAprPersistencia
    {
        List<OPERACAO_APR> PesquisarPorId(List<long> idOperacao);
        void InsereOuEditaOperacaoAPR(long codAPR, OPERACAO_APR operacao, DB_APRPTEntities entities = null);

        void DesativarOperacoesDeApr(long codApr, DB_APRPTEntities entities = null);

    }
}
