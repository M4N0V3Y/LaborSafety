using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class AmbienteNegocio : IAmbienteNegocio
    {
        private readonly IAmbientePersistencia sistemaOperacionalPersistencia;
        private readonly IInventariosAmbientePersistencia inventariosAmbientePersistencia;
        public AmbienteNegocio(IAmbientePersistencia sistemaOperacionalPersistencia, IInventariosAmbientePersistencia inventariosAmbientePersistencia)
        {
            this.sistemaOperacionalPersistencia = sistemaOperacionalPersistencia;
            this.inventariosAmbientePersistencia = inventariosAmbientePersistencia;
        }

        public AmbienteModelo MapeamentoSistemaOperacional(AMBIENTE sistema)
        {
            AmbienteModelo sistemaOperacional = new AmbienteModelo()
            {
                CodAmbiente = sistema.CodAmbiente,
                Nome = sistema.Nome,
                Descricao = sistema.Descricao
            };

            return sistemaOperacional;
        }

        public AmbienteModelo ListarSistemaOperacionalPorId(long id)
        {
            AMBIENTE sis = this.sistemaOperacionalPersistencia.ListarSistemaOperacionalPorId(id);
            if (sis == null)
            {
                throw new KeyNotFoundException("Ambiente não encontrado.");
            }
            return MapeamentoSistemaOperacional(sis);
        }

        public AmbienteModelo ListarSistemaOperacionalPorNome(string nome)
        {
            AMBIENTE sis = this.sistemaOperacionalPersistencia.ListarSistemaOperacionalPorNome(nome);
            if (sis == null)
            {
                throw new KeyNotFoundException("Ambiente não encontrado.");
            }
            return MapeamentoSistemaOperacional(sis);
        }

        public IEnumerable<AmbienteModelo> ListarTodosSOs()
        {
            List<AmbienteModelo> sistemaOperacionalModelo = new List<AmbienteModelo>();

            IEnumerable<AMBIENTE> sistemas = this.sistemaOperacionalPersistencia.ListarTodosSOs();

            if (sistemas == null)
            {
                throw new KeyNotFoundException("Ambiente não encontrado.");
            }

            foreach (AMBIENTE sis in sistemas)
            {
                sistemaOperacionalModelo.Add(MapeamentoSistemaOperacional(sis));
            }

            return sistemaOperacionalModelo;
        }

        public AmbienteModelo Inserir(AmbienteModelo ambienteModelo)
        {
            var resultado = this.sistemaOperacionalPersistencia.Inserir(ambienteModelo);

            AmbienteModelo ambienteModeloNovo = new AmbienteModelo();

            ambienteModeloNovo.CodAmbiente = resultado.CodAmbiente;
            ambienteModeloNovo.Descricao = resultado.Descricao;
            ambienteModeloNovo.Nome = resultado.Nome;

            return ambienteModeloNovo;
        }

        public AmbienteModelo Editar(AmbienteModelo ambienteModelo)
        {
            var ambienteExistente = sistemaOperacionalPersistencia.ListarAmbienteSemInventarioAmbienteVinculado(ambienteModelo.CodAmbiente);

            if (ambienteExistente == null)
                throw new Exception("Este ambiente possui algum inventário de ambiente vinculado!");

            var resultado = this.sistemaOperacionalPersistencia.Editar(ambienteModelo);

            AmbienteModelo ambienteModeloNovo = new AmbienteModelo();

            ambienteModeloNovo.CodAmbiente = resultado.CodAmbiente;
            ambienteModeloNovo.Descricao = resultado.Descricao;
            ambienteModeloNovo.Nome = resultado.Nome;

            return ambienteModeloNovo;
        }

        public void DesativarAmbiente(long codAmbienteExistente)
        {
            if (codAmbienteExistente <= 1)
                throw new Exception("Não é possível deletar um ambiente de código menor ou igual a um.");

            var invAmbiente = this.inventariosAmbientePersistencia.ListarInventarioAmbientePorIdAmbiente(codAmbienteExistente);

            if (invAmbiente != null)
                throw new Exception($"Não é possível excluir o ambiente informado pois o mesmo possui o inventário de ambiente {invAmbiente.Codigo} associado.");

            this.sistemaOperacionalPersistencia.DesativarAmbiente(codAmbienteExistente);
        }

    }
}
