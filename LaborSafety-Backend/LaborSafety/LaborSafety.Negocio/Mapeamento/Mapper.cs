using AutoMapper;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;

namespace LaborSafety.Negocio.Mapeamento
{
    public class Mapper : Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(mapper => { mapper.AddProfile<Mapper>(); });
        }

        public Mapper()
        {
            CreateMap<APR, AprModelo>();
            CreateMap<AprModelo, APR>();

            CreateMap<RISCO_APR, RiscoAprModelo>();
            CreateMap<RiscoAprModelo, RISCO_APR>();

            //CreateMap<FOLHA_ANEXO_APR, FolhaAnexoAprModelo>();
            //CreateMap<FolhaAnexoAprModelo, FOLHA_ANEXO_APR>();

            CreateMap<OPERACAO_APR, OperacaoAprModelo>();
            CreateMap<OperacaoAprModelo, OPERACAO_APR>();

            CreateMap<APROVADOR_APR, AprovadorAprModelo>();
            CreateMap<AprovadorAprModelo, APROVADOR_APR>();

            CreateMap<EXECUTANTE_APR, ExecutanteAprModelo>();
            CreateMap<ExecutanteAprModelo, EXECUTANTE_APR>();

            CreateMap<FUNCIONALIDADE, FuncionalidadeModelo>();
            CreateMap<FuncionalidadeModelo, FUNCIONALIDADE>();

            CreateMap<TELA, TelaModelo>();
            CreateMap<TelaModelo, TELA>();

            CreateMap<PESSOA, PessoaModelo>();
            CreateMap<PessoaModelo, PESSOA>();

            CreateMap<PERFIL, PerfilModelo>();
            CreateMap<PerfilModelo, PERFIL>();

            CreateMap<INVENTARIO_AMBIENTE, InventarioAmbienteModelo>();
            CreateMap<InventarioAmbienteModelo, INVENTARIO_AMBIENTE>();

            CreateMap<INVENTARIO_ATIVIDADE, InventarioAtividadeModelo>();
            CreateMap<InventarioAtividadeModelo, INVENTARIO_ATIVIDADE>();
                       
            CreateMap<LOCAL_INSTALACAO, LocalInstalacaoModelo>();
            CreateMap<LocalInstalacaoModelo, LOCAL_INSTALACAO>();

            CreateMap<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE, LocalInstalacaoInventarioAtividadeModelo>();
            CreateMap<LocalInstalacaoInventarioAtividadeModelo, LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE>();

            CreateMap<LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE, LocalInstalacaoRascunhoInventarioAtividadeModelo>();
            CreateMap<LocalInstalacaoRascunhoInventarioAtividadeModelo, LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE>();

            CreateMap<EPI_RISCO_INVENTARIO_AMBIENTE, EPIRiscoInventarioAmbienteModelo>();
            CreateMap<EPIRiscoInventarioAmbienteModelo, EPI_RISCO_INVENTARIO_AMBIENTE>();

            CreateMap<EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE, EPIRiscoRascunhoInventarioAmbienteModelo>();
            CreateMap<EPIRiscoRascunhoInventarioAmbienteModelo, EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE>();

            CreateMap<EPI_RISCO_INVENTARIO_ATIVIDADE, EPIRiscoInventarioAtividadeModelo>();
            CreateMap<EPIRiscoInventarioAtividadeModelo, EPI_RISCO_INVENTARIO_ATIVIDADE>();

            CreateMap<EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE, EPIRiscoRascunhoInventarioAtividadeModelo>();
            CreateMap<EPIRiscoRascunhoInventarioAtividadeModelo, EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE>();

            CreateMap<EPI, EPIModelo>();
            CreateMap<EPIModelo, EPI>();

            CreateMap<RISCO_INVENTARIO_ATIVIDADE, RiscoInventarioAtividadeModelo>();
            CreateMap<RiscoInventarioAtividadeModelo, RISCO_INVENTARIO_ATIVIDADE>();

            CreateMap<RISCO_INVENTARIO_AMBIENTE, RiscoInventarioAmbienteModelo>();
            CreateMap<RiscoInventarioAmbienteModelo, RISCO_INVENTARIO_AMBIENTE>();

            CreateMap<RISCO, RiscoModelo>();
            CreateMap<RiscoModelo, RISCO>();

            CreateMap<TIPO_RISCO, TipoRiscoModelo>();
            CreateMap<TipoRiscoModelo, TIPO_RISCO>();

            CreateMap<RESPONSAVEL_INVENTARIO_AMBIENTE, ResponsavelInventarioAmbienteModelo>();
            CreateMap<ResponsavelInventarioAmbienteModelo, RESPONSAVEL_INVENTARIO_AMBIENTE>();

            CreateMap<RESPONSAVEL_INVENTARIO_ATIVIDADE, ResponsavelInventarioAtividadeModelo>();
            CreateMap<ResponsavelInventarioAtividadeModelo, RESPONSAVEL_INVENTARIO_ATIVIDADE>();

            CreateMap<SEVERIDADE, SeveridadeModelo>();
            CreateMap<SeveridadeModelo, SEVERIDADE>();

            CreateMap<DURACAO, DuracaoModelo>();
            CreateMap<DuracaoModelo, DURACAO>();

            CreateMap<NR, NrModelo>();
            CreateMap<NrModelo, NR>();

            CreateMap<NR_INVENTARIO_AMBIENTE, NrInventarioAmbienteModelo>();
            CreateMap<NrInventarioAmbienteModelo, NR_INVENTARIO_AMBIENTE>();

            CreateMap<NR_RASCUNHO_INVENTARIO_AMBIENTE, NrRascunhoInventarioAmbienteModelo>();
            CreateMap<NrRascunhoInventarioAmbienteModelo, NR_RASCUNHO_INVENTARIO_AMBIENTE>();

            CreateMap<PERFIL_CATALOGO, PerfilCatalogoModelo>();
            CreateMap<PerfilCatalogoModelo, PERFIL_CATALOGO>();

            CreateMap<ATIVIDADE_PADRAO, AtividadePadraoModelo>();
            CreateMap<AtividadePadraoModelo, ATIVIDADE_PADRAO>();

            CreateMap<AMBIENTE, AmbienteModelo>();
            CreateMap<AmbienteModelo, AMBIENTE>();

            CreateMap<PESO, PesoModelo>();
            CreateMap<PesoModelo, PESO>();

            CreateMap<PROBABILIDADE, ProbabilidadeModelo>();
            CreateMap<ProbabilidadeModelo, PROBABILIDADE>();

            CreateMap<RISCO_RASCUNHO_INVENTARIO_AMBIENTE, RiscoRascunhoInventarioAmbienteModelo>();
            CreateMap<RiscoRascunhoInventarioAmbienteModelo, RISCO_RASCUNHO_INVENTARIO_AMBIENTE>();

            CreateMap<RISCO_RASCUNHO_INVENTARIO_ATIVIDADE, RiscoRascunhoInventarioAtividadeModelo>();
            CreateMap<RiscoRascunhoInventarioAtividadeModelo, RISCO_RASCUNHO_INVENTARIO_ATIVIDADE>();

            CreateMap<RASCUNHO_INVENTARIO_AMBIENTE, RascunhoInventarioAmbienteModelo>();
            CreateMap<RascunhoInventarioAmbienteModelo, RASCUNHO_INVENTARIO_AMBIENTE>();

            CreateMap<RASCUNHO_INVENTARIO_ATIVIDADE, RascunhoInventarioAtividadeModelo>();
            CreateMap<RascunhoInventarioAtividadeModelo, RASCUNHO_INVENTARIO_ATIVIDADE>();

            CreateMap<BLOQUEIO, BloqueioModelo>();
            CreateMap<BloqueioModelo, BLOQUEIO>();

            CreateMap<LOCAL_A_BLOQUEAR, LocalABloquearModelo>();
            CreateMap<LocalABloquearModelo, LOCAL_A_BLOQUEAR>();

            CreateMap<AREA, AreaModelo>();
            CreateMap<AreaModelo, AREA>();

            CreateMap<TAG_KKS_BLOQUEIO, Tag_Kks_Modelo>();
            CreateMap<Tag_Kks_Modelo, TAG_KKS_BLOQUEIO>();

            CreateMap<DISPOSITIVO_BLOQUEIO, DispositivoBloqueioModelo>();
            CreateMap<DispositivoBloqueioModelo, DISPOSITIVO_BLOQUEIO>();

            CreateMap<TIPO_ENERGIA_BLOQUEIO, TipoEnergiaBloqueioModelo>();
            CreateMap<TipoEnergiaBloqueioModelo, TIPO_ENERGIA_BLOQUEIO>();

            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMissingTypeMaps = true;
        }
    }
}
