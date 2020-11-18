using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos.SAP;

namespace LaborSafety.Negocio.Interfaces.SAP
{
    public interface ILocalInstalacaoSAPNegocio
    {
        LocalInstalacaoSAPModelo LoadFromXMLString(string XmlInput);
        LocalInstalacaoSAPResponse ProcessaLocalInstalacao(LocalInstalacaoSAPModelo modelo);
    }
}
