
namespace Tools_Injector_Mod_Menu
{
    partial class FrmAddFunction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddFunction));
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboFunction = new MaterialSkin.Controls.MaterialComboBox();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.panelFunction = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Roboto", 20F);
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(12, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(190, 33);
            this.label14.TabIndex = 12;
            this.label14.Text = "Function Type:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 64);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.comboFunction);
            this.panel2.Location = new System.Drawing.Point(208, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(577, 35);
            this.panel2.TabIndex = 0;
            // 
            // comboFunction
            // 
            this.comboFunction.AutoResize = false;
            this.comboFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.comboFunction.Depth = 0;
            this.comboFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboFunction.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboFunction.DropDownHeight = 437;
            this.comboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFunction.DropDownWidth = 121;
            this.comboFunction.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.comboFunction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFunction.FormattingEnabled = true;
            this.comboFunction.IntegralHeight = false;
            this.comboFunction.ItemHeight = 29;
            this.comboFunction.Items.AddRange(new object[] {
            "Category",
            "Hook: Button",
            "Hook: Button On/Off & Toggle",
            "Hook: Input",
            "Patch"});
            this.comboFunction.Location = new System.Drawing.Point(0, 0);
            this.comboFunction.MaxDropDownItems = 15;
            this.comboFunction.MouseState = MaterialSkin.MouseState.OUT;
            this.comboFunction.Name = "comboFunction";
            this.comboFunction.Size = new System.Drawing.Size(577, 35);
            this.comboFunction.StartIndex = 0;
            this.comboFunction.TabIndex = 13;
            this.comboFunction.UseTallSize = false;
            this.comboFunction.SelectedIndexChanged += new System.EventHandler(this.comboFunction_SelectedIndexChanged);
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.panelFunction);
            this.materialCard2.Depth = 0;
            this.materialCard2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(0, 64);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(799, 452);
            this.materialCard2.TabIndex = 4;
            // 
            // panelFunction
            // 
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFunction.Location = new System.Drawing.Point(14, 14);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(771, 424);
            this.panelFunction.TabIndex = 3;
            // 
            // FrmAddFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.ClientSize = new System.Drawing.Size(799, 516);
            this.Controls.Add(this.materialCard2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddFunction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Category";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.materialCard2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelFunction;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialComboBox comboFunction;
        private System.Windows.Forms.Panel panel2;
    }
}