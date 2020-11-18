using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class PerfisPersistencia : IPerfisPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public PerfisPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public IEnumerable<PERFIL> ListarTodosOsTiposPerfis()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var perfis = entities.PERFIL.ToList();
                return perfis;
            }
        }

        public PERFIL ListarTipoPerfil(long id)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                PERFIL perfil = entities.PERFIL.First(tipoPerfis => tipoPerfis.CodPerfil == id);
                return perfil;
            }
        }

        public IEnumerable<PERFIL> ObterPerfisPor8ID(string id)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                var perfis = entities.PERFIL
                    .Join(entities.USUARIO_PERFIL, perfil => perfil.CodPerfil, up => up.CodPerfil, (perfil, up) => new { perfil, up })
                    .Join(entities.USUARIO, up => up.up.CodUsuario, usuario => usuario.CodUsuario, (up, usuario) => new { up, usuario })
                    .Where(x => x.usuario.C8ID == id)
                    .Select(x => x.up.perfil)
                    .ToList();

                return perfis;
            }
        }

        public List<TelaModelo> ListarListaTelasEFuncionalidadesPorPerfil(long codPerfil)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                var query = (from pf in entities.PERFIL_FUNCIONALIDADE
                                           join f in entities.FUNCIONALIDADE on pf.CodFuncionalidade equals f.CodFuncionalidade
                                           let up = entities.USUARIO_PERFIL.Where(x=> x.CodPerfil == pf.CodPerfil).FirstOrDefault() //on pf.CodPerfil equals up.CodPerfil
                                           join t in entities.TELA on pf.CodTela equals t.CodTela
                                           where pf.CodPerfil == codPerfil
                                           select new
                                           {
                                               t.CodTela,
                                               t.Codigo,
                                               t.Nome,
                                               t.Descricao,
                                               f.CodFuncionalidade,
                                               DescricaoFuncionalidade = f.Descricao
                                           }).ToList();

                List<TelaModelo> result = new List<TelaModelo>();
                List<long> listaIDTelasImportadas = new List<long>();

                for (int i = 0; i < query.Count(); i++)
                {
                    TelaModelo telaModelo = new TelaModelo();
                    telaModelo.Funcionalidades = new List<FuncionalidadeModelo>();

                    telaModelo.Codigo = query[i].Codigo;
                    telaModelo.CodTela = query[i].CodTela;
                    telaModelo.Descricao = query[i].Descricao;
                    telaModelo.Nome = query[i].Nome;

                    var funcionalidades = query.Where(x => x.CodTela == query[i].CodTela).ToList();
                    foreach (var funcionalidade in funcionalidades)
                    {
                        FuncionalidadeModelo funcionalidadeModelo = new FuncionalidadeModelo();
                        funcionalidadeModelo.CodFuncionalidade = funcionalidade.CodFuncionalidade;
                        funcionalidadeModelo.Descricao = funcionalidade.DescricaoFuncionalidade;

                        telaModelo.Funcionalidades.Add(funcionalidadeModelo);
                    }

                    if (!listaIDTelasImportadas.Contains(telaModelo.CodTela))
                        result.Add(telaModelo);

                    listaIDTelasImportadas.Add(telaModelo.CodTela);
                }

                return result;
            }
        }

        public TelaModelo ListarTelaEFuncionalidadesPorPerfilETela(long codPerfil, long codTela)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var query = (from pf in entities.PERFIL_FUNCIONALIDADE
                             join f in entities.FUNCIONALIDADE on pf.CodFuncionalidade equals f.CodFuncionalidade
                             let up = entities.USUARIO_PERFIL.Where(x => x.CodPerfil == pf.CodPerfil).FirstOrDefault() //on pf.CodPerfil equals up.CodPerfil
                             join t in entities.TELA on pf.CodTela equals t.CodTela
                             where pf.CodPerfil == codPerfil && pf.CodTela == codTela
                             select new
                             {
                                 t.CodTela,
                                 t.Codigo,
                                 t.Nome,
                                 t.Descricao,
                                 f.CodFuncionalidade,
                                 DescricaoFuncionalidade = f.Descricao
                             }).ToList();

                TelaModelo result = new TelaModelo();

                if (query.Count > 0)
                {
                    result.Funcionalidades = new List<FuncionalidadeModelo>();

                    result.Codigo = query[0].Codigo;
                    result.CodTela = query[0].CodTela;
                    result.Descricao = query[0].Descricao;
                    result.Nome = query[0].Nome;

                    foreach (var funcionalidade in query)
                    {
                        FuncionalidadeModelo funcionalidadeModelo = new FuncionalidadeModelo();
                        funcionalidadeModelo.CodFuncionalidade = funcionalidade.CodFuncionalidade;
                        funcionalidadeModelo.Descricao = funcionalidade.DescricaoFuncionalidade;

                        result.Funcionalidades.Add(funcionalidadeModelo);
                    }
                }

                return result;
            }
        }

        public bool Insercao(PerfilFuncionalidadeModelo perfilFuncionalidadeModelo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                //Verifica se o perfil existe
                var perfilExistente = entities.PERFIL.Where(x => x.CodPerfil == perfilFuncionalidadeModelo.CodPerfil).Select(x => x.CodPerfil).FirstOrDefault();
                if (perfilExistente == 0)
                    throw new Exception("O perfil informado é inválido!");

                //Verifica se a tela existe
                var telaExistente = entities.TELA.Where(x => x.CodTela == perfilFuncionalidadeModelo.CodTela).Select(x => x.CodTela).FirstOrDefault();
                if (telaExistente == 0)
                    throw new Exception("A tela informada é inválida!");

                List<FuncionalidadeModelo> funcionalidades = perfilFuncionalidadeModelo.Funcionalidades;

                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        entities.PERFIL_FUNCIONALIDADE.RemoveRange(
                            entities.PERFIL_FUNCIONALIDADE
                                .Where(perfil => perfil.CodPerfil == perfilExistente && perfil.CodTela == telaExistente).ToList());
                        for (int i = 0; i < funcionalidades.Count; i++)
                        {
                            entities.PERFIL_FUNCIONALIDADE.Add(new PERFIL_FUNCIONALIDADE()
                            {
                                CodPerfil = (long)perfilExistente,
                                CodFuncionalidade = (long)funcionalidades[i].CodFuncionalidade,
                                CodTela = telaExistente,
                                Edicao = true
                            });
                        }

                        entities.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw exception;
                    }
                }
            }

            return true;
        }


        /*
        public bool InserirFuncionalidadesTelaPorPerfil(List<TelaModelo> telas)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var tela in telas)
                        {
                            entities.FUNCIONALIDADE_TELA.RemoveRange(
                                   entities.FUNCIONALIDADE_TELA.Where(ft => ft.CodTela == tela.CodTela).ToList());

                            foreach (var funcionalidade in tela.Funcionalidades)
                            {
                                entities.FUNCIONALIDADE_TELA.Add(new FUNCIONALIDADE_TELA()
                                {
                                    CodTela = tela.CodTela,
                                    CodFuncionalidade = funcionalidade.CodFuncionalidade
                                });
                            }

                            entities.SaveChanges();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }*/
    }
}
