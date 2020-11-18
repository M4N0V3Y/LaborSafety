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
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos
{
    public class PerfisNegocio : IPerfisNegocio
    {
        private readonly IPerfisPersistencia perfisPersistencia;
        private readonly ILogPerfilFuncionalidadePersistencia logPerfilFuncionalidadePersistencia;

        public PerfisNegocio(IPerfisPersistencia perfisPersistencia, Validador<PerfilFuncionalidadeModelo> validadorPerfilFuncionalidade,
             ILogPerfilFuncionalidadePersistencia logPerfilFuncionalidadePersistencia = null)
        {
            this.perfisPersistencia = perfisPersistencia;
            this.logPerfilFuncionalidadePersistencia = logPerfilFuncionalidadePersistencia;
        }
        public IEnumerable<PerfilModelo> ListarTodosOsTiposPerfis()
        {
            IEnumerable<PerfilModelo> listaPerfisRetorno = new List<PerfilModelo>();
            // Mapeia de TiposPerfis para o objeto de destino listaPerfisRetorno
            AutoMapper.Mapper.Map(this.perfisPersistencia.ListarTodosOsTiposPerfis(), listaPerfisRetorno);
            return listaPerfisRetorno;
        }

        public PerfilModelo ListarPerfilPorId(long id)
        {
            PerfilModelo perfil = new PerfilModelo();
            
            // Mapeia de TiposPerfis para o objeto de destino listaPerfisRetorno
            AutoMapper.Mapper.Map(this.perfisPersistencia.ListarTipoPerfil(id), perfil);

            // Busca o nome do servidor
            perfil.ServerName = Environment.MachineName;

            //Busca o nome do banco
            using (var entities = new DB_APRPTEntities())
            {
                perfil.DatabaseName = entities.Database.Connection.DataSource;
            }

            return perfil;
        }

        public IEnumerable<PerfilModelo> ObterPerfisPor8ID(string id)
        {
            IEnumerable<PerfilModelo> tiposPerfisModeloRetorno = new List<PerfilModelo>();
            // Mapeia de TiposPerfis para o objeto de destino tiposPerfisModelos
            AutoMapper.Mapper.Map(this.perfisPersistencia.ObterPerfisPor8ID(id), tiposPerfisModeloRetorno);

            foreach (var itemTipoPerfil in tiposPerfisModeloRetorno)
            {
                if (itemTipoPerfil.Nome != Constantes.Perfil.Administrador && itemTipoPerfil.Nome != Constantes.Perfil.Master && itemTipoPerfil.Nome != Constantes.Perfil.Cadastro
                    && itemTipoPerfil.Nome != Constantes.Perfil.Executor && itemTipoPerfil.Nome != Constantes.Perfil.Consulta)
                    throw new Exception("O usuário não possui perfil válido!");
            }

            return tiposPerfisModeloRetorno;
        }

        public List<TelaModelo> ListarListaTelasEFuncionalidadesPorPerfil(long codPerfil)
        {
            List<TelaModelo> funcionalidadesTelaModelos = new List<TelaModelo>();
            AutoMapper.Mapper.Map(this.perfisPersistencia.ListarListaTelasEFuncionalidadesPorPerfil(codPerfil),funcionalidadesTelaModelos);
            return funcionalidadesTelaModelos;
        }

        public TelaModelo ListarTelaEFuncionalidadesPorPerfilETela(long codPerfil, long codTela)
        {
            TelaModelo telaModelo = new TelaModelo();
            telaModelo = this.perfisPersistencia.ListarTelaEFuncionalidadesPorPerfilETela(codPerfil, codTela);
            return telaModelo;
        }

        public bool Insercao(PerfilFuncionalidadeModelo perfilFuncionalidadeModelo)
        {
            try
            {
                if (perfilFuncionalidadeModelo.Funcionalidades == null)
                    throw new Exception("É necessário informar as funcionalidades!");

                if (string.IsNullOrEmpty(perfilFuncionalidadeModelo.eightIDUsuarioModificador))
                    throw new Exception("O usuário modificador não foi informado!");

                //Registra o LOG
                /*
                List<FUNCIONALIDADE> funcionalidades = new List<FUNCIONALIDADE>();
                logPerfilFuncionalidadePersistencia.BuscaEInsereFuncionalidades(perfilFuncionalidadeModelo.eightIDUsuarioModificador, 
                    perfilFuncionalidadeModelo.CodPerfil,perfilFuncionalidadeModelo.CodTela, 
                    AutoMapper.Mapper.Map(perfilFuncionalidadeModelo.Funcionalidades, funcionalidades));
                    */

                bool result = false;
                result = this.perfisPersistencia.Insercao(perfilFuncionalidadeModelo);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
