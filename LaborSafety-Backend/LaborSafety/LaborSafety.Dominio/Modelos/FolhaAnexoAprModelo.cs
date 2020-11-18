using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class FolhaAnexoAprModelo
    {
        public long CodFolhaAnexoApr { get; set; }
        public long CodAnexo { get; set; }
        public long CodApr { get; set; }
        public bool Ativo { get; set; }
    }
}
