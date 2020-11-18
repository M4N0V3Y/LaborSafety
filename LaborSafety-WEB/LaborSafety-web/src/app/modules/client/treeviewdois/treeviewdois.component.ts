import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnDestroy, ElementRef } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Injectable } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener, MatTreeNode, MatTree } from '@angular/material/tree';
import { BehaviorSubject } from 'rxjs';
import { GestaoAtividadeInventarioService } from '../inventario-atividade/gestao-inv-atividade/gestao-inv-atividade.service';
import { Tela } from 'src/app/shared/models/tela.model';

export class TodoItemNode {
  children: TodoItemNode[];
  item: string;
  hierarchy: string;
  nome: string;
}
export class TodoItemFlatNode {
  item: string;
  nome: string;
  hierarchy: string;
  level: number;
  expandable: boolean;
  isBase: boolean;
}

const TREE_DATA = {
  CSA: null
};

@Injectable()
export class ChecklistDatabase {
  dataChange = new BehaviorSubject<TodoItemNode[]>([]);
  get data(): TodoItemNode[] { return this.dataChange.value; }

  constructor() {
    this.initialize();
  }

  initialize() {
    const data = this.buildFileTree(TREE_DATA, 0);
    this.dataChange.next(data);
  }

  buildFileTree(obj: { [key: string]: any }, level: number): TodoItemNode[] {
    return Object.keys(obj).reduce<TodoItemNode[]>((accumulator, key) => {
      const value = obj[key];
      const node = new TodoItemNode();
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

  insertItem(parent: TodoItemNode, name: string, hierc: string, nomeCompleto: string) {
    if (parent.children && name) {
      parent.children.push({ item: name, hierarchy: hierc, nome: nomeCompleto } as TodoItemNode);
      this.dataChange.next(this.data);
    }
  }
}


@Component({
  selector: 'app-treeviewdois',
  templateUrl: './treeviewdois.component.html',
  styleUrls: ['./treeviewdois.component.scss']
})
export class TreeviewdoisComponent implements OnInit, OnDestroy {
  @ViewChild('treeSelector') tree: MatTree<TodoItemNode>;
  @Input() treeviewPage: Tela;
  @Output() selectedNodes = new EventEmitter();
  @Input() temPermissao: boolean;
  @Input() temPermissaoEditar: boolean;
  @Input() hierarchyInput: string[];

  expandedNodes: any[];
  unavailableChecks: TodoItemFlatNode[] = [];
  unexpandableNodes: TodoItemFlatNode[] = [];
  baseChild: TodoItemFlatNode[] = [];
  flatNodeMap = new Map<TodoItemFlatNode, TodoItemNode>();
  nestedNodeMap = new Map<TodoItemNode, TodoItemFlatNode>();
  selectedParent: TodoItemFlatNode | null = null;
  treeControl: FlatTreeControl<TodoItemFlatNode>;
  treeFlattener: MatTreeFlattener<TodoItemNode, TodoItemFlatNode>;
  dataSource: MatTreeFlatDataSource<TodoItemNode, TodoItemFlatNode>;
  checklistSelection = new SelectionModel<TodoItemFlatNode>(true);
  getLevel = (node: TodoItemFlatNode) => node.level;
  isExpandable = (node: TodoItemFlatNode) => node.expandable;
  getChildren = (node: TodoItemNode): TodoItemNode[] => node.children;
  hasChild = (_: number, nodeData: TodoItemFlatNode) => nodeData.expandable;
  hasNoContent = (_: number, nodeData: TodoItemFlatNode) => nodeData.item === '';

  constructor(private databaseVariable: ChecklistDatabase,
              public gestaoService: GestaoAtividadeInventarioService,
              private elementRef: ElementRef) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
      this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<TodoItemFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
    databaseVariable.dataChange.subscribe(data => {
      this.dataSource.data = data;
    });

    const csaNode = this.treeControl.dataNodes.find(e => e.item === 'CSA');
    csaNode.nome = 'CSA';
    csaNode.expandable = true;
    csaNode.hierarchy = 'CSA';
    const csaNodeReference = this.flatNodeMap.get(csaNode);
    csaNodeReference.hierarchy = 'CSA';
    csaNodeReference.nome = 'CSA';
    csaNodeReference.item = 'CSA';
    csaNodeReference.children = [];
  }

  ngOnDestroy(): void {
     this.elementRef.nativeElement.remove();
  }

  async ngOnInit() {
    if (this.hierarchyInput) {
      await this.checkPreviouslyExistingNodesToInventory(this.hierarchyInput);
    }
    this.setNodesToExpandable(this.treeControl.dataNodes);
  }

  async checkPreviouslyExistingNodesToInventory(sixLevelCodes: string[]) {
    for (const sixLevelCode of sixLevelCodes) {
      const temp = sixLevelCode.split('-');
      let i = 0;
      let currentNodeString = '';
      let nextNode: TodoItemFlatNode;
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
        nextNode = this.treeControl.dataNodes.find(e => e.hierarchy === currentNodeString);
        await this.callBackend(nextNode);
      }
      this.checklistSelection.select(nextNode);
      this.todoItemSelection(nextNode);
      if (!this.temPermissao) {
        const nodeToDisable = this.treeControl.dataNodes.find(e => e.hierarchy === sixLevelCode);
        this.unavailableChecks.push(nodeToDisable);
        const descendants = this.treeControl.getDescendants(nodeToDisable);
        this.setNodesToDisabled(descendants);
      }
    }
  }

  disabledNodes(node: TodoItemFlatNode) {
    if (this.temPermissaoEditar !== undefined && this.temPermissaoEditar !== null) {
      if (!this.temPermissaoEditar) {
        return this.unavailableChecks.some(n => n.hierarchy === node.hierarchy);
      } else {
        return true;
      }
    }
    return this.unavailableChecks.some(n => n.hierarchy === node.hierarchy);

  }

  nodeUnavailable(node: TodoItemFlatNode) {
    return this.unavailableChecks.some(n => n.hierarchy === node.hierarchy);
  }


  setNodesToExpandable(nodes: TodoItemFlatNode[]) {
    if (nodes) {
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < nodes.length; i++) {
        if (nodes[i].level !== 5 && (!nodes[i].item.includes('Base'))) {
          if (this.unexpandableNodes.some(n => n === nodes[i])) {
            nodes[i].expandable = false;
          } else {
            nodes[i].expandable = true;
          }
        }
      }
    }
  }

