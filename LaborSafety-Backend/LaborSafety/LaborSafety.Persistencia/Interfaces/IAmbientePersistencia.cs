using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IAmbientePersistencia
    {
        AMBIENTE Inserir(AmbienteModelo ambienteModelo);
        AMBIENTE Editar(AmbienteModelo ambienteModelo);
        AMBIENTE ListarSistemaOperacionalPorId(long id);
        AMBIENTE ListarSistemaOperacionalPorNome(string nome);
        IEnumerable<AMBIENTE> ListarTodosSOs();
        AMBIENTE ListarAmbienteSemInventarioAmbienteVinculado(long id);
        void DesativarAmbiente(long codAmbienteExistente);
    }
}
