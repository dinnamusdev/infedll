using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Runtime.InteropServices;

namespace iNFeDll.RegraDeNegocios
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("AssinaturaDigital")]
    [ComVisible(true)]
    public class AssinaturaDigital
    {


        public int intResultado { get; private set; }
        public string strResultadoAss { get; private set; }
        public string vXMLStringAssinado { get; private set; }

        private XmlDocument XMLDoc;

        public void Assinar(string strArqXMLAssinar, string strUri, X509Certificate2 x509Certificado, string strArqXMLAssinado) {
             Assinar(strArqXMLAssinar, strUri, x509Certificado, strArqXMLAssinado,"");
        }
        public void Assinar(string strArqXMLAssinar, string strUri, X509Certificate2 x509Certificado, string strArqXMLAssinado,string TagPaiDaAssinatura)
        {
            //Atualizar atributos de retorno com conteúdo padrão
            this.intResultado = 0;
            this.strResultadoAss = "XML Assinado Com Sucesso!";
            StreamReader SR = null;

            try
            {
                //Abrir o arquivo XML a ser assinado e ler o seu conteúdo
                SR = File.OpenText(strArqXMLAssinar);
                string vXMLString = SR.ReadToEnd();
                SR.Close();

                try
                {
                    // Verifica o certificado a ser utilizado na assinatura
                    string _xnome = "";
                    if (x509Certificado != null)
                    {
                        _xnome = x509Certificado.Subject.ToString();
                    }

                    X509Certificate2 _X509Cert = new X509Certificate2();
                    X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                    X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, _xnome, false);

                    if (collection1.Count == 0)
                    {
                        this.strResultadoAss = "O Integra detectou problemas com o certificado digital. (Código do Erro: 2)";
                        this.intResultado = 2;
                        return;
                    }
                    else
                    {
                        // certificado ok
                        _X509Cert = collection1[0];
                        string x;
                        x = _X509Cert.GetKeyAlgorithm().ToString();

                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = false;

                        try
                        {
                            doc.LoadXml(vXMLString);

                            // Verifica se a tag a ser assinada existe é única
                            int qtdeRefUri = doc.GetElementsByTagName(strUri).Count;

                            if (qtdeRefUri == 0)
                            {
                                // a URI indicada não existe
                                this.strResultadoAss = "A tag de assinatura " + strUri.Trim() + " não existe no XML. (Código do Erro: 4)";
                                this.intResultado = 4;
                                return;
                            }
                            // Exsiste mais de uma tag a ser assinada
                            else
                            {
                                if (qtdeRefUri > 1)
                                {
                                    // existe mais de uma URI indicada
                                    this.strResultadoAss = "A tag de assinatura " + strUri.Trim() + " não é unica. (Código do Erro: 5)";
                                    this.intResultado = 5;
                                    return;
                                }
                                else 
                                {
                                    if (doc.GetElementsByTagName("Signature").Count == 0) //Documento ainda não assinado (Se já tiver assinado não vamos fazer nada, somente retornar ok para continuar o envio). Wandrey 12/05/2009
                                    {
                                        try
                                        {
                     
                                            SignedXml signedXml = new SignedXml(doc);

                         
                                            signedXml.SigningKey = _X509Cert.PrivateKey;

                                            Reference reference = new Reference();

                                            // pega o uri que deve ser assinada
                                            XmlAttributeCollection _Uri = doc.GetElementsByTagName(strUri).Item(0).Attributes;
                                            foreach (XmlAttribute _atributo in _Uri)
                                            {
                                                if (_atributo.Name == "Id")
                                                {
                                                    reference.Uri = "#" + _atributo.InnerText;
                                                }
                                            }

                         
                                            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                                            reference.AddTransform(env);

                                            XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                                            reference.AddTransform(c14);

                                            signedXml.AddReference(reference);

                                            KeyInfo keyInfo = new KeyInfo();

                                            keyInfo.AddClause(new KeyInfoX509Data(_X509Cert));

               
                                            signedXml.KeyInfo = keyInfo;
                                            signedXml.ComputeSignature();

 
                                            XmlElement xmlDigitalSignature = signedXml.GetXml();

                                            // Gravar o elemento no documento XML
                                            if (TagPaiDaAssinatura == "")
                                            {
                                                doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                            }
                                            else {
                                                doc.DocumentElement["evento"].AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                            }
                                            XMLDoc = new XmlDocument();
                                            XMLDoc.PreserveWhitespace = false;
                                            XMLDoc = doc;

                                            // Atualizar a string do XML já assinada
                                            this.vXMLStringAssinado = XMLDoc.OuterXml;

                                            // Gravar o XML Assinado no HD
                                            StreamWriter SW_2 = File.CreateText(strArqXMLAssinado);
                                            SW_2.Write(this.vXMLStringAssinado);
                                            SW_2.Close();
                                        }
                                        catch (Exception caught)
                                        {
                                            if (intResultado == 0)
                                            {
                                                this.intResultado = 3;
                                                this.strResultadoAss = caught.Message;
                                                return;
                                            }
                              
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception caught)
                        {
                            if (intResultado == 0)
                            {
                                this.intResultado = 6;
                                this.strResultadoAss = caught.Message;
                                return;
                            }
                           
                        }
                    }
                }
                catch (Exception caught)
                {
                    if (intResultado == 0)
                    {
                        this.intResultado = 1;
                        this.strResultadoAss = caught.Message;
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                if (intResultado == 0)
                {
                    this.intResultado = 6;
                    this.strResultadoAss = ex.Message;
                    return;
                }

            }
            finally
            {
                SR.Close();
            }
        }

        /// <summary>
        /// Assina o XML sobrepondo-o
        /// </summary>
        /// <param name="strArqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="strUri">URI (TAG) a ser assinada</param>
        /// <param name="x509Certificado">Certificado a ser utilizado na assinatura</param>
        /// <remarks>Veja anotações na sobrecarga deste método</remarks>
        /// <by>Rafael Almeida Santos</by>
        /// <date>22/03/2011</date>
        public void Assinar(string strArqXMLAssinar, string strUri, X509Certificate2 x509Certificado)
        {
            try
            {
                this.Assinar(strArqXMLAssinar, strUri, x509Certificado, strArqXMLAssinar);
            }
            catch (Exception ex)
            {
                this.intResultado = 99;
                this.strResultadoAss = ex.Message;
                return;
            }
        }
    }
}
