using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class InventarioAtividadePersistencia : IInventariosAtividadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public InventarioAtividadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadePorId(long id, bool eAtivo = true)
        {
            INVENTARIO_ATIVIDADE inventarioAtividade;

            if (eAtivo)
            {
                using (var entities = new DB_LaborSafetyEntities())
                {
                    inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                        .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                        .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Select(a => a.LOCAL_INSTALACAO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                        .Include(x => x.RESPONSAVEL_INVENTARIO_ATIVIDADE)
                        .Where(invAtv => invAtv.CodInventarioAtividade == id && invAtv.Ativo).FirstOrDefault();
                    return inventarioAtividade;
                }
            }
            else
            {
                using (var entities = new DB_LaborSafetyEntities())
                {
                    inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                        .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                        .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Select(a => a.LOCAL_INSTALACAO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                        .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                        .Include(x => x.RESPONSAVEL_INVENTARIO_ATIVIDADE)
                        .Where(invAtv => invAtv.CodInventarioAtividade == id).FirstOrDefault();
                    return inventarioAtividade;
                }
            }

        }

        public List<INVENTARIO_ATIVIDADE> ListarTodosInventarios()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<INVENTARIO_ATIVIDADE> inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Select(a => a.LOCAL_INSTALACAO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Include(x => x.RESPONSAVEL_INVENTARIO_ATIVIDADE)
                    .Where(invAtv => invAtv.Ativo).ToList();

                return inventarioAtividade;
            }
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeEDisciplina(long codAtividade, long codDisciplina)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                INVENTARIO_ATIVIDADE inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Where(invAtv => invAtv.CodAtividade == codAtividade && invAtv.CodDisciplina == codDisciplina && !invAtv.Ativo).FirstOrDefault();

                if (inventarioAtividade == null)
                    throw new Exception("Inventário de atividade não se encontra na base de dados.");

                return inventarioAtividade;
            }
        }

        public List<INVENTARIO_ATIVIDADE> ListarVariosInventariosAtividadePorLI(long li)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<INVENTARIO_ATIVIDADE> inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Where(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(y => y.CodLocalInstalacao == li) && x.Ativo).ToList();

                return inventarioAtividade;
            }
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeDisciplinaLIInv(long atividadePadrao, long disciplina, long li, long invAtividade, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_ATIVIDADE inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Where(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(y => y.CodLocalInstalacao == li && y.Ativo) && 
                    x.CodAtividade == atividadePadrao && x.CodDisciplina == disciplina && x.Ativo && x.CodInventarioAtividade != invAtividade).FirstOrDefault();

                return inventarioAtividade;
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeDisciplinaLI(long atividadePadrao, long disciplina, long li, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_ATIVIDADE inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Where(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(y => y.CodLocalInstalacao == li) &&
                    x.CodAtividade == atividadePadrao && x.CodDisciplina == disciplina && x.Ativo).FirstOrDefault();

            return inventarioAtividade;
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadeAtivadoEDesativadoPorAtividadeDisciplinaLI(long atividadePadrao, long disciplina, long li, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_ATIVIDADE inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Where(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(y => y.CodLocalInstalacao == li) &&
                    x.CodAtividade == atividadePadrao && x.CodDisciplina == disciplina).FirstOrDefault();

            return inventarioAtividade;
        }

        public List<INVENTARIO_ATIVIDADE> ListarTodos(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            List<INVENTARIO_ATIVIDADE> inventarios = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Where(x => x.Ativo)
                    .ToList();

            return inventarios;
        }

        public INVENTARIO_ATIVIDADE ListarInventarioAtividadeAtivadoEDesativadoPorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_ATIVIDADE inventarioAtividade = entities.INVENTARIO_ATIVIDADE
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                .Where(invAmb => invAmb.CodInventarioAtividade == id).FirstOrDefault();
            return inventarioAtividade;

        }

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codLocal, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal).FirstOrDefault();

            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 ).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6).ToList();

            return locaisFiltrados;
        }

        public IEnumerable<INVENTARIO_ATIVIDADE> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Select(local => local.LOCAL_INSTALACAO))
                    .Where(x => x.Ativo);

                if (filtroInventarioAtividadeModelo.CodAtividade != 0)
                    resultado = resultado.Where(o => o.CodAtividade == filtroInventarioAtividadeModelo.CodAtividade);

                if (filtroInventarioAtividadeModelo.CodDisciplina != 0)
                    resultado = resultado.Where(o => o.CodDisciplina == filtroInventarioAtividadeModelo.CodDisciplina);

                if (filtroInventarioAtividadeModelo.CodPeso != 0)
                    resultado = resultado.Where(o => o.CodPeso == filtroInventarioAtividadeModelo.CodPeso);

                if (filtroInventarioAtividadeModelo.CodPerfilCatalogo != 0)
                    resultado = resultado.Where(o => o.CodPerfilCatalogo == filtroInventarioAtividadeModelo.CodPerfilCatalogo);

                var resultadoFinal = resultado.ToList();

                if (filtroInventarioAtividadeModelo.CodSeveridade != 0)
                    resultadoFinal = resultadoFinal.Where(x => x.RISCO_INVENTARIO_ATIVIDADE.Any(y => y.CodSeveridade == filtroInventarioAtividadeModelo.CodSeveridade))
                        .ToList();

                if (filtroInventarioAtividadeModelo.Riscos.Count != 0 || filtroInventarioAtividadeModelo.Riscos == null)
                    foreach (var risco in filtroInventarioAtividadeModelo.Riscos)
                        resultadoFinal = resultadoFinal.Where(a => a.RISCO_INVENTARIO_ATIVIDADE.Any(x => x.RISCO.CodRisco == risco))
                            .ToList();

                if (filtroInventarioAtividadeModelo.Locais.Count > 0)
                {
                    List<LOCAL_INSTALACAO> locaisOrigem = new List<LOCAL_INSTALACAO>();
                    List<LOCAL_INSTALACAO> locaisAPesquisar = new List<LOCAL_INSTALACAO>();
                    //Busca todos os locais

                    foreach (var nlocal in filtroInventarioAtividadeModelo.Locais)
                    {
                        
                        var local = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == nlocal).FirstOrDefault();

                        if (local == null)
                            throw new Exception($"O local de instalação {local.Nome} não consta na base de dados.");

                        locaisOrigem.Add(local);
                    }

                    foreach (var local in locaisOrigem)
                    {
                        //Filtra somente os locais do pai
                        List<LOCAL_INSTALACAO> locaisFilhos = this.BuscaLocaisEFilhos(entities, local);

                        locaisAPesquisar.AddRange(locaisFilhos);
                    }

                    resultadoFinal = resultadoFinal.Where(b => b.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any
                    (x => locaisAPesquisar.Contains(x.LOCAL_INSTALACAO))).ToList();
                }

                return resultadoFinal;
            }
        }

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(DB_LaborSafetyEntities entities, LOCAL_INSTALACAO localEnviado)
        {
            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6).ToList();

            return locaisFiltrados;
        }

        //public List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(List<LOCAL_INSTALACAO> locais, LOCAL_INSTALACAO localEnviado, long idInventario)
        //{
        //    List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

        //    if (String.IsNullOrEmpty(localEnviado.N2))
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.CodInventarioAmbiente != idInventario).ToList();
        //    else if (String.IsNullOrEmpty(localEnviado.N3))
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
        //        && x.CodInventarioAmbiente != idInventario).ToList();
        //    else if (String.IsNullOrEmpty(localEnviado.N4))
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
        //        x.N3 == localEnviado.N3 && x.CodInventarioAmbiente != idInventario).ToList();
        //    else if (String.IsNullOrEmpty(localEnviado.N5))
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
        //        x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.CodInventarioAmbiente != idInventario).ToList();
        //    else if (String.IsNullOrEmpty(localEnviado.N6))
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
        //        && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.CodInventarioAmbiente != idInventario).ToList();
        //    else
        //        locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
        //        && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6
        //        && x.CodInventarioAmbiente != idInventario).ToList();

        //    return locaisFiltrados;
        //}

        public List<INVENTARIO_ATIVIDADE> ListarInventarioAtividadeExportacao(DadosExportacaoAtividadeModelo dados)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<INVENTARIO_ATIVIDADE> resultadoAtvPadrao = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoDisciplina = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoPeso = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoPerfilCatalogo = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoSeveridade = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoProbabilidade = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoRisco = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoEPI = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoLocalInstalacao = new List<INVENTARIO_ATIVIDADE>();
                List<INVENTARIO_ATIVIDADE> resultadoFinal = new List<INVENTARIO_ATIVIDADE>();
                int cont = 0;

                var resultado = entities.INVENTARIO_ATIVIDADE
                    .Include(x => x.ATIVIDADE_PADRAO)
                    .Include(x => x.DISCIPLINA)
                    .Include(x => x.PESO)
                    .Include(x => x.PERFIL_CATALOGO)
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                    .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Select(local => local.LOCAL_INSTALACAO))
                    .Where(x => x.Ativo).ToList();

                if (dados.ATIVIDADE_PADRAO.Count > 0 && dados.ATIVIDADE_PADRAO != null)
                {
                    resultadoAtvPadrao = resultado.Where(b => dados.ATIVIDADE_PADRAO.Contains(b.CodAtividade)).ToList();
                    cont++;
                }
                    
                if (dados.DISCIPLINA.Count > 0 && dados.DISCIPLINA != null)
                {
                    resultadoDisciplina = resultado.Where(b => dados.DISCIPLINA.Contains(b.CodDisciplina)).ToList();
                    cont++;
                }

                if (dados.PESO.Count > 0 && dados.PESO != null)
                {
                    resultadoPeso = resultado.Where(b => dados.PESO.Contains(b.CodPeso)).ToList();
                    cont++;
                }

                if (dados.PERFIL_CATALOGO.Count > 0 && dados.PERFIL_CATALOGO != null)
                {
                    resultadoPerfilCatalogo = resultado.Where(b => dados.PERFIL_CATALOGO.Contains(b.CodPerfilCatalogo)).ToList();
                    cont++;
                }

                if (dados.SEVERIDADE.Count > 0 && dados.SEVERIDADE != null)
                {
                    resultadoSeveridade = resultado.Where(teste => teste.RISCO_INVENTARIO_ATIVIDADE.Any(a => dados.SEVERIDADE.Contains(a.CodSeveridade))).ToList();
                    cont++;
                }

                if (dados.RISCO.Count > 0 && dados.RISCO != null)
                {
                    resultadoRisco = resultado.Where(teste => teste.RISCO_INVENTARIO_ATIVIDADE.Any(a => dados.RISCO.Contains(a.CodRisco))).ToList();
                    cont++;
                }

                if (dados.EPI.Count > 0 && dados.EPI != null)
                {
                    resultadoEPI = resultado.Where(teste => teste.RISCO_INVENTARIO_ATIVIDADE.All(i => i.EPI_RISCO_INVENTARIO_ATIVIDADE.Any(a => dados.EPI.Contains(a.CodEPI)))).ToList();
                    cont++;
                }

                if (dados.LOCAL_INSTALACAO.Count > 0 && dados.LOCAL_INSTALACAO != null)
                {
                    List<long> listaLocais = new List<long>();
                    foreach (var item in dados.LOCAL_INSTALACAO)
                    {
                        var localInstalacao = entities.LOCAL_INSTALACAO.Where(nomeLocal => nomeLocal.CodLocalInstalacao == item).FirstOrDefault();
                        var codigoLocal = localInstalacao.CodLocalInstalacao;

                        listaLocais.Add(item);
                    }
                    resultadoLocalInstalacao = resultado.Where(b => b.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(a => listaLocais.Contains(a.CodLocalInstalacao))).ToList();
                    cont++;
                }

                foreach (var itemAtvPadrao in resultadoAtvPadrao)
                    resultadoFinal.Add(itemAtvPadrao);

                foreach (var itemDisciplina in resultadoDisciplina)
                    resultadoFinal.Add(itemDisciplina);

                foreach (var itemPeso in resultadoPeso)
                    resultadoFinal.Add(itemPeso);

                foreach (var itemPerfilCatalogo in resultadoPerfilCatalogo)
                    resultadoFinal.Add(itemPerfilCatalogo);

                foreach (var itemSeveridade in resultadoSeveridade)
                    resultadoFinal.Add(itemSeveridade);

                foreach (var itemProbabilidade in resultadoProbabilidade)
                    resultadoFinal.Add(itemProbabilidade);

                foreach (var itemRisco in resultadoRisco)
                    resultadoFinal.Add(itemRisco);

                foreach (var itemEPI in resultadoEPI)
                    resultadoFinal.Add(itemEPI);

                foreach (var itemLocalInstalacao in resultadoLocalInstalacao)
                    resultadoFinal.Add(itemLocalInstalacao);

                if (resultado.Count > 0 && cont != 0)
                    resultadoFinal = resultadoFinal.Distinct().ToList();

                else if (resultado.Count > 0 && cont == 0)
                    resultadoFinal = resultado.Distinct().ToList();

                else
                    throw new Exception("Não foram encontrados inventários para exportação.");

                return resultadoFinal;
            }
        }

        public INVENTARIO_ATIVIDADE InserirItemListaInventarioAtividade(InventarioAtividadeModelo inventario, DB_LaborSafetyEntities entities)
        {
            var result = this.InserirImportacao(inventario, entities);

            return result;
        }

        public INVENTARIO_ATIVIDADE InserirImportacao(InventarioAtividadeModelo inventarioAtividadeModelo, DB_LaborSafetyEntities entities, bool eEdicao = false)
        {
            List<LocalInstalacaoInventarioAtividadeModelo> localInstalacao = inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE;
            List<RiscoInventarioAtividadeModelo> riscos = inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE;

            try
            {
                

                var inventario = new INVENTARIO_ATIVIDADE()
                {
                    Codigo = inventarioAtividadeModelo.Codigo == null ? "" : inventarioAtividadeModelo.Codigo,
                    CodPeso = inventarioAtividadeModelo.CodPeso,
                    CodPerfilCatalogo = inventarioAtividadeModelo.CodPerfilCatalogo,
                    CodDuracao = inventarioAtividadeModelo.CodDuracao,
                    CodAtividade = inventarioAtividadeModelo.CodAtividade,
                    CodDisciplina = inventarioAtividadeModelo.CodDisciplina,
                    Descricao = inventarioAtividadeModelo.Descricao,
                    RiscoGeral = inventarioAtividadeModelo.RiscoGeral,
                    ObservacaoGeral = inventarioAtividadeModelo.ObservacaoGeral,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                };

                entities.INVENTARIO_ATIVIDADE.Add(inventario);
                entities.SaveChanges();

                entities.ChangeTracker.DetectChanges();
                long idInv = inventario.CodInventarioAtividade;

                if (eEdicao == false)
                {
                    inventario.Codigo = $"INV_ATV - {idInv}";
                    entities.SaveChanges();

                    entities.ChangeTracker.DetectChanges();
                }

                if (localInstalacao.Count == 0)
                {
                    throw new Exception("Existem inventários sem local de instalação a serem inseridos.");
                }

                if (localInstalacao != null)
                {
                    foreach (var li in localInstalacao)
                    {
                        entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Add(new LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE()
                        {
                            CodInventarioAtividade = idInv,
                            CodLocalInstalacao = li.CodLocalInstalacao,
                            Ativo = true
                        });
                    }
                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_INVENTARIO_ATIVIDADE()
                        {
                            CodInventarioAtividade = idInv,
                            CodRisco = risco.CodRisco,
                            CodSeveridade = risco.CodSeveridade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentoAplicavel = risco.ProcedimentoAplicavel,
                            ContraMedidas = risco.ContraMedidas,
                            Ativo = true
                        };
                        entities.RISCO_INVENTARIO_ATIVIDADE.Add(novoRisco);
                        entities.SaveChanges();

                        foreach (var epi in risco.EPIRiscoInventarioAtividadeModelo)
                        {
                            entities.EPI_RISCO_INVENTARIO_ATIVIDADE.Add(new EPI_RISCO_INVENTARIO_ATIVIDADE()
                            {
                                CodEPI = epi.CodEPI,
                                CodRiscoInventarioAtividade = novoRisco.CodRiscoInventarioAtividade
                            });
                        }
                    }
                    entities.SaveChanges();
                }
                return inventario;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public INVENTARIO_ATIVIDADE Inserir(InventarioAtividadeModelo inventarioAtividadeModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInsercao = null)
        {
            List<RiscoInventarioAtividadeModelo> riscos = inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Configuration.AutoDetectChangesEnabled = false;

            try
            {
                var inventario = new INVENTARIO_ATIVIDADE()
                {
                    Codigo = $"INV_ATV - {inventarioAtividadeModelo.CodInventarioAtividade}",
                    CodPeso = inventarioAtividadeModelo.CodPeso,
                    CodPerfilCatalogo = inventarioAtividadeModelo.CodPerfilCatalogo,
                    CodDuracao = inventarioAtividadeModelo.CodDuracao,
                    CodAtividade = inventarioAtividadeModelo.CodAtividade,
                    CodDisciplina = inventarioAtividadeModelo.CodDisciplina,
                    Descricao = inventarioAtividadeModelo.Descricao,
                    RiscoGeral = inventarioAtividadeModelo.RiscoGeral,
                    ObservacaoGeral = inventarioAtividadeModelo.ObservacaoGeral,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                };

                entities.INVENTARIO_ATIVIDADE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodInventarioAtividade;

                inventario.Codigo = $"INV_ATV - {idInv}";
                entities.SaveChanges();

                if (locaisInsercao != null)
                {
                    foreach (var local in locaisInsercao)
                    {
                        entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Add(new LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE()
                        {
                            CodInventarioAtividade = idInv,
                            CodLocalInstalacao = local.CodLocalInstalacao,
                            Ativo = true
                        });
                    }
                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_INVENTARIO_ATIVIDADE()
                        {
                            CodInventarioAtividade = idInv,
                            CodRisco = risco.CodRisco,
                            CodSeveridade = risco.CodSeveridade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentoAplicavel = risco.ProcedimentoAplicavel,
                            ContraMedidas = risco.ContraMedidas,
                            Ativo = true
                        };
                        entities.RISCO_INVENTARIO_ATIVIDADE.Add(novoRisco);
                        entities.SaveChanges();


                        if (risco.EPIRiscoInventarioAtividadeModelo.Count > 0)
                        {
                            foreach (var epi in risco.EPIRiscoInventarioAtividadeModelo)
                            {
                                entities.EPI_RISCO_INVENTARIO_ATIVIDADE.Add(new EPI_RISCO_INVENTARIO_ATIVIDADE()
                                {
                                    CodEPI = epi.CodEPI,
                                    CodRiscoInventarioAtividade = novoRisco.CodRiscoInventarioAtividade
                                });
                            }
                            entities.SaveChanges();
                        }
                    }
                }

                entities.ChangeTracker.DetectChanges();
                entities.SaveChanges();

                return inventario;
            }

            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                entities.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void InserirInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo, List<LOCAL_INSTALACAO> locais = null)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.Inserir(inventarioAtividadeModelo, entities);
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

        public long ListarCodAprPorInventario(long codInventario, long codAtividade, long codDiscipllina, long codLocal, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = (
                from inv in entities.INVENTARIO_ATIVIDADE
                join li in entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE on inv.CodInventarioAtividade equals li.CodInventarioAtividade
                join op in entities.OPERACAO_APR on li.CodLocalInstalacao equals op.CodLI
                join apr in entities.APR on op.CodAPR equals apr.CodAPR
                where inv.CodInventarioAtividade == codInventario && op.CodAtvPadrao == codAtividade && op.CodDisciplina == codDiscipllina && op.CodLI == codLocal
                select apr.CodAPR).FirstOrDefault();

            return resultado;
        }

        public long ListarCodAprPorInventarioTela(long codInventario, long codAtividade, long codDiscipllina, long codLocal, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = (
                from inv in entities.INVENTARIO_ATIVIDADE
                join li in entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE on inv.CodInventarioAtividade equals li.CodInventarioAtividade
                join op in entities.OPERACAO_APR on li.CodLocalInstalacao equals op.CodLI
                join apr in entities.APR on op.CodAPR equals apr.CodAPR
                where inv.CodInventarioAtividade == codInventario && op.CodAtvPadrao == codAtividade && op.CodDisciplina == codDiscipllina && op.CodLI == codLocal && apr.Ativo
                select apr.CodAPR).FirstOrDefault();

            return resultado;
        }

        // update
        public INVENTARIO_ATIVIDADE EditarInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo,
            DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao)
        {
            INVENTARIO_ATIVIDADE inventarioAtividadeExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            inventarioAtividadeExistente =
                entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == inventarioAtividadeModelo.CodInventarioAtividade).FirstOrDefault();

            if (inventarioAtividadeExistente == null)
                throw new KeyNotFoundException();

            if (inventarioAtividadeModelo.Ativo)
            {
                inventarioAtividadeExistente.Descricao = inventarioAtividadeModelo.Descricao;
                inventarioAtividadeExistente.ObservacaoGeral = inventarioAtividadeModelo.ObservacaoGeral;

                entities.SaveChanges();
            }
            else
            {
                try
                {
                    DesativarInventario(inventarioAtividadeExistente.CodInventarioAtividade, entities);
                    inventarioAtividadeExistente = Inserir(inventarioAtividadeModelo, entities, locaisInstalacao);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return inventarioAtividadeExistente;
        }


    public INVENTARIO_ATIVIDADE ListarInventarioDeAtividadePorCodigo(string codigo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
            {
                entities = new DB_LaborSafetyEntities();
            }
            var inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.Codigo == codigo && invAtv.Ativo).FirstOrDefault();
            if (inventarioAtividadeExistente == null)
            {
                return null;
            }
            return ListarInventarioAtividadePorId(inventarioAtividadeExistente.CodInventarioAtividade);
        }


        public INVENTARIO_ATIVIDADE EditarInventarioAtividadePorImportacao(InventarioAtividadeModelo inventarioAtividadeModelo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
            {
                entities = new DB_LaborSafetyEntities();
            }
            var inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.Codigo == inventarioAtividadeModelo.Codigo && invAtv.Ativo).FirstOrDefault();
            if (inventarioAtividadeExistente == null)
            {
                throw new KeyNotFoundException();
            }
            DesativarInventario(inventarioAtividadeExistente.CodInventarioAtividade,entities);
            var result = InserirImportacao(inventarioAtividadeModelo,entities, true);
            return result;
        }


        public void DesativarInventario(long codInventarioExistente, DB_LaborSafetyEntities entities)
        {
            INVENTARIO_ATIVIDADE inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == codInventarioExistente).FirstOrDefault();
            List<RISCO_INVENTARIO_ATIVIDADE> riscoExistente = entities.RISCO_INVENTARIO_ATIVIDADE.Where(risco => risco.CodInventarioAtividade == codInventarioExistente).ToList();
            List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> localExistente = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(local => local.CodInventarioAtividade == codInventarioExistente).ToList();

            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                List<long> CodApr = new List<long>();
                long inventarioPorApr;

                if (inventarioAtividadeExistente == null)
                    throw new Exception("Ocorreu um erro ao listar inventário de atividade");

                foreach (var itemLocal in localExistente)
                {
                    inventarioPorApr = ListarCodAprPorInventario(codInventarioExistente, inventarioAtividadeExistente.CodAtividade, inventarioAtividadeExistente.CodDisciplina, itemLocal.CodLocalInstalacao, entities);

                    if(inventarioPorApr > 0)
                    CodApr.Add(inventarioPorApr);
                }

                if (CodApr.Count > 0)
                {
                    foreach (var itemCodAPR in CodApr)
                    {
                        InserirLocalInstalacaoHistorico(itemCodAPR, localExistente, entities);
                    }
                }

                inventarioAtividadeExistente.Ativo = false;

                foreach (var item in riscoExistente)
                    item.Ativo = false;

                foreach (var item in localExistente)
                    item.Ativo = false;

                entities.SaveChanges();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void InserirLocalInstalacaoHistorico(long codAPR, List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> locais, DB_LaborSafetyEntities entities)
        {
            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                foreach (var item in locais)
                {
                    var localComOperacao = entities.OPERACAO_APR.Where(x => x.CodLI == item.CodLocalInstalacao).FirstOrDefault();

                    if (localComOperacao != null)
                    {
                        var localInstalacao = new LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR()
                        {
                            CodInventarioAtividade = item.CodInventarioAtividade,
                            CodLocalInstalacao = item.CodLocalInstalacao,
                            CodApr = codAPR,
                            Ativo = true
                        };
                        entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR.Add(localInstalacao);
                        entities.SaveChanges();
                    }
                }

                APR apr = entities.APR.Where(x => x.CodAPR == codAPR).FirstOrDefault();
                apr.Ativo = false;
                entities.SaveChanges();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void EditarLocalInstalacaoInventarioAtividade(long idInventario, long idLi)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE inventarioAtividadeExistente = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == idInventario && invAtv.Ativo).FirstOrDefault();
                LOCAL_INSTALACAO liInventarioAtividadeExistente = entities.LOCAL_INSTALACAO.FirstOrDefault(invAtv => invAtv.CodLocalInstalacao == idLi);

                if (inventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else if (liInventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    inventarioAtividadeExistente.CodLocalInstalacao = idLi;
                }

                entities.SaveChanges();
            }
        }

        public void EditarRiscoInventarioAtividade(long idInventario, long idRisco)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                RISCO_INVENTARIO_ATIVIDADE inventarioAtividadeExistente = entities.RISCO_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == idInventario && invAtv.Ativo).FirstOrDefault();
                RISCO riscoInventarioAtividadeExistente = entities.RISCO.FirstOrDefault(invAtv => invAtv.CodRisco == idRisco);

                if (inventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else if (riscoInventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    inventarioAtividadeExistente.CodRisco = idRisco;
                }

                entities.SaveChanges();
            }
        }

        public void EditarResponsavelInventarioAtividade(long idInventario, long idResponsavel)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                RESPONSAVEL_INVENTARIO_ATIVIDADE inventarioAtividadeExistente = entities.RESPONSAVEL_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == idInventario).FirstOrDefault();
                PESSOA responsavelInventarioAtividadeExistente = entities.PESSOA.FirstOrDefault(invAtv => invAtv.CodPessoa == idResponsavel);

                if (inventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else if (responsavelInventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    inventarioAtividadeExistente.CodUsuario = idResponsavel;
                }

                entities.SaveChanges();
            }
        }

        public void ExcluirInventarioAtividade(long id)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                INVENTARIO_ATIVIDADE inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == id).FirstOrDefault();
                List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> localExistente = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Where(local => local.CodInventarioAtividade == inventarioAtividadeExistente.CodInventarioAtividade).ToList();

                entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.RemoveRange(
                    entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE
                        .Where(local => local.CodInventarioAtividade == id).ToList());

                entities.RISCO_INVENTARIO_ATIVIDADE.RemoveRange(
                    entities.RISCO_INVENTARIO_ATIVIDADE
                        .Where(risc => risc.CodInventarioAtividade == id).ToList());

                var delInv = entities.INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodInventarioAtividade == id).FirstOrDefault();

                entities.INVENTARIO_ATIVIDADE.Remove(delInv);

                entities.SaveChanges();
            }
        }
    }
}