using ClosedXML.Excel;
using System.Configuration;
using System.Runtime.Caching;

namespace LaborSafety.Negocio.Servicos
{
    public static class CacheFolhaAprNegocio
    {
        static MemoryCache  cache = new MemoryCache("CacheApr");
        public static XLWorkbook ObterFolhaApr()
        {
            try
            {
                if (cache.Contains("FolhaAPR"))
                {
                    var folhaApr = (XLWorkbook)cache.Get("FolhaAPR");
                    LimparFolhaAPR(folhaApr);
                    return folhaApr;
                }
                else
                {
                    var caminhoFolhaApr = ConfigurationManager.AppSettings["caminhoFolhaApr"];
                    // A cache por default está como inexpirável ( Avaliar possibilidade de mudança)
                    var cachePolicy = new CacheItemPolicy();
                    var folhaApr = new XLWorkbook(caminhoFolhaApr);
                    var itemCacheFolhaApr = new CacheItem("FolhaAPR", folhaApr);
                    cache.Add(itemCacheFolhaApr, cachePolicy);
                    return folhaApr;
                }
            }

            catch
            {
                throw;
            }

        }

        public static XLWorkbook ObterFolhaComplementar()
        {
            try
            {
                if (cache.Contains("FolhaComplementar"))
                {
                    var folhaComplementar = (XLWorkbook)cache.Get("FolhaComplementar");
                    return RealizarCopiaWorkbook(folhaComplementar);
                }
                else
                {
                    var caminhoFolhaComplementar = ConfigurationManager.AppSettings["caminhoFolhaComplementar"];
                    var cachePolicy = new CacheItemPolicy();
                    var folhaComplementar = new XLWorkbook(caminhoFolhaComplementar);
                    var itemCacheFolhaComplementar = new CacheItem("FolhaComplementar", folhaComplementar);
                    cache.Add(itemCacheFolhaComplementar, cachePolicy);
                    return RealizarCopiaWorkbook(folhaComplementar);
                }
            }

            catch
            {
                throw;
            }

        }


        public static void InicializarCacheFolhas()
        {
            ObterFolhaApr();
            //ObterFolhaComplementar();
        }





        private static XLWorkbook RealizarCopiaWorkbook(XLWorkbook folhaOriginal)
        {
            XLWorkbook novoWorkBook = new XLWorkbook();
            foreach (var folha in folhaOriginal.Worksheets)
            {
                folha.CopyTo(novoWorkBook,folha.Worksheet.Name);
            }
            return novoWorkBook;
        }



        private static void LimparFolhaAPR(XLWorkbook folhaOriginal)
        {
            var primeiraFolha = folhaOriginal.Worksheet(1);
            var segundaFolha = folhaOriginal.Worksheet(2);
            var terceiraFolha = folhaOriginal.Worksheet(3);
            LimparPrimeiraFolha(primeiraFolha);
            LimparSegundaFolha(segundaFolha);
            LimparTerceiraFolha(terceiraFolha);
        }

