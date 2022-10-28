using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iNFeDll.RegraDeNegocios;

namespace iNFeDll
{
    public partial class frmNotaEncontradas : Form
    {
        public frmNotaEncontradas()
        {
            InitializeComponent();
        }

        private void btInterromper_Click(object sender, EventArgs e)
        {

        }

        private void frmNotaEncontradas_Load(object sender, EventArgs e)
        {
            AtualizarUI();
        }

        public void AtualizarUI(){
            dbgDocumentosLocalizados.DataSource = DocumentosEmitidos.Documentos().Tables[0];
        }

    }
}
