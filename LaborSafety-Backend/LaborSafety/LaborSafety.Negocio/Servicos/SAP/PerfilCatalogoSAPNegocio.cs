using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.SAP
{
    public class PerfilCatalogoSAPNegocio : IPerfilCatalogoSAPNegocio
    {
        readonly IPerfilCatalogoPersistencia perfilCatalogoPersistencia;

        public PerfilCatalogoSAPNegocio(IPerfilCatalogoPersistencia perfilCatalogoPersistencia)
        {
            this.perfilCatalogoPersistencia = perfilCatalogoPersistencia;
        }

        public PerfilCatalogoSAPModelo LoadFromXMLString(string XmlInput)
        {
            using (var stringReader = new System.IO.StringReader(XmlInput))
            {
                var serializer = new XmlSerializer(typeof(PerfilCatalogoSAPModelo));
                return serializer.Deserialize(stringReader) as PerfilCatalogoSAPModelo;
            }
        }

        public PerfilCatalogoSAPResponse ProcessaPerfilCatalogo(PerfilCatalogoSAPModelo modelo)
        {
            PerfilCatalogoSAPResponse response = new PerfilCatalogoSAPResponse();
            response.Itens = new List<PerfilCatalogoItemSAPResponse>();

            this.ValidaPerfilCatalogo(modelo);

            using (var entities = new DB_APRPTEntities())
            {
                entities.Database.CommandTimeout = 9999;

                using (var transaction = entities.Database.BeginTransaction())
                {
                    PerfilCatalogoItemSAPResponse itemResponse = new PerfilCatalogoItemSAPResponse();
                    PerfilCatalogoModelo perfilCatalogo = new PerfilCatalogoModelo();

                    try
                    {
                        List<PerfilCatalogoModelo> listaPerfilCatalogo = new List<PerfilCatalogoModelo>();
                        foreach (var item in modelo.Itens)
                        {
                            listaPerfilCatalogo.Add(new PerfilCatalogoModelo()
                            {
                                Codigo = item.PerfilCatalogo,
                                Nome = item.Descricao,
                            });
                        }

                        foreach (var perfil in listaPerfilCatalogo)
                        {
                            perfilCatalogo = perfil;

                            var perfilExistente = perfilCatalogoPersistencia.ListarPerfilCatalogoPorCodigo(perfilCatalogo.Codigo);

                            if (perfilExistente == null)
                                perfilCatalogoPersistencia.Inserir(perfilCatalogo);
                            else
                                perfilCatalogoPersistencia.Editar(perfilCatalogo, entities);

                            itemResponse.Perfil_Do_Catalogo = perfilCatalogo.Codigo;
                            itemResponse.Descricao = "";
                            itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();

                            response.Itens.Add(itemResponse);
                        }

                        entities.SaveChanges();
                        transaction.Commit();

                        return response;

                    }
                    catch (Exception ex)
                    {
                        itemResponse.Perfil_Do_Catalogo = perfilCatalogo.Codigo;
                        itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                        itemResponse.Descricao = ex.Message;

                        throw ex;
                    }
                }
            }
        }

        private void ValidaPerfilCatalogo(PerfilCatalogoSAPModelo modelo)
        {
            try
            {
                if (modelo == null)
                    throw new Exception("O modelo de perfil de catálogo não foi informado!");

                if (modelo.Itens == null)
                    throw new Exception("O(s) perfil(perfis) de catálogo não foi(ram) informado(s)!");

                if (modelo.Itens.Count == 0)
                    throw new Exception("O(s) perfil(perfis) de catálogo não foi(ram) informado(s)!");
                else
                {
                    for (int i = 0; i < modelo.Itens.Count; i++)
                    {
                        PerfilCatalogoItemSAPModelo item = modelo.Itens[i];

                        if (String.IsNullOrEmpty(item.PerfilCatalogo) && String.IsNullOrEmpty(item.Descricao))
                            throw new Exception($"O item de posição {i + 1} não possui valores para nenhum dos campos!");

                        if (String.IsNullOrEmpty(item.PerfilCatalogo))
                            throw new Exception($"O item de posição {i + 1} não possui valor para o campo PERFIL_CATALOGO");

                        if (String.IsNullOrEmpty(item.Descricao))
                            throw new Exception($"O item de posição {i + 1} não possui valor para o campo DESCRICAO");
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