  setNodesToDisabled(nodes: TodoItemFlatNode[]) {
    nodes.forEach((node) => { this.unavailableChecks.push(node); });
  }

  addBaseChild(node: TodoItemFlatNode) {
    const nodeReference = this.flatNodeMap.get(node);
    nodeReference.children = [];
    if (nodeReference.item !== 'CSA') {
      this.databaseVariable.insertItem(nodeReference, node.item + 'Base', node.hierarchy, node.nome + ' (BASE)');
      this.treeControl.expand(node);
      this.databaseVariable.dataChange.next(this.databaseVariable.data);
    }
  }

  checkIfNodeHasChildren(node: any) {
    const currentNodeReference = this.flatNodeMap.get(node);
    if (currentNodeReference.children && currentNodeReference.children.length > 1) {
      return true;
    } else {
      return false;
    }
  }

  async callBackend(node: TodoItemFlatNode) {
    if (!this.checkIfNodeHasChildren(node)) {
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
      const dividedSTR = node.hierarchy.split('-');
      switch (node.level) {
        case 0:
          newObject.N1 = node.item;
          nodeLevel = 'N2';
          constructionTrigger = true;
          break;
        case 1:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = node.item;
          nodeLevel = 'N3';
          constructionTrigger = true;
          break;
        case 2:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = node.item;
          nodeLevel = 'N4';
          constructionTrigger = true;
          break;
        case 3:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = dividedSTR.slice(2, 3).join();
          newObject.N4 = node.item;
          nodeLevel = 'N5';
          constructionTrigger = true;
          break;
        case 4:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = dividedSTR.slice(2, 3).join();
          newObject.N4 = dividedSTR.slice(3, 4).join();
          newObject.N5 = node.item;
          nodeLevel = 'N6';
          constructionTrigger = true;
          break;
        case 5:
          newObject.N1 = dividedSTR.slice(0, 1).join();
          newObject.N2 = dividedSTR.slice(1, 2).join();
          newObject.N3 = dividedSTR.slice(2, 3).join();
          newObject.N4 = dividedSTR.slice(3, 4).join();
          newObject.N5 = dividedSTR.slice(4, 5).join();
          newObject.N6 = node.item;
          constructionTrigger = true;
          break;
        default:
          break;
      }
      if (constructionTrigger) {
        await this.gestaoService.getLocalInstalacao(newObject).then(
          (data: any[]) => { this.addAllDataToTreeview(data, node, nodeLevel); }
        );
      }
    }
  }

