using System;
using System.Security.Policy;

namespace LaborSafety.Utils.Constantes
{
    public class Constantes
    {
        public static readonly int PERFIL_CONTROLADOR = 4;

        public static readonly int LINHA_INICIAL_PLANILHA_BLOQUEIO_NEGOCIO = 9;
        public static readonly int LIMITE_ITENS_POR_FOLHA_COMPLEMENTAR = 8;
        public static readonly float LIMITE_CARACTERES_POR_LINHA = 34.0f;
        public static readonly float LIMITE_CARACTERES_POR_LINHA_EPI = 26.0f;
        public static readonly float LIMITE_CARACTERES_PROCEDIMENTOS_APLICAVEIS = 48.0f;    
        public static readonly int TAMANHO_FONTE_FOLHA_COMPLEMENTAR = 20;
        public static readonly string PRIMEIRO_NIVEL_LOCAL_INSTALACAO = "CSA";
        public static readonly string NOME_FOLHA_COMPLEMENTAR_FRENTE = "Frente";
        public static readonly string NOME_FOLHA_COMPLEMENTAR_VERSO = "Verso";
        public static readonly int QUANTIDADE_FUNCIONARIOS_FOLHA_COMPLEMENTAR = 20;
        public static readonly int QUANTIDADE_FUNCIONARIOS_FOLHA_APR = 12;
        public static readonly int POSICAO_INICIAL_FUNCIONARIO_FOLHA_COMPLEMENTAR = 7;
        public static readonly int POSICAO_FINAL_FUNCIONARIO_FOLHA_COMPLEMENTAR = 26;
        public static readonly int ALTURA_CELULA_FOLHA_COMPLEMENTAR = 35;
        public static readonly string LOCAL_INSTALACAO_BASE = "000_BASE";


        public static class ClassesLocalInstalacao
        {
            public const string Peso = "Peso";
            public const string TagDeBloqueio = "Tag de bloqueio";
        }

        public static class DescricaoCaracteristicaClassesLocalInstalacao
        {
            public const string FaixaDePeso = "FAIXADEPESO";
            public const string TagDeBloqueio = "TAG_BLOQUEIO";
            public const string TagLocalBloqueio = "TAG_LOCAL_BLOQUEIO";
            public const string DispositivoBloqueio = "DISP_BLOQUEIO";
            public const string TipoDeEnergia = "ENERGIA";
            public const string Area = "AREA";
        }

        public static class StatusAPRIntegracaoOrdem

        {

            public const string Criado = "CRI";
            public const string Aberto = "ABER";
            public const string Editado = "EDIT";
            public const string Liberado = "LIB";
            public const string Imprimir = "IMPR";
            public const string Asap = "ASAP";
            public const string EnviarRecalcular = "ASAP CALC";
        }

        public enum Probabilidade
        {
            QUASE_IMPOSSIVEL = 1,
            POSSIVEL = 2,
            ALGUMA_CHANCE = 3,
            PROVAVEL = 4,
            MUITO_PROVAVEL = 5,
            CERTO_DE_OCORRENCIA = 6
        }

        public enum PesoProbabilidade
        {
            PESO_0 = 1,
            PESO_2 = 2,
            PESO_5 = 3,
            PESO_8 = 4,
            PESO_10 = 5,
            PESO_15 = 6
        }

        public enum Severidade
        {
            SEM_LESAO = 1,
            LACERACAO = 2,
            FRATURA_ENFERMIDADE_LEVE = 3,
            ENFERMIDADE_GRAVE_TEMPORARIA = 4,
            PERDA_DE_MEMBRO_ENFERMIDADE_PERMANENTE = 5,
            MORTE = 6
        }

        public enum DisciplinaAtividade
        {
            MECANICA = 1,
            ELETRICA = 2,
            INSTRUMENTACAO = 3,
            AUTOMACAO = 4
        }

        public enum Atividade
        {
            INSPECIONAR = 1,
            LUBRIFICAR = 2,
            TROCAR = 3,
            REPARAR = 4
        }

        public enum RiscoAmbiente
        {
            RUIDO = 1,
            VIBRACAO = 2,
            ALTAS_TEMPERATURAS = 3,
            BAIXAS_TEMPERATURAS = 4,
            PRESSAO = 5,
            RADIACAO_IONIZANTE = 6,
            RADIACAO_NAO_IONIZANTE = 7,
            UMIDADE = 8,
            POEIRAS = 9,
            FUMOS = 10,
            NEVOAS = 11,
            NEBLINA = 12,
            GASES = 13,
            VAPORES = 14,
            BACTERIAS = 15,
            FUNGOS = 16,
            PARASITAS = 17,
            BACILOS = 18,
            VIRUS = 19,
            LEVANTAMENTO_PESO = 20,
            RITMO_EXCESSIVO = 21,
            MONOTONIA = 22,
            REPETITIVIDADE = 23,
            POSICAO = 24,
            QUEDA = 25,
            CHOQUE_ELETRICO = 26,
            ILUMINACAO = 27,
            CHOQUE_MECANICO = 28,
            INCENDIO = 29,
            EXPLOSAO = 30,
            SOTERRAMENTO = 31
        }

        public enum RiscoAtividade
        {
            PRENSAMENTOS = 32,
            QUEDAS = 33,
            CORTES = 34,
            AMPUTACOES = 35,
            PERFURACOES = 36,
            QUEIMADURAS = 37,
            CHOQUE_ELETRICO = 38,
            OUTROS = 39
        }

        public enum TipoRisco
        {
            INV_ATV = 1,
            FISICO = 2,
            QUIMICO = 3,
            BIOLOGICO = 4,
            ERGONOMICO = 5,
            ACIDENTE = 6
        }

