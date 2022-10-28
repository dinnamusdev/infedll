using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iNFeDll.Integra_ERP
{
    public enum TWebServico
    {
        NfeRecepcao,
        NfeRetRecepcao,
        [XmlEnumAttribute("CANCELAR")]
        NfeCancelamento,
        [XmlEnumAttribute("INUTILIZAR")]
        NfeInutilizacao,
        [XmlEnumAttribute("CONSULTAR")]
        NfeConsulta,
        [XmlEnumAttribute("STATUS")]
        NfeStatusServico
    }
}
