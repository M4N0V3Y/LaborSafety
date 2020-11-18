using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class DuracaoNegocio : IDuracaoNegocio
    {
        private readonly IDuracaoPersistencia duracaoPersistencia;
        public DuracaoNegocio(IDuracaoPersistencia duracaoPersistencia)
        {
            this.duracaoPersistencia = duracaoPersistencia;
        }

        public DuracaoModelo MapeamentoDuracao(DURACAO duracao)
        {
            DuracaoModelo duracaoModelo = new DuracaoModelo()
            {
                CodDuracao = duracao.CodDuracao,
                Indice = duracao.Indice,
                Nome = duracao.Nome
            };

            return duracaoModelo;
        }

        public DuracaoModelo ListarDuracaoPorId(long id)
        {
            DURACAO dur = this.duracaoPersistencia.ListarDuracaoPorId(id);
            if (dur == null)
            {
                throw new KeyNotFoundException("Duração não encontrada.");
            }
            return MapeamentoDuracao(dur);
        }

        public DuracaoModelo ListarDuracaoPorIndice(int indice)
        {
            DURACAO dur = this.duracaoPersistencia.ListarDuracaoPorIndice(indice);
            if (dur == null)
            {
                throw new KeyNotFoundException("Duração não encontrada.");
            }
            return MapeamentoDuracao(dur);
        }

        public DuracaoModelo ListarDuracaoPorNome(string nome)
        {
            DURACAO dur = this.duracaoPersistencia.ListarDuracaoPorNome(nome);
            if (dur == null)
            {
                throw new KeyNotFoundException("Duração não encontrada.");
            }
            return MapeamentoDuracao(dur);
        }

        public IEnumerable<DuracaoModelo> ListarTodasDuracoes()
        {
            List<DuracaoModelo> duracaoModelo = new List<DuracaoModelo>();

            IEnumerable<DURACAO> duracoes = this.duracaoPersistencia.ListarTodasDuracoes();

            if (duracoes == null)
            {
                throw new KeyNotFoundException("Duração não encontrada.");
            }

            foreach (DURACAO duracao in duracoes)
            {
                duracaoModelo.Add(MapeamentoDuracao(duracao));
            }

            return duracaoModelo;
        }
    }
}
