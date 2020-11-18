
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Validadores.Interface
{
    public interface Validador<T>
    {
        void ValidaInsercao(T insercao);
        void ValidaEdicao(T edicao);
    }
}