        public enum TempoAtividade
        {
            MENOR_1H = 1,
            ENTRE_1H_2H = 2,
            ENTRE_2H_5H = 3,
            ENTRE_5H_8H = 4,
            ENTRE_8H_12H = 5,
            ENTRE_12H_24H = 6,
            ENTRE_24H_48H = 7,
            MAIOR_IGUAL_48H = 8
        }

        public enum PesoFisico
        {
            SEM_PESO = 1,
            ATE_20KG = 2,
            DE_20KG_A_50KG = 3,
            DE_50KG_A_100KG = 4,
            DE_100KG_A_500KG = 5,
            ACIMA_DE_500KG = 6
        }

        public enum PesoSeveridade
        {
            PESO_0 = 1,
            PESO_0_5 = 2,
            PESO_2 = 3,
            PESO_8 = 4,
            PESO_12 = 5,
            PESO_15 = 6
        }

        public enum LocalInstalacao
        {
            SEM_ASSOCIACAO = 1,
        }

        public enum InventarioAmbiente
        {
            SEM_INVENTARIO = 1,
        }

        public enum PerfilCatalogo
        {
            SEM_PERFIL_CATALOGO = 1,
        }


        public enum Status
        {
            INATIVO = 0,
            ATIVO = 1
        }

        public enum TipoInventario
        {
            INVENTARIO_AMBIENTE = 1,
            INVENTARIO_ATIVIDADE = 2,
            INVENTARIO_BLOQUEIO = 3,
            APR = 4
        }

        public enum NR
        {
            NR0 = 1,
            NR1 = 2,
            NR2 = 3,
            NR3 = 4,
            NR4 = 5,
            NR5 = 6,
            NR6 = 7,
            NR7 = 8,
            NR8 = 9,
            NR9 = 10,
            NR10 = 11,
            NR11 = 12,
            NR12 = 13,
            NR13 = 14,
            NR14 = 15,
            NR15 = 16,
            NR16 = 17,
            NR17 = 18,
            NR18 = 19,
            NR19 = 20,
            NR20 = 21,
            NR21 = 22,
            NR22 = 23,
            NR23 = 24,
            NR24 = 25,
            NR25 = 26,
            NR26 = 27,
            NR28 = 28,
            NR29 = 29,
            NR30 = 30,
            NR31 = 31,
            NR32 = 32,
            NR33 = 33,
            NR34 = 34,
            NR35 = 35,
            NR36 = 36,
            NR37 = 37
        }

        public enum StatusIntegracao
        {
            I = 1,
            U = 2,
            E = 3
        }

        public enum StatusResponseIntegracao
        {
            S = 1,
            E = 2
        }
        public enum StatusAPR
        {
            Criado = 1,
            Aberto = 1,
            Editado = 2,
            Liberado = 4,
            Recalcular = 5
        }


        public enum StatusOrdem
        {
            CRI = 1,
            IMPR = 2,
            ASAP = 3,
            ASPD = 4,
            ASAP_CALC = 5
        }

        public enum QuebraDadosPlanilhaApr
        {
            MAX_FONTE_GERADORA = 36,
            MAX_EPI = 38,
            MAX_PA_CM = 86
        }

        public enum Area
        {
            SEM_AREA = 1
        }

        public enum DispositivoBloqueio
        {
            SEM_DISPOSITIVO_BLOQUEIO = 1
        }

        public enum TagKKsBloqueio
        {
            SEM_TAG_KKS_BLOQUEIO = 1
        }

        public enum TipoEnergiaBloqueio
        {
            SEM_TIPO_ENERGIA_BLOQUEIO = 1
        }
        public enum LocalABloquear
        {
            SEM_LOCAL_A_BLOQUEAR = 1
        }

        public static string GerarNumeroSerie(long codApr, bool origemTela)
        {
            string padraoNumSerie;
            if (origemTela)
                padraoNumSerie = "M-APR";
            else
                padraoNumSerie = "APR";
            string codAprString = codApr.ToString();
            if (string.IsNullOrEmpty(codAprString) || codApr == 0)
                throw new Exception("O valor da APR não pode ser nulo ou zero!");
            if (codAprString.Length > 10)
            {
                throw new Exception("A quantidade de 10 numéricos após a sigla 'APR' do número de série foi excedida!");
            }
            for (var i=1; i<= 10-codAprString.Length; i++)
            {
                padraoNumSerie += "0";
            }
            padraoNumSerie = $"{padraoNumSerie}{codAprString}";
            return padraoNumSerie;
        }


        public static double CalcularNumeroLinhasPorCaracteres(double alturaBase,int quantidadeCaracteres, double limiteCaracteresLinha)
        {
            if (quantidadeCaracteres > limiteCaracteresLinha)
            {
                var alturaCalculada = (quantidadeCaracteres / limiteCaracteresLinha) * alturaBase;
                return alturaCalculada;
            }
            else
            {
                return alturaBase;
            }

        }



        public enum TipoOperacaoLog
        {
            INSERCAO = 1,
            EDICAO = 2,
            DELECAO = 3
        }

        public enum TipoCodStatusApr
        {
            CRI = 1,
            IMPR = 2,
            ASAP = 3,
            LIB = 4
        }

        public enum ValorRisco
        {
            RISCO_BAIXO = 4,
            RISCO_MEDIO = 8,
            RISCO_ALTO = 16
        }

        public static class Perfil
        {
            public const string Administrador = "Administrador";
            public const string Master = "Master";
            public const string Cadastro = "Cadastro";
            public const string Executor = "Executor";
            public const string Consulta = "Consulta";
        }

        public enum Ambiente
        {
            SEM_AMBIENTE = 1,
        }

        public enum Sheets_Planilha_APR
        {
            SHEET2 = 2,
            SHEET3= 3
        }

    }
}