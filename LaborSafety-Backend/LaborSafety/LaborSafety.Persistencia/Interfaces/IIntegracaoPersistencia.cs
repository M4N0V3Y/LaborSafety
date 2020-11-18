using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IIntegracaoPersistencia
    {
        void ProcessaCaracteristica(IntegracaoModelo disciplina);
        void ProcessaAtividadePadrao(IntegracaoModelo atividadePadrao);
        void ProcessaPerfilCatalogo(IntegracaoModelo perfilCatalogo);
    }
}
