using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IAtividadePadraoPersistencia
    {
        void Inserir(AtividadePadraoModelo modelo);
        void Editar(AtividadePadraoModelo modelo);
        void ExcluirAtividade(AtividadePadraoModelo modelo);
        ATIVIDADE_PADRAO ListarAtividadePorNome(string nome, DB_APRPTEntities entities = null);
        ATIVIDADE_PADRAO ListarAtividadePorId(long id, DB_APRPTEntities entities = null);
        IEnumerable<ATIVIDADE_PADRAO> ListarTodasAtividades();
        List<ATIVIDADE_PADRAO> ListarTodasAtividadesExportacao(List<long> atividades);
    }
}
