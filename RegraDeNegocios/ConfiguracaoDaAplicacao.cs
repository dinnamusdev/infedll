using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Runtime.InteropServices;
using iNFeDll.RegraDeNegocios.Enums;
using iNFeDll.Integra_ERP;
namespace iNFeDll.RegraDeNegocios
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("ConfiguracaoDaAplicacao")]
    [ComVisible(true)]
    public class ConfiguracaoDaAplicacao
    {
       

        /// <summary>
        /// Classe responsável por realizar algumas tarefas na parte de configurações da aplicação.
        /// Arquivo de configurações: IntegraConf.xml
        /// </summary>
        [ClassInterface(ClassInterfaceType.AutoDual)]
        [ProgId("ConfiguracaoApp")]
        [ComVisible(true)]
        public class ConfiguracaoApp
        {
            #region Propriedades

            #region Propriedades diversas
            /// <summary>
            /// Namespace URI associado (Endereço http dos schemas de XML)
            /// </summary>
            public static string nsURI { get; set; }
            public static Enums.TipoAplicativo TipoAplicativo { get; set; }
            #endregion

            #region Propriedades das versões dos XML´s da NFe
            public static string VersaoXMLStatusServico { get; set; }
            public static string VersaoXMLNFe { get; set; }
            public static string VersaoXMLPedRec { get; set; }
            public static string VersaoXMLCanc { get; set; }
            public static string VersaoXMLInut { get; set; }
            public static string VersaoXMLPedSit { get; set; }
            public static string VersaoXMLConsCad { get; set; }
            public static string VersaoXMLCabecMsg { get; set; }
            public static string VersaoXMLEnvDPEC { get; set; }
            public static string VersaoXMLConsDPEC { get; set; }
            #endregion

            #region Propriedades para controle de servidor proxy
            public static bool Proxy { get; set; }
            public static string ProxyServidor { get; set; }
            public static string ProxyUsuario { get; set; }
            public static string ProxySenha { get; set; }
            public static int ProxyPorta { get; set; }
            #endregion

            #endregion

            /// <summary>
            /// Metodo resposável pela impressao do DANFE.
            /// </summary>
            
            public void ImpressaoDANFE()
            {
                try
                {
                    System.Diagnostics.Process.Start(Integra_ERP.VariavesEstaticas.DiretorioSistema + @"\iDanfe.exe", "\"" + Integra_ERP.VariavesEstaticas.UltimaNFe + "\" " + "\"" + Integra_ERP.VariavesEstaticas.Protocolo.ToString() + "\"");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erro na impressão do DANFE!", "INTEGRA Solutions");
                }
            }

            public string Dados(int estado, int HP = 2, int tipoEmissao_ = 1, Servicos servico = Servicos.GerarChaveNFe)
            {
                return DefURLWS(estado, HP, tipoEmissao_, (Servicos)servico);
            }
            public void setDadosDaAplicacao(string strDiretorioSistema,string UF="SE",int cUF=28,int Ambiente=2)
            {
                
                
                VariavesEstaticas.DiretorioSistema = strDiretorioSistema;
                VariavesEstaticas.cUF = cUF;
                VariavesEstaticas.Ambiente = Ambiente;
                VariavesEstaticas.UF = UF;
                VariavesEstaticas.Url_Recepcao      = DefURLWS(VariavesEstaticas.cUF, VariavesEstaticas.Ambiente, 1, Servicos.EnviarLoteNfe);
                VariavesEstaticas.Url_RetRecepcao   = DefURLWS(VariavesEstaticas.cUF, VariavesEstaticas.Ambiente,1, Servicos.PedidoSituacaoLoteNFe);
                VariavesEstaticas.Url_Consulta = DefURLWS(VariavesEstaticas.cUF, VariavesEstaticas.Ambiente, 1, Servicos.PedidoConsultaSituacaoNFe);
                VariavesEstaticas.Url_Cancelamento = DefURLWS(VariavesEstaticas.cUF, VariavesEstaticas.Ambiente, 1, Servicos.CancelarNFe);


            }

            #region Métodos gerais

            #region CarregarDados()
            /// <summary>
            /// Carrega as configurações realizadas na Aplicação gravadas no XML IntegraConf.xml para
            /// propriedades, para facilitar a leitura das informações necessárias para as transações da NF-e.
            /// </summary>
            /// <remarks>
            /// Autor: Rafael Almeida
            /// </remarks>
            public static void CarregarDados()
            {
                string vArquivoConfig = InfoApp.PastaExecutavel() + "\\" + InfoApp.NomeArqConfig;

                if (File.Exists(vArquivoConfig))
                {
                    XmlTextReader oLerXml = null;
                    try
                    {
                        //Carregar os dados do arquivo XML de configurações da Aplicação
                        oLerXml = new XmlTextReader(vArquivoConfig);

                        while (oLerXml.Read())
                        {
                            if (oLerXml.NodeType == XmlNodeType.Element)
                            {
                                if (oLerXml.Name == "nfe_configuracoes")
                                {
                                    while (oLerXml.Read())
                                    {
                                        if (oLerXml.NodeType == XmlNodeType.Element)
                                        {
                                            if (oLerXml.Name == "Proxy") { oLerXml.Read(); ConfiguracaoApp.Proxy = Convert.ToBoolean(oLerXml.Value); }
                                            else if (oLerXml.Name == "ProxyServidor") { oLerXml.Read(); ConfiguracaoApp.ProxyServidor = oLerXml.Value.Trim(); }
                                            else if (oLerXml.Name == "ProxyUsuario") { oLerXml.Read(); ConfiguracaoApp.ProxyUsuario = oLerXml.Value.Trim(); }
                                            else if (oLerXml.Name == "ProxySenha") { oLerXml.Read(); ConfiguracaoApp.ProxySenha = oLerXml.Value.Trim(); }
                                            else if (oLerXml.Name == "ProxyPorta") { oLerXml.Read(); ConfiguracaoApp.ProxyPorta = Convert.ToInt32(oLerXml.Value.Trim()); }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (oLerXml != null)
                            oLerXml.Close();
                    }

                }
            }
            #endregion


            private static List<webServices> webservicesList = null;


          
            public static string DefURLWS(int CodigoUF, int tipoAmbiente, int tipoEmissao, Servicos servico)
            {
                string ArqXML = InfoApp.PastaExecutavel() + "\\iNFeWebService.xml";
                string URL = string.Empty;
                switch (tipoEmissao)
                {
                        
                    case TipoEmissao.teSCAN:
                        CodigoUF = 900;
                        break;

                    case TipoEmissao.teDPEC:
                        if (servico == Servicos.ConsultarDPEC || servico == Servicos.EnviarDPEC)
                            CodigoUF = 901;
                        break;

                    default:
                        break;
                }
                string ufNome = CodigoUF.ToString();  


                #region --carrega os WebServices na lista de webServices

                if (webservicesList == null)
                {
                    webservicesList = new List<webServices>();
                    if (File.Exists(ArqXML))
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(ArqXML);
                            XmlNodeList estadoList = doc.GetElementsByTagName("Estado");
                            foreach (XmlNode estadoNode in estadoList)
                            {
                                XmlElement estadoElemento = (XmlElement)estadoNode;
                                if (estadoElemento.Attributes.Count > 0)
                                {
                                    if (estadoElemento.Attributes[0].Value != "XX")
                                    {
                                        int ID = Convert.ToInt32(estadoElemento.Attributes[0].Value);
                                        string Nome = estadoElemento.Attributes[1].Value;
                                        string UF = estadoElemento.Attributes[2].Value;

                                        webServices wsItem = new webServices(ID, Nome, UF);

                                        XmlNodeList urlList = estadoElemento.GetElementsByTagName("URLHomologacao");
                                        for (int i = 0; i < urlList.Count; ++i)
                                        {
                                            for (int j = 0; j < urlList[i].ChildNodes.Count; ++j)
                                            {
                                                switch (urlList[i].ChildNodes[j].Name)
                                                {
                                                    case "NFeCancelamento":

                                                        wsItem.URLHomologacao.NFeCancelamento = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeConsulta":
                                                        wsItem.URLHomologacao.NFeConsulta = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeConsultaCadastro":
                                                        wsItem.URLHomologacao.NFeConsultaCadastro = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeInutilizacao":
                                                        wsItem.URLHomologacao.NFeInutilizacao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeRecepcao":
                                                        wsItem.URLHomologacao.NFeRecepcao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeRetRecepcao":
                                                        wsItem.URLHomologacao.NFeRetRecepcao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeStatusServico":
                                                        wsItem.URLHomologacao.NFeStatusServico = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                }
                                            }
                                        }
                                        urlList = estadoElemento.GetElementsByTagName("URLProducao");
                                        for (int i = 0; i < urlList.Count; ++i)
                                        {
                                            for (int j = 0; j < urlList[i].ChildNodes.Count; ++j)
                                            {
                                                switch (urlList[i].ChildNodes[j].Name)
                                                {
                                                    case "NFeCancelamento":
                                                        wsItem.URLProducao.NFeCancelamento = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeConsulta":
                                                        wsItem.URLProducao.NFeConsulta = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeConsultaCadastro":
                                                        wsItem.URLProducao.NFeConsultaCadastro = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeInutilizacao":
                                                        wsItem.URLProducao.NFeInutilizacao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeRecepcao":
                                                        wsItem.URLProducao.NFeRecepcao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeRetRecepcao":
                                                        wsItem.URLProducao.NFeRetRecepcao = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                    case "NFeStatusServico":
                                                        wsItem.URLProducao.NFeStatusServico = urlList[i].ChildNodes[j].InnerText;
                                                        break;
                                                }
                                            }
                                        }
                                        webservicesList.Add(wsItem);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                }

                #endregion


                #region --varre a lista de webservices baseado no codigo da UF

                foreach (webServices list in webservicesList)
                {
                    
                    if (list.ID == CodigoUF)
                    {
                        
                        switch (servico)
                        {
                            case Servicos.CancelarNFe:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeCancelamento : list.URLProducao.NFeCancelamento);
                                break;

                            case Servicos.ConsultaCadastroContribuinte:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeConsultaCadastro : list.URLProducao.NFeConsultaCadastro);
                                break;

                            case Servicos.EnviarLoteNfe:
                            case Servicos.EnviarDPEC:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeRecepcao : list.URLProducao.NFeRecepcao);
                                break;

                            case Servicos.InutilizarNumerosNFe:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeInutilizacao : list.URLProducao.NFeInutilizacao);
                                break;

                            case Servicos.PedidoConsultaSituacaoNFe:
                            case Servicos.ConsultarDPEC:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeConsulta : list.URLProducao.NFeConsulta);
                                break;

                            case Servicos.PedidoConsultaStatusServicoNFe:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeStatusServico : list.URLProducao.NFeStatusServico);
                                break;

                            case Servicos.PedidoSituacaoLoteNFe:
                                URL = (tipoAmbiente == TipoAmbiente.taHomologacao ? list.URLHomologacao.NFeRetRecepcao : list.URLProducao.NFeRetRecepcao);
                                break;
                        }
                        if (URL != string.Empty)
                        {
                            if (tipoEmissao == TipoEmissao.teDPEC) 
                                ufNome = "DPEC";
                            else
                                ufNome = "de " + list.Nome;  

                            break;
                        }
                    }
                }

                #endregion

                if (URL == string.Empty)
                {
                    string Ambiente = string.Empty;
                    switch (tipoAmbiente)
                    {
                        case TipoAmbiente.taProducao:
                            Ambiente = "produção";
                            break;

                        case TipoAmbiente.taHomologacao:
                            Ambiente = "homologação";
                            break;

                        default:
                            break;
                    }

                    throw new Exception("O Estado " + ufNome/*CodigoUF.ToString()*/ + " ainda não dispõe deste serviço no layout 4.0.1 da NF-e para o ambiente de " + Ambiente + ".");
                }

                return URL;
            }
            #endregion

           

           

            
        }



        internal class folderCompare
        {
            public Int32 id { get; set; }
            public string folder { get; set; }

            public folderCompare(Int32 _id, string _folder)
            {
                this.id = _id;
                this.folder = _folder;
            }
        }
    }
}
