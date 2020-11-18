using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class IntegracaoPersistencia : IIntegracaoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public IntegracaoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void ProcessaCaracteristica(IntegracaoModelo caracteristica)
        {
            try
            {
                if (caracteristica == null)
                    throw new Exception("Objeto característica não informada!");

                if(caracteristica.Status == '\0')
                    throw new Exception("Status não informado!");

                DisciplinaPersistencia persistencia = new DisciplinaPersistencia(this.databaseEntities);
                
                DisciplinaModelo modelo = new DisciplinaModelo();
                modelo.Nome = caracteristica.Nome;
                modelo.Descricao = caracteristica.Valor;

                //Verifica o tipo de operação
                if (caracteristica.Status == Constantes.StatusIntegracao.I.ToString()[0])
                    persistencia.Inserir(modelo);
                else if (caracteristica.Status == Constantes.StatusIntegracao.U.ToString()[0])
                    persistencia.Editar(modelo);
                else if (caracteristica.Status == Constantes.StatusIntegracao.E.ToString()[0])
                    persistencia.ExcluirDisciplina(modelo);
                else
                    throw new Exception($"O status '{caracteristica.Status}' informado é inválido!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void ProcessaAtividadePadrao(IntegracaoModelo chaveDeModelo)
        {
            try
            {
                if (chaveDeModelo == null)
                    throw new Exception("Objeto chave de modelo não informado!");

                if (chaveDeModelo.Status == '\0')
                    throw new Exception("Operação não informada!");

                AtividadePadraoPersistencia persistencia = new AtividadePadraoPersistencia(this.databaseEntities);

                AtividadePadraoModelo modelo = new AtividadePadraoModelo();
                modelo.Nome = chaveDeModelo.Nome;
                modelo.Descricao = chaveDeModelo.Valor;

                //Verifica o tipo de operação
                if (chaveDeModelo.Status == Constantes.StatusIntegracao.I.ToString()[0])
                    persistencia.Inserir(modelo);
                else if (chaveDeModelo.Status == Constantes.StatusIntegracao.U.ToString()[0])
                    persistencia.Editar(modelo);
                else if (chaveDeModelo.Status == Constantes.StatusIntegracao.E.ToString()[0])
                    persistencia.ExcluirAtividade(modelo);
                else
                    throw new Exception($"A operação '{chaveDeModelo.Status}' informálida é inválida!");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessaPerfilCatalogo(IntegracaoModelo perfilCatalogo)
        {
            try
            {
                
                if (perfilCatalogo == null)
                    throw new Exception("Objeto perfil de catalogo não informado!");

                if (perfilCatalogo.Status == '\0')
                    throw new Exception("Operação não informada!");

                PerfilCatalogoPersistencia persistencia = new PerfilCatalogoPersistencia(this.databaseEntities);

                PerfilCatalogoModelo modelo = new PerfilCatalogoModelo();
                modelo.Codigo = perfilCatalogo.Nome;
                modelo.Nome = perfilCatalogo.Valor;

                //Verifica o tipo de operação
                if (perfilCatalogo.Status == Constantes.StatusIntegracao.I.ToString()[0])
                    persistencia.Inserir(modelo);
                else if (perfilCatalogo.Status == Constantes.StatusIntegracao.U.ToString()[0])
                    persistencia.Editar(modelo);
                else if (perfilCatalogo.Status == Constantes.StatusIntegracao.E.ToString()[0])
                    persistencia.Excluir(modelo);
                else
                    throw new Exception($"A operação '{perfilCatalogo.Status}' informálida é inválida!");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
