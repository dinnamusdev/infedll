namespace iNFeDll.RegraDeNegocios
{
    partial class frmBaixarNotas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtChavesDeAcesso = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPastaDestino = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btBaixar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtProcessamento = new System.Windows.Forms.RichTextBox();
            this.BARRA = new System.Windows.Forms.ProgressBar();
            this.txtRejeicao = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "INFORME AS CHAVES DE ACESSO";
            // 
            // txtChavesDeAcesso
            // 
            this.txtChavesDeAcesso.Location = new System.Drawing.Point(12, 30);
            this.txtChavesDeAcesso.Multiline = true;
            this.txtChavesDeAcesso.Name = "txtChavesDeAcesso";
            this.txtChavesDeAcesso.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChavesDeAcesso.Size = new System.Drawing.Size(497, 140);
            this.txtChavesDeAcesso.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "PASTA DE DESTINO DOS XML";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPastaDestino
            // 
            this.txtPastaDestino.Location = new System.Drawing.Point(12, 194);
            this.txtPastaDestino.Name = "txtPastaDestino";
            this.txtPastaDestino.ReadOnly = true;
            this.txtPastaDestino.Size = new System.Drawing.Size(596, 20);
            this.txtPastaDestino.TabIndex = 4;
            this.txtPastaDestino.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "ANDAMENTO DO PROCESSO...";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btBaixar
            // 
            this.btBaixar.Location = new System.Drawing.Point(515, 151);
            this.btBaixar.Name = "btBaixar";
            this.btBaixar.Size = new System.Drawing.Size(148, 19);
            this.btBaixar.TabIndex = 6;
            this.btBaixar.Text = "BAIXAR";
            this.btBaixar.UseVisualStyleBackColor = true;
            this.btBaixar.Click += new System.EventHandler(this.btBaixar_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::iNFeDll.Properties.Resources.nfe_;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(515, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 120);
            this.panel1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 20);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtProcessamento
            // 
            this.txtProcessamento.Location = new System.Drawing.Point(12, 238);
            this.txtProcessamento.Name = "txtProcessamento";
            this.txtProcessamento.Size = new System.Drawing.Size(649, 100);
            this.txtProcessamento.TabIndex = 10;
            this.txtProcessamento.Text = "";
            this.txtProcessamento.WordWrap = false;
            // 
            // BARRA
            // 
            this.BARRA.Location = new System.Drawing.Point(220, 216);
            this.BARRA.Name = "BARRA";
            this.BARRA.Size = new System.Drawing.Size(441, 19);
            this.BARRA.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.BARRA.TabIndex = 11;
            // 
            // txtRejeicao
            // 
            this.txtRejeicao.Location = new System.Drawing.Point(12, 360);
            this.txtRejeicao.Name = "txtRejeicao";
            this.txtRejeicao.Size = new System.Drawing.Size(649, 109);
            this.txtRejeicao.TabIndex = 12;
            this.txtRejeicao.Text = "";
            this.txtRejeicao.WordWrap = false;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(12, 339);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(224, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "CHAVES DE ACESSO REJEITADAS";
            // 
            // frmBaixarNotas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 481);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRejeicao);
            this.Controls.Add(this.BARRA);
            this.Controls.Add(this.txtProcessamento);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btBaixar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPastaDestino);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtChavesDeAcesso);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBaixarNotas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baixar XMLs";
            this.Load += new System.EventHandler(this.frmBaixarNotas_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChavesDeAcesso;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btBaixar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.TextBox txtPastaDestino;
        public System.Windows.Forms.RichTextBox txtProcessamento;
        public System.Windows.Forms.ProgressBar BARRA;
        public System.Windows.Forms.RichTextBox txtRejeicao;
        private System.Windows.Forms.Label label4;
    }
}