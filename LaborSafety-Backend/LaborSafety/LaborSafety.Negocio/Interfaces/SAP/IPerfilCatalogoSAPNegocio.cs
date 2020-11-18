﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos.SAP;

namespace LaborSafety.Negocio.Interfaces.SAP
{
    public interface IPerfilCatalogoSAPNegocio
    {
        PerfilCatalogoSAPModelo LoadFromXMLString(string XmlInput);
        PerfilCatalogoSAPResponse ProcessaPerfilCatalogo (PerfilCatalogoSAPModelo modelo);
    }
}