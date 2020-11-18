using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using LaborSafety.Persistencia;
using System;
using ClosedXML.Excel;
using System.Threading.Tasks;
using System.Linq;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IRascunhoInventarioAtividadeNegocio
    {
        RascunhoInventarioAtividadeModelo ListarInventarioAtividadePorId(long id);
        List<RascunhoInventarioAtividadeModelo> ListarInventarioAtividadePorLI(List<string> nomesLi);
        List<RascunhoInventarioAtividadeModelo> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo);
        void InserirRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo);
        void EditarInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo);
        void ExcluirRascunhoInventarioAtividade(long id);
    }
}
