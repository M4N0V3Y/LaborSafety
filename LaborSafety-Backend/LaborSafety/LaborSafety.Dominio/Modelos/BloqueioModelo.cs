using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class BloqueioModelo
    {
        public long CodBloqueio { get; set; }
        public long CodLocalABloquear { get; set; }
        public long CodArea { get; set; }
        public long CodTagKKSBloqueio { get; set; }
        public long CodDispositivoBloqueio { get; set; }
        public long CodTipoEnergiaBloqueio { get; set; }

        public List<LocalABloquearModelo> LOCAL_A_BLOQUEAR { get; set; }
        public List<AreaModelo> AREA { get; set; }
        public List<Tag_Kks_Modelo> TAG_KKS_BLOQUEIO { get; set; }
        public List<DispositivoBloqueioModelo> DISPOSITIVO_BLOQUEIO { get; set; }
        public List<TipoEnergiaBloqueioModelo> TIPO_ENERGIA_BLOQUEIO { get; set; }
    }
}
