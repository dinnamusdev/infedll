using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Runtime.InteropServices; 

namespace iNFeDll.RegraDeNegocios
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Funcoes")]
    [ComVisible(true)]
    public static class Funcoes
    {
        public static string removeFormatacao(string texto)
        {
            string txt = "";

            txt = texto.Replace(".", "");
            txt = txt.Replace("-", "");
            txt = txt.Replace("/", "");
            txt = txt.Replace("(", "");
            txt = txt.Replace(")", "");
            txt = txt.Replace(" ", "");
            txt = txt.Replace(",", "");
            return txt;
        }

        public static void retornaAtributos(object[] obj, out CultureInfo cultura, out string formato, out bool obrigatorio)
        {
            cultura = CultureInfo.CreateSpecificCulture("en-US");
            formato = "###0.00";
            obrigatorio = false;
            foreach (object objeto in obj)
            {
                if (objeto is Formato)
                {
                    string culturaStr = ((Formato)objeto).cultura;
                    formato = ((Formato)objeto).formato;
                    cultura = CultureInfo.CreateSpecificCulture(culturaStr);
                }
                else
                    if (objeto is Obrigatorio)
                        obrigatorio = ((Obrigatorio)objeto).propriedadeObrigatoria;

            }

            //cultura.NumberFormat.NumberDecimalSeparator = ",";
            //cultura.NumberFormat.NumberGroupSeparator = ".";
        }

        public static int modulo11(string chaveNFE)
        {
            if (chaveNFE.Length != 43)
            {
                System.Windows.Forms.MessageBox.Show("Chave inválida, não é possível calcular o digito verificador");
                return 0;
            }


            string baseCalculo = "4329876543298765432987654329876543298765432";
            int somaResultados = 0;

            for (int i = 0; i <= chaveNFE.Length - 1; i++)
            {
                int numNF = Convert.ToInt32(chaveNFE[i].ToString());
                int numBaseCalculo = Convert.ToInt32(baseCalculo[i].ToString());

                somaResultados += (numBaseCalculo * numNF);
            }

            int restoDivisao = (somaResultados % 11);
            int dv = 11 - restoDivisao;
            if ((dv == 0) || (dv > 9))
                return 0;
            else
                return dv;
        }

        public static string TirarAcento(string palavra)
        {
            string palavraSemAcento = "";
            string caracterComAcento = "áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ";
            string caracterSemAcento = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC";

            for (int i = 0; i < palavra.Length; i++)
            {
                if (caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1))) >= 0)
                {
                    int car = caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1)));
                    palavraSemAcento += caracterSemAcento.Substring(car, 1);
                }
                else
                {
                    palavraSemAcento += palavra.Substring(i, 1);
                }
            }

            return palavraSemAcento;
        }


        public static bool novaTag(PropertyInfo propriedade)
        {

            switch (propriedade.PropertyType.ToString())
            {
                case "System.DateTime":
                case "System.Int32":
                case "System.String":
                case "System.Double":
                case "System.Nullable":
                case "System.Decimal":

                case "System.Nullable`1[System.Int32]":
                case "System.Nullable`1[System.DateTime]":
                case "System.Nullable`1[System.Decimal]":
                case "System.Nullable`1[System.Double]":
                    return false;
                default: return true;
            }
        }

        public static void gravarElemento(XmlWriter writer, string nomeTag, object valorTag, object[] atributos)
        {

            CultureInfo cult;
            string formato;
            bool obrigatorio;
            retornaAtributos(atributos, out cult, out formato, out obrigatorio);

            if (valorTag != null)
            {
                string valor = "";
                switch (valorTag.GetType().ToString())
                {
                    case "System.DateTime":
                        valor = ((DateTime)(valorTag)).ToString("yyyy-MM-dd");   //formata no padrão necessário para a NFe                     
                        break;
                    case "System.Int32":
                        valor = valorTag.ToString();
                        if (valor.Trim() == "0") //campos do tipo inteiro com valor 0 serão ignorados
                            valor = "";
                        break;
                    case "System.String":
                        valor = TirarAcento(valorTag.ToString().Replace("\r\n", " - ")).Trim(); //remove linhas... e tira acentos
                        break;
                    case "System.Double":
                        valor = ((double)valorTag).ToString(formato, cult.NumberFormat); //formata de acordo com o atributo
                        break;
                    case "System.Decimal":
                        valor = ((decimal)valorTag).ToString(formato, cult.NumberFormat); //formata de acordo com o atributo
                        break;

                }
                if ((valor.Trim().Length > 0) || (obrigatorio))
                    writer.WriteElementString(nomeTag, valor);
            }
        }

        public static long tamanhoXML(XmlDocument documento)
        {
            string nomeArquivo = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

            try
            {
                documento.Save(nomeArquivo);
                FileInfo info = new FileInfo(nomeArquivo);
                long tamanhoArquivo = info.Length;

                info.Delete();

                return tamanhoArquivo;
            }
            catch
            {
                return 0;
            }

        }
    }
}
