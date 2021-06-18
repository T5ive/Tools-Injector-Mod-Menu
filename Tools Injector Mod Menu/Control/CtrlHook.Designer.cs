
namespace Tools_Injector_Mod_Menu
{
    partial class CtrlHook
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.dataList = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLinks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radButtonOnOff = new MaterialSkin.Controls.MaterialRadioButton();
            this.radToggle = new MaterialSkin.Controls.MaterialRadioButton();
            this.txtNameCheat = new MaterialSkin.Controls.MaterialTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
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
            this.colFieldType,
            this.colField,
            this.colValue,
            this.colLinks});
            this.dataList.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataList.Location = new System.Drawing.Point(0, 0);
            this.dataList.MultiSelect = false;
            this.dataList.Name = "dataList";
            this.dataList.RowHeadersVisible = false;
            this.dataList.Size = new System.Drawing.Size(771, 315);
            this.dataList.TabIndex = 1;
            this.dataList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataList_EditingControlShowing);
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
            this.colOffset.Width = 120;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Items.AddRange(new object[] {
            "bool",
            "double",
            "float",
            "int",
            "long",
            "vector3",
            "void",
            "links"});
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colFieldType
            // 
            this.colFieldType.HeaderText = "Field Type";
            this.colFieldType.Items.AddRange(new object[] {
            "bool",
            "double",
            "float",
            "int",
            "long"});
            this.colFieldType.Name = "colFieldType";
            this.colFieldType.ReadOnly = true;
            this.colFieldType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFieldType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colField
            // 
            this.colField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.colField.DefaultCellStyle = dataGridViewCellStyle2;
            this.colField.HeaderText = "Field Offset";
            this.colField.Name = "colField";
            this.colField.ReadOnly = true;
            // 
            // colValue
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.colValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // colLinks
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Silver;
            this.colLinks.DefaultCellStyle = dataGridViewCellStyle4;
            this.colLinks.HeaderText = "Links";
            this.colLinks.Name = "colLinks";
            this.colLinks.ReadOnly = true;
            this.colLinks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLinks.Width = 60;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radButtonOnOff);
            this.groupBox1.Controls.Add(this.radToggle);
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
            this.label1.Location = new System.Drawing.Point(460, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "Hook Type:";
            // 
            // radButtonOnOff
            // 
            this.radButtonOnOff.AutoSize = true;
            this.radButtonOnOff.Checked = true;
            this.radButtonOnOff.Depth = 0;
            this.radButtonOnOff.Location = new System.Drawing.Point(548, 30);
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
            this.radToggle.Location = new System.Drawing.Point(684, 30);
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
            this.txtNameCheat.Size = new System.Drawing.Size(347, 36);
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
            // CtrlHook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialCard1);
            this.Name = "CtrlHook";
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
        private MaterialSkin.Controls.MaterialRadioButton radButtonOnOff;
        private MaterialSkin.Controls.MaterialRadioButton radToggle;
        private MaterialSkin.Controls.MaterialTextBox txtNameCheat;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOffset;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colField;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLinks;
    }
}
