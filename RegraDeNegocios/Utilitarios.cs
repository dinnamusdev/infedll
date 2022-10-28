using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Security;
using System.CodeDom;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.CodeDom.Compiler;
using System.IO;
using System.Globalization;
using Microsoft.CSharp;
using System.Reflection;
using iNFeDll.br.gov.fazenda.sefazvirtual.www;
using iNFeDll.br.gov.fazenda.nfe.www;
using iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310;
using iNFeDll.br.gov.fazenda.sefazvirtual.NfeRetAut310;
using iNFeDll.NfeAutorizacao4_svan;
using iNFeDll.NFeRecepcaoEvento4_svan;
/// <summary>
/// Classe Utilitarios - NF-e Integra Solutions
/// </summary>
/// <remarks>
/// Autor: Rafael Almeida - ralms@r7.com
/// Data: 21/03/2011
/// </remarks>
namespace iNFeDll.RegraDeNegocios
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Utilitarios")]
    [ComVisible(true)]
    public class Utilitarios
    {
        /// <summary>
        /// Delacrações
        /// </summary>
        /// <summary>
        /// Funcao Enviar Nf-e
        /// </summary>
        /// 
        private frmNotaEncontradas formNotas = new frmNotaEncontradas();
        private X509Certificate2 certificado = RegraDeNegocios.CertificadoDigital.SelecionarCertificado();

       
        
        public string EnviarNFe(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new HomologacaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPArecepcao.NfeRecepcao2 nfHPA = new HomologacaoPArecepcao.NfeRecepcao2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBArecepcao.NfeRecepcao2 nfHBA = new HomologacaoBArecepcao.NfeRecepcao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new ProducaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {

                        ProducaoPArecepcao.NfeRecepcao2 nfHPA = new ProducaoPArecepcao.NfeRecepcao2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new ProducaoPArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBANfeRecepcao.NfeRecepcao2 nfHBA = new ProducaoBANfeRecepcao.NfeRecepcao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBANfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefaz(rets, "nRec", "xMotivo");
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message.ToString()+"\n"+erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }
           
        }
        public string EnviarNFe400(XmlDocument xmlAssinado)
        {

            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new HomologacaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        NFeAutorizacao4 nfHPA400 = new NFeAutorizacao4();
                        nfHPA400.Timeout = 100000;
                        nfHPA400.Url = "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";//"https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NfeAutorizacao4.asmx";
                        //nfHPA400.Url = "https://www.sefazvirtual.fazenda.gov.br/NFeAutorizacao4/NFeAutorizacao4.asmx";// Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA400.ClientCertificates.Add(certificado);
                        //nfHPA400.
                        //iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg cabecalho = new iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg();
                        //cabecalho.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        //cabecalho.versaoDados = "3.10";
                        //nfHPA310.nfeCabecMsgValue = cabecalho;
                        //nfHPA400.nfeCabecMsgValue.versaoDados = "3.10";
                        rets = nfHPA400.nfeAutorizacaoLote(xmlAssinado.ChildNodes[1]);
                        //nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;es[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBArecepcao.NfeRecepcao2 nfHBA = new HomologacaoBArecepcao.NfeRecepcao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new ProducaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        NFeAutorizacao4 nfHPA400 = new NFeAutorizacao4();
                        nfHPA400.Timeout = 100000;
                        nfHPA400.Url ="https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NfeAutorizacao4.asmx";
                        //nfHPA400.Url = "https://www.sefazvirtual.fazenda.gov.br/NFeAutorizacao4/NFeAutorizacao4.asmx";// Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA400.ClientCertificates.Add(certificado);
                        //nfHPA400.
                        //iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg cabecalho = new iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg();
                        //cabecalho.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        //cabecalho.versaoDados = "3.10";
                        //nfHPA310.nfeCabecMsgValue = cabecalho;
                        //nfHPA400.nfeCabecMsgValue.versaoDados = "3.10";
                        rets = nfHPA400.nfeAutorizacaoLote(xmlAssinado.ChildNodes[1]);
                        //nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;es[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBANfeRecepcao.NfeRecepcao2 nfHBA = new ProducaoBANfeRecepcao.NfeRecepcao2();

                        nfHBA.Timeout = 100000;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBANfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefaz(rets, "cUF", "xMotivo");
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }
        public string EnviarNFe310(XmlDocument xmlAssinado)
        {
            
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new HomologacaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPArecepcao.NfeRecepcao2 nfHPA = new HomologacaoPArecepcao.NfeRecepcao2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBArecepcao.NfeRecepcao2 nfHBA = new HomologacaoBArecepcao.NfeRecepcao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBArecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSEnfeRecepcao.NfeRecepcao2 nfHSE = new ProducaoSEnfeRecepcao.NfeRecepcao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSEnfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        NfeAutorizacao nfHPA310 = new NfeAutorizacao();
                        nfHPA310.Timeout = 100000;
                        
                        nfHPA310.Url = "https://www.sefazvirtual.fazenda.gov.br/NfeAutorizacao/NfeAutorizacao.asmx?wsdl";// Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHPA310.ClientCertificates.Add(certificado);
                        
                        iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg cabecalho = new iNFeDll.br.gov.fazenda.sefazvirtual.NfeAut310.nfeCabecMsg();
                        cabecalho.cUF  = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        cabecalho.versaoDados = "3.10";
                        nfHPA310.nfeCabecMsgValue = cabecalho;
                        nfHPA310.nfeCabecMsgValue.versaoDados = "3.10";
                        rets = nfHPA310.nfeAutorizacaoLote(xmlAssinado.ChildNodes[1]);                        
                        //nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;es[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBANfeRecepcao.NfeRecepcao2 nfHBA = new ProducaoBANfeRecepcao.NfeRecepcao2(); 
                        
                        nfHBA.Timeout = 100000;
                        nfHBA.AllowAutoRedirect = true;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Recepcao;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBANfeRecepcao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeRecepcaoLote2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefaz(rets, "cUF", "xMotivo");
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }

        public string EnviarNFeCancelamento(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPAcancelamento.NfeCancelamento2 nfHPA = new HomologacaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new ProducaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefaz(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }

        public string EnviarNFeCancelamentoEvento400(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        NFeRecepcaoEvento4 ProducaoRecepcaoEventoAN = new NFeRecepcaoEvento4();
                        ProducaoRecepcaoEventoAN.ClientCertificates.Add(certificado);
                        ProducaoRecepcaoEventoAN.Url ="https://nfe-homologacao.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue = new iNFeDll.br.gov.fazenda.sefazvirtual.www.nfeCabecMsg();
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoEventoCancelamento.ToString();
                        rets = ProducaoRecepcaoEventoAN.nfeRecepcaoEvento(xmlAssinado);
                        //MessageBox.Show(ProducaoRecepcaoEventoAN.SoapVersion.ToString());
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        //ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();

                        NFeRecepcaoEvento4 ProducaoRecepcaoEventoAN = new NFeRecepcaoEvento4();
                        ProducaoRecepcaoEventoAN.ClientCertificates.Add(certificado);
                        ProducaoRecepcaoEventoAN.Url = "https://nfe.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue = new iNFeDll.br.gov.fazenda.sefazvirtual.www.nfeCabecMsg();
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        //ProducaoRecepcaoEventoAN.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoEventoCancelamento.ToString();
                        rets = ProducaoRecepcaoEventoAN.nfeRecepcaoEvento(xmlAssinado);
                        //MessageBox.Show(ProducaoRecepcaoEventoAN.SoapVersion.ToString());
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefazEvento(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                //MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }



        
        public string EnviarNFeCancelamentoEvento(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPAcancelamento.NfeCancelamento2 nfHPA = new HomologacaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        //ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();
                                                
                        RecepcaoEvento ProducaoRecepcaoEventoAN = new RecepcaoEvento();
                        ProducaoRecepcaoEventoAN.ClientCertificates.Add(certificado);
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue = new iNFeDll.br.gov.fazenda.sefazvirtual.www.nfeCabecMsg();                        
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoEventoCancelamento.ToString();                        
                        rets = ProducaoRecepcaoEventoAN.nfeRecepcaoEvento(xmlAssinado);
                        //MessageBox.Show(ProducaoRecepcaoEventoAN.SoapVersion.ToString());
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefazEvento(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                //MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }
        public string EnviarNFeDOwnload(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPAcancelamento.NfeCancelamento2 nfHPA = new HomologacaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        //ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();

                        br.gov.fazenda.nfe.www.download.NfeDownloadNF ProducaoRecepcaoEventoAN = new br.gov.fazenda.nfe.www.download.NfeDownloadNF();
                        ProducaoRecepcaoEventoAN.ClientCertificates.Add(certificado);
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue = new br.gov.fazenda.nfe.www.download.nfeCabecMsg();
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoEventoCancelamento.ToString();
                        rets = ProducaoRecepcaoEventoAN.nfeDownloadNF(xmlAssinado);
                        //MessageBox.Show(ProducaoRecepcaoEventoAN.SoapVersion.ToString());
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefazEventoDownload(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                //MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }
        public string EnviarNFeConfirmacaoRecebimento(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPAcancelamento.NfeCancelamento2 nfHPA = new HomologacaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        //ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();

                        br.gov.fazenda.nfe.recepcaoevento.an.RecepcaoEvento ProducaoRecepcaoEventoAN = new br.gov.fazenda.nfe.recepcaoevento.an.RecepcaoEvento();
                        ProducaoRecepcaoEventoAN.ClientCertificates.Add(certificado);
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue = new br.gov.fazenda.nfe.recepcaoevento.an.nfeCabecMsg();
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        ProducaoRecepcaoEventoAN.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoEventoCancelamento.ToString();
                        rets = ProducaoRecepcaoEventoAN.nfeRecepcaoEvento(xmlAssinado);
                        //MessageBox.Show(ProducaoRecepcaoEventoAN.SoapVersion.ToString());
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefazEventoConfirmacaoRecebimento(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                //MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }
        public String EnviarNFeConsultaDestinatario_Tratar_Retorno(XmlDocument doc)
        {
            String cRetorno = "";
            try
            {
                XmlNodeList resNFe = doc.GetElementsByTagName("resNFe");
                if (resNFe.Count > 0) {
                    foreach (XmlNode item in resNFe)
                    {

                        DocumentosEmitidos.Documentos().Tables[0].Rows.Add();
                        int nNovaLinha=DocumentosEmitidos.Documentos().Tables[0].Rows.Count;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["CNPJ"] = item["CNPJ"].ChildNodes[0].Value;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["xNome"] = item["xNome"].ChildNodes[0].Value;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["dEmi"] = item["dEmi"].ChildNodes[0].Value;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["vNF"] = item["vNF"].ChildNodes[0].Value;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["dhRecbto"] = item["dhRecbto"].ChildNodes[0].Value;
                        String cSituacao = item["cSitNFe"].ChildNodes[0].Value;
                        if (item["cSitNFe"].ChildNodes[0].Value == "1")
                        {
                            cSituacao = "Autorizada";
                        }
                        else if (item["cSitNFe"].ChildNodes[0].Value == "3")
                        {
                            cSituacao = "Cancelada";
                        }
                        else {
                            cSituacao = "Denegado";
                        }

                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["cSitNFe"] = cSituacao;//item["cSitNFe"].ChildNodes[0].Value;
                        DocumentosEmitidos.Documentos().Tables[0].Rows[nNovaLinha-1]["chNFe"] = item["chNFe"].ChildNodes[0].Value;
                        formNotas.AtualizarUI();
                        
                    }

                }

                //String ChaveDeAcesso = doc__.GetElementsByTagName("cStat")[0].InnerText.ToString();

                return cRetorno;
            }
            catch (Exception erro)
            {
                
                MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "";
            }
        }
        public string EnviarNFeConsultaDestinatario(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        HomologacaoPAcancelamento.NfeCancelamento2 nfHPA = new HomologacaoPAcancelamento.NfeCancelamento2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBACancelamento.NfeCancelamento2 nfHBA = new HomologacaoBACancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBACancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSECancelamento.NfeCancelamento2 nfHSE = new ProducaoSECancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSECancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        //ProducaoPAcancelamento.NfeCancelamento2 nfHPA = new ProducaoPAcancelamento.NfeCancelamento2();
                        NFeConsultaDest producaoConsultaDest = new NFeConsultaDest();
                        
                        //ecepcaoEvento ProducaoRecepcaoEventoAN = new RecepcaoEvento();
                        producaoConsultaDest.ClientCertificates.Add(certificado);
                        producaoConsultaDest.nfeCabecMsgValue = new br.gov.fazenda.nfe.www.nfeCabecMsg();
                        producaoConsultaDest.nfeCabecMsgValue.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                        producaoConsultaDest.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoConsultaDestinario.ToString();
                        String indCont = "";
                        String cStatus = "";
                        String ultNSU ="";
                        formNotas = new frmNotaEncontradas();
                        formNotas.Show();
                        bool bExecutar = true;
                        while (bExecutar || !DocumentosEmitidos.Interromper)
                        {

                            ultNSU = xmlAssinado.GetElementsByTagName("ultNSU")[0].ChildNodes[0].Value;
                            
                            Application.DoEvents();

                            formNotas.txtProcessamento.Text = "REALIZANDO CONSULTA LOTE " + ultNSU;

                            rets = producaoConsultaDest.nfeConsultaNFDest(xmlAssinado);
                            
                            XmlDocument doc__ = new XmlDocument();

                            doc__.LoadXml((rets as XmlElement).OuterXml);
                            
                            cStatus = doc__.GetElementsByTagName("cStat")[0].InnerText.ToString();
                            //MessageBox.Show(doc__.InnerText + "\n"+ cStatus);
                            if (cStatus.Trim()== "138") {
                                EnviarNFeConsultaDestinatario_Tratar_Retorno(doc__); 
                            }

                            indCont = doc__.GetElementsByTagName("indCont")[0].InnerText.ToString();

                            if (indCont == "1")
                            {

                                ultNSU = doc__.GetElementsByTagName("ultNSU")[0].ChildNodes[0].Value;

                                XmlNode x = xmlAssinado.GetElementsByTagName("ultNSU")[0].ChildNodes[0];

                                x.Value = ultNSU;


                            }
                            else {
                                bExecutar = false;
                                //break;
                            }


                        }
                        formNotas.Dispose();
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAcancelamento.NfeCancelamento2 nfHBA = new ProducaoBAcancelamento.NfeCancelamento2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAcancelamento.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeCancelamentoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefazEvento(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                //MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO " + erro.Message + "\n" + erro.StackTrace;
            }

        }

        public string EnviarNFeInutilizacao(XmlDocument xmlAssinado)
        {
            var rets = new object();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            try
            {
                //Homologacao
                if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        HomologacaoSEinutilizacao.NfeInutilizacao2 nfHSE = new HomologacaoSEinutilizacao.NfeInutilizacao2();
                        //HomologacaoSEcancelamento.NfeCancelamento2 nfHSE = new HomologacaoSEcancelamento.NfeCancelamento2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.AllowAutoRedirect = true;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new HomologacaoSEinutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {

                        HomologacaoPAinutilizacao.NfeInutilizacao2 nfHPA = new HomologacaoPAinutilizacao.NfeInutilizacao2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new HomologacaoPAinutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        HomologacaoBAInutilizacao.NfeInutilizacao2 nfHBA = new HomologacaoBAInutilizacao.NfeInutilizacao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new HomologacaoBAInutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                else
                {
                    #region NFe Sergipe
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                    {
                        ProducaoSEinutilizacao.NfeInutilizacao2 nfHSE = new ProducaoSEinutilizacao.NfeInutilizacao2();
                        nfHSE.Timeout = 100000;
                        nfHSE.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHSE.ClientCertificates.Add(certificado);
                        nfHSE.nfeCabecMsgValue = (new ProducaoSEinutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHSE.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHSE.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Pará
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                    {
                        ProducaoPAinutilizacao.NfeInutilizacao2 nfHPA = new ProducaoPAinutilizacao.NfeInutilizacao2();
                        nfHPA.Timeout = 100000;
                        nfHPA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHPA.ClientCertificates.Add(certificado);
                        nfHPA.nfeCabecMsgValue = (new ProducaoPAinutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHPA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHPA.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion

                    #region NFe Bahia
                    if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                    {
                        ProducaoBAinutilizacao.NfeInutilizacao2 nfHBA = new ProducaoBAinutilizacao.NfeInutilizacao2();
                        nfHBA.Timeout = 100000;
                        nfHBA.Url = Integra_ERP.VariavesEstaticas.Url_Cancelamento;
                        nfHBA.ClientCertificates.Add(certificado);
                        nfHBA.nfeCabecMsgValue = (new ProducaoBAinutilizacao.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        nfHBA.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        rets = nfHBA.nfeInutilizacaoNF2(xmlAssinado.ChildNodes[1]);
                    }
                    #endregion
                }
                return RetornoSefaz(rets, "nProt", "xMotivo");
            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message.ToString() + "\n" + erro.Source.ToString());
                return "1989|ERRO DE CONEXAO";
            }

        }
        /// <summary>
        /// Mostra retorno XML (sefaz)
        /// </summary>
        /// 

        private int contatdor_=1;
        public string RetornoSefaz(object rets,string pegarTag,string motivo)
        {
            
            var xDoc = XElement.Parse((rets as XmlElement).OuterXml);
            if (contatdor_ == 2)
            {
                xDoc.Save(Integra_ERP.VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\Protocolo.xml");
            }
            contatdor_ = contatdor_ + 1;
            string nrec = string.Empty;
            string status = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString();
            switch (status)
            {
                case "100":
                    nrec = status + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(pegarTag)).FirstOrDefault().Value.ToString() + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();
                  XmlDocument doc__ = new XmlDocument();
                  doc__.LoadXml((rets as XmlElement).OuterXml);
                  XmlNodeList retRetNFeList_ = doc__.GetElementsByTagName("protNFe");
                  foreach (XmlNode retRetNFeNode in retRetNFeList_)
                  {
                      XmlElement retCancNFeElemento = (XmlElement)retRetNFeNode;

                      XmlNodeList infRetcList = retCancNFeElemento.GetElementsByTagName("infProt");

                      foreach (XmlNode infRetcNode in infRetcList)
                      {
                          XmlElement infRetElemento = (XmlElement)infRetcNode;
                          if(infRetElemento.GetElementsByTagName("cStat")[0].InnerText.ToString().Equals("100"))
                          {
                              nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("nProt")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("chNFe")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("dhRecbto")[0].InnerText+"|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                          }
                          else
                          {
                              nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                          }
                      }
                  }
                    break;
                case "101":
                    nrec = status + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(pegarTag)).FirstOrDefault().Value.ToString() + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();
                    XmlDocument doc_canc = new XmlDocument();
                    doc_canc.LoadXml((rets as XmlElement).OuterXml);
                    XmlNodeList retRetNFeList_Canc = doc_canc.GetElementsByTagName("retCancNFe");
                    foreach (XmlNode retRetNFeNode in retRetNFeList_Canc)
                    {
                        XmlElement retCancNFeElemento = (XmlElement)retRetNFeNode;

                        XmlNodeList infRetcList = retCancNFeElemento.GetElementsByTagName("infCanc");

                        foreach (XmlNode infRetcNode in infRetcList)
                        {
                            XmlElement infRetElemento = (XmlElement)infRetcNode;
                            if (infRetElemento.GetElementsByTagName("cStat")[0].InnerText.ToString().Equals("101"))
                            {
                                nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("nProt")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("chNFe")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("dhRecbto")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                            }
                            else
                            {
                                nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                            }
                        }
                    }
                    break;
                case "103":
                    nrec = status + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(pegarTag)).FirstOrDefault().Value.ToString() + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();
                    break;
                case "104":
                   nrec = status + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(pegarTag)).FirstOrDefault().Value.ToString() + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();
                  XmlDocument doc_ = new XmlDocument();
                  doc_.LoadXml((rets as XmlElement).OuterXml);
                  XmlNodeList retRetNFeList = doc_.GetElementsByTagName("protNFe");
                  foreach (XmlNode retRetNFeNode in retRetNFeList)
                  {
                      XmlElement retCancNFeElemento = (XmlElement)retRetNFeNode;

                      XmlNodeList infRetcList = retCancNFeElemento.GetElementsByTagName("infProt");

                      foreach (XmlNode infRetcNode in infRetcList)
                      {
                          XmlElement infRetElemento = (XmlElement)infRetcNode;
                          if(infRetElemento.GetElementsByTagName("cStat")[0].InnerText.ToString().Equals("100"))
                          {
                              nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("nProt")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("chNFe")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("dhRecbto")[0].InnerText+"|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                          }
                          else
                          {
                              nrec = infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "|" + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                          }
                      }
                  }

                    
                    break;
                default:
                    nrec = status + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();
                    break;
            }
            return nrec;

        }
        public string RetornoSefazEventoDownload(object rets, string pegarTag, string motivo)
        {
            string nrec = string.Empty;

            try
            {
                //XmlNode xDoc = (XmlNode)rets;

                var xDoc = XElement.Parse((rets as XmlElement).OuterXml);

                string status = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString();
                string motivoprocesso = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("xMotivo")).FirstOrDefault().Value.ToString();
                XmlDocument doc__ = new XmlDocument();
                doc__.LoadXml((rets as XmlElement).OuterXml);

                if (status == "139")
                {

                    XmlNodeList lista = doc__.GetElementsByTagName("retNFe")[0].ChildNodes;
                    String cStatusNFE = "";
                    String xMotivo = "";

                    for (int i = 0; i < lista.Count; i++)
                    {
                        XmlNode no = lista[i];
                        if (no.Name == "cStat")
                        {
                            cStatusNFE = no.InnerText;
                            if (cStatusNFE == "140")
                            {
                                XmlNodeList xml = doc__.GetElementsByTagName("nfeProc");
                                String XMLTexto = xml[0].InnerXml;
                                int nPos = XMLTexto.IndexOf("</NFe>")+6;

                                XMLTexto = XMLTexto.Substring(0, nPos);

                                String cRetornoDownload = "140|" + XMLTexto;

                                return cRetornoDownload;
                                //break;

                            }

                        }
                        else if (no.Name == "xMotivo")
                        {
                            xMotivo = no.InnerText;
                        }

                    }
                    status = cStatusNFE + "|" + xMotivo;
                }
                else {
                    status += "|" + motivoprocesso;
                }
                
                

                return status;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "|" + ex.Source);
                nrec = "|";
            }

            //MessageBox.Show("RetornoSefazEvento: " + nrec);
            return nrec;

        }
        public string RetornoSefazEventoConfirmacaoRecebimento(object rets, string pegarTag, string motivo)
        {
            string nrec = string.Empty;

            try
            {
                //XmlNode xDoc = (XmlNode)rets;

                var xDoc = XElement.Parse((rets as XmlElement).OuterXml);

                string status = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString();


                return status;
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "|" + ex.Source);
                nrec = "";
            }

            //MessageBox.Show("RetornoSefazEvento: " + nrec);
            return nrec;

        }


        public string RetornoSefazEvento(object rets, string pegarTag, string motivo)
        {
            string nrec = string.Empty;

            try
            {
                //XmlNode xDoc = (XmlNode)rets;

                var xDoc = XElement.Parse((rets as XmlElement).OuterXml);
                
                string status = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString();
                XmlDocument doc__ = new XmlDocument();
                doc__.LoadXml((rets as XmlElement).OuterXml);
                switch (status)
                {
                    case "128": // 100-Autorizado o Uso
                        //nrec = status + "|" + //xDoc.Descendants().Where(r => r.Name.LocalName.Equals(pegarTag)).FirstOrDefault().Value.ToString() + "|" + xDoc.Descendants().Where(r => r.Name.LocalName.Equals(motivo)).FirstOrDefault().Value.ToString();

                        XmlNode retRetNFeList = doc__.GetElementsByTagName("infEvento")[0];
                        nrec = retRetNFeList["cStat"].InnerText + "|" + retRetNFeList["xMotivo"].InnerText;

                        //XmlNode noProtocolo = retRetNFeList.GetElementsByTagName("cStat");
                        if (doc__.GetElementsByTagName("nProt").Count > 0)
                        {
                            nrec = nrec + "|" + doc__.GetElementsByTagName("nProt")[0].InnerText.ToString();
                        }

                        if (doc__.GetElementsByTagName("dhRegEvento").Count > 0)
                        {
                            nrec = nrec + "|" + doc__.GetElementsByTagName("dhRegEvento")[0].InnerText.ToString();
                        }

                        
                        //nrec = xDoc["cStat"].InnerText.ToString() + "|" + xDoc["xMotivo"].InnerText.ToString() ;//+ "|" + xDoc["nProt"].InnerText.ToString();

                        break;

                    default:
                        nrec =  doc__["cStat"].InnerText + "|" + doc__["xMotivo"].InnerText;//xDoc["cStat"].InnerText.ToString() + "|" + xDoc["xMotivo"].InnerText.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "|" + ex.Source);
                nrec = "";
            }

            //MessageBox.Show("RetornoSefazEvento: " + nrec);
            return nrec;

        }
        /// <summary>
        /// Funcao Cria XML Consulta recibo
        /// </summary>
        public XmlDocument IntegraConsultaRecibo(string nrec)
        {
            var xEle = new XmlDocument().CreateElement("Xml");
            try{
                string strUri = "http://www.portalfiscal.inf.br/nfe";
                string vartemp = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                  "<consReciNFe versao=\"" + "2.00" + "\" xmlns=\"" + strUri + "\">" +
                  "<tpAmb>" + Integra_ERP.VariavesEstaticas.Ambiente.ToString() + "</tpAmb>" +
                  "<nRec>" + nrec + "</nRec>" +
                  "</consReciNFe>";
                xEle.InnerXml = vartemp.ToString();
            }
            catch(Exception erro)
            {
                Log(erro.Message);
                xEle.InnerXml ="";
                
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xEle.InnerXml);
            return doc;
        }
        public XmlDocument IntegraConsultaRecibo310(string nrec)
        {
            var xEle = new XmlDocument().CreateElement("Xml");
            try
            {
                string strUri = "http://www.portalfiscal.inf.br/nfe";
                string vartemp = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                  "<consReciNFe versao=\"" + "3.10" + "\" xmlns=\"" + strUri + "\">" +
                  "<tpAmb>" + Integra_ERP.VariavesEstaticas.Ambiente.ToString() + "</tpAmb>" +
                  "<nRec>" + nrec + "</nRec>" +
                  "</consReciNFe>";
                xEle.InnerXml = vartemp.ToString();
            }
            catch (Exception erro)
            {
                Log(erro.Message);
                xEle.InnerXml = "";

            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xEle.InnerXml);
            return doc;
        }


        /// <summary>
        /// Funcao Consulta Recibo
        /// </summary>
        public string IntegraExecutaConsultaRecebico(XmlDocument sDados)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            string resp = string.Empty;
                        var rets = new object();
            //Homologacao
#region homologacao/producao
                        if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
                        {
                            #region NFe Segipe
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                            {
                                HomologacaoSEnfeRetRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoSEnfeRetRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                HomologacaoSEnfeRetRecepcao.nfeCabecMsg cabret = new HomologacaoSEnfeRetRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion

                            #region NFe Para
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                            {
                                HomologacaoPAretRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoPAretRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                HomologacaoPAretRecepcao.nfeCabecMsg cabret = new HomologacaoPAretRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion

                            #region NFe Bahia
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                            {
                                HomologacaoBAretRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoBAretRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                HomologacaoBAretRecepcao.nfeCabecMsg cabret = new HomologacaoBAretRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion
                        }
                        else
                        {
                            #region NFe Segipe
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                            {
                                ProducaoSEnfeRetRecepcao.NfeRetRecepcao2 retNFe = new ProducaoSEnfeRetRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                ProducaoSEnfeRetRecepcao.nfeCabecMsg cabret = new ProducaoSEnfeRetRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion

                            #region NFe Para
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                            {

                                ProducaoPAretRecepcao.NfeRetRecepcao2 retNFe = new ProducaoPAretRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                ProducaoPAretRecepcao.nfeCabecMsg cabret = new ProducaoPAretRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion

                            #region NFe Bahia
                            if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                            {
                                ProducaoBAretRecepcao.NfeRetRecepcao2 retNFe = new ProducaoBAretRecepcao.NfeRetRecepcao2();
                                retNFe.ClientCertificates.Add(certificado);
                                retNFe.Timeout = 100000;
                                retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                                ProducaoBAretRecepcao.nfeCabecMsg cabret = new ProducaoBAretRecepcao.nfeCabecMsg();
                                cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                                cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                                retNFe.nfeCabecMsgValue = cabret;
                                var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                                return RetornoSefaz(dDados, "nRec", "xMotivo");
                            }
                            #endregion
                        }
#endregion
                        return resp;
        }

        public string IntegraExecutaConsultaRecibo310(XmlDocument sDados)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);
            string resp = string.Empty;
            var rets = new object();
            //Homologacao
            #region homologacao/producao
            if (Integra_ERP.VariavesEstaticas.Ambiente == 2)
            {
                #region NFe Segipe
                if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                {
                    HomologacaoSEnfeRetRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoSEnfeRetRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    HomologacaoSEnfeRetRecepcao.nfeCabecMsg cabret = new HomologacaoSEnfeRetRecepcao.nfeCabecMsg();
                    cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                    cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion

                #region NFe Para
                if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                {
                    HomologacaoPAretRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoPAretRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    HomologacaoPAretRecepcao.nfeCabecMsg cabret = new HomologacaoPAretRecepcao.nfeCabecMsg();
                    cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                    cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion

                #region NFe Bahia
                if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                {
                    HomologacaoBAretRecepcao.NfeRetRecepcao2 retNFe = new HomologacaoBAretRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    HomologacaoBAretRecepcao.nfeCabecMsg cabret = new HomologacaoBAretRecepcao.nfeCabecMsg();
                    cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                    cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion
            }
            else
            {
                #region NFe Segipe
                if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                {
                    ProducaoSEnfeRetRecepcao.NfeRetRecepcao2 retNFe = new ProducaoSEnfeRetRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    ProducaoSEnfeRetRecepcao.nfeCabecMsg cabret = new ProducaoSEnfeRetRecepcao.nfeCabecMsg();
                    cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                    cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion

                #region NFe Para
                if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                {
                    NfeRetAutorizacao retNFe = new NfeRetAutorizacao();
                    //ProducaoPAretRecepcao.NfeRetRecepcao2 retNFe = new ProducaoPAretRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = "https://www.sefazvirtual.fazenda.gov.br/NfeRetAutorizacao/NfeRetAutorizacao.asmx?wsdl";////Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    br.gov.fazenda.sefazvirtual.NfeRetAut310.nfeCabecMsg cabret = new br.gov.fazenda.sefazvirtual.NfeRetAut310.nfeCabecMsg();
                    cabret.cUF = "15";
                    cabret.versaoDados = "3.10";//Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                    var dDados = retNFe.nfeRetAutorizacaoLote(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion

                #region NFe Bahia
                if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                {
                    ProducaoBAretRecepcao.NfeRetRecepcao2 retNFe = new ProducaoBAretRecepcao.NfeRetRecepcao2();
                    retNFe.ClientCertificates.Add(certificado);
                    retNFe.Timeout = 100000;
                    retNFe.Url = Integra_ERP.VariavesEstaticas.Url_RetRecepcao;
                    ProducaoBAretRecepcao.nfeCabecMsg cabret = new ProducaoBAretRecepcao.nfeCabecMsg();
                    cabret.cUF = Integra_ERP.VariavesEstaticas.cUF.ToString();
                    cabret.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                    retNFe.nfeCabecMsgValue = cabret;
                    var dDados = retNFe.nfeRetRecepcao2(sDados.ChildNodes[1]);
                    return RetornoSefaz(dDados, "nRec", "xMotivo");
                }
                #endregion
            }
            #endregion
            return resp;
        }

        public void ExecMensagem()
        {
            System.Windows.Forms.MessageBox.Show("TESTE");
        }
        /// <summary>
        /// Funcao Log
        /// </summary>
        ///
        public void Log(string txt)
        {

        }


        //Nova Implantacao
        #region Propriedades

        /// <summary>
        /// Descrição do serviço (WSDL)
        /// </summary>
        private ServiceDescription serviceDescription { get; set; }
        /// <summary>
        /// Código assembly do serviço
        /// </summary>
        private Assembly serviceAssemby { get; set; }
        /// <summary>
        /// Certificado digital a ser utilizado no consumo dos serviços
        /// </summary>
        private X509Certificate2 oCertificado { get; set; }

        #endregion

        #region Construtores

        /*
        #region WebServiceProxy
        public Utilitarios(Uri requestUri, X509Certificate2 Certificado)
        {
            //Definir o certificado digital que será utilizado na conexão com os serviços
            this.oCertificado = Certificado;

            //Confirmar a solicitação SSL automaticamente
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateValidation);

            try
            {
                //Obeter a descrção do serviço (WSDL)
                this.DescricaoServico(requestUri, this.oCertificado);

                //Gerar e compilar a classe
                this.GerarClasse();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
         */ 

        #endregion

        #region Métodos públicos

        #region ReturnArray()
        /// <summary>
        /// Método que verifica se o tipo de retornjo de uma operação/método é array ou não
        /// </summary>
        /// <param name="Instance">Instancia do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <returns>true se o tipo de retorno do método passado por parâmetro for um array</returns>
        public bool ReturnArray(object Instance, string methodName)
        {
            Type tipoInstance = Instance.GetType();

            return tipoInstance.GetMethod(methodName).ReturnType.IsSubclassOf(typeof(Array));
        }
        #endregion

        #region Invoke()
        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Objeto - Um objeto somente, podendo ser primário ou complexo</returns>
        public object Invoke(object Instance, string methodName, object[] parameters)
        {
            //Relacionar o certificado digital que será utilizado no serviço que será consumido do webservice
            Type tipoInstance = Instance.GetType();
            object oClientCertificates = tipoInstance.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, Instance, new Object[] { });
            Type tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });

            //Invocar método do serviço
            return tipoInstance.GetMethod(methodName).Invoke(Instance, parameters);
        }
        #endregion

        #region InvokeXML()
        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Um objeto do tipo XML</returns>
        public XmlNode InvokeXML(object Instance, string methodName, object[] parameters)
        {
            //Relacionar o certificado digital que será utilizado no serviço que será consumido do webservice
            Type tipoInstance = Instance.GetType();
            object oClientCertificates = tipoInstance.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, Instance, new Object[] { });
            Type tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });

            //Invocar método do serviço
            return (XmlNode)tipoInstance.GetMethod(methodName).Invoke(Instance, parameters);
        }
        #endregion

        #region SetProp()
        public void SetProp(object Instance, string propertyName, object novoValor)
        {
            Type tipoInstance = Instance.GetType();
            PropertyInfo property = tipoInstance.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            property.SetValue(Instance, novoValor, null);
        }
        #endregion

        #region InvokeArray()
        /// <summary>
        /// Invocar o método da classe
        /// </summary>
        /// <param name="Instance">Instância do objeto</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Objeto com o conteúdo dos parâmetros do método</param>
        /// <returns>Vetor de objetos - uma lista de objetos primários ou complexos</returns>
        public object[] InvokeArray(object Instance, string methodName, object[] parameters)
        {
            //Relacionar o certificado digital que será utilizado no serviço que será consumido do webservice
            Type tipoInstance = Instance.GetType();
            object oClientCertificates = tipoInstance.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, Instance, new Object[] { });
            Type tipoClientCertificates;
            tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });

            //Invocar método do serviço
            return (object[])tipoInstance.GetMethod(methodName).Invoke(Instance, parameters);
        }
        #endregion

        #region CertificateValidation
        /// <summary>
        /// Responsável por retornar uma confirmação verdadeira para a proriedade ServerCertificateValidationCallback 
        /// da classe ServicePointManager para confirmar a solicitação SSL automaticamente.
        /// </summary>
        /// <returns>Retornará sempre true</returns>
        public bool CertificateValidation(object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErros)
        {            
            return true;
        }
        #endregion

        #region CriarObjeto()
        /// <summary>
        /// Criar objeto das classes do serviço
        /// </summary>
        /// <param name="NomeClasse">Nome da classe que é para ser instanciado um novo objeto</param>
        /// <returns>Retorna o objeto</returns>
        public object CriarObjeto(string NomeClasse)
        {
            try
            {
                return Activator.CreateInstance(this.serviceAssemby.GetType(NomeClasse));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion

        #region Métodos privados

        #region DescricaoServico()
        /// <summary>
        /// Obter a descrição completa do serviço, ou seja, o WSDL do webservice
        /// </summary>
        /// <param name="requestUri">Uri (endereço https) para obter o WSDL</param>
        /// <param name="Certificado">Certificado digital</param>
        private void DescricaoServico(Uri requestUri, X509Certificate2 Certificado)
        {
            try
            {
                //Forçar utilizar o protocolo SSL 3.0 que está de acordo com o manual de integração do SEFAZ
                //Wandrey 31/03/2010
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                //Definir o endereço para a requisição do wsdl
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);

                //Definir o certificado digital que deve ser utilizado na requisição do wsdl
                request.ClientCertificates.Add(Certificado);                

                //Requisitar o WSDL e gravar em um stream                
                Stream stream = request.GetResponse().GetResponseStream();               

                //Definir a descrição completa do servido (WSDL)
                this.serviceDescription = ServiceDescription.Read(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
        }
        #endregion

        #region GerarClasse()
        private void GerarClasse()
        {
            #region Gerar o código da classe
            StringWriter writer = new StringWriter(CultureInfo.CurrentCulture);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            provider.GenerateCodeFromNamespace(this.GerarGrafo(), writer, null);
            #endregion
            #region Compilar o código da classe
            CompilerResults results = provider.CompileAssemblyFromSource(this.ParametroCompilacao(), writer.ToString());
            this.serviceAssemby = results.CompiledAssembly;
            #endregion
        }
        #endregion

        #region ParametroCompilacao
        private CompilerParameters ParametroCompilacao()
        {
            CompilerParameters parameters = new CompilerParameters(new string[] { "System.dll", "System.Xml.dll", "System.Web.Services.dll", "System.Data.dll" });
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.TreatWarningsAsErrors = false;
            parameters.WarningLevel = 4;

            return parameters;
        }
        #endregion

        #region GerarGrafo()
        private CodeNamespace GerarGrafo()
        {

            #region Gerar a estrutura da classe do serviço
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.AddServiceDescription(this.serviceDescription, string.Empty, string.Empty);
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;
            #endregion

            #region Gerar o o grafo da classe para depois gerar o código
            CodeNamespace @namespace = new CodeNamespace();
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(@namespace);
            ServiceDescriptionImportWarnings warmings = importer.Import(@namespace, unit);
            #endregion

            return @namespace;
        }
        #endregion

        #region RelacCertificado
        private void RelacCertificado(object instance)
        {
            Type tipoInstance = instance.GetType();
            object oClientCertificates = tipoInstance.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, instance, new Object[] { });
            Type tipoClientCertificates;
            tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        }
        #endregion

        #endregion

    }
}
