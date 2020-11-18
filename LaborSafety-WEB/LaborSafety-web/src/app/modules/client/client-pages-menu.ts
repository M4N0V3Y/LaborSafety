export class MenuItem {
  title: string;
  titlePage: string;
  icon: string;
  link?: string;
  requiredPermission?: number;
  children?: Array<{ title: string; link: string, icon?: string, requiredPermission?: number, titlePage?: string }>;
  childrenPage?: Array<{ title: string; link: string }>;
  home?: boolean;
  CodTela?: number;
}
export const MENU_ITEMS: MenuItem[] = [
  {
    title: 'Home',
    titlePage: 'Home',
    icon: 'home',
    link: '/client/dashboard',
    home: true,
    requiredPermission: 2,
    CodTela: 0,
  },
  {
    title: 'APR',
    titlePage: 'APR',
    icon: 'insert_drive_file',
    link: '/client/aprpt/',
    requiredPermission: 2,
    CodTela: 4,
  },
  {
    title: 'Inventário de Ambiente',
    titlePage: 'Inventário de Ambiente',
    icon: 'format_list_bulleted',
    link: '/client/inventario-ambiente/',
    requiredPermission: 2,
    CodTela: 2,
  },
  {
    title: 'Inventário de Atividade',
    titlePage: 'Inventário de Atividade',
    icon: 'format_list_numbered',
    link: '/client/inventario-atividade/',
    requiredPermission: 2,
    CodTela: 3,
  },
  {
    title: 'Importar/Exportar',
    titlePage: 'Importar/Exportar',
    icon: 'import_export',
    link: '/client/importacao/',
    requiredPermission: 2,
    CodTela: 5,
  },
  {
    title: 'Gestão de Perfis',
    titlePage: 'Gestão de Perfis',
    icon: 'person_pin',
    link: '/client/perfil/',
    requiredPermission: 2,
    CodTela: 1,
  },
  {
    title: 'Impressão',
    titlePage: 'Impressão',
    icon: 'print',
    link: '/client/impressao',
    requiredPermission: 2,
    CodTela: 6,
  },
  // {
  //   title: 'Usuários',
  //   titlePage: 'Usuários',
  //   icon: 'person',
  //   link: '/client/usuarios/listagem',
  //   requiredPermission: 2
  // },
];