  addAllDataToTreeview(data: any[], node: TodoItemFlatNode, nodeLevel: string) {
    const parentNode = this.treeControl.dataNodes.find(e => e.hierarchy === node.hierarchy);
    const parentReference = this.flatNodeMap.get(parentNode);
    if (data.length === 0) {
      parentNode.expandable = false;
      this.unexpandableNodes.push(parentNode);
    } else {
      data.forEach((dado: any) => {
        if (dado.Nome) {
          if (!parentReference.children && node.level !== 5) {
            parentNode.expandable = true;
            this.addBaseChild(node);
          }
          this.databaseVariable.insertItem(parentReference, dado[nodeLevel], dado.Nome.substr(0, dado.Nome.indexOf(' ')), dado.Nome);
          this.treeControl.expand(parentNode);
        }
      });
    }
    if (this.nodeUnavailable(parentNode)) {
      this.setNodesToDisabled(this.treeControl.getDescendants(parentNode));
    }
    this.setNodesToExpandable(this.treeControl.dataNodes);
    this.todoItemSelection(parentNode);
    this.saveExpandedNodes();
    this.treeControl.collapseAll();
    this.restoreExpandedNodes();
  }

  saveExpandedNodes() {
    this.expandedNodes = new Array<TodoItemFlatNode>();
    this.treeControl.dataNodes.forEach(node => {
      if (node.expandable && this.treeControl.isExpanded(node)) {
        this.expandedNodes.push(node);
      }
    });
  }

  restoreExpandedNodes() {
    this.expandedNodes.forEach(node => {
      this.treeControl.expand(this.treeControl.dataNodes.find(n => n.hierarchy === node.hierarchy));
    });
  }

