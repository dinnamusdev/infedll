using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iNFeDll.Integra_ERP;
using System.IO;

namespace iNFeDll.RegraDeNegocios
{
    public partial class frmBaixarNotas : Form
    {
        public EnvioRecebimento env = null;
        public String CNPJEmitente = "";
        public String PastaPadrao = "";

        public frmBaixarNotas()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                    txtPastaDestino.Text = folderBrowserDialog1.SelectedPath;
                }

            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btBaixar_Click(object sender, EventArgs e)
        {
            if (btBaixar.Text != "Interromper")
            {
                String[] clChaves = txtChavesDeAcesso.Text.Trim().Replace('\n', ' ').Trim().Split('\r');
                if (clChaves[0] =="")
                {
                    MessageBox.Show("INFORME AS CHAVES DE ACESSO", "BAIXAR XMLs");
                    return;

                }
                if (txtPastaDestino.Text == "")
                {
                    MessageBox.Show("Informa a pasta destino para a gravação dos xml", "BAIXAR XMLs");
                    return;
                }


                if (!Directory.Exists(txtPastaDestino.Text)) {
                    Directory.CreateDirectory(txtPastaDestino.Text);
                }
                if (MessageBox.Show("Confirma o download das nfe informadas?", "Download NFE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    btBaixar.Text = "Interromper";
                    txtProcessamento.Text = "";
                    env.BaixarXML(txtChavesDeAcesso.Text, CNPJEmitente);
                    btBaixar.Text = "Baixar";
                    MessageBox.Show("PROCEDIMENTO CONCLUIDO");
                }
            }
            else {
                VariavesEstaticas.InterromperDownload = true;
            }
            
        }

        private void frmBaixarNotas_Load(object sender, EventArgs e)
        {
            IniciarUI();
        }
        public void IniciarUI() {
            try
            {
                //dbgDocumentosLocalizados.DataSource = DocumentosEmitidos.Documentos().Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);

            }
        }
    }
}
