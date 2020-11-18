using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class MapeamentoLocalInventarioAmbientePersistencia : IMapeamentoLocalInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public MapeamentoLocalInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public List<INVENTARIO_AMBIENTE> InserirInventarioAmbienteComLocalInstalacao(List<InventarioAmbienteModelo> inventarios, DB_LaborSafetyEntities entities)

        { 
            List<INVENTARIO_AMBIENTE> result = new List<INVENTARIO_AMBIENTE>();
            foreach (var inventario in inventarios)
            {
                result.Add(this.InserirInventarioAmbienteComLocalInstalacao(inventario, entities));
            }

            return result;
        }


        private INVENTARIO_AMBIENTE InserirInventarioAmbienteComLocalInstalacao(InventarioAmbienteModelo inventarioAmbienteModelo, DB_LaborSafetyEntities entities)
        {
            List<NrInventarioAmbienteModelo> nrs = inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE;
            List<RiscoInventarioAmbienteModelo> riscos = inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE;

            try
            {
                if (inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO == null)
                    throw new Exception("Local de instalação não informado!");

                INVENTARIO_AMBIENTE inventario = new INVENTARIO_AMBIENTE();

                if (string.IsNullOrEmpty(inventarioAmbienteModelo.Codigo))
                {
                    inventario = new INVENTARIO_AMBIENTE()
                    {
                        Codigo = "IMPORTACAO",
                        CodAmbiente = inventarioAmbienteModelo.CodAmbiente,
                        Descricao = inventarioAmbienteModelo.Descricao,
                        ObservacaoGeral = inventarioAmbienteModelo.ObservacaoGeral,
                        RiscoGeral = inventarioAmbienteModelo.RiscoGeral,
                        DataAtualizacao = DateTime.Now,
                        Ativo = true
                    };
                }
                else
                {
                    inventario = new INVENTARIO_AMBIENTE()
                    {
                        Codigo = inventarioAmbienteModelo.Codigo,
                        CodAmbiente = inventarioAmbienteModelo.CodAmbiente,
                        Descricao = inventarioAmbienteModelo.Descricao,
                        ObservacaoGeral = inventarioAmbienteModelo.ObservacaoGeral,
                        RiscoGeral = inventarioAmbienteModelo.RiscoGeral,
                        DataAtualizacao = DateTime.Now,
                        Ativo = true
                    };
                }
                entities.INVENTARIO_AMBIENTE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodInventarioAmbiente;

                inventario.Codigo = $"INV_AMB - {idInv}";
                entities.SaveChanges();

                if (nrs != null)
                {
                    foreach (var nr in nrs)
                    {
                        entities.NR_INVENTARIO_AMBIENTE.Add(new NR_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodNR = nr.CodNR,
                            Ativo = true
                        });
                    }

                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodRiscoAmbiente = risco.CodRiscoAmbiente,
                            CodSeveridade = risco.CodSeveridade,
                            CodProbabilidade = risco.CodProbabilidade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentosAplicaveis = risco.ProcedimentosAplicaveis,
                            ContraMedidas = risco.ContraMedidas,
                            Ativo = true
                        };
                        entities.RISCO_INVENTARIO_AMBIENTE.Add(novoRisco);
                        entities.SaveChanges();

                        if (risco.EPIRiscoInventarioAmbienteModelo.Count >= 0)
                        {
                            foreach (var epi in risco.EPIRiscoInventarioAmbienteModelo)
                            {
                                entities.EPI_RISCO_INVENTARIO_AMBIENTE.Add(new EPI_RISCO_INVENTARIO_AMBIENTE()
                                {
                                    CodRiscoInventarioAmbiente = novoRisco.CodRiscoInventarioAmbiente,
                                    CodEPI = epi.CodEPI
                                });
                            }
                        }
                        entities.SaveChanges();
                    }
                }

                entities.Configuration.AutoDetectChangesEnabled = true;

                if(inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.Count == 0)
                {
                    throw new Exception("Existem inventários sem local de instalação a serem inseridos.");
                }

                foreach (var item in inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO)
                {
                    var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == item.CodLocalInstalacao).FirstOrDefault();
                    localEnviado.CodInventarioAmbiente = idInv;
                }
                entities.SaveChanges();
                entities.Configuration.AutoDetectChangesEnabled = false;

                return inventario;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }
    }
}
