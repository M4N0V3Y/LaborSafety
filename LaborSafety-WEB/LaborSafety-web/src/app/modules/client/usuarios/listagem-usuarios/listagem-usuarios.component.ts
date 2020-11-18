import { Component, OnInit, ViewChild } from '@angular/core';
import { UsuarioPerfis } from 'src/app/shared/models/UsuarioPerfis.Model';
import { MatTableDataSource, MatPaginator, MatTable } from '@angular/material';

@Component({
  selector: 'app-listagem-usuarios',
  templateUrl: './listagem-usuarios.component.html',
  styleUrls: ['./listagem-usuarios.component.scss']
})
export class ListagemUsuariosComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatTable) table: MatTable<any>;

  constructor() { }

  displayedColumns: string[] = ['C8ID','Nome','Perfis'];

  usuariosPerfis: UsuarioPerfis[];
  usuariosPerfisDataSource: MatTableDataSource<UsuarioPerfis>;
  termoPesquisa: string;

  ngOnInit() {
   
  }
  
  filtrar() {
    this.usuariosPerfisDataSource.filter = this.termoPesquisa.trim().toLowerCase();
    if (this.usuariosPerfisDataSource.paginator) {
      this.usuariosPerfisDataSource.paginator.firstPage();
    }
  }

}
