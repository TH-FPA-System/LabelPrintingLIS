namespace ZebraPrinterGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblCom = new Label();
            cboComPort = new ComboBox();
            lblQty = new Label();
            txtQty = new TextBox();
            btnPrint = new Button();
            txtZPL = new TextBox();
            comboModels = new ComboBox();
            label1 = new Label();
            PartNoTxt = new TextBox();
            label2 = new Label();
            label3 = new Label();
            DescTxt = new TextBox();
            btnAddModel = new Button();
            btnRemoveModel = new Button();
            SuspendLayout();
            // 
            // lblCom
            // 
            lblCom.AutoSize = true;
            lblCom.Location = new Point(20, 20);
            lblCom.Name = "lblCom";
            lblCom.Size = new Size(63, 15);
            lblCom.TabIndex = 0;
            lblCom.Text = "COM Port:";
            // 
            // cboComPort
            // 
            cboComPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cboComPort.FormattingEnabled = true;
            cboComPort.Location = new Point(89, 17);
            cboComPort.Name = "cboComPort";
            cboComPort.Size = new Size(88, 23);
            cboComPort.TabIndex = 1;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Location = new Point(102, 248);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(102, 15);
            lblQty.TabIndex = 2;
            lblQty.Text = "Quantity Per Row:";
            // 
            // txtQty
            // 
            txtQty.Location = new Point(210, 245);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(55, 23);
            txtQty.TabIndex = 3;
            // 
            // btnPrint
            // 
            btnPrint.Location = new Point(271, 238);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(99, 35);
            btnPrint.TabIndex = 4;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // txtZPL
            // 
            txtZPL.Location = new Point(20, 279);
            txtZPL.Multiline = true;
            txtZPL.Name = "txtZPL";
            txtZPL.Size = new Size(350, 196);
            txtZPL.TabIndex = 5;
            // 
            // comboModels
            // 
            comboModels.DropDownStyle = ComboBoxStyle.DropDownList;
            comboModels.FormattingEnabled = true;
            comboModels.Location = new Point(242, 17);
            comboModels.Name = "comboModels";
            comboModels.Size = new Size(117, 23);
            comboModels.TabIndex = 6;
            comboModels.SelectedIndexChanged += comboModels_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(192, 20);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 7;
            label1.Text = "Model:";
            label1.Click += label1_Click;
            // 
            // btnAddModel
            // 
            btnAddModel.Location = new Point(242, 46);
            btnAddModel.Name = "btnAddModel";
            btnAddModel.Size = new Size(55, 23);
            btnAddModel.TabIndex = 12;
            btnAddModel.Text = "+ Add";
            btnAddModel.UseVisualStyleBackColor = true;
            btnAddModel.Click += btnAddModel_Click;
            // 
            // btnRemoveModel
            // 
            btnRemoveModel.Location = new Point(304, 46);
            btnRemoveModel.Name = "btnRemoveModel";
            btnRemoveModel.Size = new Size(55, 23);
            btnRemoveModel.TabIndex = 13;
            btnRemoveModel.Text = "- Remove";
            btnRemoveModel.UseVisualStyleBackColor = true;
            btnRemoveModel.Click += btnRemoveModel_Click;
            // 
            // PartNoTxt
            // 
            PartNoTxt.Location = new Point(89, 85);
            PartNoTxt.MaxLength = 12;
            PartNoTxt.Name = "PartNoTxt";
            PartNoTxt.Size = new Size(88, 23);
            PartNoTxt.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 88);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 8;
            label2.Text = "PartNo:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 140);
            label3.Name = "label3";
            label3.Size = new Size(111, 15);
            label3.TabIndex = 10;
            label3.Text = "Desc (Max 25 Char):";
            // 
            // DescTxt
            // 
            DescTxt.Location = new Point(20, 158);
            DescTxt.MaxLength = 25;
            DescTxt.Multiline = true;
            DescTxt.Name = "DescTxt";
            DescTxt.Size = new Size(350, 63);
            DescTxt.TabIndex = 11;
            DescTxt.TextChanged += DescTxt_TextChanged;
            // 
            // Form1
            // 
            ClientSize = new Size(391, 499);
            Controls.Add(btnAddModel);
            Controls.Add(btnRemoveModel);
            Controls.Add(DescTxt);
            Controls.Add(label3);
            Controls.Add(PartNoTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboModels);
            Controls.Add(txtZPL);
            Controls.Add(btnPrint);
            Controls.Add(txtQty);
            Controls.Add(lblQty);
            Controls.Add(cboComPort);
            Controls.Add(lblCom);
            Name = "Form1";
            Text = "Zebra Printer GUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCom;
        private System.Windows.Forms.ComboBox cboComPort;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtZPL;
        private ComboBox comboModels;
        private Label label1;
        private TextBox PartNoTxt;
        private Label label2;
        private Label label3;
        private TextBox DescTxt;
        private Button btnAddModel;
        private Button btnRemoveModel;
    }
}