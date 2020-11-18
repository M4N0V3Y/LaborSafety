using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class TelaPersistencia : ITelaPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public TelaPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public List<TelaModelo> Listar()
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                var query = (from ft in entities.FUNCIONALIDADE_TELA
                             join f in entities.FUNCIONALIDADE on ft.CodFuncionalidade equals f.CodFuncionalidade
                             join t in entities.TELA on ft.CodTela equals t.CodTela
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

        public TelaModelo ListarPorCodigo(long codigo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                var query = (from ft in entities.FUNCIONALIDADE_TELA
                             join f in entities.FUNCIONALIDADE on ft.CodFuncionalidade equals f.CodFuncionalidade
                             join t in entities.TELA on ft.CodTela equals t.CodTela
                             where t.CodTela == codigo
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

                if(query.Count > 0)
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
                        funcionalidadeModelo.Descricao = funcionalidade.Descricao;

                        result.Funcionalidades.Add(funcionalidadeModelo);
                    }

                }

                return result;
            }
        }
    }
}
