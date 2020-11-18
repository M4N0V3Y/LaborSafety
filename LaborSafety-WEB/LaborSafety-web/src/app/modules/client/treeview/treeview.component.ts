import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { TreeviewService } from 'src/app/core/http/treeView/treeview.service';
import { TreeBuilder } from './treeview.treebuilder.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-treeview',
  templateUrl: './treeview.component.html',
  styleUrls: ['./treeview.component.scss'],
  providers: [TreeBuilder]
})
export class TreeViewComponent implements OnInit {
  @Input() dadosPopularTreeView: any;
  @Output() checksSelecionados = new EventEmitter();

  // Mapa de nodos achatados para nodos encadeados. Necessário para achar os nodos a serem "modificados"
  private mapaNodoAchatado = new Map<any, any>();
  // Mapa de nodos encadeados para nodos achatados. Necessário para manter o objeto para seleção no lugar.
  private mapaNodoEncadeado = new Map<any, any>();
  // Propriedades necessárias para controle da TreeView
  private controleTree: FlatTreeControl<any>;
  private achatadorTree: MatTreeFlattener<any, any>;
  private fonteDados: MatTreeFlatDataSource<any, any>;

  // Objeto que armazena as seleções da TreeView
  // O valor booleano define se a TreeView pode selecionar vários nodos
  private checklistSelecionados = new SelectionModel<any>(true);

  // Métodos para abstrair lógica de controle dos nodos.

  // Descobrir nível do nodo
  private getNivel = (node: any) => node.level;

  // Descobrir se o nodo é expansível
  private ehExpansivel = (node: any) => node.expandable;

  // Descobrir quais são os filhos do nodo atual
  private getFilhos = (node: any): any[] => node.Filhos;

  // Descobrir se o nodo atual possui filhos para permitir que o ícone de expansão apareça no frontend
  private temFilho = (_: number, nodeData: any) => nodeData.expandable;

  constructor(public httpClient: HttpClient, public treeViewService: TreeviewService) {
  }

  ngOnInit() {
    // Envia os dados do componente pai para o serviço de construção e organização de TreeView
    this.treeViewService.setDadosTreeView(this.dadosPopularTreeView);
    const database = new TreeBuilder(this.treeViewService);

    // Achata a TreeView para ficar no formato aceito pelo componente
    this.achatadorTree = new MatTreeFlattener(this.transformador, this.getNivel,
      this.ehExpansivel, this.getFilhos);

    // Referencia o objeto de controle da TreeView e os dados organizados para o frontend
    this.controleTree = new FlatTreeControl<any>(this.getNivel, this.ehExpansivel);
    this.fonteDados = new MatTreeFlatDataSource(this.controleTree, this.achatadorTree);

    // Observa se há mudanças na TreeView provocadas pelo componente pai e a atualiza
    database.dataChange.subscribe(data => {
      this.fonteDados.data = data;
    });
  }

   /* Transformador de nodo encadeado para achatado. Serve para mapear os nodos nas estruturas
        reconhecidas pelo componente de TreeView */
   transformador = (node: any, level: number) => {
     const nodoExistente = this.mapaNodoEncadeado.get(node);

     const nodoAchatado = nodoExistente && nodoExistente.nome === node.nome
         ? nodoExistente
         : { nome: '', Filhos: []};
     nodoAchatado.nome = node.nome;
     nodoAchatado.level = level;
     nodoAchatado.expandable = node.Filhos.length > 0;

     this.mapaNodoAchatado.set(nodoAchatado, node);
     this.mapaNodoEncadeado.set(node, nodoAchatado);

     return nodoAchatado;
   }

   /* Decide se todos os descendentes de um nodo estão selecionados
      para que o ícone apropriado de seleção seja mostrado no frontend */
   todosDescendentesSelecionados(node: any): boolean {
     const descendants = this.controleTree.getDescendants(node);
     const todosDescendentesSelecionados = descendants.every(child =>
       this.checklistSelecionados.isSelected(child)
     );
     return todosDescendentesSelecionados;
   }

   /* Decide se apenas parte dos descendentes de um nodo foi selecionada
      para que o ícone apropriado de seleção seja mostrado no frontend */
   descendentesParcialmenteSelecionados(node: any): boolean {
     const descendants = this.controleTree.getDescendants(node);
     const result = descendants.some(child => this.checklistSelecionados.isSelected(child));
     return result && !this.todosDescendentesSelecionados(node);
   }

   // Muda o estado de seleção de um nodo e todos os seus filhos caso existam
   mudancaItemSelecao(node: any): void {
     this.checklistSelecionados.toggle(node);
     const descendants = this.controleTree.getDescendants(node);
     this.checklistSelecionados.isSelected(node)
       ? this.checklistSelecionados.select(...descendants)
       : this.checklistSelecionados.deselect(...descendants);

     // Força a atualização do nodo pai
     descendants.every(child =>
       this.checklistSelecionados.isSelected(child)
     );
     this.checaTodosPaisSelecionados(node);
     this.filterSelections();
   }

   // Muda o estado de seleção de um nodo filho e checa se todos os pais foram modificados
   mudancaItemFilhoSelecao(node: any): void {
     this.checklistSelecionados.toggle(node);
     this.checaTodosPaisSelecionados(node);
     this.filterSelections();
   }

   // Checa todos os nodos pais caso um nodo filho seja selecionado/deselecionado
   checaTodosPaisSelecionados(node: any): void {
     let parent: any | null = this.getNodoPai(node);
     while (parent) {
       this.checaNodoRaizSelecionado(parent);
       parent = this.getNodoPai(parent);
     }
   }

   // Checa o estado nodo raiz e o muda de acordo
   checaNodoRaizSelecionado(node: any): void {
     const nodeSelected = this.checklistSelecionados.isSelected(node);
     const descendants = this.controleTree.getDescendants(node);

     const todosDescendentesSelecionados = descendants.every(child =>
       this.checklistSelecionados.isSelected(child)
     );

     if (nodeSelected && !todosDescendentesSelecionados) {
       this.checklistSelecionados.deselect(node);
     } else if (!nodeSelected && todosDescendentesSelecionados) {
       this.checklistSelecionados.select(node);
     }
   }

   // Checao o pai do nodo atual
   getNodoPai(node: any): any | null {
     const currentLevel = this.getNivel(node);

     if (currentLevel < 1) {
       return null;
     }

     const startIndex = this.controleTree.dataNodes.indexOf(node) - 1;

     for (let i = startIndex; i >= 0; i--) {
       const currentNode = this.controleTree.dataNodes[i];

       if (this.getNivel(currentNode) < currentLevel) {
         return currentNode;
       }
     }
     return null;
   }

   // Faz o filtro de acordo com o estado atual da lista de seleção
   filterSelections(): void {
    const arrayListados = [].concat(this.checklistSelecionados.selected);

    arrayListados.forEach((node) => {
      const ix = arrayListados.indexOf(this.getNodoPai(node));
      if (ix !== -1) {
        arrayListados.splice(ix, 1);
      }
    });
    this.checksSelecionados.emit(arrayListados);
   }
 }
