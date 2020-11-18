using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IPessoaNegocio
    {
        bool Inserir(PessoaModelo pessoa);
        void Editar(PessoaModelo pessoa);
        List<PessoaModelo> Listar();
        PessoaModelo ListarPorCodigo(long codigo);
        PessoaModelo ListarPorCpf(string cpf);
        IEnumerable<PessoaModelo> ListarPorCodigos(List<long> codigos);
        void Excluir(long codPessoa);
    }
}
