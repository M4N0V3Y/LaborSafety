using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;
using static LaborSafety.Negocio.Servicos.InventarioAtividadeNegocio;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IInventariosAtividadeNegocio 
    {
        InventarioAtividadeModelo ListarInventarioAtividadePorId(long id);
        long ListarCodAprPorInventario(long codInventario);
        List<InventarioAtividadeModelo> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo);
        int CalcularRiscoAtividade(decimal numSeveridade, int numArt, int numDa, int numPa);
        int CalcularRiscoTotalLista(int codSeveridade, int codRisco, int codAtividade, long codDisciplina, int codDuracao, int codPeso);
        int CalcularRiscoTotalTela(RiscoTotalAtividadeModelo riscoTotalAtividadeModelo);
        void InserirInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo);
        void EditarInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo);
        void EditarLocalInstalacaoInventarioAtividade(long idInventario, long idlI);
        void EditarRiscoInventarioAtividade(long idInventario, long idRisco);
        void EditarResponsavelInventarioAtividade(long idInventario, long idResponsavel);
        void DesativarInventario(InventarioAtividadeDelecaoComLogModelo inventarioAtividadeDelecao, DB_LaborSafetyEntities entities);
        void ExcluirInventarioAtividade(long id);
        ResultadoImportacao ImportarPlanilha(string path, string eightId);
        ArquivoLog EscreverLogEmTxt(List<long> codInventariosAtividade);
        void InserirInventarioAtividadeImportacaoLog(List<InventarioAtividadeModelo> inventarios, string eightId);
        void EditarInventarioAtividadeImportacaoLog(List<InventarioAtividadeModelo> inventarios, string eightId);
        long ListarCodAprPorInventarioTela(long codInventario);

        ArquivoLog EscreverLogTodosInventarios();
    }
}
