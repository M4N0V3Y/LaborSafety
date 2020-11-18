using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IAmbienteNegocio
    {
        AmbienteModelo ListarSistemaOperacionalPorId(long id);
        AmbienteModelo ListarSistemaOperacionalPorNome(string nome);
        IEnumerable<AmbienteModelo> ListarTodosSOs();
        AmbienteModelo Inserir(AmbienteModelo ambienteModelo);
        AmbienteModelo Editar(AmbienteModelo ambienteModelo);
        void DesativarAmbiente(long codAmbienteExistente);
    }
}
