
namespace Tools_Injector_Mod_Menu
{
    partial class CtrlHookButton
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.dataList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radButton = new MaterialSkin.Controls.MaterialRadioButton();
            this.radInput = new MaterialSkin.Controls.MaterialRadioButton();
            this.txtNameCheat = new MaterialSkin.Controls.MaterialTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.materialCard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colOffset,
            this.colType,
            this.colValue});
            this.dataList.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataList.DefaultCellStyle = dataGridViewCellStyle2;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radButton);
            this.groupBox1.Controls.Add(this.radInput);
            this.groupBox1.Controls.Add(this.txtNameCheat);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(771, 73);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(477, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "Hook Type:";
            // 
            // radButton
            // 
            this.radButton.AutoSize = true;
            this.radButton.Checked = true;
            this.radButton.Depth = 0;
            this.radButton.Location = new System.Drawing.Point(564, 30);
            this.radButton.Margin = new System.Windows.Forms.Padding(0);
            this.radButton.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.radButton.Name = "radButton";
            this.radButton.Ripple = true;
            this.radButton.Size = new System.Drawing.Size(82, 37);
            this.radButton.TabIndex = 18;
            this.radButton.TabStop = true;
            this.radButton.Text = "Button";
            this.radButton.UseVisualStyleBackColor = true;
            this.radButton.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radInput
            // 
            this.radInput.AutoSize = true;
            this.radInput.Depth = 0;
            this.radInput.Location = new System.Drawing.Point(646, 30);
            this.radInput.Margin = new System.Windows.Forms.Padding(0);
            this.radInput.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radInput.MouseState = MaterialSkin.MouseState.HOVER;
            this.radInput.Name = "radInput";
            this.radInput.Ripple = true;
            this.radInput.Size = new System.Drawing.Size(122, 37);
            this.radInput.TabIndex = 17;
            this.radInput.TabStop = true;
            this.radInput.Text = "Input Button";
            this.radInput.UseVisualStyleBackColor = true;
            this.radInput.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
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
            this.txtNameCheat.Size = new System.Drawing.Size(364, 36);
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
            this.colOffset.Width = 120;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.Width = 265;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.HeaderText = "Values";
            this.colValue.Name = "colValue";
            // 
            // CtrlHookButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialCard1);
            this.Name = "CtrlHookButton";
            this.Size = new System.Drawing.Size(771, 424);
            this.contextMenuStrip1.ResumeLayout(false);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private System.Windows.Forms.DataGridView dataList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialRadioButton radButton;
        private MaterialSkin.Controls.MaterialRadioButton radInput;
        private MaterialSkin.Controls.MaterialTextBox txtNameCheat;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
    }
}
