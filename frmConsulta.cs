using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iNFeDll.RegraDeNegocios;
namespace iNFeDll
{
    public partial class frmConsulta : Form
    {
        public string ChaveDeAcesso="";
        public frmConsulta()
        {
            InitializeComponent();
        }

        private void frmConsulta_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
              
    }
}
