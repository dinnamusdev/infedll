using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace iNFeDll.Integra_ERP
{


    public class ClasseConsultaNFe
    {
        public void AbreTelaConsulta()
        {
            frmConsulta f = new frmConsulta();
            f.ShowDialog();
            f.Dispose();
        }

        public object ExecutaConsulta(string chaveDeAcesso)
        {

            var xEle = new XmlDocument().CreateElement("Xml");
            xEle.InnerXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<consSitNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\""+Integra_ERP.VariavesEstaticas.VersaoNF+"\">" +
                            "<tpAmb>1</tpAmb>" +
                            "<xServ>CONSULTAR</xServ>" +
                            "<chNFe>" + chaveDeAcesso + "</chNFe>" +
                            "</consSitNFe>";


            var resposta = new object();
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificado = null;
            //Homologacao
           // if (Integra_ERP.VariavesEstaticas.Ambiente ==2 )
            //{
                #region NFe Sergipe
                if (Integra_ERP.VariavesEstaticas.UF.Equals("SE"))
                {

                    ProducaoSEconsulta.NfeConsulta2 ws = new ProducaoSEconsulta.NfeConsulta2();
                    ws.Timeout = 100000;
                    ws.Url = VariavesEstaticas.Url_Consulta;
                    certificado  = RegraDeNegocios.CertificadoDigital.SelecionarCertificado();
                    if (certificado != null)
                    {

                        ws.ClientCertificates.Add(certificado);
                        ws.nfeCabecMsgValue = (new ProducaoSEconsulta.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        ws.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        resposta = ws.nfeConsultaNF2(xEle.ChildNodes[1]);
                    }
                    else
                        return null;
                }
                #endregion

                #region NFe Pará
                if (Integra_ERP.VariavesEstaticas.UF.Equals("PA"))
                {
                    ProducaoPAconsulta.NfeConsulta2 ws = new ProducaoPAconsulta.NfeConsulta2();
                    ws.Timeout = 100000;
                    ws.Url = VariavesEstaticas.Url_Consulta;
                    certificado  = RegraDeNegocios.CertificadoDigital.SelecionarCertificado();
                    if (certificado != null)
                    {
                        ws.ClientCertificates.Add(certificado);
                        ws.nfeCabecMsgValue = (new ProducaoPAconsulta.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        ws.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        resposta = ws.nfeConsultaNF2(xEle.ChildNodes[1]);
                    }
                    else
                        return null;
                }
                #endregion

                #region NFe Bahia
                if (Integra_ERP.VariavesEstaticas.UF.Equals("BA"))
                {
                    HomologacaoBAConsulta.NfeConsulta2 ws = new HomologacaoBAConsulta.NfeConsulta2();
                    ws.Timeout = 100000;
                    ws.Url = VariavesEstaticas.Url_Consulta;
                    certificado  = RegraDeNegocios.CertificadoDigital.SelecionarCertificado();
                    if (certificado != null)
                    {
                        ws.ClientCertificates.Add(certificado);
                        ws.nfeCabecMsgValue = (new HomologacaoBAConsulta.nfeCabecMsg() { cUF = Integra_ERP.VariavesEstaticas.cUF.ToString(), versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF });
                        ws.nfeCabecMsgValue.versaoDados = Integra_ERP.VariavesEstaticas.VersaoNF;
                        resposta = ws.nfeConsultaNF2(xEle.ChildNodes[1]);
                    }
                    else
                        return null;
                }
                #endregion
            //}
            var xDoc = resposta;
            return xDoc;

        }
       
    }
}
