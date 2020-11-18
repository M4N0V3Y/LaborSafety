using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class PerfilFuncionalidadeModelo
    {
        public long CodPerfilFuncionalidade { get; set; }
        public long CodPerfil { get; set; }
        public long CodFuncionalidade { get; set; }
        public bool Edicao { get; set; }
        public long CodTela { get; set; }

        public string eightIDUsuarioModificador { get; set; }

        public List<PerfilModelo> Perfis { get; set; }
        public List<FuncionalidadeModelo> Funcionalidades { get; set; }
        public List<TelaModelo> Telas { get; set; }
    }
}
