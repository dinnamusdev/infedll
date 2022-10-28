using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace iNFeDll.RegraDeNegocios
{
    public class InfoApp
    {
        #region Constantes
        /// <summary>
        /// Nome para a pasta dos XML assinados
        /// </summary>
        public const string NomePastaXMLAssinado = "\\Assinado";
        public const string NomeArqERRUniNFe = "iNFeNFeErro_{0}.err";
        /// <summary>
        /// Nome do arquivo XML de configurações
        /// </summary>
        public const string NomeArqConfig = "iNFeConfig.xml";
        /// <summary>
        /// Nome do arquivo XML que é gravado as empresas cadastradas
        /// </summary>
        public static readonly string NomeArqEmpresa = PastaExecutavel() + "\\iNFeEmpresa.xml";
        /// <summary>     
        /// Nome do arquivo para controle da numeração sequencial do lote.
        /// </summary>
        public const string NomeArqXmlLote = "iNFeLote.xml";
        /// <summary>
        /// Nome do arquivo 1 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp1 = "Bkp1_iNFeLote.xml";
        /// <summary>
        /// Nome do arquivo 2 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp2 = "Bkp2_iNFeLote.xml";
        /// <summary>
        /// Nome do arquivo 3 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp3 = "Bkp3_iNFeLote.xml";
        /// <summary>
        /// Nome do arquivo que grava as notas fiscais em fluxo de envio
        /// </summary>
        public const string NomeArqXmlFluxoNfe = "iNFeFluxo.xml";
        #endregion

        #region Propriedades Estaticas
        /// <summary>
        /// Sempre na execução do aplicativo (EXE) já defina este campo estático ou a classe não conseguirá pegar
        /// as informações do executável, tais como: Versão, Nome da aplicação, etc.
        /// Defina sempre com o conteúdo: Assembly.GetExecutingAssembly
        /// </summary>
        public static Assembly oAssemblyEXE;
        #endregion

        /// <summary>
        /// Retorna a versão do aplicativo 
        /// </summary>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <returns>string contendo a versão do Aplicativo</returns>
        /// <by>Rafael Almeida</by>
        public static string Versao()
        {
            //Montar a versão do programa
            string versao;

            foreach (Attribute attr in Attribute.GetCustomAttributes(oAssemblyEXE))
            {
                if (attr.GetType() == typeof(AssemblyVersionAttribute))
                {
                    versao = ((AssemblyVersionAttribute)attr).Version;
                    break;
                }
            }
            string delimStr = ",=";
            char[] delimiter = delimStr.ToCharArray();
            string[] strAssembly = oAssemblyEXE.ToString().Split(delimiter);
            versao = strAssembly[2];

            return versao;
        }

        /// <summary>
        /// Retorna o nome do aplicativo 
        /// </summary>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <returns>string contendo o nome do Aplicativo</returns>
        /// <by>Rafael Almeida</by>
        public static string NomeAplicacao()
        {
            //Montar o nome da aplicação
            string Produto = string.Empty;

            foreach (Attribute attr in Attribute.GetCustomAttributes(oAssemblyEXE))
            {
                if (attr.GetType() == typeof(AssemblyProductAttribute))
                {
                    Produto = ((AssemblyProductAttribute)attr).Product;
                    break;
                }
            }

            return Produto;
        }

        /// <summary>
        /// Retorna a pasta do executável
        /// </summary>
        /// <returns>Retorna a pasta onde está o executável</returns>
        public static string PastaExecutavel()
        {
            return Integra_ERP.VariavesEstaticas.DiretorioSistema;
        }

        /// <summary>
        /// Retorna a pasta dos schemas para validar os XML´s
        /// </summary>
        /// <returns></returns>
        public static string PastaSchemas()
        {
            return PastaExecutavel() + "\\schemas";
        }

        /// <summary>
        /// Retorna o XML para salvar os parametros das telas
        /// </summary>
        public static string NomeArqXMLParams()
        {
            return PastaExecutavel() + "\\iNFeParamentros.xml";
        }

      

    }
}
