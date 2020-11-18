using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class FuncionalidadesNegocio : IFuncionalidadesNegocio
    {
        private readonly IFuncionalidadesPersistencia funcionalidadePersistencia;
        public FuncionalidadesNegocio(IFuncionalidadesPersistencia funcionalidadePersistencia)
        {
            this.funcionalidadePersistencia = funcionalidadePersistencia;
        }
        public IEnumerable<FuncionalidadeModelo> ListarTodasFuncionalidades()
        {
            IEnumerable<FuncionalidadeModelo> listaFuncionalidades = new List<FuncionalidadeModelo>();
            // Mapeia de Funcionalidades para o objeto de destino listaFuncionalidades
            AutoMapper.Mapper.Map(this.funcionalidadePersistencia.ListarTodasFuncionalidades(), listaFuncionalidades);

            var count = 0;
            foreach (var item in listaFuncionalidades)
            {
                count++;
            }

            if (count == 0)
            {
                throw new KeyNotFoundException("Não existe nenhuma funcionalidade!");
            }

            return listaFuncionalidades;
        }

        public FuncionalidadeModelo ListarFuncionalidadePorId(long id)
        {
            FuncionalidadeModelo funcionalidade = new FuncionalidadeModelo();
            // Mapeia de Funcionalidades para o objeto de destino listaFuncionalidades
            AutoMapper.Mapper.Map(this.funcionalidadePersistencia.ListarFuncionalidadePorId(id), funcionalidade);

            if (id == 0)
            {
                throw new KeyNotFoundException("Funcionalidade não encontrada.");
            }
            return funcionalidade;
        }

        public FuncionalidadesPor8IdModelo ListarFuncionalidadesPor8ID(string c8id)
        {
            List<PERFIL_FUNCIONALIDADE> perfilFuncionalidades = this.funcionalidadePersistencia.ListarFuncionalidadesPor8ID(c8id);

            if (c8id == null || c8id == "")
            {
                throw new KeyNotFoundException("Funcionalidade não encontrada.");
            }

            List<AdministracaoPerfilModelo> perfilFuncionalidadesModelo = new List<AdministracaoPerfilModelo>();

            List<PERFIL_FUNCIONALIDADE> perfilFuncionalidadesControlador = perfilFuncionalidades.FindAll(x => x.CodPerfil == 3);

            perfilFuncionalidadesControlador.ForEach(pf => {
                perfilFuncionalidadesModelo.Add(new AdministracaoPerfilModelo()
                {
                    CodFuncionalidade = pf.CodFuncionalidade,
                    Edicao = pf.Edicao,
                });
            });

            perfilFuncionalidades.ForEach(pf => {
                if (pf.CodPerfil != 3)
                {
                    AdministracaoPerfilModelo admPerfil = perfilFuncionalidadesModelo.Find(x => x.CodFuncionalidade == pf.CodFuncionalidade);
                    if (pf.Edicao && admPerfil != null)
                    {
                        admPerfil.Edicao = true;
                        admPerfil.Edicao = pf.Edicao;
                    }
                    else
                    {
                        perfilFuncionalidadesModelo.Add(new AdministracaoPerfilModelo()
                        {
                            CodFuncionalidade = pf.CodFuncionalidade,
                            Edicao = pf.Edicao,
                        });
                    }
                }

            });
            FuncionalidadesPor8IdModelo funcionalidadesPor8IDModeloRetorno =
                new FuncionalidadesPor8IdModelo()
                {
                    C8ID = c8id,
                    funcionalidades = perfilFuncionalidadesModelo
                };

            return funcionalidadesPor8IDModeloRetorno;
        }
    }
}
