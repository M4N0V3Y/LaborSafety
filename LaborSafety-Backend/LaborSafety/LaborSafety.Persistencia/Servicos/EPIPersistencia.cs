using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class EPIPersistencia : IEPIPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public EPIPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }
        public EPI ListarEPIPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                EPI epi = entities.EPI
                    .Where(eqp => eqp.CodEPI == id).FirstOrDefault();
                return epi;
            }
        }

        public EPI ListarEPIPorIdString(string id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                EPI epi = entities.EPI
                    .Where(eqp => eqp.CodEPI.ToString() == id).FirstOrDefault();
                return epi;
            }
        }

        public List<EPI> ListarEPIsPorListaId(List<long> id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var epi = entities.EPI.Where(ep => id.Contains(ep.CodEPI)).ToList();

                return epi;
            }
        }

        public EPI ListarEPIPorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                EPI epi = entities.EPI
                    .Where(eqp => eqp.Nome == nome).FirstOrDefault();
                return epi;
            }
        }

        public EPI ListarEPIPorNivel(string nomeCompleto)
        {
            string[] niveis = nomeCompleto.ToString().Split('/');

            using (var entities = new DB_LaborSafetyEntities())
            {
                EPI result = null;
                var posicao0 = niveis[0];
                var qtdPosicoes = niveis.Length;

                if (qtdPosicoes == 1)
                {
                    result = entities.EPI.Where(eqp => eqp.N1 == posicao0).FirstOrDefault();
                }
                else if (qtdPosicoes == 2)
                {
                    var posicao1 = niveis[1];
                    result = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1).FirstOrDefault();
                }
                else
                {
                    if (string.IsNullOrEmpty(niveis[1]))
                    {
                        result = entities.EPI.Where(eqp => eqp.N1 == posicao0).FirstOrDefault();
                    }
                    else if (string.IsNullOrEmpty(niveis[2]))
                    {
                        var posicao1 = niveis[1];
                        result = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1).FirstOrDefault();
                    }
                    else
                    {
                        var posicao1 = niveis[1];
                        var posicao2 = niveis[2];
                        result = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1 && eqp.N3 == posicao2).FirstOrDefault();
                    }
                }

                    return result;
                }
            }

        public List<EPIModelo> ListarTodosEPIPorNivel(string nomeCompleto)
        {
            string[] niveis = nomeCompleto.ToString().Split('/');

            using (var entities = new DB_LaborSafetyEntities())
            {
                List<EPI> epis = new List<EPI>();
                var posicao0 = niveis[0];
                var qtdPosicoes = niveis.Length;

                if (qtdPosicoes == 1)
                {
                    epis = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 != null && eqp.N3 == null).ToList();
                }
                else if (qtdPosicoes == 2)
                {
                    var posicao1 = niveis[1];
                    epis = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1 && eqp.N3 != null).ToList();
                }
                else
                {
                    if (string.IsNullOrEmpty(niveis[1]))
                    {
                        epis = entities.EPI.Where(eqp => eqp.N1 == posicao0).ToList();
                    }
                    else if (string.IsNullOrEmpty(niveis[2]))
                    {
                        var posicao1 = niveis[1];
                        epis = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1).ToList();
                    }
                    else
                    {
                        var posicao1 = niveis[1];
                        var posicao2 = niveis[2];
                        epis = entities.EPI.Where(eqp => eqp.N1 == posicao0 && eqp.N2 == posicao1 && eqp.N3 == posicao2).ToList();
                    }
                }

                List<EPIModelo> episResult = new List<EPIModelo>();

                //Associa os inventarios de Atividade, se houver
                foreach (var local in epis)
                {
                    EPIModelo modelo = new EPIModelo();

                    modelo.CodEPI = local.CodEPI;
                    modelo.Descricao = local.Descricao;
                    modelo.Nome = local.Nome;
                    modelo.N1 = local.N1;
                    modelo.N2 = local.N2;
                    modelo.N3 = local.N3;

                    episResult.Add(modelo);
                }

                return episResult;
            }
        }

        public IEnumerable<EPI> ListarTodosEPIs()
            {
                using (var entities = new DB_LaborSafetyEntities())
                {
                    var resultado = entities.EPI
                        .ToList();

                    return resultado;
                }
            }
        }
    }
