using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace iNFeDll.Integra_ERP
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("VariavesEstaticas")]
    [ComVisible(true)]
    public static class VariavesEstaticas
    {
        public static String XMLBaixados = "";
        public static X509Certificate2 certificado=null;
        public static string DiretorioSistema = string.Empty;
        public static string  UF="SE";
        public static int cUF=28;
        public static int Ambiente = 2;
        public static string VersaoNF = "2.00";
        public static string VersaoEventoCancelamento = "1.00";
        public static string VersaoConsultaDestinario = "1.01";
        public static string Url_Recepcao = string.Empty;
        public static string Url_RetRecepcao = string.Empty;
        public static string Url_Consulta = string.Empty;
        public static string Url_ConStatus = string.Empty;
        public static string Url_Inultulizacao = string.Empty;
        public static string Url_Cancelamento = string.Empty;
        public static string UltimaNFe = string.Empty;
        public static bool InterromperDownload = false;
        public static string Protocolo = string.Empty;
        public static string Xml_Download=
            "<downloadNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\">"+
  "<tpAmb>1</tpAmb>"+
  "<xServ>DOWNLOAD NFE</xServ>"+
  "<CNPJ>10142785000190</CNPJ>"+
  "<chNFe>43120479999975000104550000000884201000884208</chNFe>"+
"</downloadNFe>";

        public static string Xml_Confirmacao =
"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
"<envEvento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\">" +
  "<idLote>1</idLote>"+
  "<evento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\">"+
    "<infEvento Id=\"ID2102001513051492855400016855001000000116100101412703\">" +
      "<cOrgao>91</cOrgao>"+
      "<tpAmb>1</tpAmb>"+
      "<CNPJ>1</CNPJ>"+
      "<chNFe>1</chNFe>"+
      "<dhEvento>2013-06-06T14:28:00-03:00</dhEvento>"+
      "<tpEvento>210200</tpEvento>" +
      "<nSeqEvento>1</nSeqEvento>"+
      "<verEvento>1.00</verEvento>"+
      "<detEvento versao=\"1.00\">"+
        "<descEvento>Confirmacao da Operacao</descEvento>" +        
      "</detEvento>"+
    "</infEvento>"+    
  "</evento>"+
"</envEvento>";
    }
}
