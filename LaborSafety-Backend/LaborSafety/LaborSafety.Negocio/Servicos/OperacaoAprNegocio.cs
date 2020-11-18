using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class OperacaoAprNegocio : IOperacaoAprNegocio
    {
        private readonly IOperacaoAprPersistencia operacaoAprPersistencia;
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly IAtividadePadraoPersistencia atividadePadraoPersistencia;
        private readonly IDisciplinaPersistencia disciplinaPersistencia;

        public OperacaoAprNegocio(IOperacaoAprPersistencia operacaoAprPersistencia, ILocalInstalacaoPersistencia localInstalacaoPersistencia,
            IAtividadePadraoPersistencia atividadePadraoPersistencia, IDisciplinaPersistencia disciplinaPersistencia)
        {
            this.operacaoAprPersistencia = operacaoAprPersistencia;
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.atividadePadraoPersistencia = atividadePadraoPersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
        }

        public OperacaoAprModelo MapeamentoOperacao(OPERACAO_APR operacao)
        {
            OperacaoAprModelo operacaoModelo = new OperacaoAprModelo()
            {
                CodOperacaoAPR = operacao.CodOperacaoAPR,
                CodAPR = operacao.CodAPR,
                CodStatusAPR = operacao.CodStatusAPR,
                Codigo = operacao.Codigo,
                Descricao = operacao.Descricao,
                CodLI = operacao.CodLI,
                CodDisciplina = operacao.CodDisciplina,
                CodAtvPadrao = operacao.CodAtvPadrao
            };

            return operacaoModelo;
        }

        public IEnumerable<OperacaoAprModelo> ListarPorCodigos(List<long> codigos)
        {
            List<OperacaoAprModelo> operacoes = new List<OperacaoAprModelo>();

            IEnumerable<OPERACAO_APR> op = this.operacaoAprPersistencia.PesquisarPorId(codigos);

            if (op == null)
            {
                throw new KeyNotFoundException("Operação não encontrada.");
            }

            foreach (OPERACAO_APR item in op)
            {
                operacoes.Add(MapeamentoOperacao(item));
            }

            foreach (var item in operacoes)
            {
                var atividade = atividadePadraoPersistencia.ListarAtividadePorId((long)item.CodAtvPadrao);
                item.NomeAtvPadrao = atividade.Nome;

                var disciplina = disciplinaPersistencia.ListarDisciplinaPorId((long)item.CodDisciplina);
                item.NomeDisciplina = disciplina.Nome;

                var li = localInstalacaoPersistencia.ListarLocalInstalacaoPorId((long)item.CodLI);
                item.NomeLI = li.Nome;
            }

            return operacoes;
        }
    }
}
