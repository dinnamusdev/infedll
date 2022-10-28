using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using iNFeDll.RegraDeNegocios;

namespace iNFeDll.Integra_ERP
{
     [ClassInterface(ClassInterfaceType.AutoDual)]
     [ProgId("EnvioRecebimento")]
	 [ComVisible(true)]
    public class EnvioRecebimento
    {
        /// <summary>
        /// Funcao para ver a estrutura das pasta
        /// </summary>
        /// <by>Rafael Almeida</by>
         public void VerEstruturaPastas()
         {
             try
             {
                 if (!Directory.Exists(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos"))
                     Directory.CreateDirectory(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\");

                 if (!Directory.Exists(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado"))
                     Directory.CreateDirectory(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\");

                 if (!Directory.Exists(VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp"))
                     Directory.CreateDirectory(VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\");

                 if (!Directory.Exists(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro"))
                     Directory.CreateDirectory(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\");
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message);
             }
         }
        /// <summary>
        /// Funcao para enviar NF-e
        /// </summary>
        /// <param name="StrXml">Arquivo xml</param>
        /// <param name="amb">Ambiente 1- Produção | 2 - Homologação</param>
        /// <param name="uf">28(SE)</param>
        /// <param name="assinarvalida">0-Enviar Arquivo Ja assinado | 1-Assina e Valida Arquivo</param>
        /// <by>Rafael Almeida</by>
        /// 
         private frmBaixarNotas baixarxml = null;
        public static string cRetornoConsulta = "";
        private string RetornoSefaz(object rets, string pegarTag, string motivo)
        {


            var xDoc = System.Xml.Linq.XElement.Parse((rets as XmlElement).OuterXml);
            string status = xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString();
            if (status != "100")
            {
                status = "Status: " + xDoc.Descendants().Where(r => r.Name.LocalName.Equals("cStat")).FirstOrDefault().Value.ToString() + "\n" +
                                "Motivo: " + xDoc.Descendants().Where(r => r.Name.LocalName.Equals("xMotivo")).FirstOrDefault().Value.ToString(); ;
            }
            else
            {
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
                        if (infRetElemento.GetElementsByTagName("cStat")[0].InnerText.ToString().Equals("100"))
                        {
                            status = "Status: " +
                                infRetElemento.GetElementsByTagName("cStat")[0].InnerText + "\n" +
                                "Protocolo: " + infRetElemento.GetElementsByTagName("nProt")[0].InnerText + "\n" +
                                "Data: " + infRetElemento.GetElementsByTagName("dhRecbto")[0].InnerText + "\n" +
                                "Motivo: " + infRetElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                        }
                    }
                }
            }
            return status;
        }

        public string consultar(string cChaveDeAcesso) {
            try
            {
                string cRetorno="";

                string status = string.Empty;
                frmConsulta consultar = new frmConsulta();
                
                consultar.Show();
                consultar.lblConsulta.Text = "CONSULTANDO SERVIDOR SEFAZ....AGUARDE";
                consultar.txtChaveDeAcesso.Text = cChaveDeAcesso;
                Application.DoEvents();
                try
                {
                    if (cChaveDeAcesso.Length == 44)
                    {
                                                
                        Integra_ERP.ClasseConsultaNFe clsCon = new Integra_ERP.ClasseConsultaNFe();
                        var retorno = clsCon.ExecutaConsulta(cChaveDeAcesso);

                        if (retorno != null)
                        {
                            String cRetornoNaoTratado = RetornoSefaz(retorno, "cStat", "xMotivo");
                            cRetorno= cRetornoNaoTratado.Replace("\\", "|");
                            consultar.txtResposta.Text = cRetorno;
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(3000);
                        }
                        else
                        {
                            MessageBox.Show("NAO FOI POSSIVEL REALIZAR A CONSULTA", "Consultar NFE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                     
                          
                        }
                    }
                    else
                    {
                        MessageBox.Show("INFORME UMA CHAVE DE ACESSO VÁLIDA", "Consultar NFE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                         
                       
                    }
                }
                catch (ExecutionEngineException ex)
                {
                    MessageBox.Show("ERRO: " + ex.Message, "Consultar NFE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                consultar.Dispose();
                return cRetorno;
            }
            catch (Exception ex)
            {
                
                return "ERROR:" + ex.Message + " - "  + ex.StackTrace;
            }  
        }
        public string Enviar(string StrXml,int assinarvalida)
        {

            System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
            VerEstruturaPastas();

            String arquivoXML = StrXml;

            frmMenNfe fmen = new frmMenNfe();
            fmen.lblVersao.Text = "NFe 2.00";
            fmen.Show();

            string nomearquivo = Path.GetFileName(arquivoXML);

            try
            {
                if (assinarvalida == 1)
                {
                   
                    //Faz Assinatura do Arquivo
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                    string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                    ASSD.Assinar(arquivoXML, "infNFe", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado);
                    if (ASSD.intResultado != 0)
                    {
                        fmen.setDatos(-2);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                    }

                    //Valida O Arquivo
                    xmlAssinado.Load(strArquivoAssinado);
                    fmen.setDatos(3);

                    //Valida o XML assinado

                    string resultado = RegraDeNegocios.ValidaXML.ValidarXML(xmlAssinado);
                    if (resultado.Trim().Length != 0)
                    {
                        fmen.setDatos(-3);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        fmen.Dispose();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return "-10|XML Com erro:\n\n" + resultado.ToString();
                    }


                    RegraDeNegocios.NFe Nfe = new RegraDeNegocios.NFe();
                    //Opcional - Função para gerar o Lote e deixar o arquivo pronto para ser enviado.
                    Nfe.GerarLoteNfe(ref xmlAssinado);
                    xmlAssinado.Save(strArquivoAssinado);

                    arquivoXML = strArquivoAssinado;

                }
                if (assinarvalida == 0)
                {
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    fmen.setDatos(3);
                    string arq_ass = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" +nomearquivo;
                    if (StrXml.ToUpper() != arq_ass.ToUpper())
                    {
                        File.Copy(StrXml, arq_ass, true);
                    }

                    xmlAssinado.Load(arq_ass);
                    arquivoXML = arq_ass;
                }

                xmlAssinado.Load(arquivoXML);
                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFe(xmlAssinado);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');

                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }

                int tentativas = 1;

                if (retorno[0].Equals("103") || retorno[0].Equals("104"))
                {
                tentativas_:
                    fmen.setDatos(5);

                    string resCon = iUtil.IntegraExecutaConsultaRecebico(iUtil.IntegraConsultaRecibo(retorno[1]));

                    string[] retorno_ = resCon.Split('|');
                    if (retorno[0].Equals("1989"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("104") || retorno_[0].Equals("105") && tentativas < 10)
                    {
                        tentativas++;
                        System.Threading.Thread.Sleep(1000);
                        goto tentativas_;
                    }
                    if (retorno_[0].Equals("103"))
                    {
                        fmen.Close();
                        fmen.Dispose();

                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("100"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno_.Length > 2)
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno_[1].ToString();
                            return (retorno_[0] + "|" + retorno_[1].ToString() + "|" + retorno_[2].ToString() + "|" + retorno_[3].ToString() + "|" + retorno_[4].ToString());
                        }
                        else
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            return (retorno_[0] + "|" + retorno_[1].ToString());
                        }
                    }
                    else
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }

                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
                if (transmitiu == 1)
                    fmen.setDatos(6);
                else
                    fmen.Close();

                fmen.Close();
                fmen.Dispose();
                return retorno.ToString();
            }
            catch (Exception erro)
            {   

                fmen.Close();
                return "1989|"+erro.Message.ToString()+"\n"+erro.Source;
            }
        }

        public string Enviar400(string StrXml, int assinarvalida)
        {
            VariavesEstaticas.VersaoNF = "4.00";
            System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
            VerEstruturaPastas();

            String arquivoXML = StrXml;

            frmMenNfe fmen = new frmMenNfe();
            fmen.lblVersao.Text = "NFe 4.00";
            fmen.Show();

            string nomearquivo = Path.GetFileName(arquivoXML);

            try
            {
                if (assinarvalida == 1)
                {

                    //Faz Assinatura do Arquivo
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                    string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                    ASSD.Assinar(arquivoXML, "infNFe", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado);
                    if (ASSD.intResultado != 0)
                    {
                        fmen.setDatos(-2);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                    }

                    //Valida O Arquivo
                    xmlAssinado.Load(strArquivoAssinado);
                    fmen.setDatos(3);

                    //Valida o XML assinado

                    string resultado = RegraDeNegocios.ValidaXML.ValidarXML400(xmlAssinado);
                    if (resultado.Trim().Length != 0)
                    {
                        fmen.setDatos(-3);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        fmen.Dispose();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return "-10|XML Com erro:\n\n" + resultado.ToString();
                    }


                    RegraDeNegocios.NFe Nfe = new RegraDeNegocios.NFe();
                    //Opcional - Função para gerar o Lote e deixar o arquivo pronto para ser enviado.
                    Nfe.GerarLoteNfe400(ref xmlAssinado);
                    xmlAssinado.Save(strArquivoAssinado);

                    arquivoXML = strArquivoAssinado;

                }
                if (assinarvalida == 0)
                {
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    fmen.setDatos(3);
                    string arq_ass = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                    if (StrXml.ToUpper() != arq_ass.ToUpper())
                    {
                        File.Copy(StrXml, arq_ass, true);
                    }

                    xmlAssinado.Load(arq_ass);
                    arquivoXML = arq_ass;
                }

                xmlAssinado.Load(arquivoXML);
                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFe400(xmlAssinado);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');

                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }

                int tentativas = 1;

                if (retorno[0].Equals("103") || retorno[0].Equals("104") || retorno[0].Equals("100"))
                {
                tentativas_:
                    fmen.setDatos(5);

                    //string resCon = iUtil.IntegraExecutaConsultaRecibo310(iUtil.IntegraConsultaRecibo310(retorno[1]));

                    //string[] retorno_ = resCon.Split('|');
                    if (retorno[0].Equals("1989"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno[1].ToString());
                    }
                    if (retorno[0].Equals("104") || retorno[0].Equals("105") && tentativas < 10)
                    {
                        tentativas++;
                        System.Threading.Thread.Sleep(1000);
                        goto tentativas_;
                    }
                    if (retorno[0].Equals("103"))
                    {
                        fmen.Close();
                        fmen.Dispose();

                        return (retorno[0] + "|" + retorno[1].ToString());
                    }
                    if (retorno[0].Equals("100"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno.Length > 2)
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno[1].ToString();
                            return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString() + "|" + retorno[4].ToString());
                        }
                        else
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            return (retorno[0] + "|" + retorno[1].ToString());
                        }
                    }
                    else
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno[1].ToString());
                    }

                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
                if (transmitiu == 1)
                    fmen.setDatos(6);
                else
                    fmen.Close();

                fmen.Close();
                fmen.Dispose();
                return retorno.ToString();
            }
            catch (Exception erro)
            {

                fmen.Close();
                return "1989|" + erro.Message.ToString() + "\n" + erro.Source;
            }
        }
        public string Enviar310(string StrXml, int assinarvalida)
        {
            VariavesEstaticas.VersaoNF = "3.10";
            System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
            VerEstruturaPastas();

            String arquivoXML = StrXml;

            frmMenNfe fmen = new frmMenNfe();
            fmen.lblVersao.Text = "NFe 3.10";
            fmen.Show();

            string nomearquivo = Path.GetFileName(arquivoXML);

            try
            {
                if (assinarvalida == 1)
                {

                    //Faz Assinatura do Arquivo
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                    string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                    ASSD.Assinar(arquivoXML, "infNFe", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado);
                    if (ASSD.intResultado != 0)
                    {
                        fmen.setDatos(-2);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                    }

                    //Valida O Arquivo
                    xmlAssinado.Load(strArquivoAssinado);
                    fmen.setDatos(3);

                    //Valida o XML assinado

                    string resultado = RegraDeNegocios.ValidaXML.ValidarXML310(xmlAssinado);
                    if (resultado.Trim().Length != 0)
                    {
                        fmen.setDatos(-3);
                        System.Threading.Thread.Sleep(3000);
                        fmen.Close();
                        fmen.Dispose();
                        File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                        return "-10|XML Com erro:\n\n" + resultado.ToString();
                    }


                    RegraDeNegocios.NFe Nfe = new RegraDeNegocios.NFe();
                    //Opcional - Função para gerar o Lote e deixar o arquivo pronto para ser enviado.
                    Nfe.GerarLoteNfe310(ref xmlAssinado);
                    xmlAssinado.Save(strArquivoAssinado);

                    arquivoXML = strArquivoAssinado;

                }
                if (assinarvalida == 0)
                {
                    fmen.setDatos(1);
                    fmen.setDatos(2);
                    fmen.setDatos(3);
                    string arq_ass = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                    if (StrXml.ToUpper() != arq_ass.ToUpper())
                    {
                        File.Copy(StrXml, arq_ass, true);
                    }

                    xmlAssinado.Load(arq_ass);
                    arquivoXML = arq_ass;
                }

                xmlAssinado.Load(arquivoXML);
                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFe310(xmlAssinado);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');

                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }

                int tentativas = 1;

                if (retorno[0].Equals("103") || retorno[0].Equals("104") || retorno[0].Equals("100"))
                {
                tentativas_:
                    fmen.setDatos(5);

                    //string resCon = iUtil.IntegraExecutaConsultaRecibo310(iUtil.IntegraConsultaRecibo310(retorno[1]));

                    //string[] retorno_ = resCon.Split('|');
                    if (retorno[0].Equals("1989"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno[1].ToString());
                    }
                    if (retorno[0].Equals("104") || retorno[0].Equals("105") && tentativas < 10)
                    {
                        tentativas++;
                        System.Threading.Thread.Sleep(1000);
                        goto tentativas_;
                    }
                    if (retorno[0].Equals("103"))
                    {
                        fmen.Close();
                        fmen.Dispose();

                        return (retorno[0] + "|" + retorno[1].ToString());
                    }
                    if (retorno[0].Equals("100"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno.Length > 2)
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno[1].ToString();
                            return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString() + "|" + retorno[4].ToString());
                        }
                        else
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            return (retorno[0] + "|" + retorno[1].ToString());
                        }
                    }
                    else
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno[1].ToString());
                    }

                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
                if (transmitiu == 1)
                    fmen.setDatos(6);
                else
                    fmen.Close();

                fmen.Close();
                fmen.Dispose();
                return retorno.ToString();
            }
            catch (Exception erro)
            {

                fmen.Close();
                return "1989|" + erro.Message.ToString() + "\n" + erro.Source;
            }
        }

        /// <summary>
        /// Funcao para Inutilizar NF-e
        /// </summary>
        /// <param name="StrXml">Arquivo xml</param>
        /// <by>Rafael Almeida</by>
        XmlDocument xmlInutilizar = null;
        public String InutilizarNota(int Estado,string DE,String ATE, string Serie, string Justificativa,String CNPJ,String ANO, String Modelo)
        {
            
            try
            {
                if (!Directory.Exists(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlInutilizados\"))
                    Directory.CreateDirectory(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlInutilizados\");

                xmlInutilizar = new XmlDocument();
                xmlInutilizar.InnerXml = ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<inutNFe versao=\"2.00\" xmlns=\"http://www.portalfiscal.inf.br/nfe\"> "+
                "<infInut Id=\"ID"+Estado+ANO+CNPJ+"55"+int.Parse(Serie.ToString()).ToString("000")+
                "55"+Int32.Parse(DE).ToString("000000000")+Int32.Parse(ATE).ToString("000000000")+"\">"+
                "<tpAmb>"+Integra_ERP.VariavesEstaticas.Ambiente+"</tpAmb>"+
                "<xServ>INUTILIZAR</xServ>"+
                "<cUF>"+Estado+"</cUF>"+
                "<ano>"+ANO+"</ano>"+
                "<CNPJ>"+CNPJ+"</CNPJ>"+
                "<mod>"+ Modelo +"</mod><serie>"+ Serie +"</serie>"+
                "<nNFIni>" + DE.TrimStart('0') + "</nNFIni>" +
                "<nNFFin>" + ATE.TrimStart('0') + "</nNFFin>" +
                "<xJust>"+Justificativa+"</xJust>"+
                "</infInut> </inutNFe>");

                xmlInutilizar.Save(Application.StartupPath + "\\Inutilizacao.XML");

                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = Application.StartupPath + "\\Inutilizacao.XML";

                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                //string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = arquivoXML; // +nomearquivo;
                ASSD.Assinar(arquivoXML, "infInut", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado);
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, Application.StartupPath + "\\InutilizacaoERRO.XML", true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }
                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLInutilizacao(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, Application.StartupPath + "\\InutilizacaoERRO.XML", true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }
                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeInutilizacao(xmlAssinado);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }

                int tentativas = 1;

                if (retorno[0].Equals("102"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString() + "|" + retorno[4].ToString());
                    }
                }

                if (retorno[0].Equals("103") || retorno[0].Equals("104"))
                {
                tentativas_:
                    fmen.setDatos(5);

                    string resCon = iUtil.IntegraExecutaConsultaRecebico(iUtil.IntegraConsultaRecibo(retorno[1]));

                    string[] retorno_ = resCon.Split('|');
                    if (retorno[0].Equals("1989"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("104") || retorno_[0].Equals("105") && tentativas < 10)
                    {
                        tentativas++;
                        System.Threading.Thread.Sleep(1000);
                        goto tentativas_;
                    }
                    if (retorno_[0].Equals("103"))
                    {
                        fmen.Close();
                        fmen.Dispose();

                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("100"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno_.Length > 2)
                        {

                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlInutilizados\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno_[1].ToString();
                            return (retorno_[0] + "|" + retorno_[1].ToString() + "|" + retorno_[2].ToString() + "|" + retorno_[3].ToString() + "|" + retorno_[4].ToString());
                        }
                        else
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            return (retorno_[0] + "|" + retorno_[1].ToString());
                        }
                    }
                    else
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }

                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
                if(transmitiu == 1)
                    fmen.setDatos(6);
                else
                    fmen.Close();

                fmen.Close();
                fmen.Dispose();
                return retorno.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
                
            }
            return "1989|INUTILIZAR_NADA";
            
        }

