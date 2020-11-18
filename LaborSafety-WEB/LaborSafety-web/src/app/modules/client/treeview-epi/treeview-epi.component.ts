import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter, OnDestroy, ElementRef, Input} from '@angular/core';
import {SelectionModel} from '@angular/cdk/collections';
import {FlatTreeControl} from '@angular/cdk/tree';
import {MatTreeFlatDataSource, MatTreeFlattener, MatTree} from '@angular/material/tree';
import {BehaviorSubject} from 'rxjs';
import { EPIService } from 'src/app/core/http/epi/epi.service';
import { EPI } from 'src/app/shared/models/EPI.model';

export class EPINode {
  children: EPINode[];
  item: string;
  description: string;
  code: number;
}

export class EPIFlatNode {
  item: string;
  level: number;
  expandable: boolean;
  description: string;
  CodEPI: number;
}

const TREE_DATA = {
  EPI: {}
};

@Injectable()
export class ChecklistDatabaseEPI {
  dataChange = new BehaviorSubject<EPINode[]>([]);

  get data(): EPINode[] { return this.dataChange.value; }

  constructor() {
    this.initialize();
  }

  initialize() {
    const data = this.buildFileTree(TREE_DATA, 0);
    this.dataChange.next(data);
  }

  buildFileTree(obj: {[key: string]: any}, level: number): EPINode[] {
    return Object.keys(obj).reduce<EPINode[]>((accumulator, key) => {
      const value = obj[key];
      const node = new EPINode();
      node.item = key;
      node.description = key;

      if (value != null) {
        if (typeof value === 'object') {
          node.children = this.buildFileTree(value, level + 1);
        } else {
          node.item = value;
        }
      }

      return accumulator.concat(node);
    }, []);
  }

  insertItem(parent: EPINode, name: string, desc: string, cod: number) {
    if (parent.children) {
      parent.children.push({item: name, description: desc, code: cod} as EPINode);
      this.dataChange.next(this.data);
    }
  }
}

@Component({
  selector: 'app-treeview-epi',
  templateUrl: './treeview-epi.component.html',
  styleUrls: ['./treeview-epi.component.scss']
})
export class TreeviewEPIComponent implements OnInit, OnDestroy {
  @ViewChild('epiTreeSelector') tree: MatTree<EPINode>;
  @Input() EPIInput: string[];
  @Output() selectedEPIs = new EventEmitter();

  flatNodeMap = new Map<EPIFlatNode, EPINode>();
  nestedNodeMap = new Map<EPINode, EPIFlatNode>();
  selectedParent: EPIFlatNode | null = null;
  treeControl: FlatTreeControl<EPIFlatNode>;
  treeFlattener: MatTreeFlattener<EPINode, EPIFlatNode>;
  dataSource: MatTreeFlatDataSource<EPINode, EPIFlatNode>;
  checklistSelection = new SelectionModel<EPIFlatNode>(true);

  getLevel = (node: EPIFlatNode) => node.level;
  isExpandable = (node: EPIFlatNode) => node.expandable;
  getChildren = (node: EPINode): EPINode[] => node.children;
  hasChild = (_: number, nodeData: EPIFlatNode) => nodeData.expandable;
  hasNoContent = (_: number, nodeData: EPIFlatNode) => nodeData.item === '';


