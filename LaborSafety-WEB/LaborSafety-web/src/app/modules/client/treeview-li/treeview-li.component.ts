import {  Component,
          OnInit,
          Injectable,
          Input,
          Output,
          EventEmitter,
          ElementRef,
          OnDestroy,
          ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlattener, MatTreeFlatDataSource, MatDialogConfig, MatDialog, MatTree } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { GestaoAtividadeInventarioService } from '../inventario-atividade/gestao-inv-atividade/gestao-inv-atividade.service';
import { LocalInstalacaoModel } from 'src/app/shared/models/invAmbinteModel';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Router } from '@angular/router';


// Node de criação estrutural da treeview
export class LocalItemNode {
  children: LocalItemNode[];
  item: string;
  hierarchy: string;
  nome: string;
  codPeso: number;
  codPerfil: number;
  codLocalInstalacao: number;
}

// Dados contidos em cada node da treeview
export class LocalItemFlatNode {
  Item: string;
  Nome: string;
  Hierarchy: string;
  Level: number;
  Expandable: boolean;
  CodPeso: number;
  CodPerfil: number;
  CodLocalInstalacao: number;
  IsLoading = false;
}

// Dados iniciais da treeview (primeiro nó de Locais sempre será CSA)
const DATA_STARTUP = {
  CSA: null
};


// Classe injetável com o estado atual, construção e inserção da treeview
@Injectable()
class LocalChecklistDatabase {
  dataChange = new BehaviorSubject<LocalItemNode[]>([]);
  get data(): LocalItemNode[] { return this.dataChange.value; }

  constructor() { this.initialize(); }

  initialize() {
    const data = this.buildFileTree(DATA_STARTUP, 0);
    this.dataChange.next(data);
  }

  buildFileTree(obj: { [key: string]: any }, level: number): LocalItemNode[] {
    return Object.keys(obj).reduce<LocalItemNode[]>((accumulator, key) => {
      const value = obj[key];
      const node = new LocalItemNode();
      node.item = key;
      node.nome = key;

      if (value != null) {
        if (typeof value === 'object') {
          node.children = this.buildFileTree(value, level + 1);
        } else {
          node.item = value;
          node.nome = value;
        }
      }
      return accumulator.concat(node);
    }, []);
  }

  // Insercao de um LocalItemNode, recebendo o node pai e os atributos do node
  insertItem(parent: LocalItemNode, name: string, hierc: string, nomeCompleto: string, codperf: number, codp: number, codloc: number) {
    if (parent.children && name) {
      parent.children.push({
        item: name,
        hierarchy: hierc,
        nome: nomeCompleto,
        codPerfil: codperf,
        codPeso: codp,
        codLocalInstalacao: codloc
      } as LocalItemNode);
      this.dataChange.next(this.data);
    }
  }
}

// Componente TreeviewLIComponent. Modo de usar:
// <app-treeview-li  [hasPermission]=""
//                   [hasEditingPermission]=""
//                   [hierarchyInput]=""
//                   [codPeso]=""
//                   [codPerfil]=""
//                   (selectedNodes)="">
// </app-treeview-li>
// <!-- hasPermission: boolean / hierarchyInput: string[] / codPeso: number / codPerfil: number / selectedNodes = $event -->
@Component({
  selector: 'app-treeview-li',
  templateUrl: './treeview-li.component.html',
  styleUrls: ['./treeview-li.component.scss'],
  providers: [LocalChecklistDatabase]
})
export class TreeviewLIComponent implements OnInit, OnDestroy {
  // Recebe os locais em hierarchyInput e os codigos de peso e perfil de catalogo
  // Retorna os locais selecionados na treeview
  @ViewChild('treeSelector') tree: MatTree<LocalItemNode>;
  @Input() hierarchyInput: string[];
  @Input() codPeso: number;
  @Input() codPerfil: number;
  @Input() hasPermission: boolean;
  @Input() hasEditingPermission: boolean;
  @Output() selectedNodes = new EventEmitter();
  @Output() isOperational = new EventEmitter();