        /// <summary>
        /// Funcao para Cancelar NF-e
        /// </summary>
        /// <param name="StrXml">Arquivo xml</param>
        /// <by>Rafael Almeida</by>
        /// 

        public string EnviarNFeCartaDeCorrecao400(string StrXml)
        {
            try
            {
                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = StrXml;

                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                ASSD.Assinar(arquivoXML, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado, "evento");
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }

                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLCartaDeCorrecao(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }

                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeCancelamentoEvento400(xmlAssinado);
                //MessageBox.Show("EnviarNFeCancelamentoEvento : " + resEnv);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                /*
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }
                */
                int tentativas = 1;

                if (retorno[0].Equals("135"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + nomearquivo;
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo, NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo);
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString());
                    }
                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }

        public string EnviarNFeCartaDeCorrecao(string StrXml)
        {
            try
            {
                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = StrXml;
                        
                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                ASSD.Assinar(arquivoXML, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado,"evento");
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }

                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLCartaDeCorrecao(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }

                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeCancelamentoEvento(xmlAssinado);
                //MessageBox.Show("EnviarNFeCancelamentoEvento : " + resEnv);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                /*
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }
                */
                int tentativas = 1;

                if (retorno[0].Equals("135"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + nomearquivo;
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo, NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo);
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString()) ;
                    }
                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }

        public string EnviarNFeCancelamentoPorEvento400(string StrXml)
        {
            try
            {
                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = StrXml;

                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                ASSD.Assinar(arquivoXML, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado, "evento");
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }

                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLCancelamentoPorEvento(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }

                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeCancelamentoEvento400(xmlAssinado);
                //MessageBox.Show("EnviarNFeCancelamentoEvento : " + resEnv);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                /*
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }
                */
                int tentativas = 1;

                if (retorno[0].Equals("135"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + nomearquivo;
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo, NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo);
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString());
                    }
                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }
        public string EnviarNFeCancelamentoPorEvento(string StrXml)
        {
            try
            {
                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = StrXml;

                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                ASSD.Assinar(arquivoXML, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado, "evento");
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }

                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLCancelamentoPorEvento(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }

                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeCancelamentoEvento(xmlAssinado);
                //MessageBox.Show("EnviarNFeCancelamentoEvento : " + resEnv);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                /*
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }
                */
                int tentativas = 1;

                if (retorno[0].Equals("135"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + nomearquivo;
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo, NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo);
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString());
                    }
                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }

         
        public String BaixarXML(String CNPJ, String PastaPadrao, String Parametro) {
            String cRetorno = "";
            try
            {
                baixarxml = new frmBaixarNotas();
                baixarxml.env = this;
                baixarxml.CNPJEmitente = CNPJ;
                baixarxml.txtPastaDestino.Text = PastaPadrao;
                baixarxml.Text = "Baixar XMLs - SPED [ " + Parametro + " ] ";
                baixarxml.ShowDialog();

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message + " - " + ex.StackTrace);
                
            }
            return cRetorno;
        
        }
        public String BaixarXML(String cChaves, String CNPJ) 
        {
            String cRetorno = "";
            try
            {
                String ChaveNFE = "";

                String[] clChaves = cChaves.Replace('\n',' ').Trim().Split('\r');
                baixarxml.BARRA.Minimum = 1;
                baixarxml.BARRA.Maximum = clChaves.Length;
                VariavesEstaticas.InterromperDownload = false;
                int nRejeicao = 0;
                for (int i = 0; i < clChaves.Length; i++)
                {
                    if (VariavesEstaticas.InterromperDownload) {
                        MessageBox.Show("DOWNLOAD INTERRROMPIDO PELO USUARIO","BAIXAR XMLs");
                        return "";
                    }
                    baixarxml.BARRA.Value = i+1;
                    ChaveNFE = clChaves[i].Trim() ;
                    Application.DoEvents();
                    baixarxml.txtProcessamento.Text += "PROCESSANDO NOTA : " + ChaveNFE;
                         
                    ConfirmarOperacao(ChaveNFE, CNPJ);
                    
                    String cRetornoDownload  =DownloadNE(ChaveNFE, CNPJ);
                    if (cRetornoDownload != "") {
                        String[] dadosret = cRetornoDownload.Split('|');
                        if (dadosret[0] == "140") {
                            
                            baixarxml.txtProcessamento.Text += " - OK\n";
                            String cNomeArquivo = baixarxml.txtPastaDestino.Text + "\\"+ ChaveNFE + "-nfe.xml" ;
                            baixarxml.txtProcessamento.Text += "----> SALVANDO EM : " + cNomeArquivo + "\n";
                            if (File.Exists(cNomeArquivo)) {
                                File.Delete(cNomeArquivo);
                            }
                            StreamWriter wr = new StreamWriter(@cNomeArquivo, true);
                            wr.Write(dadosret[1]);
                            wr.Close();

                        }else{
                            baixarxml.txtProcessamento.Text += " - " + dadosret[1] + "\n";
                            baixarxml.txtRejeicao.Text += "CHAVE ACESSO : " + ChaveNFE + "\n";
                            baixarxml.txtRejeicao.Text += "---->MOTIVO  : " + dadosret[1] + "\n";
                            nRejeicao++;
                        }
                        //DocumentosEmitidos.Documentos=

                    }

                }
                if (nRejeicao > 0) {
                    MessageBox.Show("EXISTEM NOTAS QUE NÃO FORAM BAIXADAS. \nVERIFIQUE A LISTA DE REJEIÇÃO","BAIXAR XML");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace);

            }
            return cRetorno;

        }
        private String DownloadNE(String cChaveDeAcesso, String CNPJ) {
            try
            {
                String cNomeArquivo = cChaveDeAcesso + "_Download_Nfe.xml";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(VariavesEstaticas.Xml_Download);
                doc.GetElementsByTagName("CNPJ")[0].ChildNodes[0].Value = CNPJ;
                doc.GetElementsByTagName("chNFe")[0].ChildNodes[0].Value = cChaveDeAcesso;

                String strArquivoTemp = @VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo;

                if (File.Exists(strArquivoTemp))
                {
                    File.Delete(strArquivoTemp);
                }

                StreamWriter wr = new StreamWriter(@VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo, true);
                wr.Write(doc.InnerXml);
                wr.Close();

                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLDownloadNFE(doc);
                if (resultado.Trim().Length != 0)
                {
                    File.Copy(strArquivoTemp, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + cNomeArquivo, true);
                    MessageBox.Show("-10|XML Com erro:\n\n" + resultado.ToString());
                    return "";
                }

                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();

                String Retorno = iUtil.EnviarNFeDOwnload(doc);

                return Retorno;
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.Message + " - " + ex.StackTrace);
                 return "";
            }
        }
        private bool ConfirmarOperacao(String cChaveDeAcesso, String CNPJ) {
            try
            {
                 
                String cNomeArquivo = cChaveDeAcesso + "_CNF_Nfe.xml";
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(VariavesEstaticas.Xml_Confirmacao);

                doc.GetElementsByTagName("idLote")[0].ChildNodes[0].Value = "1";
                doc.GetElementsByTagName("infEvento")[0].Attributes["Id"].Value = "ID210200" + cChaveDeAcesso + "01";
                doc.GetElementsByTagName("CNPJ")[0].ChildNodes[0].Value = CNPJ;
                doc.GetElementsByTagName("chNFe")[0].ChildNodes[0].Value = cChaveDeAcesso;
                String cData = DateTime.Now.ToString("s") + "-03:00";
                doc.GetElementsByTagName("dhEvento")[0].ChildNodes[0].Value = cData;
                //doc.Save(VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo);
                String strArquivoTemp = @VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo;

                if (File.Exists(strArquivoTemp)) {
                    File.Delete(strArquivoTemp);
                }

                StreamWriter wr = new StreamWriter(@VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo, true);
                wr.Write(doc.InnerXml);
                wr.Close();

               
               RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
               string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + cNomeArquivo;
               if (File.Exists(strArquivoAssinado)) {
                   File.Delete(strArquivoAssinado);
               }

               ASSD.Assinar(strArquivoTemp, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado, "evento");
               if (ASSD.intResultado != 0)
               {
                   String arquivoXML = @VariavesEstaticas.DiretorioSistema + @"\iNfe\Temp\" + cNomeArquivo;
                   File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + cNomeArquivo, true);
                   MessageBox.Show ( ASSD.intResultado.ToString() + "\n" + ASSD.strResultadoAss);
               }

               XmlDocument xmlAssinado = new XmlDocument();
               xmlAssinado.Load(strArquivoAssinado);

               string resultado = RegraDeNegocios.ValidaXML.ValidarXMLConfirmacaoRectoNFE(xmlAssinado);
               if (resultado.Trim().Length != 0)
               {

                   File.Copy(strArquivoTemp, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + cNomeArquivo, true);
                   MessageBox.Show("-10|XML Com erro:\n\n" + resultado.ToString());
                   return false;
               }
 

               iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();

               String Retorno = iUtil.EnviarNFeConfirmacaoRecebimento(xmlAssinado);
               if (Retorno == "128") { 
                    return true; 
               } 

    
               return false;
           }
           catch (Exception ex)
           {
                
                 MessageBox.Show(ex.Message + " - " + ex.StackTrace);
                 return false;
           }
       }
       public string EnviarConsultaSituacaoDestinario(string StrXml)
       {
           try
           {
               System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
               VerEstruturaPastas();

               String arquivoXML = StrXml;

               frmMenNfe fmen = new frmMenNfe();
               fmen.Show();

               string nomearquivo = Path.GetFileName(arquivoXML);

               //Faz Assinatura do Arquivo
               fmen.setDatos(1);
               fmen.setDatos(2);

               XmlDocument doc = new XmlDocument();

               doc.Load(arquivoXML);
               /*
               RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
               string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
               ASSD.Assinar(arquivoXML, "infEvento", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado, "evento");
               if (ASSD.intResultado != 0)
               {
                   fmen.setDatos(-2);
                   System.Threading.Thread.Sleep(3000);
                   fmen.Close();
                   File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                   return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
               }

               xmlAssinado.Load(strArquivoAssinado);
                * */
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLConsultaDestinario(doc);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }

                /*xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;*/

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeConsultaDestinatario(doc);
                //MessageBox.Show("EnviarNFeCancelamentoEvento : " + resEnv);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                /*
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }
                */
                int tentativas = 1;

                if (retorno[0].Equals("135"))
                {
                    fmen.Close();
                    fmen.Dispose();
                    transmitiu = 1;
                    if (retorno.Length > 2)
                    {
                        string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + nomearquivo;
                        File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo, NFEENVIADA, true);
                        File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo);
                        VariavesEstaticas.UltimaNFe = NFEENVIADA;
                        VariavesEstaticas.Protocolo = retorno[1].ToString();
                        return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString());
                    }
                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }
        public string EnviarNFeCancelamento(string StrXml)
        {
            try
            {
                System.Xml.XmlDocument xmlAssinado = new System.Xml.XmlDocument();
                VerEstruturaPastas();

                String arquivoXML = StrXml;

                frmMenNfe fmen = new frmMenNfe();
                fmen.Show();

                string nomearquivo = Path.GetFileName(arquivoXML);

                //Faz Assinatura do Arquivo
                fmen.setDatos(1);
                fmen.setDatos(2);
                RegraDeNegocios.AssinaturaDigital ASSD = new RegraDeNegocios.AssinaturaDigital();
                string strArquivoAssinado = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + nomearquivo;
                ASSD.Assinar(arquivoXML, "infCanc", RegraDeNegocios.CertificadoDigital.SelecionarCertificado(), strArquivoAssinado);
                if (ASSD.intResultado != 0)
                {
                    fmen.setDatos(-2);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return ASSD.intResultado.ToString() + "|" + ASSD.strResultadoAss;
                }

                xmlAssinado.Load(strArquivoAssinado);
                fmen.setDatos(3);
                string resultado = RegraDeNegocios.ValidaXML.ValidarXMLCancelamento(xmlAssinado);
                if (resultado.Trim().Length != 0)
                {
                    fmen.setDatos(-3);
                    System.Threading.Thread.Sleep(3000);
                    fmen.Close();
                    fmen.Dispose();
                    File.Copy(arquivoXML, VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlComErro\" + nomearquivo, true);
                    return "-10|XML Com erro:\n\n" + resultado.ToString();
                }
               
                xmlAssinado.Save(strArquivoAssinado);
                arquivoXML = strArquivoAssinado;

                int transmitiu = 0;
                iNFeDll.RegraDeNegocios.Utilitarios iUtil = new iNFeDll.RegraDeNegocios.Utilitarios();
                string resEnv = iUtil.EnviarNFeCancelamento(xmlAssinado);
                fmen.setDatos(4);
                string[] retorno = resEnv.Split('|');
                if (retorno[0].Equals("1989"))
                {
                    fmen.setDatos(-4);
                    System.Threading.Thread.Sleep(2000);
                    fmen.Close();
                    fmen.Dispose();
                    return retorno[0] + "|" + retorno[1].ToString();
                }

                int tentativas = 1;
  
                if (retorno[0].Equals("101"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno.Length > 2)
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno[1].ToString();
                            return (retorno[0] + "|" + retorno[1].ToString() + "|" + retorno[2].ToString() + "|" + retorno[3].ToString() + "|" + retorno[4].ToString());
                        }
                }

                if (retorno[0].Equals("103") || retorno[0].Equals("104"))
                {
                tentativas_:
                    fmen.setDatos(5);

                    string resCon = iUtil.IntegraExecutaConsultaRecebico(iUtil.IntegraConsultaRecibo(retorno[1]));

                    string[] retorno_ = resCon.Split('|');
                    if (retorno[0].Equals("1989"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("104") || retorno_[0].Equals("105") && tentativas < 10)
                    {
                        tentativas++;
                        System.Threading.Thread.Sleep(1000);
                        goto tentativas_;
                    }
                    if (retorno_[0].Equals("103"))
                    {
                        fmen.Close();
                        fmen.Dispose();

                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }
                    if (retorno_[0].Equals("100"))
                    {
                        fmen.Close();
                        fmen.Dispose();
                        transmitiu = 1;
                        if (retorno_.Length > 2)
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            VariavesEstaticas.Protocolo = retorno_[1].ToString();
                            return (retorno_[0] + "|" + retorno_[1].ToString() + "|" + retorno_[2].ToString() + "|" + retorno_[3].ToString() + "|" + retorno_[4].ToString());
                        }
                        else
                        {
                            string NFEENVIADA = VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlTransmitidos\" + retorno_[2].ToString() + "-nfe.xml";
                            File.Copy(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml", NFEENVIADA, true);
                            File.Delete(VariavesEstaticas.DiretorioSistema + @"\iNfe\XmlAssinado\" + retorno_[2].ToString() + "-nfe.xml");
                            VariavesEstaticas.UltimaNFe = NFEENVIADA;
                            return (retorno_[0] + "|" + retorno_[1].ToString());
                        }
                   
                        
                    }
                    else
                    {
                        fmen.Close();
                        fmen.Dispose();
                        return (retorno_[0] + "|" + retorno_[1].ToString());
                    }

                }
                else
                {
                    fmen.Close();
                    fmen.Dispose();
                    return (retorno[0] + "|" + retorno[1].ToString());
                }
                if (transmitiu == 1)
                    fmen.setDatos(6);
                else
                    fmen.Close();

                fmen.Close();
                fmen.Dispose();
                return retorno.ToString();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " - " + ex.StackTrace); ;
            }
            return "1989|SEM Exception";
        }
    }
}
