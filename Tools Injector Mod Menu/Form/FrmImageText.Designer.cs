namespace Tools_Injector_Mod_Menu
{
    partial class FrmImageText
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
            this.txtImg = new System.Windows.Forms.RichTextBox();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.btnClose = new MaterialSkin.Controls.MaterialButton();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.lbImageEncoder = new System.Windows.Forms.LinkLabel();
            this.lbImgCompress = new System.Windows.Forms.LinkLabel();
            this.materialCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtImg
            // 
            this.txtImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtImg.Location = new System.Drawing.Point(14, 14);
            this.txtImg.Name = "txtImg";
            this.txtImg.Size = new System.Drawing.Size(506, 438);
            this.txtImg.TabIndex = 18;
            this.txtImg.Text = "";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.lbImgCompress);
            this.materialCard1.Controls.Add(this.lbImageEncoder);
            this.materialCard1.Controls.Add(this.btnClose);
            this.materialCard1.Controls.Add(this.btnSave);
            this.materialCard1.Controls.Add(this.txtImg);
            this.materialCard1.Depth = 0;
            this.materialCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(0, 0);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(534, 511);
            this.materialCard1.TabIndex = 19;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Depth = 0;
            this.btnClose.DrawShadows = true;
            this.btnClose.HighEmphasis = true;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(421, 461);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Cancel";
            this.btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnClose.UseAccentColor = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.DrawShadows = true;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(313, 461);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbImageEncoder
            // 
            this.lbImageEncoder.AutoSize = true;
            this.lbImageEncoder.Location = new System.Drawing.Point(154, 472);
            this.lbImageEncoder.Name = "lbImageEncoder";
            this.lbImageEncoder.Size = new System.Drawing.Size(79, 13);
            this.lbImageEncoder.TabIndex = 21;
            this.lbImageEncoder.TabStop = true;
            this.lbImageEncoder.Text = "Image Encoder";
            this.lbImageEncoder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbImageEncoder_LinkClicked);
            // 
            // lbImgCompress
            // 
            this.lbImgCompress.AutoSize = true;
            this.lbImgCompress.Location = new System.Drawing.Point(69, 472);
            this.lbImgCompress.Name = "lbImgCompress";
            this.lbImgCompress.Size = new System.Drawing.Size(79, 13);
            this.lbImgCompress.TabIndex = 22;
            this.lbImgCompress.TabStop = true;
            this.lbImgCompress.Text = "Compress PNG";
            this.lbImgCompress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbImgCompress_LinkClicked);
            // 
            // FrmImageText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.materialCard1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmImageText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Code";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmImageText_Load);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtImg;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private System.Windows.Forms.LinkLabel lbImageEncoder;
        private System.Windows.Forms.LinkLabel lbImgCompress;
    }
}