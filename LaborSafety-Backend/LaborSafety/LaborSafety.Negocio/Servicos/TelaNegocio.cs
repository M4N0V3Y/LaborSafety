using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class TelaNegocio : ITelaNegocio
    {
        private readonly ITelaPersistencia telaPersistencia;
        public TelaNegocio(ITelaPersistencia telaPersistencia)
        {
            this.telaPersistencia = telaPersistencia;
        }

        public List<TelaModelo> Listar()
        {
            List<TelaModelo> listaTelas = new List<TelaModelo>();
            AutoMapper.Mapper.Map(this.telaPersistencia.Listar(), listaTelas);
            return listaTelas;
        }

        public TelaModelo ListarPorCodigo(long codigo)
        {
            TelaModelo tela = this.telaPersistencia.ListarPorCodigo(codigo);
            if (tela == null)
            {
                throw new KeyNotFoundException("Tela não encontrada.");
            }
            return tela;
        }
    }
}
