using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace iNFeDll.RegraDeNegocios
{
    class DocumentosEmitidos
    {
        public static bool Interromper = false;
        private static DataSet ds = new DataSet();
        private static bool Iniciar() {

            try
            {
                if (ds.Tables.Count == 0)
                {
                    ds.Tables.Add();
                    ds.Tables[0].Columns.Add("CNPJ");
                    ds.Tables[0].Columns.Add("xNome");
                    ds.Tables[0].Columns.Add("dEmi");
                    ds.Tables[0].Columns.Add("vNF");
                    ds.Tables[0].Columns.Add("dhRecbto");
                    ds.Tables[0].Columns.Add("cSitNFe");
                    ds.Tables[0].Columns.Add("chNFe");
                    ds.Tables[0].Columns.Add("S");
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static DataSet Documentos() {
            Iniciar();
            return ds;
        }
       
    }
}
