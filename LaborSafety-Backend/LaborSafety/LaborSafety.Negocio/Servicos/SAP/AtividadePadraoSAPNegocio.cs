using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.SAP
{
    public class AtividadePadraoSAPNegocio : IAtividadePadraoSAPNegocio
    {
        readonly IAtividadePadraoPersistencia atividadePadraoPersistencia;

        public AtividadePadraoSAPNegocio(IAtividadePadraoPersistencia atividadePadraoPersistencia)
        {
            this.atividadePadraoPersistencia = atividadePadraoPersistencia;
        }

        public AtividadePadraoSAPModelo LoadFromXMLString(string XmlInput)
        {
            using (var stringReader = new System.IO.StringReader(XmlInput))
            {
                var serializer = new XmlSerializer(typeof(AtividadePadraoSAPModelo));
                return serializer.Deserialize(stringReader) as AtividadePadraoSAPModelo;
            }
        }

        public AtividadePadraoSAPResponse ProcessarAtividadePadrao(AtividadePadraoSAPModelo modelo)
        {
            AtividadePadraoSAPResponse response = new AtividadePadraoSAPResponse();
            response.Itens = new List<AtividadePadraoItemSAPResponse>();

            this.ValidaAtividadePadrao(modelo);

            List<AtividadePadraoModelo> listaAtividadePadrao = new List<AtividadePadraoModelo>();
            foreach (var item in modelo.Itens)
            {
                listaAtividadePadrao.Add(new AtividadePadraoModelo()
                {
                    Nome = item.Chave_Modelo,
                    Descricao = item.Texto_Chave_Modelo
                });
            }

            foreach (var atividadePadrao in listaAtividadePadrao)
            {
                AtividadePadraoItemSAPResponse itemResponse = new AtividadePadraoItemSAPResponse();

                try
                {
                    //disciplina.Descricao = Utils.StringUtils.RemoveAcentuacao(disciplina.Descricao);
                    var atividadeExistente = atividadePadraoPersistencia.ListarAtividadePorNome(atividadePadrao.Nome);

                    if (atividadeExistente == null)
                        atividadePadraoPersistencia.Inserir(atividadePadrao);
                    else
                        atividadePadraoPersistencia.Editar(atividadePadrao);

                    itemResponse.Chave_Modelo = atividadePadrao.Nome;
                    itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();
                    itemResponse.Descricao = "";

                }
                catch (Exception ex)
                {
                    itemResponse.Chave_Modelo = atividadePadrao.Nome;
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

        private void ValidaAtividadePadrao(AtividadePadraoSAPModelo modelo)
        {
            try
            {
                if (modelo == null)
                    throw new Exception("O modelo de atividade padrão não foi informado!");

                if (modelo.Itens == null)
                    throw new Exception("A(s) atividade(s) não foi(ram) informada(s)!");

                if (modelo.Itens.Count == 0)
                    throw new Exception("A(s) atividade(s) não foi(ram) informada(s)!");
                else
                {
                    for (int i = 0; i < modelo.Itens.Count; i++)
                    {
                        AtividadePadraoItemSAPModelo item = modelo.Itens[i];

                        if(String.IsNullOrEmpty(item.Chave_Modelo) && String.IsNullOrEmpty(item.Texto_Chave_Modelo))
                            throw new Exception($"O item de posição {i+1} não possui valores para nenhum dos campos!");

                        if (String.IsNullOrEmpty(item.Texto_Chave_Modelo))
                            throw new Exception($"O item de posição {i + 1} não possui valor para o campo TEXTO_CHAVE_MODELO");

                        if (String.IsNullOrEmpty(item.Chave_Modelo))
                            throw new Exception($"O item de posição {i+1} não possui valor para o campo CHAVE_MODELO");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}