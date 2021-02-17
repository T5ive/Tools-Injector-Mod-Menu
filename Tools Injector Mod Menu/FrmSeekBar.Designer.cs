
namespace Tools_Injector_Mod_Menu
{
    partial class FrmSeekBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSeekBar));
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.btnClose = new MaterialSkin.Controls.MaterialButton();
            this.chkField = new MaterialSkin.Controls.MaterialCheckbox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOffset = new MaterialSkin.Controls.MaterialTextBox();
            this.comboType = new MaterialSkin.Controls.MaterialComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.materialCard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
            this.SuspendLayout();
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.btnSave);
            this.materialCard1.Controls.Add(this.btnClose);
            this.materialCard1.Controls.Add(this.chkField);
            this.materialCard1.Controls.Add(this.label3);
            this.materialCard1.Controls.Add(this.txtOffset);
            this.materialCard1.Controls.Add(this.comboType);
            this.materialCard1.Controls.Add(this.label2);
            this.materialCard1.Controls.Add(this.numMax);
            this.materialCard1.Controls.Add(this.numMin);
            this.materialCard1.Controls.Add(this.label1);
            this.materialCard1.Controls.Add(this.label10);
            this.materialCard1.Depth = 0;
            this.materialCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(0, 0);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(239, 267);
            this.materialCard1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.DrawShadows = true;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(13, 214);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Depth = 0;
            this.btnClose.DrawShadows = true;
            this.btnClose.HighEmphasis = true;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(126, 214);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Cancel";
            this.btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnClose.UseAccentColor = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkField
            // 
            this.chkField.AutoSize = true;
            this.chkField.Depth = 0;
            this.chkField.Location = new System.Drawing.Point(9, 85);
            this.chkField.Margin = new System.Windows.Forms.Padding(0);
            this.chkField.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkField.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkField.Name = "chkField";
            this.chkField.Ripple = true;
            this.chkField.Size = new System.Drawing.Size(69, 37);
            this.chkField.TabIndex = 26;
            this.chkField.Text = "Field";
            this.chkField.UseVisualStyleBackColor = true;
            this.chkField.CheckedChanged += new System.EventHandler(this.chkField_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(16, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 18);
            this.label3.TabIndex = 21;
            this.label3.Text = "Field Offset";
            // 
            // txtOffset
            // 
            this.txtOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOffset.Depth = 0;
            this.txtOffset.Enabled = false;
            this.txtOffset.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtOffset.Hint = "0x10";
            this.txtOffset.Location = new System.Drawing.Point(126, 166);
            this.txtOffset.MaxLength = 10;
            this.txtOffset.MouseState = MaterialSkin.MouseState.OUT;
            this.txtOffset.Multiline = false;
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(100, 36);
            this.txtOffset.TabIndex = 20;
            this.txtOffset.Text = "";
            this.txtOffset.UseTallSize = false;
            // 
            // comboType
            // 
            this.comboType.AutoResize = false;
            this.comboType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboType.Depth = 0;
            this.comboType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboType.DropDownHeight = 118;
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.DropDownWidth = 121;
            this.comboType.Enabled = false;
            this.comboType.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IntegralHeight = false;
            this.comboType.ItemHeight = 29;
            this.comboType.Items.AddRange(new object[] {
            "int",
            "long",
            "float",
            "double"});
            this.comboType.Location = new System.Drawing.Point(80, 125);
            this.comboType.MaxDropDownItems = 4;
            this.comboType.MouseState = MaterialSkin.MouseState.OUT;
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(147, 35);
            this.comboType.StartIndex = 0;
            this.comboType.TabIndex = 19;
            this.comboType.UseTallSize = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(16, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Type";
            // 
            // numMax
            // 
            this.numMax.Font = new System.Drawing.Font("Roboto", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numMax.Location = new System.Drawing.Point(73, 50);
            this.numMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(154, 32);
            this.numMax.TabIndex = 7;
            this.numMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMax.ValueChanged += new System.EventHandler(this.NumValueChanged);
            // 
            // numMin
            // 
            this.numMin.Font = new System.Drawing.Font("Roboto", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numMin.Location = new System.Drawing.Point(73, 12);
            this.numMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(154, 32);
            this.numMin.TabIndex = 6;
            this.numMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMin.ValueChanged += new System.EventHandler(this.NumValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(17, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Max";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(17, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 18);
            this.label10.TabIndex = 3;
            this.label10.Text = "Min";
            // 
            // FrmSeekBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 267);
            this.Controls.Add(this.materialCard1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSeekBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SeekBar";
            this.TopMost = true;
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.Label label2;
        private MaterialSkin.Controls.MaterialComboBox comboType;
        private System.Windows.Forms.Label label3;
        private MaterialSkin.Controls.MaterialTextBox txtOffset;
        private MaterialSkin.Controls.MaterialCheckbox chkField;
    }
}