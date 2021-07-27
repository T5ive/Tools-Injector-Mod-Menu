
namespace Tools_Injector_Mod_Menu
{
    partial class CtrlPatch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.dataList = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radButtonOnOff = new MaterialSkin.Controls.MaterialRadioButton();
            this.radToggle = new MaterialSkin.Controls.MaterialRadioButton();
            this.radLabel = new MaterialSkin.Controls.MaterialRadioButton();
            this.txtNameCheat = new MaterialSkin.Controls.MaterialTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.materialCard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.dataList);
            this.materialCard1.Controls.Add(this.groupBox1);
            this.materialCard1.Controls.Add(this.btnSave);
            this.materialCard1.Depth = 0;
            this.materialCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(0, 0);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Size = new System.Drawing.Size(771, 424);
            this.materialCard1.TabIndex = 0;
            // 
            // dataList
            // 
            this.dataList.AllowUserToAddRows = false;
            this.dataList.AllowUserToDeleteRows = false;
            this.dataList.AllowUserToResizeColumns = false;
            this.dataList.AllowUserToResizeRows = false;
            this.dataList.BackgroundColor = System.Drawing.Color.White;
            this.dataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colOffset,
            this.colHex});
            this.dataList.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataList.Location = new System.Drawing.Point(0, 0);
            this.dataList.MultiSelect = false;
            this.dataList.Name = "dataList";
            this.dataList.RowHeadersVisible = false;
            this.dataList.Size = new System.Drawing.Size(771, 315);
            this.dataList.TabIndex = 1;
            this.dataList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataList_KeyDown);
            this.dataList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataList_MouseDown);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name (notice)";
            this.colName.Name = "colName";
            this.colName.Width = 120;
            // 
            // colOffset
            // 
            this.colOffset.HeaderText = "Offset";
            this.colOffset.Name = "colOffset";
            // 
            // colHex
            // 
            this.colHex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHex.HeaderText = "Hex";
            this.colHex.Name = "colHex";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 48);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radButtonOnOff);
            this.groupBox1.Controls.Add(this.radToggle);
            this.groupBox1.Controls.Add(this.radLabel);
            this.groupBox1.Controls.Add(this.txtNameCheat);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(771, 73);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(386, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "Patch Type:";
            // 
            // radButtonOnOff
            // 
            this.radButtonOnOff.AutoSize = true;
            this.radButtonOnOff.Checked = true;
            this.radButtonOnOff.Depth = 0;
            this.radButtonOnOff.Location = new System.Drawing.Point(474, 30);
            this.radButtonOnOff.Margin = new System.Windows.Forms.Padding(0);
            this.radButtonOnOff.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radButtonOnOff.MouseState = MaterialSkin.MouseState.HOVER;
            this.radButtonOnOff.Name = "radButtonOnOff";
            this.radButtonOnOff.Ripple = true;
            this.radButtonOnOff.Size = new System.Drawing.Size(136, 37);
            this.radButtonOnOff.TabIndex = 18;
            this.radButtonOnOff.TabStop = true;
            this.radButtonOnOff.Text = "Button On/Off";
            this.radButtonOnOff.UseVisualStyleBackColor = true;
            this.radButtonOnOff.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radToggle
            // 
            this.radToggle.AutoSize = true;
            this.radToggle.Depth = 0;
            this.radToggle.Location = new System.Drawing.Point(610, 30);
            this.radToggle.Margin = new System.Windows.Forms.Padding(0);
            this.radToggle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radToggle.MouseState = MaterialSkin.MouseState.HOVER;
            this.radToggle.Name = "radToggle";
            this.radToggle.Ripple = true;
            this.radToggle.Size = new System.Drawing.Size(84, 37);
            this.radToggle.TabIndex = 17;
            this.radToggle.TabStop = true;
            this.radToggle.Text = "Toggle";
            this.radToggle.UseVisualStyleBackColor = true;
            this.radToggle.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radLabel
            // 
            this.radLabel.AutoSize = true;
            this.radLabel.Depth = 0;
            this.radLabel.Location = new System.Drawing.Point(694, 30);
            this.radLabel.Margin = new System.Windows.Forms.Padding(0);
            this.radLabel.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.radLabel.Name = "radLabel";
            this.radLabel.Ripple = true;
            this.radLabel.Size = new System.Drawing.Size(74, 37);
            this.radLabel.TabIndex = 16;
            this.radLabel.TabStop = true;
            this.radLabel.Text = "Label";
            this.radLabel.UseVisualStyleBackColor = true;
            this.radLabel.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // txtNameCheat
            // 
            this.txtNameCheat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNameCheat.Depth = 0;
            this.txtNameCheat.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtNameCheat.LeadingIcon = null;
            this.txtNameCheat.Location = new System.Drawing.Point(107, 28);
            this.txtNameCheat.MaxLength = 50;
            this.txtNameCheat.MouseState = MaterialSkin.MouseState.OUT;
            this.txtNameCheat.Multiline = false;
            this.txtNameCheat.Name = "txtNameCheat";
            this.txtNameCheat.Size = new System.Drawing.Size(273, 36);
            this.txtNameCheat.TabIndex = 15;
            this.txtNameCheat.Text = "";
            this.txtNameCheat.TrailingIcon = null;
            this.txtNameCheat.UseTallSize = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 37);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 18);
            this.label16.TabIndex = 14;
            this.label16.Text = "Name Cheat:";
            // 
            // btnSave
            // 
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSave.Depth = 0;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(0, 388);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(771, 36);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CtrlPatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialCard1);
            this.Name = "CtrlPatch";
            this.Size = new System.Drawing.Size(771, 424);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private System.Windows.Forms.DataGridView dataList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private MaterialSkin.Controls.MaterialTextBox txtNameCheat;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialRadioButton radButtonOnOff;
        private MaterialSkin.Controls.MaterialRadioButton radToggle;
        private MaterialSkin.Controls.MaterialRadioButton radLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHex;
    }
}
