namespace iNFeDll
{
    partial class frmNotaEncontradas
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
            this.dbgDocumentosLocalizados = new System.Windows.Forms.DataGridView();
            this.txtProcessamento = new System.Windows.Forms.Label();
            this.btInterromper = new System.Windows.Forms.Button();
            this.CNPJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dEmi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vNF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dhRecbto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSitNFe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chNFe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dbgDocumentosLocalizados)).BeginInit();
            this.SuspendLayout();
            // 
            // dbgDocumentosLocalizados
            // 
            this.dbgDocumentosLocalizados.AllowUserToAddRows = false;
            this.dbgDocumentosLocalizados.AllowUserToDeleteRows = false;
            this.dbgDocumentosLocalizados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbgDocumentosLocalizados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CNPJ,
            this.xNome,
            this.dEmi,
            this.vNF,
            this.dhRecbto,
            this.cSitNFe,
            this.chNFe});
            this.dbgDocumentosLocalizados.Location = new System.Drawing.Point(3, 34);
            this.dbgDocumentosLocalizados.Name = "dbgDocumentosLocalizados";
            this.dbgDocumentosLocalizados.ReadOnly = true;
            this.dbgDocumentosLocalizados.Size = new System.Drawing.Size(606, 275);
            this.dbgDocumentosLocalizados.TabIndex = 0;
            // 
            // txtProcessamento
            // 
            this.txtProcessamento.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtProcessamento.Location = new System.Drawing.Point(3, 8);
            this.txtProcessamento.Name = "txtProcessamento";
            this.txtProcessamento.Size = new System.Drawing.Size(436, 23);
            this.txtProcessamento.TabIndex = 1;
            // 
            // btInterromper
            // 
            this.btInterromper.Location = new System.Drawing.Point(445, 8);
            this.btInterromper.Name = "btInterromper";
            this.btInterromper.Size = new System.Drawing.Size(164, 23);
            this.btInterromper.TabIndex = 2;
            this.btInterromper.Text = "Interromper Pesquisa";
            this.btInterromper.UseVisualStyleBackColor = true;
            this.btInterromper.Click += new System.EventHandler(this.btInterromper_Click);
            // 
            // CNPJ
            // 
            this.CNPJ.DataPropertyName = "CNPJ";
            this.CNPJ.HeaderText = "CNPJ";
            this.CNPJ.Name = "CNPJ";
            this.CNPJ.ReadOnly = true;
            // 
            // xNome
            // 
            this.xNome.DataPropertyName = "xNome";
            this.xNome.HeaderText = "Nome Emit.";
            this.xNome.Name = "xNome";
            this.xNome.ReadOnly = true;
            // 
            // dEmi
            // 
            this.dEmi.DataPropertyName = "dEmi";
            this.dEmi.HeaderText = "Emissão";
            this.dEmi.Name = "dEmi";
            this.dEmi.ReadOnly = true;
            // 
            // vNF
            // 
            this.vNF.DataPropertyName = "vNF";
            this.vNF.HeaderText = "Num.NF";
            this.vNF.Name = "vNF";
            this.vNF.ReadOnly = true;
            // 
            // dhRecbto
            // 
            this.dhRecbto.DataPropertyName = "dhRecbto";
            this.dhRecbto.HeaderText = "Data Autorização";
            this.dhRecbto.Name = "dhRecbto";
            this.dhRecbto.ReadOnly = true;
            // 
            // cSitNFe
            // 
            this.cSitNFe.DataPropertyName = "cSitNFe";
            this.cSitNFe.HeaderText = "Situação";
            this.cSitNFe.Name = "cSitNFe";
            this.cSitNFe.ReadOnly = true;
            // 
            // chNFe
            // 
            this.chNFe.DataPropertyName = "chNFe";
            this.chNFe.HeaderText = "Chave";
            this.chNFe.Name = "chNFe";
            this.chNFe.ReadOnly = true;
            // 
            // frmNotaEncontradas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 331);
            this.Controls.Add(this.btInterromper);
            this.Controls.Add(this.txtProcessamento);
            this.Controls.Add(this.dbgDocumentosLocalizados);
            this.Name = "frmNotaEncontradas";
            this.Text = "Localizando Documentos Servidor Receita";
            this.Load += new System.EventHandler(this.frmNotaEncontradas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dbgDocumentosLocalizados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dbgDocumentosLocalizados;
        private System.Windows.Forms.Button btInterromper;
        public System.Windows.Forms.Label txtProcessamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn CNPJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn xNome;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEmi;
        private System.Windows.Forms.DataGridViewTextBoxColumn vNF;
        private System.Windows.Forms.DataGridViewTextBoxColumn dhRecbto;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSitNFe;
        private System.Windows.Forms.DataGridViewTextBoxColumn chNFe;
    }
}