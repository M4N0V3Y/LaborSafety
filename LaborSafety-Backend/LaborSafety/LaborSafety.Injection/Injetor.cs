using Ternium.LaborSafety.Dominio.Modelos;
using Ternium.LaborSafety.Negocio.Interfaces;
using Ternium.LaborSafety.Negocio.Interfaces.Exportacao;
using Ternium.LaborSafety.Negocio.Interfaces.SAP;
using Ternium.LaborSafety.Negocio.Servicos;
using Ternium.LaborSafety.Negocio.Servicos.Exportacao;
using Ternium.LaborSafety.Negocio.Servicos.SAP;
using Ternium.LaborSafety.Negocio.Validadores;
using Ternium.LaborSafety.Negocio.Validadores.Interface;
using Ternium.LaborSafety.Persistencia;
using Ternium.LaborSafety.Persistencia.Interfaces;
using Ternium.LaborSafety.Persistencia.Servicos;
using Unity;

namespace Ternium.LaborSafety.Injection
{
    public static class Injetor
    {
        public static IUnityContainer ConstrutorContainer;

        public static void Inicializar(IUnityContainer container)
        {
            ConstrutorContainer = container;
            ConfigurarInjectorDbContext();
            ConfigurarInjecoesNegocio();
            ConfigurarInjecoesPersistencia();
        }

        private static void ConfigurarInjectorDbContext()
        {
            ConstrutorContainer.RegisterType<IDbLaborSafetyEntities, InjectionDbContext>();
        }

        private static void ConfigurarInjecoesPersistencia()
        {
            // Injeção camada de persistência
            ConstrutorContainer.RegisterType<IAprPersistencia, AprPersistencia>();
            ConstrutorContainer.RegisterType<IFuncionalidadesPersistencia, FuncionalidadePersistencia>();
            ConstrutorContainer.RegisterType<IPerfisPersistencia, PerfisPersistencia>();
            ConstrutorContainer.RegisterType<IPessoaPersistencia, PessoaPersistencia>();
            ConstrutorContainer.RegisterType<IInventariosAmbientePersistencia, InventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<IMapeamentoLocalInventarioAmbientePersistencia, MapeamentoLocalInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<IInventariosAtividadePersistencia, InventarioAtividadePersistencia>();
            ConstrutorContainer.RegisterType<IAmbientePersistencia, AmbientePersistencia>();
            ConstrutorContainer.RegisterType<ILocalInstalacaoPersistencia, LocalInstalacaoPersistencia>();
            ConstrutorContainer.RegisterType<IDisciplinaPersistencia, DisciplinaPersistencia>();
            ConstrutorContainer.RegisterType<IAtividadePadraoPersistencia, AtividadePadraoPersistencia>();
            ConstrutorContainer.RegisterType<IPerfilCatalogoPersistencia, PerfilCatalogoPersistencia>();
            ConstrutorContainer.RegisterType<IPesoPersistencia, PesoPersistencia>();
            ConstrutorContainer.RegisterType<IDuracaoPersistencia, DuracaoPersistencia>();
            ConstrutorContainer.RegisterType<IRiscoPersistencia, RiscoPersistencia>();
            ConstrutorContainer.RegisterType<ISeveridadePersistencia, SeveridadePersistencia>();
            ConstrutorContainer.RegisterType<IProbabilidadePersistencia, ProbabilidadePersistencia>();
            ConstrutorContainer.RegisterType<IEPIPersistencia, EPIPersistencia>();
            ConstrutorContainer.RegisterType<IRiscoInventarioAmbientePersistencia, RiscoInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<INrInventarioAmbientePersistencia, NrInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<INrPersistencia, NrPersistencia>();
            ConstrutorContainer.RegisterType<ITelaPersistencia, TelaPersistencia>();
            ConstrutorContainer.RegisterType<IIntegracaoPersistencia, IntegracaoPersistencia>();
            ConstrutorContainer.RegisterType<IRascunhoInventarioAmbientePersistencia, RascunhoInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<IRascunhoInventarioAtividadePersistencia, RascunhoInventarioAtividadePersistencia>();
            ConstrutorContainer.RegisterType<IOperacaoAprPersistencia, OperacaoAprPersistencia>();
            ConstrutorContainer.RegisterType<IBloqueioPersistencia, BloqueioPersistencia>();
            ConstrutorContainer.RegisterType<IEPIRiscoInventarioAmbientePersistencia, EPIRiscoInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<IEPIRiscoInventarioAtividadePersistencia, EPIRiscoInventarioAtividadePersistencia>();
            ConstrutorContainer.RegisterType<IBloqueioPersistencia, BloqueioPersistencia>();
        }

