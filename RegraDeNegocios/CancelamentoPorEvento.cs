using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iNFeDll.RegraDeNegocios
{
    class CancelamentoPorEvento
    {

           public static string CancelarNotaFiscalEvento(int NFe_ID, string danfe, string nProt, string tpAmb, string CNPJ)
{
iNFe.NFe nfeCancelar = new iNFe.NFe();

nfeCancelar.versao = "1.00″;
nfeCancelar.Id = danfe;
nfeCancelar.nProt = nProt;
nfeCancelar.xJust = "Cliente Desistiu da Compra";
nfeCancelar.tpAmb = tpAmb;

XmlDocument xmlGerado = nfeCancelar.GerarXMLCancelamentoEvento(nfeCancelar.Id, nfeCancelar.nProt, nfeCancelar.tpAmb, nfeCancelar.xJust, nfeCancelar.versao);

string NomeArquivoXML = nfeCancelar.Id + "-env-canc.xml";
string endereco = DPFuncoes.Arquivo.Unidade_Disco_UniNFe() + @"Unimake\UniNFe\\" + CNPJ + @"\\Envio\\" + NomeArquivoXML;
String enderecoTemporario = @"c:\\temp\\" + VFPToolkit.files.JustFName(endereco);
xmlGerado.Save(enderecoTemporario);
if (File.Exists(enderecoTemporario))
{
DPFuncoes.Arquivo.CopiarArquivo(enderecoTemporario, endereco);
}

System.Threading.Thread.Sleep(3000);

string caminho = DPFuncoes.Arquivo.Unidade_Disco_UniNFe() + @"Unimake\UniNFe\\" + CNPJ + @"\\Retorno\\" + NomeArquivoXML;

string Resultado = iNFe.NFe.RespostaCancelamentoEvento(caminho, NFe_ID);

return Resultado;

}

public XmlDocument GerarXMLCancelamentoEvento(string danfe, string nProt, string tpAmb, string justificativa, string versao)
{
string clUF = DPFuncoes.Web.Sessao.ObterSessao("EmpCodUF", "");
string clCNPJ = DPFuncoes.Web.CampoForm.soNum(DPFuncoes.Web.Sessao.ObterSessao("EmpCNPJ", ""));
string clDataEvento = string.Format("{0:yyyy-MM-dd}T{1}-03:00″, System.DateTime.Today, VFPToolkit.dates.Time());

XmlWriterSettings configXML = new XmlWriterSettings();
configXML.Indent = true;
configXML.IndentChars = "";
configXML.NewLineOnAttributes = false;
configXML.OmitXmlDeclaration = false;

Stream xmlSaida = new MemoryStream();

XmlWriter oXmlGravar = XmlWriter.Create(xmlSaida, configXML);

oXmlGravar.WriteStartDocument();
oXmlGravar.WriteStartElement("envEvento", "http://www.portalfiscal.inf.br/nfe&#8221;);
oXmlGravar.WriteAttributeString("versao", versao);
oXmlGravar.WriteElementString("idLote", "101″);

oXmlGravar.WriteStartElement("evento");
oXmlGravar.WriteAttributeString("versao", "1.00″);
oXmlGravar.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe&#8221;);

oXmlGravar.WriteStartElement("infEvento");
oXmlGravar.WriteAttributeString("Id", "ID110111″ + danfe + "01″);

oXmlGravar.WriteElementString("cOrgao", clUF);
oXmlGravar.WriteElementString("tpAmb", tpAmb);
oXmlGravar.WriteElementString("CNPJ", clCNPJ);
oXmlGravar.WriteElementString("chNFe", danfe);
oXmlGravar.WriteElementString("dhEvento", clDataEvento);
oXmlGravar.WriteElementString("tpEvento", "110111″);
oXmlGravar.WriteElementString("nSeqEvento", "1″);
oXmlGravar.WriteElementString("verEvento", "1.00″);

oXmlGravar.WriteStartElement("detEvento" );
oXmlGravar.WriteAttributeString("versao", "1.00″);
oXmlGravar.WriteElementString("descEvento", "Cancelamento");
oXmlGravar.WriteElementString("nProt", nProt);
oXmlGravar.WriteElementString("xJust", justificativa);

oXmlGravar.WriteEndElement(); //Fecha elemento detEvento

oXmlGravar.WriteEndElement(); //Fecha elemento infEvento

oXmlGravar.WriteEndElement(); //Fecha elemento cancNFe
oXmlGravar.WriteEndElement(); //Fecha elemento envEvento

oXmlGravar.Flush();
xmlSaida.Flush();
xmlSaida.Position = 0;

XmlDocument documento = new XmlDocument();
documento.Load(xmlSaida);
oXmlGravar.Close();

return documento;
}

public static string RespostaCancelamentoEvento(string caminho, int NFe_ID)
{

string caminhoRetono = caminho.Replace("-env-canc.xml", "-ret-env-canc.xml");
string Resultado = Funcoes.EsperarPeloArquivo(caminhoRetono);

if (Resultado != "OK") return Resultado;
Resultado = caminho.Replace("-env-canc.xml", "-ret-env-canc.xml");

XDocument nCancel = XDocument.Load(caminhoRetono);
XNamespace namspace = "http://www.portalfiscal.inf.br/nfe&#8221;;

if (nCancel.Root != null)
{
var xretEnvEvento = nCancel.Element(namspace + "retEnvEvento");
if (xretEnvEvento != null)
{
var xcStat = xretEnvEvento.Element(namspace + "cStat");
if (xcStat != null)
{
string nCancelamento = xcStat.Value.ToString();
if (nCancelamento == "128″)
{
string cStatEvento = nCancel.Root.Element(namspace + "retEvento").Element(namspace + "infEvento").Element(namspace + "cStat").Value.ToString();
string nMotivo = nCancel.Root.Element(namspace + "retEvento").Element(namspace + "infEvento").Element(namspace + "xMotivo").Value.ToString();

if (!string.IsNullOrEmpty(cStatEvento)) Resultado = cStatEvento;
if (!string.IsNullOrEmpty(nMotivo)) Resultado += " / " + nMotivo;
DPFuncoes.Arquivo.ApagarAquivo(caminho.Replace("-env-canc.xml", "-ret-env-canc.xml"));

if (!string.IsNullOrEmpty(cStatEvento) && cStatEvento == "135″)
{
//AD.NFe.NotaFiscalEletronica_FinalAD.AtualizarCamposNFe(NFe_ID, null, null, null);
AD.NFe.NotaFiscalEletronica_FinalAD.CancelarNFE(NFe_ID);
return "Cancelamento Realizado com Sucesso!";
}
else
{
return "RN.CE.003 – Erro de Cancelamento por Evento = " + cStatEvento + " – " + nMotivo;
}
}
else
{
Resultado = nCancelamento;
}
}
else
{
return "RN.CE.002 – Erro de Cancelamento por Evento (cStat)";
}
}
else
{
return "RN.CE.001 – Erro de Cancelamento por Evento (retEnvEvento)";
}
}
else
{
return "RN.CE.000 – Erro de Cancelamento por Evento (root)";
}

return Resultado;
}
    }
}
