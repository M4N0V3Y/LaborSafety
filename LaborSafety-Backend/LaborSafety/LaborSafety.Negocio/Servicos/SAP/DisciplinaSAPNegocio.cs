using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.SAP
{
    public class DisciplinaSAPNegocio : IDisciplinaSAPNegocio
    {
        readonly IDisciplinaPersistencia disciplinaPersistencia;

        public DisciplinaSAPNegocio(IDisciplinaPersistencia disciplinaPersistencia) 
        {
            this.disciplinaPersistencia = disciplinaPersistencia;
        }

        public DisciplinaSAPModelo LoadFromXMLString(string XmlInput)
        {
            using (var stringReader = new System.IO.StringReader(XmlInput))
            {
                var serializer = new XmlSerializer(typeof(DisciplinaSAPModelo));
                return serializer.Deserialize(stringReader) as DisciplinaSAPModelo;
            }
        }

        public DisciplinaSAPResponse ProcessarDisciplina(DisciplinaSAPModelo modelo)
        {
            DisciplinaSAPResponse response = new DisciplinaSAPResponse();
            response.Itens = new List<DisciplinaItemSAPResponse>();

            this.ValidaDisciplina(modelo);

            List<DisciplinaModelo> listaDisciplina = new List<DisciplinaModelo>();
            foreach (var item in modelo.Itens)
            {
                listaDisciplina.Add(new DisciplinaModelo()
                {
                    Nome = item.ValorCaracteristica,
                    Descricao = item.ValorCaracteristica
                });
            }

            foreach (var disciplina in listaDisciplina)
            {
                DisciplinaItemSAPResponse itemResponse = new DisciplinaItemSAPResponse();

                try
                {
                    //disciplina.Descricao = Utils.StringUtils.RemoveAcentuacao(disciplina.Descricao);
                    var disciplinaExistente = disciplinaPersistencia.ListarDisciplinaPorNome(disciplina.Descricao);

                    if (disciplinaExistente == null)
                        disciplinaPersistencia.Inserir(disciplina);
                    else
                        disciplinaPersistencia.Editar(disciplina);

                    itemResponse.Caracteristica = disciplina.Descricao;
                    itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();
                    itemResponse.Descricao = "";
                }
                catch (Exception ex)
                {
                    itemResponse.Caracteristica = disciplina.Descricao;
                    itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                    itemResponse.Descricao = ex.Message;
                }
                finally
                {
                    response.Itens.Add(itemResponse);
                }
            }

            return response;
        }

        private void ValidaDisciplina(DisciplinaSAPModelo modelo)
        {
            try
            {
                if (modelo == null)
                    throw new Exception("O modelo de disciplina não foi informado!");

                if(modelo.Itens == null)
                    throw new Exception("A(s) disciplina(s) não foi(ram) informada(s)!");

                if (modelo.Itens.Count == 0)
                    throw new Exception("A(s) disciplina(s) não foi(ram) informada(s)!");
                else
                {
                    for (int i = 0; i < modelo.Itens.Count; i++)
                    {
                        DisciplinaItemSAPModelo item = modelo.Itens[i];

                        if (String.IsNullOrEmpty(item.Caracteristica) && String.IsNullOrEmpty(item.ValorCaracteristica))
                            throw new Exception($"O item de posição {i + 1} não possui valores para nenhum dos campos!");

                        if (String.IsNullOrEmpty(item.ValorCaracteristica))
                            throw new Exception($"O item de posição {i + 1} não possui valor para o campo VALOR_CARACTERISTICA");
                    }

                    //foreach (var item in modelo.Itens)
                    //{
                    //    if(String.IsNullOrEmpty(item.ValorCaracteristica))
                    //        throw new Exception($"A disciplina {item.Caracteristica} não foi informada!");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
