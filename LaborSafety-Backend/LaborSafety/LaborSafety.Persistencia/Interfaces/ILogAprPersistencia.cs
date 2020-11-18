using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ILogAprPersistencia
    {
        LOG_APR Inserir(AprModelo apr, long codAprInserido, DB_APRPTEntities entities = null);
        LOG_APR Editar(APR aprExistente, AprModelo aprModelo, APR novaApr, DB_APRPTEntities entities = null);
        LOG_APR Excluir(AprDelecaoComLogModelo aprDelecaoComLogModelo, DB_APRPTEntities entities = null);
        List<LOG_APR> ListarLogApr(long codApr, DB_APRPTEntities entities = null);
        List<LOG_APR> BuscarTodos(DB_APRPTEntities entities = null);
        List<LOG_APR> BuscarPorInventario(long codApr, DB_APRPTEntities entities = null);
        List<LOG_OPERACAO_APR> ListarLogOperacaoApr(long codApr, DB_APRPTEntities entities = null);
        void InserirLogOperacaoApr(APR aprAntiga, long codLogApr, DB_APRPTEntities entities = null);
    }
}