  // --------------------------------------------------------------------------
  //    * SEÇÃO 1: DEFINIÇÕES DE VARIÁVEIS, CONSTRUTURES E STARTUP *
  // --------------------------------------------------------------------------

  // Variaveis de uso geral
  unexpandableNodes: LocalItemFlatNode[] = []; // Todos os nodes que não podem ser expandidos
  unavailableChecks: LocalItemFlatNode[] = []; // Todos os nodes que não podem ser marcados/desmarcados
  pesquisaLocal: string = ''; // Variavel para guardar o valor da pesquisa
  searchTrigger = false;
  currentSearchNode: string;

  // Variaveis de controle da árvore
  flatNodeMap = new Map<LocalItemFlatNode, LocalItemNode>();
  nestedNodeMap = new Map<LocalItemNode, LocalItemFlatNode>();
  selectedParent: LocalItemFlatNode | null = null;
  treeControlLI: FlatTreeControl<LocalItemFlatNode>;
  treeFlattener: MatTreeFlattener<LocalItemNode, LocalItemFlatNode>;
  dataSourceLI: MatTreeFlatDataSource<LocalItemNode, LocalItemFlatNode>;
  checklistSelection = new SelectionModel<LocalItemFlatNode>(true);

  // Variaveis de edição
  editingMode = false;
  charge = 0;

  // Funcoes de controle da treeview
  getLevel = (node: LocalItemFlatNode) => node.Level;
  isExpandable = (node: LocalItemFlatNode) => node.Expandable;
  getChildren = (node: LocalItemNode): LocalItemNode[] => node.children;
  hasChild = (_: number, nodeData: LocalItemFlatNode) => nodeData.Expandable;
  hasNoContent = (_: number, nodeData: LocalItemFlatNode) => nodeData.Item === '';


  // Construtor que inicia a treeview e serviço de locais. Coloca dados no primeiro node
  constructor(private databaseVariable: LocalChecklistDatabase,
              public gestaoService: GestaoAtividadeInventarioService,
              public elementRef: ElementRef,
              public router: Router,
              public dialog: MatDialog) {
  }

