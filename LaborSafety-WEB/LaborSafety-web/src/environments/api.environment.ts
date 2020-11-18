export const apiPaths = {
  login: 'login',
  permissaoPerfil: {
    getPerfilCom8id(id) {
      return `Perfil/BuscarPor8ID?usuario=${id}`;
    },
    getListaTelasEFuncionalidadesPorPerfil(codPerfil) {
      return `Perfil/ListarListaTelasEFuncionalidadesPorPerfil?codPerfil=${codPerfil}`;
    },
    getPerfil(codPerfil) {
      return `Perfil/${codPerfil}`
    }
  },
  apr: {
    inserirAPR(APRModelo) {
      return `APR/Inserir?apr=${APRModelo}`;
    },
    verificaOrdemManutencao(ordemManutencao, codApr) {
      return `APR/ValidarExistenciaOrdemManutencao?ordemManutencao=${ordemManutencao}&codApr=${codApr}`;
    },
    inserirTela(APRModelo) {
      return `APR/InserirTela?apr=${APRModelo}`;
    },
    listarAPR(FiltroAPR) {
      return `APR/ListarApr?filtroAprModelo=${FiltroAPR}`;
    },
    getAPRPorId(id) {
      return `APR/PesquisarPorId?idApr=${id}`;
    },
    editarAPR(APRModelo) {
      return `APR/Editar?apr=${APRModelo}`;
    },
    calcularRiscoGeral(modelCalculo) {
      return `APR/CalcularRiscoAprPorAtividadeDisciplinaLI?filtro=${modelCalculo}`;
    },
    getModeloAPR() {
      return `APR/GerarAPREmBrancoComNumeroSerie`;
    },
    getAllLogs() {
      return `APR/EscreverLogTodasAPRs`;
    },
    getBase64(numeroDeSerie) {
      return `ExportacaoDados/agruparAprPdf?numeroSerie=${numeroDeSerie}`;
    },
    getMapaBase64(ordens) {
      return `ExportacaoDados/GerarMapaBloqueioAgrupado?listaOrdemManutencao=${ordens}`;
    },
    gerarLog(ids) {
      return `APR/EscreverLogEmTxt?codApr	=${ids}`;
    }
  },
  perfil: {
    getAll: 'Perfil',
    get(id) {
      return `Perfil/${id}`;
    },
    getListarTelaEFuncionalidadesPorPerfilETela(codPerfil, codTela) {
      return `Perfil/ListarTelaEFuncionalidadesPorPerfilETela?codPerfil=${codPerfil}&codtela=${codTela}`;
    },
    getListarListaTelasEFuncionalidadesPorPerfil(codPerfil) {
      return `Perfil/ListarListaTelasEFuncionalidadesPorPerfil?codPerfil=${codPerfil}`;
    },
    put(perfil) {
      return `Perfil/AssociarFuncionalidades?perfilFuncionalidadeModelo=${perfil}`;
    }
  },
  usuario: {
    obterUsuarioPor8ID(id) {
      return `Usuario/${id}`;
    },
    obterUsuariosPerfis() {
      return 'Usuario/UsuariosPerfis';
    }
  },
  funcionalidade: {
    get(id) {
      return `Funcionalidades/${id}`;
    },
    getc8id(id) {
      return `funcionalidades/c8id/${id}`;
    },
    getAll: 'Funcionalidades'
  },
  perfil_funcionalidades: {
    get(id) {
      return `AdministracaoPerfis/${id}`;
    },
    put(id) {
      return `AdministracaoPerfis/${id}`;
    }
  },
  homepage: {
    getAll: 'Perfil/ListarListaTelasEFuncionalidadesPorPerfil?codPerfil=2'
  },
  tela: {
    getAll: 'Tela'
  },
  operacao: {
    getOperacoesPorIDs(ids) {
      return `OperacaoApr/ListarPorId?codigo=${ids}`;
    }
  },
  pessoas: {
    getAllPessoas() {
      return `Pessoa/Listar`;
    },
    inserirPessoa(pessoa) {
      return `Pessoa/Inserir?pessoaModelo=${pessoa}`;
    },
    listarPessoaPorCPF(CPF) {
      return `Pessoa/ListarPorCPF?cpf=${CPF}`;
    },
    getPessoasPorIds(id) {
      return `Pessoa/ListarPorCodigos?codigo=${id}`;
    },
    deletePessoa(id) {
      return `Pessoa/Excluir?codPessoa=${id}`;
    },
    editarPessoa(pessoa) {
      return `Pessoa/Editar?pessoaModelo=${pessoa}`;
    }
  },
  treeview: {
    getAll: 'Treeview'
  },
  atividade: {
    getAll: 'AtividadePadrao/ListarTodasAtividades',
    getPorId(id) {
      return `InventarioAtividade/FiltrarPorId?id=${id}`;
    },
    getAllLogs() {
      return `InventarioAtividade/EscreverLogTodosInventarios`;
    },
    inserir(inventario) {
      return `InventarioAtividade/Inserir?inventarioAtividadeModelo=${inventario}`;
    },
    editar(inventario) {
      return `InventarioAtividade/Editar?inventarioAtividadeModelo=${inventario}`;
    },
    temAPR(CodInvAtividade) {
      return `InventarioAtividade/ListarCodAprPorInventarioTela/?codInventario=${CodInvAtividade}`;
    },
    deletar(id) {
      return `InventarioAtividade/Desativar?codInventarioExistente=${id}`;
    },
    getLocal(nivel) {
      return `LocalInstalacao/ListarPorNivel?filtro=${nivel}`;
    },
    gerarLog(ids) {
      return `InventarioAtividade/EscreverLogEmTxt?codInventariosAtividade=${ids}`;
    }
  },
  duracao: {
    getAll: 'Duracao/ListarTodasDuracoes'
  },
  perfildecatalogo: {
    getAll: 'PerfilCatalogo/ListarTodosPCs'
  },
  pesofisico: {
    getAll: 'Peso/ListarTodosPesos'
  },
  disciplina: {
    getAll: 'Disciplina/ListarTodasDisciplinas'
  },
  localInstalacao: {
    filtrar(obj: any) {
      return `LocalInstalacao/ListarPorNivel?filtro=${obj}`;
    },
  },
  tiposrisco: {
    getAll: 'Risco/ListarTodosTiposRisco'
  },
  risco: {
    getAll: 'Risco/ListarTodosRiscos',
    get(id) {
      return `Risco/ListarPorTipoRisco?codTipo=${id}`;
    },
    calcularRiscoGeral(riscoTotalAmbienteModelo) {
      return `InventarioAmbiente/CalcularRiscoTotalTela?riscoTotalAmbienteModelo=${riscoTotalAmbienteModelo}`;
    },
    calcularRiscoGeralAtividade(model) {
      return `InventarioAtividade/CalcularRiscoTotalTela?riscoTotalAtividadeModelo=${model}`;
    }
  },
  severidade: {
    getAll: 'Severidade/ListarTodasSeveridades'
  },
  frequencia: {
    getAll: 'Probabilidade/ListarTodasProbabilidades'
  },
  invAtividade: {
    filtrar(filtro) {
      return `InventarioAtividade/Filtrar?filtroInventarioAtividadeModelo=${filtro}`;
    },
    inserir(dados) {
      return `InventarioAtividade/Inserir?inventarioAtividadeModelo=${dados}`;
    }
  },
  rascunhoAmbiente: {
    inserirRascunho(inventarioRascunho) {
      return `RascunhoInventarioAmbiente/Inserir?rascunhInventarioAmbienteModelo=${inventarioRascunho}`;
    },
    filtrarRascunho(inventarioRascunho) {
      return `RascunhoInventarioAmbiente/Filtrar?filtroInventarioAmbienteModelo=${inventarioRascunho}`;
    },
    getPorId(id) {
      return `RascunhoInventarioAmbiente/FiltrarPorId?id=${id}`;
    },
    editarRascunho(inventarioRascunho) {
      return `RascunhoInventarioAmbiente/Editar?rascunhoInventarioAmbienteModelo=${inventarioRascunho}`;
    },
    deletarRascunho(idRascunho) {
      return `RascunhoInventarioAmbiente/Excluir?id=${idRascunho}`;
    }
  },
  rascunhoAtividade: {
    inserirRascunho(inventarioRascunho) {
      return `RascunhoInventarioAtividade/Inserir?rascunhoInventarioAtividadeModelo=${inventarioRascunho}`;
    },
    filtrarRascunho(inventarioRascunho) {
      return `RascunhoInventarioAtividade/Filtrar?filtroInventarioAtividadeModelo=${inventarioRascunho}`;
    },
    getPorId(id) {
      return `RascunhoInventarioAtividade/FiltrarPorId?id=${id}`;
    },
    editarRascunho(inventarioRascunho) {
      return `RascunhoInventarioAtividade/Editar?rascunhoInventarioAtividadeModelo=${inventarioRascunho}`;
    },
    deletarRascunho(idRascunho) {
      return `RascunhoInventarioAtividade/Excluir?id=${idRascunho}`;
    }
  },
  invAmbiente: {
    temAPR(codInvAmbiente) {
      return `InventarioAmbiente/ListarCodAprPorInventarioTela/?codInventario=${codInvAmbiente}`;
    },
    filtrar(filtros) {
      return `InventarioAmbiente/Filtrar?filtroInventarioAmbienteModelo=${filtros}`;
    },
    getAllLogs() {
      return `InventarioAmbiente/EscreverLogTodosInventarios`;
    },
    getAllSOs() {
      return `SistemaOperacional/ListarTodosSOs`;
    },
    insertSO(ambiente) {
      return `SistemaOperacional/Inserir?ambienteModelo=${ambiente}`;
    },
    editarSO(model) {
      return `SistemaOperacional/Editar?ambienteModelo=${model}`;
    },
    deletarSO(id) {
      return `SistemaOperacional/Desativar?codAmbienteExistente=${id}`;
    },
    getPorId(id) {
      return `InventarioAmbiente/${id}`;
    },
    inserir(inventario) {
      return `InventarioAmbiente/Inserir?inventarioAmbienteModelo=${inventario}`;
    },
    editar(inventario) {
      return `InventarioAmbiente/Editar?inventarioAmbienteModelo=${inventario}`;
    },
    deletar(model) {
      return `InventarioAmbiente/Desativar?inventarioAmbienteDelecaoComLogModelo=${model}`;
    },
    listarTodasNRs() {
      return `Nr/ListarTodasNRs`;
    },
    gerarLog(ids) {
      return `InventarioAmbiente/EscreverLogEmTxt?codInventariosAmbiente=${ids}`;
    }

  },
  importacao: {
    getModeloAmbiente() {
      return `InventarioAmbiente/GeraModelo`;
    },
    getModeloAtividade() {
      return `InventarioAtividade/GeraModelo`;
    },
    inserirPlanilhaAtividade(planilhaBase64) {
      return `InventarioAtividade/ImportarPlanilha?caminhoPlanilha=${planilhaBase64}`;
    },
    inserirPlanilhaAmbiente(arquivo) {
      return `InventarioAmbiente/ImportarPlanilha`;
    },
    getAllRiscos() {

      return `Risco/ListarTodosRiscos`;
    },
    getAllSeveridades() {
      return `Severidade/ListarTodasSeveridades`;
    },
    getAllProbabilidades() {
      return `Probabilidade/ListarTodasProbabilidades`;
    },
    getAllSistemasOperacionais() {
      return `SistemaOperacional/ListarTodosSOs`;

    },
    getAllDisciplina() {
      return `Disciplina/ListarTodasDisciplinas`;
    },
    getAllAtividadePadrao() {
      return `AtividadePadrao/ListarTodasAtividades`;
    },
    getAllPesos() {
      return `Peso/ListarTodosPesos`;
    },
    getAllPerfilCatalogo() {
      return `PerfilCatalogo/ListarTodosPCs`;
    },
    exportarDadosAmbiente(dadosExportacao) {
      return `ExportacaoDados/ExportarDadosAmbiente?dadosExportacaoModelo=${dadosExportacao}`;
    },
    exportarDadosAtividade(dadosExportacao) {
      return `ExportacaoDados/ExportarDadosAtividade?dadosExportacaoModelo=${dadosExportacao}`;
    },
    exportarDadosAPR(dadosExportacao) {
      return `ExportacaoDados/ExportarDadosApr?dadosExportacaoModel=${dadosExportacao}`;
    },
    /* enviarFiltrosAmbiente(filtros) {
       return `InventarioAmbiente/Filtrar?filtroInventarioAmbienteModelo=${filtros}`;
     },
     enviarFiltrosAtividade(filtros) {
       return `InventarioAtividade/Filtrar?filtroInventarioAtividadeModelo=${filtros}`;
     },*/
  },
  epi: {
    getAll() {
      return  `EPI/ListarTodosEPIs`;
    },
    getPerLevel(nome: string) {
      return `EPI/ListarTodosEPIPorNivel?nome=${nome}`;
    },
    getEPIsporIds(listaIds: []) {
      return `EPI/ListarEPIsPorListaId?epis=${listaIds}`;
    }
  },
};
