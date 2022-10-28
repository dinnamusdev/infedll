using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iNFeDll.RegraDeNegocios
{
    public class clsAuxiliar
    {
    }
    public class URLws
    {
        public string NFeRecepcao { get; set; }
        public string NFeRetRecepcao { get; set; }
        public string NFeCancelamento { get; set; }
        public string NFeInutilizacao { get; set; }
        public string NFeConsulta { get; set; }
        public string NFeStatusServico { get; set; }
        public string NFeConsultaCadastro { get; set; }
    }

    public class webServices
    {
        public int ID { get; private set; }
        public string Nome { get; private set; }
        public string UF { get; private set; }
        public URLws URLHomologacao { get; private set; }
        public URLws URLProducao { get; private set; }

        public webServices(int id, string nome, string uf)
        {
            URLHomologacao = new URLws();
            URLProducao = new URLws();
            ID = id;
            Nome = nome;
            UF = uf;
        }
    }

    public class TipoEmissao
    {
        public const int teNormal = 1;
        public const int teContingencia = 2;
        public const int teSCAN = 3;
        public const int teDPEC = 4;
        public const int teFSDA = 5;
    }


    #region Classe dos tipos de ambiente da NFe
    /// <summary>
    /// Tipos de ambientes da NFe - danasa 8-2009
    /// </summary>
    public class TipoAmbiente
    {
        public const int taProducao = 1;
        public const int taHomologacao = 2;
    }
    #endregion

    #region Classe dos Parmâmetros necessários para o envio dos XML´s
    /// <summary>
    /// Parâmetros necessários para o envio dos XML´s
    /// </summary>
    public class ParametroEnvioXML
    {
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public int UFCod { get; set; }
    }
#endregion
}
