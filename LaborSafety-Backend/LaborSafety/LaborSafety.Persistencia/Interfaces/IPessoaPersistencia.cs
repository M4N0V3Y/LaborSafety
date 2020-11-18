using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IPessoaPersistencia
    {
        bool Inserir(PessoaModelo pessoa);
        void Editar(PessoaModelo pessoa);
        List<PESSOA> Listar();
        long ValidarPessoaComAprAssociada(long codPessoa);
        PESSOA ListarPorCodigo(long codigo);
        PESSOA ListarPorCpf(string cpf);
        void Excluir(long codPessoa);
        List<PESSOA> ListarPorCodigos(List<long> codigo);
    }
}

