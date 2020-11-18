using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class LocalInstalacaoNegocio : ILocalInstalacaoNegocio
    {
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        public LocalInstalacaoNegocio(ILocalInstalacaoPersistencia LocalInstalacaoPersistencia)
        {
            this.localInstalacaoPersistencia = LocalInstalacaoPersistencia;
        }

        public LocalInstalacaoModelo MapeamentoLocalInstalacao(LOCAL_INSTALACAO sistema)
        {
            LocalInstalacaoModelo LocalInstalacao = new LocalInstalacaoModelo()
            {
                CodLocalInstalacao = sistema.CodLocalInstalacao,
                CodInventarioAmbiente = sistema.CodInventarioAmbiente,
                CodPeso = sistema.CodPeso,
                CodPerfilCatalogo = sistema.CodPerfilCatalogo,
                N1 = sistema.N1,
                N2 = sistema.N2,
                N3 = sistema.N3,
                N4 = sistema.N4,
                N5 = sistema.N5,
                N6 = sistema.N6,
                Nome = sistema.Nome,
                Descricao = sistema.Descricao
            };

            return LocalInstalacao;
        }

        public LocalInstalacaoModelo ListarLocalInstalacaoPorId(long id)
        {
            LOCAL_INSTALACAO sis = this.localInstalacaoPersistencia.ListarLocalInstalacaoPorId(id);
            if (sis == null)
            {
                throw new KeyNotFoundException("Local de Instalação não encontrado.");
            }
            return MapeamentoLocalInstalacao(sis);
        }

        public LocalInstalacaoModelo ListarLocalInstalacaoPorNome(string nome)
        {
            LOCAL_INSTALACAO sis = this.localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(nome);
            if (sis == null)
            {
                throw new KeyNotFoundException("Local de Instalação não encontrado.");
            }
            return MapeamentoLocalInstalacao(sis);
        }

        public List<LocalInstalacaoModelo> ListarLIPorNivel(LocalInstalacaoFiltroModelo filtro)
        {
            if (filtro == null)
                throw new KeyNotFoundException("Nenhum filtro foi informado!");

            if (String.IsNullOrEmpty(filtro.N1))
                throw new KeyNotFoundException("É necessário informar o nivel 1!");

            var li = this.localInstalacaoPersistencia.ListarLIPorNivelModelo(filtro);

            return li;
        }

        public IEnumerable<LocalInstalacaoModelo> ListarTodosLIs()
        {
            List<LocalInstalacaoModelo> localInstalacaoModelo = new List<LocalInstalacaoModelo>();

            IEnumerable<LOCAL_INSTALACAO> li = this.localInstalacaoPersistencia.ListarTodosLIs();

            if (li == null)
                throw new KeyNotFoundException("Local de Instalação não encontrado.");

            //Ordenação por nível, decrescente
            //li = li.OrderBy(u => u.Nivel).ToList();

            //var niveis = li.GroupBy(x => x.Nivel).ToList();

            //Mapper...
            foreach (LOCAL_INSTALACAO local in li)
                localInstalacaoModelo.Add(MapeamentoLocalInstalacao(local));

            //Cria uma task por nivel

            /*
            Task primeiroNivel = Task.Factory.StartNew(() =>
            {
                List<LocalInstalacaoModelo> listaNivel1 = localInstalacaoModelo.Where(x => x.Nivel == 1).ToList();

                foreach (var item in listaNivel1)
                {
                    var registroPai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();
                    var indicePaiLista = localInstalacaoModelo.FindIndex(x => x.CodPai == registroPai.CodPai);
                    localInstalacaoModelo[indicePaiLista].Filhos.Add(item);
                }
            });
            */

            /*
            Task segundoNivel = Task.Factory.StartNew(() =>
            {
                List<LocalInstalacaoModelo> listaNivel2 = localInstalacaoModelo.Where(x => x.Nivel == 2).ToList();

                foreach (var item in listaNivel2)
                {
                    var registroPai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();
                    var indicePaiLista = localInstalacaoModelo.FindIndex(x => x.CodPai == registroPai.CodPai);
                    localInstalacaoModelo[indicePaiLista].Filhos.Add(item);
                }
            });

            Task terceiroNivel = Task.Factory.StartNew(() =>
            {
                List<LocalInstalacaoModelo> listaNivel3 = localInstalacaoModelo.Where(x => x.Nivel == 3).ToList();

                foreach (var item in listaNivel3)
                {
                    var registroPai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();
                    var indicePaiLista = localInstalacaoModelo.FindIndex(x => x.CodPai == registroPai.CodPai);
                    localInstalacaoModelo[indicePaiLista].Filhos.Add(item);
                }
            });

            
            Task quartoNivel = Task.Factory.StartNew(() =>
            {
                List<LocalInstalacaoModelo> listaNivel4 = localInstalacaoModelo.Where(x => x.Nivel == 4).ToList();

                foreach (var item in listaNivel4)
                {
                    var registroPai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();
                    var indicePaiLista = localInstalacaoModelo.FindIndex(x => x.CodPai == registroPai.CodPai);
                    localInstalacaoModelo[indicePaiLista].Filhos.Add(item);
                }
            });
            

            
            Task quintoNivel = Task.Factory.StartNew(() =>
            {
                List<LocalInstalacaoModelo> listaNivel5 = localInstalacaoModelo.Where(x => x.Nivel == 5).ToList();

                foreach (var item in listaNivel5)
                {
                    var registroPai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();
                    var indicePaiLista = localInstalacaoModelo.FindIndex(x => x.CodPai == registroPai.CodPai);
                    localInstalacaoModelo[indicePaiLista].Filhos.Add(item);
                }
            });
            
            */

            //Task.WaitAll(primeiroNivel, segundoNivel, terceiroNivel, quartoNivel, quintoNivel);
            //Task.WaitAll(primeiroNivel, segundoNivel, terceiroNivel, quartoNivel);
            //Task.WaitAll(primeiroNivel, segundoNivel, terceiroNivel);
            //Task.WaitAll(segundoNivel, terceiroNivel, quartoNivel, quintoNivel);

            /*

            foreach (var item in localInstalacaoModelo)
            {
                if (item.Nivel == 2)
                {
                    var pai = localInstalacaoModelo.Where(a => a.CodLocalInstalacao == item.CodPai).FirstOrDefault();

                    //Para o caso do primeiro nível (que não tem pai)
                    if (pai != null)
                    {
                        var indicePai = localInstalacaoModelo.FindIndex(x => x.CodPai == pai.CodPai);
                        localInstalacaoModelo[indicePai].Filhos.Add(item);
                    }
                }

            }
            */

            //List<LocalInstalacaoModelo> listaresult = new List<LocalInstalacaoModelo>();
            //listaresult.Add(localInstalacaoModelo[localInstalacaoModelo.Count - 1]);

            //return listaresult;
            return localInstalacaoModelo;
        }
    }
}