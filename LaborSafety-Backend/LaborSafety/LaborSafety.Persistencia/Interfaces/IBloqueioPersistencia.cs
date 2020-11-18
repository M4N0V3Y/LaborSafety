using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IBloqueioPersistencia
    {
        BLOQUEIO InserirBloqueio(BLOQUEIO bloqueio, DB_APRPTEntities entities = null);
        BLOQUEIO_LOCAL_INSTALACAO InserirBloqueioLocalInstalacao(long codBloqueio, long codLocalInstalacao, string codigoBloqueioLocal,
             DB_APRPTEntities entities = null);
        BLOQUEIO_LOCAL_INSTALACAO ListarBloqueioLocalInstalacaoPorNomeELocal(string codigoBloqueioLocal, long codLocal, 
            DB_APRPTEntities entities = null);

        BLOQUEIO ListarBLoqueioPorCodigo(long codBloqueio, DB_APRPTEntities entities = null);

        void ApagarBloqueioLocal(long CodLocal, DB_APRPTEntities entities = null);
        BLOQUEIO EditaBloqueio(BLOQUEIO modelo, DB_APRPTEntities entities);

        AREA InserirAreaBloqueio(AREA area, DB_APRPTEntities entities);
        AREA EditarAreaBloqueio(AREA area, DB_APRPTEntities entities);
        DISPOSITIVO_BLOQUEIO InserirDispositivoBloqueio(DISPOSITIVO_BLOQUEIO dispositivo, DB_APRPTEntities entities);
        DISPOSITIVO_BLOQUEIO EditarDispositivoBloqueio(DISPOSITIVO_BLOQUEIO dispositivoBloqueio, DB_APRPTEntities entities);
        TAG_KKS_BLOQUEIO InserirTagKKSBloqueio(TAG_KKS_BLOQUEIO tagKKSBloqueio, DB_APRPTEntities entities);
        TAG_KKS_BLOQUEIO EditarTagKKS(TAG_KKS_BLOQUEIO tagKKSBloqueio, DB_APRPTEntities entities);
        TIPO_ENERGIA_BLOQUEIO InserirTipoEnergia(TIPO_ENERGIA_BLOQUEIO tipoEnergiaBloqueio, DB_APRPTEntities entities);
        TIPO_ENERGIA_BLOQUEIO EditarTipoEnergia(TIPO_ENERGIA_BLOQUEIO tipoEnergia, DB_APRPTEntities entities);
        LOCAL_A_BLOQUEAR InserirLocalABloquear(LOCAL_A_BLOQUEAR localABloquear, DB_APRPTEntities entities);
        LOCAL_A_BLOQUEAR EditarLocalABloquear(LOCAL_A_BLOQUEAR localABloquear, DB_APRPTEntities entities);
        List<BLOQUEIO> ListarBloqueioPorListaLIPorArea(List<string> locais);
        TIPO_ENERGIA_BLOQUEIO ListarTipoEnergia(long idTipoEnergia, DB_APRPTEntities entities);
        TAG_KKS_BLOQUEIO ListarTagKKS(long idTagKKS, DB_APRPTEntities entities);
        DISPOSITIVO_BLOQUEIO ListarDispositivo(long idDispositivo, DB_APRPTEntities entities);
        LOCAL_A_BLOQUEAR ListarLocalABloquear(long idLocalABloquear, DB_APRPTEntities entities);
        AREA ListaAreaPorNome(string nome, DB_APRPTEntities entities);
        DISPOSITIVO_BLOQUEIO ListaDispositivoBloqueioNome(string nome, DB_APRPTEntities entities);
        TAG_KKS_BLOQUEIO ListaTagKKSPorNome(string nome, DB_APRPTEntities entities);
        TIPO_ENERGIA_BLOQUEIO ListaTipoEnergiaBloqueioNome(string nome, DB_APRPTEntities entities);
        LOCAL_A_BLOQUEAR ListaLocalABloquearPorNome(string nome, DB_APRPTEntities entities);
        List<BLOQUEIO> ListarBloqueioPorListaLIPorAreaLong(List<long> locais);
    }
}
