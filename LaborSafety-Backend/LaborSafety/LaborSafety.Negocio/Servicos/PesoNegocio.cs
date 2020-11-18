using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class PesoNegocio : IPesoNegocio
    {
        private readonly IPesoPersistencia pesoPersistencia;
        public PesoNegocio(IPesoPersistencia pesoPersistencia)
        {
            this.pesoPersistencia = pesoPersistencia;
        }

        public PesoModelo MapeamentoPeso(PESO peso)
        {
            PesoModelo pesoModelo = new PesoModelo()
            {
                CodPeso = peso.CodPeso,
                Indice = peso.Indice,
                Nome = peso.Nome
            };

            return pesoModelo;
        }

        public PesoModelo ListarPesoPorId(long id)
        {
            PESO pes = this.pesoPersistencia.ListarPesoPorId(id);
            if (pes == null)
            {
                throw new KeyNotFoundException("Peso não encontrado.");
            }
            return MapeamentoPeso(pes);
        }

        public PesoModelo ListarPesoPorIndice(int indice)
        {
            PESO pes = this.pesoPersistencia.ListarPesoPorIndice(indice);
            if (pes == null)
            {
                throw new KeyNotFoundException("Peso não encontrado.");
            }
            return MapeamentoPeso(pes);
        }

        public PesoModelo ListarPesoPorNome(string nome)
        {
            PESO pes = this.pesoPersistencia.ListarPesoPorNome(nome);
            if (pes == null)
            {
                throw new KeyNotFoundException("Peso não encontrado.");
            }
            return MapeamentoPeso(pes);
        }

        public IEnumerable<PesoModelo> ListarTodosPesos()
        {
            List<PesoModelo> pesoModelo = new List<PesoModelo>();

            IEnumerable<PESO> pesos = this.pesoPersistencia.ListarTodosPesos();

            if (pesos == null)
            {
                throw new KeyNotFoundException("Peso não encontrado.");
            }

            foreach (PESO peso in pesos)
            {
                pesoModelo.Add(MapeamentoPeso(peso));
            }

            return pesoModelo;
        }
    }
}
