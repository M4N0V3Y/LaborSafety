using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class IntegracaoNegocio : IIntegracaoNegocio
    {
        private readonly IIntegracaoPersistencia persistencia;

        public IntegracaoNegocio(IIntegracaoPersistencia nrPersistencia)
        {
            this.persistencia = nrPersistencia;
        }

        public void ProcessaDisciplina(IntegracaoModelo disciplina)
        {
            this.persistencia.ProcessaCaracteristica(disciplina);
        }

        public void ProcessaAtividadePadrao(IntegracaoModelo atividadePadrao)
        {
            this.persistencia.ProcessaAtividadePadrao(atividadePadrao);
        }

        public void ProcessaPerfilCatalogo(IntegracaoModelo perfilCatalogo)
        {
            this.persistencia.ProcessaPerfilCatalogo(perfilCatalogo);
        }
    }
}