        private static void ConfigurarInjecoesNegocio()
        {
            // Injeção camada de negócio
            ConstrutorContainer.RegisterType<IAprNegocio, AprNegocio>();
            ConstrutorContainer.RegisterType<IFuncionalidadesNegocio, FuncionalidadesNegocio>();
            ConstrutorContainer.RegisterType<IPerfisNegocio, PerfisNegocio>();
            ConstrutorContainer.RegisterType<IPessoaNegocio, PessoaNegocio>();
            ConstrutorContainer.RegisterType<IInventariosAmbienteNegocio, InventarioAmbienteNegocio>();
            ConstrutorContainer.RegisterType<IInventariosAtividadeNegocio, InventarioAtividadeNegocio>();
            ConstrutorContainer.RegisterType<IAmbienteNegocio, AmbienteNegocio>();
            ConstrutorContainer.RegisterType<ILocalInstalacaoNegocio, LocalInstalacaoNegocio>();
            ConstrutorContainer.RegisterType<IDisciplinaNegocio, DisciplinaNegocio>();
            ConstrutorContainer.RegisterType<IAtividadePadraoNegocio, AtividadePadraoNegocio>();
            ConstrutorContainer.RegisterType<IPerfilCatalogoNegocio, PerfilCatalogoNegocio>();
            ConstrutorContainer.RegisterType<IPesoNegocio, PesoNegocio>();
            ConstrutorContainer.RegisterType<IDuracaoNegocio, DuracaoNegocio>();
            ConstrutorContainer.RegisterType<IRiscoNegocio, RiscoNegocio>();
            ConstrutorContainer.RegisterType<ISeveridadeNegocio, SeveridadeNegocio>();
            ConstrutorContainer.RegisterType<IProbabilidadeNegocio, ProbabilidadeNegocio>();
            ConstrutorContainer.RegisterType<IEpiNegocio, EPINegocio>();
            ConstrutorContainer.RegisterType<IRiscoInventarioAmbienteNegocio, RiscoInventarioAmbienteNegocio>();
            ConstrutorContainer.RegisterType<INrInventarioAmbienteNegocio, NrInventarioAmbienteNegocio>();
            ConstrutorContainer.RegisterType<INrNegocio, NrNegocio>();
            ConstrutorContainer.RegisterType<ITelaNegocio, TelaNegocio>();
            ConstrutorContainer.RegisterType<IIntegracaoNegocio, IntegracaoNegocio>();
            ConstrutorContainer.RegisterType<IExportacaoDadosNegocio, ExportacaoDadosNegocio>();
            ConstrutorContainer.RegisterType<IRascunhoInventarioAmbienteNegocio, RascunhoInventarioAmbienteNegocio>();
            ConstrutorContainer.RegisterType<IRascunhoInventarioAtividadeNegocio, RascunhoInventarioAtividadeNegocio>();
            ConstrutorContainer.RegisterType<IOperacaoAprNegocio, OperacaoAprNegocio>();
            ConstrutorContainer.RegisterType<IBloqueioNegocio, BloqueioNegocio>();
            ConstrutorContainer.RegisterType<IEPIRiscoInventarioAmbienteNegocio, EPIRiscoInventarioAmbienteNegocio>();
            ConstrutorContainer.RegisterType<IEPIRiscoInventarioAtividadeNegocio, EPIRiscoInventarioAtividadeNegocio>();



            // Injeção camada de validãção
            ConstrutorContainer.RegisterType<Validador<PerfilFuncionalidadeModelo>, ValidadorPerfilFuncionalidade>();
            ConstrutorContainer.RegisterType<Validador<InventarioAtividadeModelo>, ValidadorInventarioAtividade>();
            ConstrutorContainer.RegisterType<Validador<InventarioAmbienteModelo>, ValidadorInventarioAmbiente>();
            ConstrutorContainer.RegisterType<Validador<AmbienteModelo>, ValidadorSistemaOperacional>();
            ConstrutorContainer.RegisterType<Validador<LocalInstalacaoModelo>, ValidadorLocalInstalacao>();
            ConstrutorContainer.RegisterType<Validador<DisciplinaModelo>, ValidadorDisciplina>();
            ConstrutorContainer.RegisterType<Validador<AtividadePadraoModelo>, ValidadorAtividadePadrao>();
            ConstrutorContainer.RegisterType<Validador<PerfilCatalogoModelo>, ValidadorPerfilCatalogo>();
            ConstrutorContainer.RegisterType<Validador<PesoModelo>, ValidadorPeso>();
            ConstrutorContainer.RegisterType<Validador<DuracaoModelo>, ValidadorDuracao>();
            ConstrutorContainer.RegisterType<Validador<RiscoModelo>, ValidadorRisco>();
            ConstrutorContainer.RegisterType<Validador<SeveridadeModelo>, ValidadorSeveridade>();
            ConstrutorContainer.RegisterType<Validador<ProbabilidadeModelo>, ValidadorProbabilidade>();
            ConstrutorContainer.RegisterType<Validador<EPIModelo>, ValidadorContraMedida>();
            ConstrutorContainer.RegisterType<Validador<RiscoInventarioAmbienteModelo>, ValidadorRiscoInventarioAmbiente>();
            ConstrutorContainer.RegisterType<Validador<NrInventarioAmbienteModelo>, ValidadorNrInventarioAmbiente>();
            ConstrutorContainer.RegisterType<Validador<NrModelo>, ValidadorNr>();
            ConstrutorContainer.RegisterType<Validador<RascunhoInventarioAmbienteModelo>, ValidadorRascunhoInventarioAmbiente>();
            ConstrutorContainer.RegisterType<Validador<RascunhoInventarioAtividadeModelo>, ValidadorRascunhoInventarioAtividade>();

            // Injeção camada de integração
            ConstrutorContainer.RegisterType<IDisciplinaSAPNegocio, DisciplinaSAPNegocio>();
            ConstrutorContainer.RegisterType<IAtividadePadraoSAPNegocio, AtividadePadraoSAPNegocio>();
            ConstrutorContainer.RegisterType<IPerfilCatalogoSAPNegocio, PerfilCatalogoSAPNegocio>();
            ConstrutorContainer.RegisterType<ILocalInstalacaoSAPNegocio, LocalInstalacaoSAPNegocio>();
            ConstrutorContainer.RegisterType<IAPRSapNegocio, APRSAPNegocio>();


            //Injeção de LOGS
            ConstrutorContainer.RegisterType<ILogPerfilFuncionalidadePersistencia, LogPerfilFuncionalidadePersistencia>();
            ConstrutorContainer.RegisterType<ILogInventarioAmbientePersistencia, LogInventarioAmbientePersistencia>();
            ConstrutorContainer.RegisterType<ILogInventarioAtividadePersistencia, LogInventarioAtividadePersistencia>();
            ConstrutorContainer.RegisterType<ILogAprPersistencia, LogAprPersistencia>();

        }
    }
}