  transformer = (node: TodoItemNode, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.item === node.item
      ? existingNode
      : new TodoItemFlatNode();
    flatNode.item = node.item;
    flatNode.level = level;
    flatNode.hierarchy = node.hierarchy;
    flatNode.nome = node.nome;
    flatNode.expandable = !!node.children;
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  descendantsAllSelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
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
        && descendantsCounter === descendants.length - this.getBaseChildrenCount(node)) {
      descAllSelected = true;
    } else {
      descAllSelected = this.checklistSelection.isSelected(node);
    }
    this.filterSelections();
    return descAllSelected;
  }

  descendantsPartiallySelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    let result = false;
    let descendantsCounter = 0;

    if (descendants.length > 0) {
      for (const desc of descendants) {
        if (this.checklistSelection.isSelected(desc)) {
          descendantsCounter++;
        }
      }
    }
    if ((descendantsCounter < descendants.length - this.getBaseChildrenCount(node))
      && (descendantsCounter > 0)) {
      result = true;
    }
    this.filterSelections();
    return result;
  }

  getBaseChildrenCount(node: TodoItemFlatNode) {
    return this.treeControl.getDescendants(node)
      .filter(currentNode => (currentNode.nome.includes('(BASE)'))).length;
  }

  todoItemSelectionToggle(node: TodoItemFlatNode): void {
    this.checklistSelection.toggle(node);
    if (this.checklistSelection.isSelected(node)) {
      this.callBackend(node);
      this.deselectBASECheck(node);
    }
    const descendants = this.treeControl.getDescendants(node);
    if (this.checklistSelection.isSelected(node)) {
      descendants.forEach((desc) => {
        if (!this.unavailableChecks.includes(desc)) {
          if (!desc.nome.includes('(BASE)')) {
            this.checklistSelection.select(desc);
          } else {
            this.checklistSelection.deselect(desc);
          }
        }
      });
    } else {
      descendants.forEach((desc) => {
        if (!this.unavailableChecks.includes(desc)) {
          this.checklistSelection.deselect(desc);
        }
      });
    }
    descendants.every(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  deselectBASECheck(node: TodoItemFlatNode) {
    if (node) {
      this.getAllParentBaseChildren(node);
      this.baseChild.forEach((child) => { this.checklistSelection.deselect(child); });
      this.baseChild = [];
    }
  }

  getAllParentBaseChildren(node: TodoItemFlatNode) {
    if (node && node !== null && node.level >= 1) {
      const parentNode = this.getParentNode(node);
      if (this.getBaseChild(parentNode).length > 0) {
        this.getBaseChild(parentNode).forEach((child) => this.baseChild.push(child));
        this.getAllParentBaseChildren(parentNode);
      }
    }
  }

  getBaseChild(node: TodoItemFlatNode) {
    if (node) {
      return this.treeControl.getDescendants(node)
        .filter(currentNode => (currentNode.level === node.level + 1)
          && (currentNode.nome.includes('(BASE)')));
    }
  }

  todoItemSelection(node: TodoItemFlatNode): void {
    const descendants = this.treeControl.getDescendants(node);

    if (this.checklistSelection.isSelected(node)) {
      descendants.forEach((desc) => {
        if (!this.unavailableChecks.includes(desc) && !desc.nome.includes('(BASE)')) {
          this.checklistSelection.select(desc);
        }
      });
    } else {
      descendants.forEach((desc) => {
        if (!this.unavailableChecks.includes(desc)) {
          this.checklistSelection.deselect(desc);
        }
      });
    }
    descendants.every(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  todoLeafItemSelectionToggle(node: TodoItemFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);
    if (node.nome.includes('(BASE)')) {
      this.switchAllBrotherNodes(node);
      const parentNode = this.getParentNode(node);
      this.deselectBASECheck(parentNode);
    } else {
      this.deselectBASECheck(node);
    }
  }

  switchAllBrotherNodes(node: TodoItemFlatNode) {
    const parentNode = this.getParentNode(node);
    const immediateChildrenList = this.treeControl.getDescendants(parentNode)
      .filter(currentNode => currentNode.level === parentNode.level + 1);
    if (this.checklistSelection.isSelected(node)) {
      immediateChildrenList.forEach((child) => {
        if (child !== node) {
          this.checklistSelection.deselect(child);
          this.treeControl.getDescendants(child).forEach((desc) => { this.checklistSelection.deselect(desc); });
        }
      });
    }
  }

  checkAllParentsSelection(node: TodoItemFlatNode): void {
    let parent: TodoItemFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  checkRootNodeSelection(node: TodoItemFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.every(child =>
      this.checklistSelection.isSelected(child)
    );
    if (nodeSelected && !descAllSelected) {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      this.checklistSelection.select(node);
    }
  }

  getParentNode(node: TodoItemFlatNode): TodoItemFlatNode | null {
    if (node && node !== null && node.level >= 1) {
      const currentLevel = this.getLevel(node);
      const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
      for (let i = startIndex; i >= 0; i--) {
        const currentNode = this.treeControl.dataNodes[i];
        if (this.getLevel(currentNode) < currentLevel) {
          return currentNode;
        }
      }
    }
    return null;
  }

  filterSelections() {
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