  // Atribuição das variaveis de controle de dados da árvore
  setTreeParameters() {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel, this.isExpandable, this.getChildren);
    this.treeControlLI = new FlatTreeControl<LocalItemFlatNode>(this.getLevel, this.isExpandable);
    this.dataSourceLI = new MatTreeFlatDataSource(this.treeControlLI, this.treeFlattener);
    this.databaseVariable.dataChange.subscribe(data => {
      this.dataSourceLI.data = data;
    });
  }

  // Atribuição dos dados do primeiro node, CSA.
  setCSAnode() {
    const csaNode = this.treeControlLI.dataNodes.find(e => e.Item === 'CSA');
    csaNode.Nome = 'CSA';
    csaNode.Expandable = true;
    csaNode.Hierarchy = 'CSA';
    csaNode.CodPerfil = 1;
    csaNode.CodPeso = 1;
    const csaNodeReference = this.flatNodeMap.get(csaNode);
    csaNodeReference.hierarchy = 'CSA';
    csaNodeReference.nome = 'CSA';
    csaNodeReference.item = 'CSA';
    csaNodeReference.children = [];

    if (this.codPerfil && this.codPerfil !== csaNode.CodPerfil) {
      this.unavailableChecks.push(csaNode);
    }
  }


  // FIM DA SEÇÃO DE DEFINIÇÕES DE VARIÁVEIS, CONSTRUTURES E STARTUP
  // __________________________________________________________________________

  // --------------------------------------------------------------------------
  //    * SEÇÃO 2: CODIGO DE OBTENÇÃO DE DADOS E REGRAS DE NEGÓCIO *
  // --------------------------------------------------------------------------

  ngOnDestroy(): void {
    this.elementRef.nativeElement.remove();
 }

  // Async para esperar a resposta do backend.
  // Verificação de existência de nodes já selecionados no caso de edição.
  async ngOnInit() {
    this.filterSelections();
    this.isOperational.emit(true);
    this.setTreeParameters();
    this.setCSAnode();
    if (this.hierarchyInput) {
      this.isOperational.emit(false);
      this.editingMode = true;
      this.charge = 0;
      await this.checkPreviouslyExistingNodesToInventory(this.hierarchyInput);
      this.setNodesToExpandable(this.treeControlLI.dataNodes);
      this.treeControlLI.collapseAll();
      this.databaseVariable.buildFileTree(this.dataSourceLI.data, 0);
      this.isOperational.emit(true);
      this.editingMode = false;
    }
    this.setNodesToExpandable(this.treeControlLI.dataNodes);
    this.filterSelections();
  }


  // Para cada codigo de 6 níveis recebido, inserir na treeview e selecionar o node e seus filhos.
  async checkPreviouslyExistingNodesToInventory(sixLevelCodes: string[]) {
    const chargeCount = 100 / sixLevelCodes.length;
    let chargeAux = 0;
    for (const sixLevelCode of sixLevelCodes) {
      const temp = sixLevelCode.split('-');
      let i = 0;
      let currentNodeString = '';
      let nextNode: LocalItemFlatNode;
      // tslint:disable-next-line: prefer-for-of
      for (i = 0; i < temp.length; i++) {
        currentNodeString = '';
        for (let j = 0; j <= i; j++) {
          if (j === 0) {
            currentNodeString += temp[j];
          } else {
            currentNodeString += '-' + temp[j];
          }
        }
        nextNode = this.treeControlLI.dataNodes.find(e => e.Hierarchy === currentNodeString);
        if (nextNode) {
          await this.callBackend(nextNode);
        }
      }
      chargeAux += chargeCount;
      this.charge = Math.round(chargeAux);

      // Apenas marcar se o peso e perfis selecionados correspoderem ao local encontrado
      if (this.codPerfil) {
        if (nextNode.CodPerfil === this.codPerfil) {
          this.checklistSelection.select(nextNode);
        }
      } else {
        this.checklistSelection.select(nextNode);
      }

      if (!this.hasPermission) {
        const nodeToDisable = this.treeControlLI.dataNodes.find(e => e.Hierarchy === sixLevelCode);
        this.unavailableChecks.push(nodeToDisable);
        this.disableAllParents(nodeToDisable);
        const descendants = this.treeControlLI.getDescendants(nodeToDisable);
        this.setNodesToDisabled(descendants);
      }
    }
  }

  // Retorna se o node está indisponível ou inativo
  disabledNodes(node: LocalItemFlatNode) {
    if (this.editingMode) {
      return true;
    }
    if (this.hasEditingPermission !== undefined && this.hasEditingPermission !== null) {
      if (!this.hasEditingPermission) {
        return this.unavailableChecks.some(n => n.Hierarchy === node.Hierarchy);
      } else {
        return true;
      }
    }
    return this.unavailableChecks.some(n => n.Hierarchy === node.Hierarchy);
  }

  // Desativa a lista de nodes recebidos, colocando-os na lista de checks inativos.
  setNodesToDisabled(nodes: LocalItemFlatNode[]) {
    nodes.forEach((node) => { if (!this.unavailableChecks.includes(node)) { this.unavailableChecks.push(node); } });
  }

  // Faz com que todos os nodes, menos o nivel 6 e bases sejam expansíveis.
  // Como a treeview é dinânica, é preciso tentar expandir antes de saber se
  // existem registros filhos. Os nodes adicionados aos nodes inexpansíveis também serão evitados.
  setNodesToExpandable(nodes: LocalItemFlatNode[]) {
    if (nodes) {
      for (const node of nodes) {
        if (node.Level !== 5 && (!node.Hierarchy.includes('000_BASE'))) {
          if (this.unexpandableNodes.some(n => n === node)) {
            node.Expandable = false;
          } else {
            node.Expandable = true;
          }
        }
      }
    }
  }

  // Chamada do serviço para obter dados do backend. Um objeto no formato
  // do código de 6 níveis é criado e enviado como parâmetro para o serviço,
  // dependendo do nivel atual da treeview.
  async callBackend(node: LocalItemFlatNode) {
    if (!this.checkIfNodeHasChildren(node) && node) {
      node.IsLoading = true;
      let nodeLevel: string;
      let constructionTrigger = false;
      const newObject = {
        N1: '',
        N2: '',
        N3: '',
        N4: '',
        N5: '',
        N6: ''
      };
      const dividedSTR = node.Hierarchy.split('-');
      switch (node.Level) {
        case 0:
          newObject.N1 = node.Item;
          nodeLevel = 'N2';
          constructionTrigger = true;
          break;
        case 1:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = node.Item;
          nodeLevel = 'N3';
          constructionTrigger = true;
          break;
        case 2:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = node.Item;
          nodeLevel = 'N4';
          constructionTrigger = true;
          break;
        case 3:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = dividedSTR.slice(2, 3).join();
          newObject.N4 = node.Item;
          nodeLevel = 'N5';
          constructionTrigger = true;
          break;
        case 4:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = dividedSTR.slice(2, 3).join();
          newObject.N4 = dividedSTR.slice(3, 4).join();
          newObject.N5 = node.Item;
          nodeLevel = 'N6';
          constructionTrigger = true;
          break;
        case 5:
          constructionTrigger = false;
          break;
        default:
          this.callErrorPopup('Tentativa de acesso a um nível superior ao máximo da treeview. Favor contactar o suporte.');
          break;
      }
      if (constructionTrigger) {
        await this.gestaoService.getLocalInstalacao(newObject).then((data: LocalInstalacaoModel[]) => {
          this.addAllDataToTreeview(data, node, nodeLevel);
          node.IsLoading = false;
        }).catch(e => {
          this.callErrorPopup('Ocorreu um erro crítico ao buscar dados de local. Retornando para a página inicial... ');
          this.router.navigate(['/client/dashboard']);
        });
      }
    }
  }

  // Verifica se o node passado como parâmetro possui ou não nodes filhos
  checkIfNodeHasChildren(node: LocalItemFlatNode) {
    const currentNodeReference = this.flatNodeMap.get(node);
    if (node) {
      if (currentNodeReference.children && currentNodeReference.children.length > 1) {
        return true;
      } else {
        return false;
      }
    }
  }

  // Adição de todos os dados recebidos pelo serviço à treeview.
  // Encontra-se o node pai e adiciona-se, um por um, todos os filhos recebidos,
  // passando o item, códigos, o nome e sua hierarquia, que é o código de 6 níveis.
  addAllDataToTreeview(data: LocalInstalacaoModel[], node: LocalItemFlatNode, nodeLevel: string) {
    const parentNode = this.treeControlLI.dataNodes.find(e => e.Hierarchy === node.Hierarchy);
    const parentReference = this.flatNodeMap.get(parentNode);
    if (data.length === 0) {
      parentNode.Expandable = false;
      this.unexpandableNodes.push(parentNode); // Se o node não tiver filhos, ele se torna um node folha.
    } else {
      parentReference.children = [];
      data.forEach((dado: LocalInstalacaoModel) => {
        if (dado.Descricao) {
          if (dado[nodeLevel] === '000_BASE') {
            dado.Descricao = dado.Descricao.concat(' (BASE)');
            dado.Descricao = [dado.Descricao.slice(0, dado.Descricao.indexOf(' ')) +
              '-' + dado[nodeLevel] +
              dado.Descricao.slice(dado.Descricao.indexOf(' '))
            ].join();
          }
          this.databaseVariable.insertItem(parentReference,
            dado[nodeLevel],
            dado.Descricao.substr(0, dado.Descricao.indexOf(' ')),
            dado.Descricao,
            dado.CodPerfilCatalogo,
            dado.CodPeso,
            dado.CodLocalInstalacao);
          this.treeControlLI.expand(parentNode);
        }
      });

      if (this.codPerfil) {
        if (parentNode.CodPerfil !== this.codPerfil) {
          if (!this.unavailableChecks.includes(parentNode)) {
            this.unavailableChecks.push(parentNode);
          }
        }
        this.setNodesToDisabled(this.treeControlLI.getDescendants(parentNode).filter(d => d.CodPerfil !== this.codPerfil));
      }
    }

    if (!this.editingMode) {
      this.LocalItemSelection(parentNode);
      this.setNodesToExpandable(this.treeControlLI.dataNodes);
      this.refreshTree();
    }
  }

  // Fecha e abre novamente a treeview para renderizar todos os nodes corretamente
  refreshTree() {
    const expandedNodes = new Array<LocalItemFlatNode>();
    this.treeControlLI.dataNodes.forEach(node => {
      if (node.Expandable && this.treeControlLI.isExpanded(node)) {
        expandedNodes.push(node);
      }
    });
    this.treeControlLI.collapseAll();
    expandedNodes.forEach(node => {
      this.treeControlLI.expand(this.treeControlLI.dataNodes.find(n => n.Item === node.Item));
    });
  }

  async searchLocal() {
    this.searchTrigger = true;
    if (this.pesquisaLocal) {
      const spaceAux = this.pesquisaLocal.split(' ').slice(0, 1).join();
      const temp = spaceAux.split('-');
      let i = 0;
      let currentNodeString = '';
      let nextNode: LocalItemFlatNode;
      // tslint:disable-next-line: prefer-for-of
      for (i = 0; i < temp.length; i++) {
        currentNodeString = '';
        for (let j = 0; j <= i; j++) {
          this.currentSearchNode = temp[j];
          if (j === 0) {
            currentNodeString += temp[j];
          } else {
            currentNodeString += '-' + temp[j];
          }
        }
        nextNode = this.treeControlLI.dataNodes.find(e => e.Hierarchy === currentNodeString);
        if (!this.checkIfNodeHasChildren(nextNode) && nextNode) {
          await this.callBackend(nextNode);
        }
        const currentNodeReference = this.flatNodeMap.get(nextNode);
        if (this.searchTrigger) {
          if (!currentNodeReference) {
            this.callErrorPopup(`O local ${this.currentSearchNode} não foi encontrado!`);
            break;
          }
        }
      }
      this.refreshTree();
      this.treeControlLI.dataNodes.forEach((node) => {
        if (this.checkIfNodeHasChildren(node)) {
          this.treeControlLI.expand(node);
        }
      });
      if (nextNode) {
        const allnodes = document.querySelectorAll('[class=mat-checkbox-label]');
        for (let nodesLength = 0; nodesLength < allnodes.length; nodesLength++) {
          if (allnodes[nodesLength].textContent.trim() === nextNode.Nome.trim()) {
            allnodes[nodesLength].setAttribute('style',
            'padding: 0 5px 0 5px; border: 1px solid rgb(240,93,34); border-radius: 3px; color: black; font-weight: 500');
            allnodes[nodesLength].scrollIntoView({ behavior: 'smooth', block: 'center'});
          }
        }
      }
    }
    this.searchTrigger = false;
  }

  callErrorPopup(text: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.minWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 0, textoRecebido: text };
    this.dialog.open(PopupComponent, dialogConfig);
  }



  // FIM DA SEÇÃO DE OBTENÇÃO DE DADOS E REGRAS DE NEGÓCIO
  // __________________________________________________________________________

  // --------------------------------------------------------------------------
  //    * SEÇÃO 3: CODIGO DE SUPORTE A HTML E CONTROLE DE NODES INTERNO *
  // --------------------------------------------------------------------------

  // Lógica de transformação de tipo LocalItemNode para LocalItemFlatNode
  transformer = (node: LocalItemNode, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.Item === node.item
      ? existingNode
      : new LocalItemFlatNode();
    flatNode.Item = node.item;
    flatNode.Level = level;
    flatNode.Hierarchy = node.hierarchy;
    flatNode.Nome = node.nome;
    flatNode.Expandable = !!node.children;
    flatNode.CodPerfil = node.codPerfil;
    flatNode.CodPeso = node.codPeso;
    flatNode.CodLocalInstalacao = node.codLocalInstalacao;
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  // Verifica se todos os descendentes de um node estão selecionados
  descendantsAllSelected(node: LocalItemFlatNode): boolean {
    const descendants = this.treeControlLI.getDescendants(node);
    let descAllSelected = false;
    let descendantsCounter = 0;

    if (descendants.length > 0) {
      descendants.forEach((desc) => {
        if (this.checklistSelection.isSelected(desc)) {
          descendantsCounter++;
        }
      });
    }
    if (this.checkIfNodeHasChildren(node) && descendantsCounter > 0
      && descendantsCounter === descendants.length) {
      descAllSelected = true;
    } else {
      descAllSelected = this.checklistSelection.isSelected(node);
    }
    return descAllSelected;
  }

  // Verifica se alguns descendentes de um node estão selecionados
  descendantsPartiallySelected(node: LocalItemFlatNode): boolean {
    const descendants = this.treeControlLI.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  // Troca o estado da checkbox de um node pai clicado. Os descendentes também são atualizados
  LocalItemSelectionToggle(node: LocalItemFlatNode): void {
    this.checklistSelection.toggle(node);
    if (this.checklistSelection.isSelected(node)) {
      this.callBackend(node);
    }
    const descendants = this.treeControlLI.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);

    descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    this.checkAllParentsSelection(node);
    this.filterSelections();
  }

  // Carrega os filhos caso um nó pai seja selecionado. Iguala o estado de checkbox do pai para todos os filhos.
  LocalItemSelection(node: LocalItemFlatNode): void {
    if (this.checklistSelection.isSelected(node) && !this.unexpandableNodes.includes(node)) {
      const descendants = this.treeControlLI.getDescendants(node);
      this.checklistSelection.isSelected(node)
        ? this.checklistSelection.select(...descendants)
        : this.checklistSelection.deselect(...descendants);

      this.checkAllParentsSelection(node);
      this.filterSelections();
    }
  }

  // Troca o estado da checkbox de um node folha clicado.
  LocalLeafItemSelectionToggle(node: LocalItemFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
    this.filterSelections();
  }

  // Desativa todos os nodes pai de um node recebido por parametro
  disableAllParents(node: LocalItemFlatNode): void {
    let parent: LocalItemFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.disableRootNode(parent);
      parent = this.getParentNode(parent);
    }
  }

  // Desativa um node.
  disableRootNode(node: LocalItemFlatNode): void {
    if (!this.unavailableChecks.includes(node)) {
      this.unavailableChecks.push(node);
    }
  }

  // Check de seleção dos nodes pais
  checkAllParentsSelection(node: LocalItemFlatNode): void {
    let parent: LocalItemFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  // Check de seleção do node recebido
  checkRootNodeSelection(node: LocalItemFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControlLI.getDescendants(node);
    const descAllSelected = descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    if (nodeSelected && !descAllSelected) {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      this.checklistSelection.select(node);
    }
    this.filterSelections();
  }

  // Retorno do node pai relativo ao node passado como parametro.
  getParentNode(node: LocalItemFlatNode): LocalItemFlatNode | null {
    if (node) {
      const currentLevel = this.getLevel(node);
      if (currentLevel < 1) {
        return null;
      }
      const startIndex = this.treeControlLI.dataNodes.indexOf(node) - 1;
      for (let i = startIndex; i >= 0; i--) {
        const currentNode = this.treeControlLI.dataNodes[i];
        if (this.getLevel(currentNode) < currentLevel) {
          return currentNode;
        }
      }
    }
    return null;
  }

  // Insere no retorno da treeview todos os nodes de ultimo nivel selecionados (não envia os pais)
  filterSelections() {
    if (!this.editingMode) {
      const selectedArray = [].concat(this.checklistSelection.selected);
      selectedArray.forEach((node) => {
        const ix = selectedArray.indexOf(this.getParentNode(node));
        if (ix !== -1) {
          selectedArray.splice(ix, 1);
        }
      });
      this.selectedNodes.emit(selectedArray);
    }
  }

  // FIM DA SEÇÃO DE CODIGO DE SUPORTE A HTML E CONTROLE DE NODES INTERNO
  // __________________________________________________________________________

}