        private static void LimparPrimeiraFolha(IXLWorksheet folha)
        {
            folha.Cell("BG4").Value = "";
            folha.Cell("BZ4").Value = "";
            folha.Cell("CF4").Value = "";
            folha.Cell("AA18").Value = "";
            folha.Cell("C26").Value = "";
            folha.Cell("BB18").Value = "";
            folha.Cell("AA10").Value = "";
            folha.Cell("AA18").Value = "";
            folha.Cell("C94").Value = "";
            folha.Cell("AF94").Value = "";
            folha.Cell("BI94").Value = "";
            folha.Cell("C98").Value = "";
            folha.Cell("AF98").Value = "";
            folha.Cell("BI98").Value = "";
            folha.Cell("C102").Value = "";
            folha.Cell("AF102").Value = "";
            folha.Cell("AI102").Value = "";
            folha.Cell("BI102").Value = "";
            folha.Cell("BL102").Value = "";
            folha.Cell("C106").Value = "";
            folha.Cell("F106").Value = "";
            folha.Cell("AF106").Value = "";
            folha.Cell("AI106").Value = "";
            folha.Cell("BI106").Value = "";
            folha.Cell("BL106").Value = "";
            folha.Cell("C114").Value = "";
            folha.Cell("C120").Value = "";
            folha.Cell("Q114").Value = "";
            folha.Cell("F41").Value = "";
            folha.Cell("AP41").Value = "";
            folha.Cell("BF41").Value = "";
            folha.Cell("F45").Value = "";
            folha.Cell("AP45").Value = "";
            folha.Cell("BF45").Value = "";
            folha.Cell("F49").Value = "";
            folha.Cell("AP49").Value = "";
            folha.Cell("BF49").Value = "";
            folha.Cell("F53").Value = "";
            folha.Cell("AP53").Value = "";
            folha.Cell("BF53").Value = "";
            folha.Cell("F57").Value = "";
            folha.Cell("AP57").Value = "";
            folha.Cell("BF57").Value = "";
            folha.Cell("F61").Value = "";
            folha.Cell("AP61").Value = "";
            folha.Cell("BF61").Value = "";
            folha.Cell("F65").Value = "";
            folha.Cell("AP65").Value = "";
            folha.Cell("BF65").Value = "";
            folha.Cell("F69").Value = "";
            folha.Cell("AP69").Value = "";
            folha.Cell("BF69").Value = "";
            folha.Cell("F73").Value = "";
            folha.Cell("AP73").Value = "";
            folha.Cell("BF73").Value = "";
            folha.Cell("F77").Value = "";
            folha.Cell("AP77").Value = "";
            folha.Cell("BF77").Value = "";
            folha.Cell("F81").Value = "";
            folha.Cell("AP81").Value = "";
            folha.Cell("BF81").Value = "";
            folha.Cell("F85").Value = "";
            folha.Cell("AP85").Value = "";
            folha.Cell("BF85").Value = "";
        }

        private static void LimparSegundaFolha(IXLWorksheet folha)
        {
            folha.Cell("BG4").Value = "";
            folha.Cell("BZ4").Value = "";
            folha.Cell("CF4").Value = "";
            folha.Cell("D12").Value = "";
            folha.Cell("R12").Value = "";
            folha.Cell("AK12").Value = "";
            folha.Cell("BC12").Value = "";
            folha.Cell("D17").Value = "";
            folha.Cell("R16").Value = "";
            folha.Cell("AK16").Value = "";
            folha.Cell("BC16").Value = "";
            folha.Cell("D22").Value = "";
            folha.Cell("R21").Value = "";
            folha.Cell("AK21").Value = "";
            folha.Cell("BC21").Value = "";
            folha.Cell("D27").Value = "";
            folha.Cell("R26").Value = "";
            folha.Cell("AK26").Value = "";
            folha.Cell("BC26").Value = "";
            folha.Cell("D32").Value = "";
            folha.Cell("R31").Value = "";
            folha.Cell("AK31").Value = "";
            folha.Cell("BC31").Value = "";
            folha.Cell("D37").Value = "";
            folha.Cell("R36").Value = "";
            folha.Cell("AK36").Value = "";
            folha.Cell("BC36").Value = "";
            folha.Cell("D42").Value = "";
            folha.Cell("R41").Value = "";
            folha.Cell("AK41").Value = "";
            folha.Cell("BC41").Value = "";
            folha.Cell("D47").Value = "";
            folha.Cell("R46").Value = "";
            folha.Cell("AK46").Value = "";
            folha.Cell("BC46").Value = "";
            folha.Cell("D57").Value = "";
            folha.Cell("R57").Value = "";
            folha.Cell("AK57").Value = "";
            folha.Cell("BC57").Value = "";
            folha.Cell("D62").Value = "";
            folha.Cell("R61").Value = "";
            folha.Cell("AK61").Value = "";
            folha.Cell("BC61").Value = "";
            folha.Cell("D67").Value = "";
            folha.Cell("R66").Value = "";
            folha.Cell("AK66").Value = "";
            folha.Cell("BC66").Value = "";
            folha.Cell("D72").Value = "";
            folha.Cell("R71").Value = "";
            folha.Cell("AK71").Value = "";
            folha.Cell("BC71").Value = "";
            folha.Cell("D77").Value = "";
            folha.Cell("R76").Value = "";
            folha.Cell("AK76").Value = "";
            folha.Cell("BC76").Value = "";
            folha.Cell("D82").Value = "";
            folha.Cell("R81").Value = "";
            folha.Cell("AK81").Value = "";
            folha.Cell("BC81").Value = "";
            folha.Cell("D92").Value = "";
            folha.Cell("R92").Value = "";
            folha.Cell("AK92").Value = "";
            folha.Cell("BC92").Value = "";
            folha.Cell("D97").Value = "";
            folha.Cell("R96").Value = "";
            folha.Cell("AK96").Value = "";
            folha.Cell("BC96").Value = "";
            folha.Cell("D102").Value = "";
            folha.Cell("R101").Value = "";
            folha.Cell("AK101").Value = "";
            folha.Cell("BC101").Value = "";
            folha.Cell("D107").Value = "";
            folha.Cell("Q106").Value = "";
            folha.Cell("AK106").Value = "";
            folha.Cell("BC106").Value = "";
            folha.Cell("D112").Value = "";
            folha.Cell("R111").Value = "";
            folha.Cell("AK111").Value = "";
            folha.Cell("BC111").Value = "";
            folha.Cell("D123").Value = "";
            folha.Cell("R122").Value = "";
            folha.Cell("AK122").Value = "";
            folha.Cell("BC122").Value = "";
            folha.Cell("D128").Value = "";
            folha.Cell("R127").Value = "";
            folha.Cell("AK127").Value = "";
            folha.Cell("BC127").Value = "";
            folha.Cell("D133").Value = "";
            folha.Cell("R132").Value = "";
            folha.Cell("AK132").Value = "";
            folha.Cell("BC132").Value = "";
            folha.Cell("D138").Value = "";
            folha.Cell("R137").Value = "";
            folha.Cell("AK137").Value = "";
            folha.Cell("BC137").Value = "";
            folha.Cell("D143").Value = "";
            folha.Cell("R142").Value = "";
            folha.Cell("AK142").Value = "";
            folha.Cell("BC142").Value = "";
            folha.Cell("C149").Value = "";
            folha.Cell("R147").Value = "";
            folha.Cell("AK147").Value = "";
            folha.Cell("BC147").Value = "";

        }

