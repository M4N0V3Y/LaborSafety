using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;
using static LaborSafety.Negocio.Servicos.InventarioAmbienteNegocio;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IInventariosAmbienteNegocio
    {
        InventarioAmbienteModelo ListarInventarioAmbientePorId(long id);
        long ListarCodAprPorInventario(long codInventario);
        IEnumerable<InventarioAmbienteModelo> ListarInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo);
        InventarioAmbienteModelo ListarInventarioAmbientePorLI(long li);
        int CalcularRiscoAmbiente(int probabilidade, decimal severidade);
        int CalcularRiscoTotalLista(int codProbabilidade, int codSeveridade);
        int CalcularRiscoTotalTela(RiscoTotalAmbienteModelo riscoTotalAmbienteModelo);
        RetornoInsercao InserirInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo);
        void EditarInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo);
        void EditarNrInventarioAmbiente(long idInventario, long idNr);
        void EditarRiscoInventarioAmbiente(long idInventario, long idRisco);
        void ExcluirInventarioAmbiente(long id);
        void DesativarInventario(InventarioAmbienteDelecaoComLogModelo inventarioAmbienteDelecaoComLogModelo, DB_LaborSafetyEntities entities);
        long ListarCodAprPorInventarioTela(long codInventario);
        ResultadoImportacao ImportarPlanilha(string caminho, string eightId);
        ArquivoLog EscreverLogEmTxt(List<long> codInventariosAmbiente);
        ArquivoLog EscreverLogTodosInventarios();
    }
}
