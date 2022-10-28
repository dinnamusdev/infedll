using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iNFeDll
{
    public partial class frmMenNfe : Form
    {
        public frmMenNfe()
        {
            InitializeComponent();
        }

        private void frmMenNfe_Load(object sender, EventArgs e)
        {
            if (Integra_ERP.VariavesEstaticas.UF == "PA")
            {
                lblEmpresa.Text = "Power By Dinnamus Automação";
            }
            foreach (Control p in this.Controls)
            {
                if (p is PictureBox)
                {
                    if (p.Name != "pcLogo")
                        p.Visible = false;
                }
            }
            
        }
        private void setOb(Label l, PictureBox p)
        {
            l.Visible = true;
            p.Visible = true;
            this.Refresh();
        }

        public void setDatos(int x)
        {
            switch (x)
            {
                case 1: setOb(lblGerando, pictureBox1); progressBar1.Value += 10; Application.DoEvents(); break;
                case 2: setOb(lblAssinando, pictureBox2); progressBar1.Value += 10; Application.DoEvents(); break;
                case 3: setOb(lblValidando, pictureBox3); progressBar1.Value += 10; Application.DoEvents(); break;
                case 4: setOb(lblTransmitindo, pictureBox4); progressBar1.Value += 10; Application.DoEvents(); break;
                case 5: setOb(lblrecebendo, pictureBox5); progressBar1.Value += 10; Application.DoEvents(); break;
                case 6: setOb(lblGerandoDANFE, pictureBox7); progressBar1.Value += 10; Application.DoEvents(); break;
                case -1: setOb(lblGerando, pc1); break;
                case -2: setOb(lblAssinando, pc2); break;
                case -3: setOb(lblValidando, pc3); break;
                case -4: setOb(lblTransmitindo, pc4); break;
                case -5: setOb(lblrecebendo, pc5); break;
                case -6: setOb(lblGerandoDANFE, pc6); break;
            }
        }
    }
}