        private static void LimparTerceiraFolha(IXLWorksheet folha)
        {
            folha.Cell("BG4").Value = "";
            folha.Cell("BZ4").Value = "";
            folha.Cell("CF4").Value = "";
            folha.Cell("D13").Value = "";
            folha.Cell("R12").Value = "";
            folha.Cell("AK12").Value = "";
            folha.Cell("BC12").Value = "";
            folha.Cell("D18").Value = "";
            folha.Cell("R17").Value = "";
            folha.Cell("AK17").Value = "";
            folha.Cell("BC17").Value = "";
            folha.Cell("D23").Value = "";
            folha.Cell("R22").Value = "";
            folha.Cell("AK22").Value = "";
            folha.Cell("BC22").Value = "";
            folha.Cell("D28").Value = "";
            folha.Cell("R27").Value = "";
            folha.Cell("AK27").Value = "";
            folha.Cell("BC27").Value = "";
            folha.Cell("D33").Value = "";
            folha.Cell("R32").Value = "";
            folha.Cell("AK32").Value = "";
            folha.Cell("BC32").Value = "";
            folha.Cell("D38").Value = "";
            folha.Cell("R37").Value = "";
            folha.Cell("AK37").Value = "";
            folha.Cell("BC37").Value = "";
            folha.Cell("D50").Value = "";
            folha.Cell("R48").Value = "";
            folha.Cell("AK48").Value = "";
            folha.Cell("BC48").Value = "";
            folha.Cell("D57").Value = "";
            folha.Cell("R55").Value = "";
            folha.Cell("AK55").Value = "";
            folha.Cell("BC55").Value = "";
            folha.Cell("D64").Value = "";
            folha.Cell("R62").Value = "";
            folha.Cell("AK62").Value = "";
            folha.Cell("BC62").Value = "";
            folha.Cell("D71").Value = "";
            folha.Cell("R69").Value = "";
            folha.Cell("AK69").Value = "";
            folha.Cell("BC69").Value = "";
            folha.Cell("D78").Value = "";
            folha.Cell("R76").Value = "";
            folha.Cell("AK76").Value = "";
            folha.Cell("BC76").Value = "";
            folha.Cell("D85").Value = "";
            folha.Cell("R83").Value = "";
            folha.Cell("AK83").Value = "";
            folha.Cell("BC83").Value = "";
            folha.Cell("D92").Value = "";
            folha.Cell("R90").Value = "";
            folha.Cell("AK90").Value = "";
            folha.Cell("BC90").Value = "";
            folha.Cell("D99").Value = "";
            folha.Cell("R97").Value = "";
            folha.Cell("AK97").Value = "";
            folha.Cell("BC97").Value = "";
            folha.Cell("C108").Value = "";
        }

    }
}
