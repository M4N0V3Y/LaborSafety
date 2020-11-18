using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class OperacaoAprPersistencia : IOperacaoAprPersistencia
    {
        public List<OPERACAO_APR> PesquisarPorId(List<long> idOperacao)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.OPERACAO_APR.Where(p => idOperacao.Contains(p.CodOperacaoAPR)).ToList();

                return resultado;
            }
        }

        public void InsereOuEditaOperacaoAPR(long codAPR, OPERACAO_APR operacao, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            if(operacao == null)
                throw new Exception($"A OPERACAO_APR não foi informada!");

            APR aprExistente = entities.APR.Where(x => x.CodAPR == codAPR).FirstOrDefault();
            if (aprExistente == null)
                throw new Exception($"Não foi encontrada, no sistema, uma APR com o código {codAPR} informado");

            OPERACAO_APR operacaoExistente = entities.OPERACAO_APR.Where(x => x.CodOperacaoAPR == operacao.CodOperacaoAPR).FirstOrDefault();

            if (operacaoExistente == null)
                aprExistente.OPERACAO_APR.Add(operacao);
            else
            {
                operacaoExistente.CodAPR = operacao.CodAPR;
                operacaoExistente.CodAtvPadrao = operacao.CodAtvPadrao;
                operacaoExistente.CodDisciplina = operacao.CodDisciplina;
                operacaoExistente.Codigo = operacao.Codigo;
                operacaoExistente.CodLI = operacao.CodLI;
                operacaoExistente.CodStatusAPR = operacao.CodStatusAPR;
                operacaoExistente.Descricao = operacao.Descricao;
                operacaoExistente.STATUS_APR = operacao.STATUS_APR;
            }

            entities.SaveChanges();
        }

        public void DesativarOperacoesDeApr(long codApr, DB_LaborSafetyEntities entities = 
            null)
        {
            if (entities == null)
            {
                entities = new DB_LaborSafetyEntities();
            }
            var operacoesApr = entities.OPERACAO_APR.Where(x => x.CodAPR == codApr && x.Ativo).ToList();

            if (operacoesApr == null)
            {
                throw new Exception($"Não foi encontrada no sistema, nenhuma Operação de APR com código de APR {codApr}");

            }

            foreach (var operacao in operacoesApr)
            {
                operacao.Ativo = false;
            }
            entities.SaveChanges();
        }

    }
}
