using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace EntregaEColeta.API.App_Start
{
    public interface IContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}
