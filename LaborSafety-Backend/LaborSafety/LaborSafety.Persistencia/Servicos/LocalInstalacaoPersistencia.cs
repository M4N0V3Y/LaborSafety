using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class LocalInstalacaoPersistencia : ILocalInstalacaoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public LocalInstalacaoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }


        public LOCAL_INSTALACAO InserirLocalInstalacao(LOCAL_INSTALACAO modelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var localInstalacao = new LOCAL_INSTALACAO()
            {
                CodInventarioAmbiente = modelo.CodInventarioAmbiente,
                CodPerfilCatalogo = modelo.CodPerfilCatalogo,
                CodPeso = modelo.CodPeso,
                Descricao = modelo.Descricao,
                Nome = modelo.Nome,
                N1 = modelo.N1,
                N2 = modelo.N2,
                N3 = modelo.N3,
                N4 = modelo.N4,
                N5 = modelo.N5,
                N6 = modelo.N6,
            };

            entities.LOCAL_INSTALACAO.Add(localInstalacao);
            entities.SaveChanges();

            return localInstalacao;

        }

        public LOCAL_INSTALACAO ListarLocalInstalacaoPorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO
                    .Where(loc => loc.CodLocalInstalacao == id && loc.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).FirstOrDefault();
            return local;
        }

        public LOCAL_INSTALACAO ListarLocalInstalacaoPorIdString(string id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO
                    .Where(loc => loc.CodLocalInstalacao.ToString() == id && loc.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).FirstOrDefault();
                return local;
            }
        }

        public LOCAL_INSTALACAO ListarLocalInstalacaoPorNome(string nome, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO
                //.Where(loc => loc.Nome == nome && loc.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).FirstOrDefault();
                .Where(loc => loc.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return local;

        }


        public List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> ListarLocaisInstalacaoPorCodInventarioAtividade(long codInventarioAtividade)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> locaisInstalacao = new List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE>();

                locaisInstalacao = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(a => a.CodInventarioAtividade == codInventarioAtividade).ToList();

                if (locaisInstalacao == null || !locaisInstalacao.Any())
                {
                    throw new Exception($"O inventário de atividade de código {codInventarioAtividade} não possui local de instalação.");
                }
                return locaisInstalacao;
            }
        }



        public List<LOCAL_INSTALACAO> ListarLocaisInstalacaoPorCodInventarioAmbiente(long codInventarioAmbiente)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

                locaisInstalacao = entities.LOCAL_INSTALACAO.Where(a => a.CodInventarioAmbiente == codInventarioAmbiente).ToList();

                if (locaisInstalacao== null || !locaisInstalacao.Any())
                {
                    throw new Exception($"O inventário de ambiente de código {codInventarioAmbiente} não possui local de instalação.");
                }
                return locaisInstalacao;
            }
        }


        public List<LOCAL_INSTALACAO> ListarLIPorNivelEntidade(LocalInstalacaoFiltroModelo filtro)
        {
            return this.ListarLIPorNivel(filtro);
        }

        public List<LocalInstalacaoModelo> ListarLIPorNivelModelo(LocalInstalacaoFiltroModelo filtro)
        {

            var locais = this.ListarLIPorNivel(filtro);

            using (var entities = new DB_LaborSafetyEntities())
            {
                List<LocalInstalacaoModelo> locaisResult = new List<LocalInstalacaoModelo>();

                //Associa os inventarios de Atividade, se houver
                foreach (var local in locais)
                {
                    LocalInstalacaoModelo modelo = new LocalInstalacaoModelo();

                    modelo.CodInventarioAmbiente = local.CodInventarioAmbiente;
                    modelo.CodLocalInstalacao = local.CodLocalInstalacao;
                    modelo.CodPerfilCatalogo = local.CodPerfilCatalogo;
                    modelo.CodPeso = local.CodPeso;
                    modelo.Descricao = local.Descricao;
                    modelo.Nome = local.Nome;
                    modelo.N1 = local.N1;
                    modelo.N2 = local.N2;
                    modelo.N3 = local.N3;
                    modelo.N4 = local.N4;
                    modelo.N5 = local.N5;
                    modelo.N6 = local.N6;

                    var inventariosAtividade = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(x => x.CodLocalInstalacao == local.CodLocalInstalacao
                    && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO)
                        .Select(x => x.CodInventarioAtividade).ToList();

                    if (inventariosAtividade.Count > 0)
                        modelo.InventariosAtividade = inventariosAtividade;

                    locaisResult.Add(modelo);
                }

                return locaisResult;
            }
        }

        public LOCAL_INSTALACAO ValidaSeExistePaiLI(LOCAL_INSTALACAO LI, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO result = new LOCAL_INSTALACAO();
            /*
             * CSA-
             * AF00-
             * MCCE-
             * PR05-
             * ANDRE*/

            if (String.IsNullOrEmpty(LI.N2))
                throw new Exception("Não é possível editar o N1, pois seus valores são fixos.");
            else if (String.IsNullOrEmpty(LI.N3))
                result = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == LI.N1 && loc.N2 == Constantes.LOCAL_INSTALACAO_BASE && loc.N3 == null).FirstOrDefault();
            else if (String.IsNullOrEmpty(LI.N4))
                result = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == LI.N1 && loc.N2 == LI.N2 && loc.N3 == Constantes.LOCAL_INSTALACAO_BASE
                && loc.N4 == null).FirstOrDefault();
            else if (String.IsNullOrEmpty(LI.N5))
                result = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == LI.N1 && loc.N2 == LI.N2 && loc.N3 == LI.N3 && loc.N4 == Constantes.LOCAL_INSTALACAO_BASE
                 && loc.N5 == null).FirstOrDefault();
            else if (String.IsNullOrEmpty(LI.N6))
                result = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == LI.N1 && loc.N2 == LI.N2 && loc.N3 == LI.N3 && loc.N4 == LI.N4
                 && loc.N5 == Constantes.LOCAL_INSTALACAO_BASE && loc.N6 == null).FirstOrDefault();
            else
                result = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == LI.N1 && loc.N2 == LI.N2 && loc.N3 == LI.N3 && loc.N4 == LI.N4
                                && loc.N5 == LI.N5 && loc.N6 == Constantes.LOCAL_INSTALACAO_BASE).FirstOrDefault();

            return result;
        }

        private List<LOCAL_INSTALACAO> ListarLIPorNivel(LocalInstalacaoFiltroModelo filtro)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<LOCAL_INSTALACAO> locais = new List<LOCAL_INSTALACAO>();

                if (String.IsNullOrEmpty(filtro.N2))
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 != null && loc.N3 == null).OrderBy(x => x.CodLocalInstalacao).ToList();
                else if (String.IsNullOrEmpty(filtro.N3))
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 == filtro.N2 && loc.N3 != null && loc.N4 == null).OrderBy(x => x.CodLocalInstalacao).ToList();
                else if (String.IsNullOrEmpty(filtro.N4))
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 == filtro.N2 && loc.N3 == filtro.N3 && loc.N4 != null && loc.N5 == null).OrderBy(x => x.CodLocalInstalacao).ToList();
                else if (String.IsNullOrEmpty(filtro.N5))
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 == filtro.N2 && loc.N3 == filtro.N3 && loc.N4 == filtro.N4 && loc.N5 != null
                    && loc.N6 == null).OrderBy(x => x.CodLocalInstalacao).ToList();
                else if (String.IsNullOrEmpty(filtro.N6))
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 == filtro.N2 && loc.N3 == filtro.N3 && loc.N4 == filtro.N4 && loc.N5 == filtro.N5
                    && loc.N6 != null).OrderBy(x => x.CodLocalInstalacao).ToList();
                else
                    locais = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == filtro.N1 && loc.N2 == filtro.N2 && loc.N3 == filtro.N3 && loc.N4 == filtro.N4 && loc.N5 == filtro.N5
                    && loc.N6 == filtro.N6).OrderBy(x => x.CodLocalInstalacao).ToList();

                return locais;
            }
        }


        public List<LOCAL_INSTALACAO> ListarTodosLIs(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = entities.LOCAL_INSTALACAO
                         .Where(x => x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).OrderBy(x => x.CodLocalInstalacao)
                         .ToList();

            return resultado;
        }

        public void AtualizaCodigoInventarioDoLocalPorLista(List<LOCAL_INSTALACAO> locais)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //IMPORTANTE: É NECESSÁRIO DESABILITAR ESSA OPÇÃO PARA OTIMIZAR OS INSERTS
                entities.Configuration.AutoDetectChangesEnabled = false;

                foreach (var local in locais)
                {

                    var localDB = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == local.CodLocalInstalacao).First();

                    //localDB.CodInventarioAmbiente

                }

            }
        }

        public LOCAL_INSTALACAO EditaPerfilCatalogoLocaDoLocal(long codLocalInstalacao, long codPerfilCatalogo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO.Where(invAmb => invAmb.CodLocalInstalacao == codLocalInstalacao).FirstOrDefault();

            if (local == null)
                throw new Exception("Local informado inexistente!");

            local.CodPerfilCatalogo = codPerfilCatalogo;
            entities.SaveChanges();

            return local;
        }

        public LOCAL_INSTALACAO EditaLocalInstalacao (LOCAL_INSTALACAO local, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO localExistente = entities.LOCAL_INSTALACAO.Where(invAmb => invAmb.CodLocalInstalacao ==
                                                                                        local.CodLocalInstalacao).FirstOrDefault();

            if (localExistente == null)
                throw new Exception("Local informado inexistente!");

            localExistente.CodInventarioAmbiente = local.CodInventarioAmbiente;
            localExistente.CodPerfilCatalogo = local.CodPerfilCatalogo;
            localExistente.CodPeso = local.CodPeso;
            localExistente.Descricao = local.Descricao;
            localExistente.N1 = local.N1;
            localExistente.N2 = local.N2;
            localExistente.N3 = local.N3;
            localExistente.N4 = local.N4;
            localExistente.N5 = local.N5;
            localExistente.N6 = local.N6;
            localExistente.Nome = local.Nome;

            entities.SaveChanges();
            return localExistente;

        }

        public void AtualizaCodigoInventarioDoLocal(long idLocal, long idInventarioAmbiente, DB_LaborSafetyEntities entities)
        {
            entities = new DB_LaborSafetyEntities();

            //entities.Configuration.AutoDetectChangesEnabled = true;

            LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO.Where(invAmb => invAmb.CodLocalInstalacao == idLocal).FirstOrDefault();

            if (local == null)
                throw new Exception("Local informado inexistente!");

            local.CodInventarioAmbiente = idInventarioAmbiente;

            entities.SaveChanges();

        }

        public void AtualizaCodigoInventarioDoLocal(long idLocal, long idInventarioAmbiente,
            DB_LaborSafetyEntities entities = null,
            DbContextTransaction transaction = null)
        {

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            using (entities)
            {

                if (transaction == null)
                    transaction = entities.Database.BeginTransaction();

                using (transaction)
                {
                    try
                    {
                        LOCAL_INSTALACAO local = entities.LOCAL_INSTALACAO.Where(invAmb => invAmb.CodLocalInstalacao == idLocal).FirstOrDefault();

                        if (local == null)
                            throw new Exception("Local informado inexistente!");

                        local.CodInventarioAmbiente = idInventarioAmbiente;

                        entities.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }

        public void ValidaLocalPorNivel(LOCAL_INSTALACAO local, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            //var todosLIs = this.ListarTodosLIs(entities);

            if (string.IsNullOrEmpty(local.N1))
                throw new Exception("Não é possível validar o local pois o nivel 1 não foi informado!");
            else if (string.IsNullOrEmpty(local.N2))
            {
                var LI = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI == null)
                    throw new Exception($"O nível ' {local.N1} ' não está cadastrado na base de dados!");
            }
            else if (string.IsNullOrEmpty(local.N3))
            {
                var LI1 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI1 == null)
                    throw new Exception($"O nível ' {local.N2} ' não está cadastrado na base de dados!");

            }
            else if (string.IsNullOrEmpty(local.N4))
            {
                var LI1 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI1 == null)
                    throw new Exception($"O nível ' {local.N1} ' não está cadastrado na base de dados!");

                var LI2 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == null).FirstOrDefault();
                if (LI2 == null)
                    throw new Exception($"O nível ' {local.N2} ' não está cadastrado na base de dados!");

            }
            else if (string.IsNullOrEmpty(local.N5))
            {
                var LI1 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI1 == null)
                    throw new Exception($"O nível ' {local.N1} ' não está cadastrado na base de dados!");

                var LI2 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == null).FirstOrDefault();
                if (LI2 == null)
                    throw new Exception($"O nível ' {local.N2} ' não está cadastrado na base de dados!");

                var LI3 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                 && loc.N4 == null).FirstOrDefault();
                if (LI3 == null)
                    throw new Exception($"O nível ' {local.N3} ' não está cadastrado na base de dados!");

            }
            else if (string.IsNullOrEmpty(local.N6))
            {
                var LI1 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI1 == null)
                    throw new Exception($"O nível ' {local.N1} ' não está cadastrado na base de dados!");

                var LI2 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == null).FirstOrDefault();
                if (LI2 == null)
                    throw new Exception($"O nível ' {local.N2} ' não está cadastrado na base de dados!");

                var LI3 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                 && loc.N4 == null).FirstOrDefault();
                if (LI3 == null)
                    throw new Exception($"O nível ' {local.N3} ' não está cadastrado na base de dados!");

                var LI4 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                 && loc.N4 == local.N4 && loc.N5 == null).FirstOrDefault();
                if (LI4 == null)
                    throw new Exception($"O nível ' {local.N4} ' não está cadastrado na base de dados!");

            }
            else
            {
                
                var LI1 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).FirstOrDefault();
                if (LI1 == null)
                    throw new Exception($"O nível ' {local.N1} ' não está cadastrado na base de dados!");

                var LI2 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == null).FirstOrDefault();
                if (LI2 == null)
                    throw new Exception($"O nível ' {local.N2} ' não está cadastrado na base de dados!");

                var LI3 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                 && loc.N4 == null).FirstOrDefault();
                if (LI3 == null)
                    throw new Exception($"O nível ' {local.N3} ' não está cadastrado na base de dados!");

                var LI4 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                 && loc.N4 == local.N4 && loc.N5 == null).FirstOrDefault();
                if (LI4 == null)
                    throw new Exception($"O nível ' {local.N4} ' não está cadastrado na base de dados!");

                var LI5 = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
               && loc.N4 == local.N4 && loc.N5 == local.N5 && loc.N6 == null).FirstOrDefault();
                if (LI5 == null)
                    throw new Exception($"O nível ' {local.N5} ' não está cadastrado na base de dados!");
                return;
            }

        }


        public LOCAL_INSTALACAO ValidaLocalPorNivelImportacao(LOCAL_INSTALACAO local, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var li = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == local.N2 && loc.N3 == local.N3
                        && loc.N4 == local.N4 && loc.N5 == local.N5 && loc.N6 == local.N6).FirstOrDefault();

            return li;
        }

        public void ValidaEInsereLocalBasePorPai(LOCAL_INSTALACAO localPai, string nomeLocalAInserir, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO localBase = new LOCAL_INSTALACAO();

            if (localPai == null)
                throw new Exception("Não é possível validar o nivel pai do local, pois o mesmo não foi informado!");

            if (string.IsNullOrEmpty(localPai.N1))
                throw new Exception("Não é possível validar o local pois o nivel 1 não foi informado!");
            else if (string.IsNullOrEmpty(localPai.N2))
            {
                throw new Exception("Não é possível editar o N1, pois seus valores são fixos.");
            }
            else if (string.IsNullOrEmpty(localPai.N3))
            {
                localBase.N1 = localPai.N1;
                localBase.N2 = nomeLocalAInserir;
                localBase.N3 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(localPai.N4))
            {
                localBase.N1 = localPai.N1;
                localBase.N2 = localPai.N2;
                localBase.N3 = nomeLocalAInserir;
                localBase.N4 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(localPai.N5))
            { 
                localBase.N1 = localPai.N1;
                localBase.N2 = localPai.N2;
                localBase.N3 = localPai.N3;
                localBase.N4 = nomeLocalAInserir;
                localBase.N5 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(localPai.N6))
            {
                localBase.N1 = localPai.N1;
                localBase.N2 = localPai.N2;
                localBase.N3 = localPai.N3;
                localBase.N4 = localPai.N4;
                localBase.N5 = nomeLocalAInserir;
                localBase.N6 = Constantes.LOCAL_INSTALACAO_BASE;

            }
            else
                return;

            localBase.CodInventarioAmbiente = (long)Constantes.InventarioAmbiente.SEM_INVENTARIO;
            localBase.CodPerfilCatalogo = localPai.CodPerfilCatalogo;
            localBase.CodPeso = localPai.CodPeso;
            localBase.Descricao = localPai.Nome + " - " + localPai.Descricao;
            localBase.Nome = localPai.Nome;

            this.InserirLocalInstalacao(localBase, entities);
        }

        /*
        public void ValidaEInsereLocalBasePorNivel(LOCAL_INSTALACAO local, DB_LaborSafety_V1_ESPEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO nivelPai = new LOCAL_INSTALACAO();
            LOCAL_INSTALACAO localBase = new LOCAL_INSTALACAO();

            entities.Database.CommandTimeout = 9999;


            if (string.IsNullOrEmpty(local.N1))
                throw new Exception("Não é possível validar o local pois o nivel 1 não foi informado!");
            else if (string.IsNullOrEmpty(local.N2))
            {
                throw new Exception("Não é possível editar o N1, pois seus valores são fixos.");
            }
            else if (string.IsNullOrEmpty(local.N3))
            {
                localBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1
                                                               && loc.N2 == Constantes.LOCAL_INSTALACAO_BASE
                                                               && loc.N3 == null).FirstOrDefault();
            }
            else if (string.IsNullOrEmpty(local.N4))
            {
                localBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && 
                                                             loc.N2 == local.N2 && 
                                                             loc.N3 == Constantes.LOCAL_INSTALACAO_BASE &&
                                                             loc.N4 == null).FirstOrDefault();
            }
            else if (string.IsNullOrEmpty(local.N5))
            {
                localBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && 
                                                             loc.N2 == local.N2 && 
                                                             loc.N3 == local.N3 &&
                                                             loc.N4 == Constantes.LOCAL_INSTALACAO_BASE && 
                                                             loc.N5 == null).FirstOrDefault();
            }
            else if (string.IsNullOrEmpty(local.N6))
            {
                localBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && 
                                                             loc.N2 == local.N2 && 
                                                             loc.N3 == local.N3 &&
                                                             loc.N4 == local.N4 &&
                                                             loc.N5 == Constantes.LOCAL_INSTALACAO_BASE &&
                                                             loc.N6 == null).FirstOrDefault();
            }
            else
                return;

            if (localBase != null)
                return;

            this.ValidaEInsereLocalBasePorPai(localBase, entities);
        }
        */

        public void ValidaEInsereLocalBasePorNivel2(LOCAL_INSTALACAO local, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOCAL_INSTALACAO nivelPai = new LOCAL_INSTALACAO();
            LOCAL_INSTALACAO localBase = new LOCAL_INSTALACAO();

            entities.Database.CommandTimeout = 9999;


            if (string.IsNullOrEmpty(local.N1))
                throw new Exception("Não é possível validar o local pois o nivel 1 não foi informado!");
            else if (string.IsNullOrEmpty(local.N2))
            {
                throw new Exception("Não é possível editar o N1, pois seus valores são fixos.");
            }
            else if (string.IsNullOrEmpty(local.N3))
            {
                var LIBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1
                                                               && loc.N2 == Constantes.LOCAL_INSTALACAO_BASE
                                                               && loc.N3 == null).FirstOrDefault();

                if (LIBase != null)
                    return;

                nivelPai = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 && loc.N2 == null).First();
                localBase.N1 = nivelPai.N1;
                localBase.N2 = nivelPai.N2;
                localBase.N3 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(local.N4))
            {
                var LIBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                             loc.N2 == local.N2 &&
                                                             loc.N3 == Constantes.LOCAL_INSTALACAO_BASE &&
                                                             loc.N4 == null).FirstOrDefault();

                if (LIBase != null)
                    return;

                nivelPai = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1
                                                           && loc.N2 == local.N2
                                                           && loc.N3 == null).First();
                localBase.N1 = nivelPai.N1;
                localBase.N2 = nivelPai.N2;
                localBase.N3 = nivelPai.N3;
                localBase.N4 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(local.N5))
            {
                var LIBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                             loc.N2 == local.N2 &&
                                                             loc.N3 == local.N3 &&
                                                             loc.N4 == Constantes.LOCAL_INSTALACAO_BASE &&
                                                             loc.N5 == null).FirstOrDefault();

                if (LIBase != null)
                    return;

                nivelPai = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                           loc.N2 == local.N2 &&
                                                           loc.N3 == local.N3 &&
                                                           loc.N4 == null).First();
                localBase.N1 = nivelPai.N1;
                localBase.N2 = nivelPai.N2;
                localBase.N3 = nivelPai.N3;
                localBase.N4 = nivelPai.N4;
                localBase.N5 = Constantes.LOCAL_INSTALACAO_BASE;
            }
            else if (string.IsNullOrEmpty(local.N6))
            {
                var LIBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                             loc.N2 == local.N2 &&
                                                             loc.N3 == local.N3 &&
                                                             loc.N4 == local.N4 &&
                                                             loc.N5 == Constantes.LOCAL_INSTALACAO_BASE &&
                                                             loc.N6 == null).FirstOrDefault();

                if (LIBase != null)
                    return;


                nivelPai = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                           loc.N2 == local.N2 &&
                                                           loc.N3 == local.N3 &&
                                                           loc.N4 == local.N4 &&
                                                           loc.N5 == null).First();
                localBase.N1 = nivelPai.N1;
                localBase.N2 = nivelPai.N2;
                localBase.N3 = nivelPai.N3;
                localBase.N4 = nivelPai.N4;
                localBase.N5 = nivelPai.N5;
                localBase.N6 = Constantes.LOCAL_INSTALACAO_BASE;

            }
            //else
            //    return;
            else
            {
                var LIBase = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                            loc.N2 == local.N2 &&
                                                            loc.N3 == local.N3 &&
                                                            loc.N4 == local.N4 &&
                                                            loc.N5 == local.N5 &&
                                                            loc.N6 == Constantes.LOCAL_INSTALACAO_BASE).FirstOrDefault();

                if (LIBase != null)
                    return;


                nivelPai = entities.LOCAL_INSTALACAO.Where(loc => loc.N1 == local.N1 &&
                                                           loc.N2 == local.N2 &&
                                                           loc.N3 == local.N3 &&
                                                           loc.N4 == local.N4 &&
                                                           loc.N5 == local.N5 &&
                                                           loc.N6 == null).First();
                localBase.N1 = nivelPai.N1;
                localBase.N2 = nivelPai.N2;
                localBase.N3 = nivelPai.N3;
                localBase.N4 = nivelPai.N4;
                localBase.N5 = nivelPai.N5;
                localBase.N6 = Constantes.LOCAL_INSTALACAO_BASE;
            }

            //Insere o local-base
            localBase.CodInventarioAmbiente = (long)Constantes.InventarioAmbiente.SEM_INVENTARIO;
            localBase.CodPerfilCatalogo = nivelPai.CodPerfilCatalogo;
            localBase.CodPeso = nivelPai.CodPeso;
            localBase.Descricao = nivelPai.Descricao;
            localBase.Nome = nivelPai.Nome;

            this.InserirLocalInstalacao(localBase, entities);

        }


        public INVENTARIO_AMBIENTE ListaInventarioAmbienteAtivoLocalInstalacao(long codLocalInstalacao, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var LIAtual = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocalInstalacao).FirstOrDefault();

            if (LIAtual == null)
                throw new Exception($"O local de código : {codLocalInstalacao} informado não existe na base de dados.");

            if (LIAtual.CodInventarioAmbiente == (long)Constantes.InventarioAmbiente.SEM_INVENTARIO)
                return null;
            else
                return entities.INVENTARIO_AMBIENTE.Where(x => x.CodInventarioAmbiente == LIAtual.CodInventarioAmbiente && x.Ativo == true).FirstOrDefault();
        }

        public List<INVENTARIO_ATIVIDADE> ListaInventariosAtividadeAtivosLocalInstalacao(long codLocalInstalacao, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var LIAtual = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocalInstalacao).FirstOrDefault();

            if (LIAtual == null)
                throw new Exception($"O local de código : {codLocalInstalacao} informado não existe na base de dados.");

            var locaisInventarioAtividade = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(x => x.CodLocalInstalacao == codLocalInstalacao).ToList();

            List<INVENTARIO_ATIVIDADE> result = new List<INVENTARIO_ATIVIDADE>();

            for (int i = 0; i < locaisInventarioAtividade.Count(); i++)
            {
                long codInventario = locaisInventarioAtividade[i].CodInventarioAtividade;
                var inventario = entities.INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == codInventario && x.Ativo == true).FirstOrDefault();
                if (inventario != null)
                {
                    result.Add(inventario);
                }
            }

            return result;

        }
    }
}