  constructor(private database: ChecklistDatabaseEPI, public epiService: EPIService,
              private elementRef: ElementRef) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
      this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<EPIFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    database.dataChange.subscribe(data => {
      this.dataSource.data = data;
    });
  }

  ngOnDestroy() {
    this.elementRef.nativeElement.remove();
  }


  async ngOnInit() {
    const parentNode = this.treeControl.dataNodes.find(e => e.item === 'EPI');
    parentNode.description = parentNode.item;
    parentNode.expandable = true;
    parentNode.level = 0;
    await this.callBackend(parentNode);
    this.treeControl.getDescendants(parentNode).forEach(async (node) => { await this.callBackend(node); });
    if (this.EPIInput) {
      await this.checkPreviouslyExistingEPIToRisk(this.EPIInput);
    }
    this.setNodesToExpandable(this.treeControl.dataNodes);
    this.treeControl.expandAll();
  }

  async checkPreviouslyExistingEPIToRisk(epis: string[]) {
    for (const epi of epis) {
      const temp = epi.split('/');
      let i = 0;
      let currentNodeString = '';
      let nextNode: EPIFlatNode;
      for (i = 0; i < temp.length; i++) {
        currentNodeString = '';
        for (let j = 0; j <= i; j++) {
          if (j === 0) {
            currentNodeString = temp[j];
          } else {
            currentNodeString += '/' + temp[j];
          }
          nextNode = this.treeControl.dataNodes.find(e => e.item === currentNodeString);
        }
      }
      this.checklistSelection.select(nextNode);
      this.todoItemSelection(nextNode);
    }
  }
  todoItemSelection(node: EPIFlatNode): void {
    const descendants = this.treeControl.getDescendants(node);

    if (this.checklistSelection.isSelected(node)) {
      descendants.forEach((desc) => {
          this.checklistSelection.select(desc);
      });
    } else {
      descendants.forEach((desc) => {
          this.checklistSelection.deselect(desc);
      });
    }
    descendants.every(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  async callBackend(node: EPIFlatNode) {
    if (!this.checkIfNodeHasChildren(node)) {
      await this.epiService.ListarTodosEPIPorNivel(node.item).then(
        (data: EPI[]) => { this.addAllDataToTreeview(data, node); }
      );
    }
  }

  checkIfNodeHasChildren(node: EPIFlatNode) {
    const currentNodeReference = this.flatNodeMap.get(node);
    if (currentNodeReference.children && currentNodeReference.children.length > 0) {
      return true;
    } else {
      return false;
    }
  }

  addAllDataToTreeview(data: EPI[], node: EPIFlatNode) {
    const parentNode = this.treeControl.dataNodes.find(e => e.item === node.item);
    const referenceNode = this.flatNodeMap.get(parentNode);
    if (data.length === 0) {
      parentNode.expandable = false;
    } else {
      referenceNode.children = [];
      data.forEach((epi) => {
        this.database.insertItem(referenceNode, epi.Nome, this.getDescriptionByLevel(epi, node), epi.CodEPI);
        this.treeControl.expand(parentNode);
      });
    }
    if (this.checklistSelection.isSelected(parentNode)) {
      this.selectAllDescendants(parentNode);
    }
    this.setNodesToExpandable(this.treeControl.dataNodes);
    this.refreshTree();
  }

  getDescriptionByLevel(epi: EPI, node: EPIFlatNode) {
    const workString = epi.Nome.split('/');
    switch (node.level) {
      case 0: return workString.slice(1, 2).join();
      case 1: return workString.slice(2, 3).join();
      default: return workString.join();
    }
  }

  selectAllDescendants(node: EPIFlatNode) {
    const descendants = this.treeControl.getDescendants(node);
    descendants.forEach((desc) => this.checklistSelection.select(desc));
  }

  setNodesToExpandable(nodes: EPIFlatNode[]) {
    if (nodes) {
      for (const node of nodes) {
        if (node.level !== 2) {
          node.expandable = true;
        } else {
          node.expandable = false;
        }
      }
    }
  }

  refreshTree() {
    const expandedNodes = new Array<EPIFlatNode>();
    this.treeControl.dataNodes.forEach(node => {
      if (node.expandable && this.treeControl.isExpanded(node)) {
        expandedNodes.push(node);
      }
    });

    this.treeControl.collapseAll();

    expandedNodes.forEach(node => {
      this.treeControl.expand(this.treeControl.dataNodes.find(n => n.item === node.item));
    });
  }


   transformer = (node: EPINode, level: number) => {
     const existingNode = this.nestedNodeMap.get(node);
     const flatNode = existingNode && existingNode.item === node.item
         ? existingNode
         : new EPIFlatNode();
     flatNode.item = node.item;
     flatNode.description = node.description;
     flatNode.level = level;
     flatNode.CodEPI = node.code;
     flatNode.expandable = !!node.children;
     this.flatNodeMap.set(flatNode, node);
     this.nestedNodeMap.set(node, flatNode);
     return flatNode;
   }

   descendantsAllSelected(node: EPIFlatNode): boolean {
     const descendants = this.treeControl.getDescendants(node);
     let descAllSelected = false;
     if (descendants.length > 0) {
      descAllSelected = descendants.every(child =>
        this.checklistSelection.isSelected(child)
      );
     }
     this.filterSelections();
     return descAllSelected;
   }

   descendantsPartiallySelected(node: EPIFlatNode): boolean {
     const descendants = this.treeControl.getDescendants(node);
     const result = descendants.some(child => this.checklistSelection.isSelected(child));
     this.filterSelections();
     return result && !this.descendantsAllSelected(node);
   }

   todoItemSelectionToggle(node: EPIFlatNode): void {
     this.checklistSelection.toggle(node);
     if (this.checklistSelection.isSelected(node)) {
       this.callBackend(node);
     }
     const descendants = this.treeControl.getDescendants(node);
     this.checklistSelection.isSelected(node)
       ? this.checklistSelection.select(...descendants)
       : this.checklistSelection.deselect(...descendants);

     descendants.every(child =>
       this.checklistSelection.isSelected(child)
     );
     this.checkAllParentsSelection(node);
   }

   todoLeafItemSelectionToggle(node: EPIFlatNode): void {
     this.checklistSelection.toggle(node);
     this.checkAllParentsSelection(node);
   }

   checkAllParentsSelection(node: EPIFlatNode): void {
     let parent: EPIFlatNode | null = this.getParentNode(node);
     while (parent) {
       this.checkRootNodeSelection(parent);
       parent = this.getParentNode(parent);
     }
   }

   checkRootNodeSelection(node: EPIFlatNode): void {
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

   getParentNode(node: EPIFlatNode): EPIFlatNode | null {
     const currentLevel = this.getLevel(node);

     if (currentLevel < 1) {
       return null;
     }
     const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
     for (let i = startIndex; i >= 0; i--) {
       const currentNode = this.treeControl.dataNodes[i];
       if (this.getLevel(currentNode) < currentLevel) {
         return currentNode;
       }
     }
     return null;
   }

   filterSelections() {
    const selectedArray = this.checklistSelection.selected;
    selectedArray.forEach((node) => {
      const ix = selectedArray.indexOf(node);
      if (node.level === 0 || node.level === 1) {
        selectedArray.splice(ix, 1);
      }
    });
    this.selectedEPIs.emit(selectedArray);
  }
}
