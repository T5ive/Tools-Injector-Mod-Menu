
namespace Tools_Injector_Mod_Menu
{
    partial class CtrlHookInput
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.dataList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.chkMultiple = new MaterialSkin.Controls.MaterialCheckbox();
            this.radSeekBarToggle = new MaterialSkin.Controls.MaterialRadioButton();
            this.radInputOnOff = new MaterialSkin.Controls.MaterialRadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radSeekBar = new MaterialSkin.Controls.MaterialRadioButton();
            this.radInput = new MaterialSkin.Controls.MaterialRadioButton();
            this.txtNameCheat = new MaterialSkin.Controls.MaterialTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLinks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.materialCard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
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
            this.colLinks});
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
            this.dataList.Size = new System.Drawing.Size(771, 284);
            this.dataList.TabIndex = 1;
            this.dataList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataList_EditingControlShowing);
            this.dataList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataList_KeyDown);
            this.dataList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataList_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numMax);
            this.groupBox1.Controls.Add(this.numMin);
            this.groupBox1.Controls.Add(this.chkMultiple);
            this.groupBox1.Controls.Add(this.radSeekBarToggle);
            this.groupBox1.Controls.Add(this.radInputOnOff);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radSeekBar);
            this.groupBox1.Controls.Add(this.radInput);
            this.groupBox1.Controls.Add(this.txtNameCheat);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Roboto", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(771, 104);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 18);
            this.label3.TabIndex = 24;
            this.label3.Text = "Max:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 18);
            this.label2.TabIndex = 23;
            this.label2.Text = "Min:";
            // 
            // numMax
            // 
            this.numMax.Font = new System.Drawing.Font("Roboto", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numMax.Location = new System.Drawing.Point(281, 67);
            this.numMax.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.numMax.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(118, 32);
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
            this.numMin.Location = new System.Drawing.Point(107, 67);
            this.numMin.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.numMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(118, 32);
            this.numMin.TabIndex = 6;
            this.numMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMin.ValueChanged += new System.EventHandler(this.NumValueChanged);
            // 
            // chkMultiple
            // 
            this.chkMultiple.AutoSize = true;
            this.chkMultiple.Depth = 0;
            this.chkMultiple.Location = new System.Drawing.Point(400, 61);
            this.chkMultiple.Margin = new System.Windows.Forms.Padding(0);
            this.chkMultiple.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkMultiple.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkMultiple.Name = "chkMultiple";
            this.chkMultiple.Ripple = true;
            this.chkMultiple.Size = new System.Drawing.Size(92, 37);
            this.chkMultiple.TabIndex = 22;
            this.chkMultiple.Text = "Multiple";
            this.chkMultiple.UseVisualStyleBackColor = true;
            // 
            // radSeekBarToggle
            // 
            this.radSeekBarToggle.AutoSize = true;
            this.radSeekBarToggle.Depth = 0;
            this.radSeekBarToggle.Location = new System.Drawing.Point(493, 61);
            this.radSeekBarToggle.Margin = new System.Windows.Forms.Padding(0);
            this.radSeekBarToggle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radSeekBarToggle.MouseState = MaterialSkin.MouseState.HOVER;
            this.radSeekBarToggle.Name = "radSeekBarToggle";
            this.radSeekBarToggle.Ripple = true;
            this.radSeekBarToggle.Size = new System.Drawing.Size(146, 37);
            this.radSeekBarToggle.TabIndex = 21;
            this.radSeekBarToggle.TabStop = true;
            this.radSeekBarToggle.Text = "SeekBar Toggle";
            this.radSeekBarToggle.UseVisualStyleBackColor = true;
            this.radSeekBarToggle.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radInputOnOff
            // 
            this.radInputOnOff.AutoSize = true;
            this.radInputOnOff.Depth = 0;
            this.radInputOnOff.Location = new System.Drawing.Point(643, 61);
            this.radInputOnOff.Margin = new System.Windows.Forms.Padding(0);
            this.radInputOnOff.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radInputOnOff.MouseState = MaterialSkin.MouseState.HOVER;
            this.radInputOnOff.Name = "radInputOnOff";
            this.radInputOnOff.Ripple = true;
            this.radInputOnOff.Size = new System.Drawing.Size(125, 37);
            this.radInputOnOff.TabIndex = 20;
            this.radInputOnOff.TabStop = true;
            this.radInputOnOff.Text = "Input On/Off";
            this.radInputOnOff.UseVisualStyleBackColor = true;
            this.radInputOnOff.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "Hook Type:";
            // 
            // radSeekBar
            // 
            this.radSeekBar.AutoSize = true;
            this.radSeekBar.Checked = true;
            this.radSeekBar.Depth = 0;
            this.radSeekBar.Location = new System.Drawing.Point(493, 30);
            this.radSeekBar.Margin = new System.Windows.Forms.Padding(0);
            this.radSeekBar.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radSeekBar.MouseState = MaterialSkin.MouseState.HOVER;
            this.radSeekBar.Name = "radSeekBar";
            this.radSeekBar.Ripple = true;
            this.radSeekBar.Size = new System.Drawing.Size(93, 37);
            this.radSeekBar.TabIndex = 18;
            this.radSeekBar.TabStop = true;
            this.radSeekBar.Text = "SeekBar";
            this.radSeekBar.UseVisualStyleBackColor = true;
            this.radSeekBar.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radInput
            // 
            this.radInput.AutoSize = true;
            this.radInput.Depth = 0;
            this.radInput.Location = new System.Drawing.Point(643, 30);
            this.radInput.Margin = new System.Windows.Forms.Padding(0);
            this.radInput.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radInput.MouseState = MaterialSkin.MouseState.HOVER;
            this.radInput.Name = "radInput";
            this.radInput.Ripple = true;
            this.radInput.Size = new System.Drawing.Size(115, 37);
            this.radInput.TabIndex = 17;
            this.radInput.TabStop = true;
            this.radInput.Text = "Input Value";
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
            this.txtNameCheat.Size = new System.Drawing.Size(292, 36);
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
            this.btnSave.AccentTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
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
            this.btnSave.NoAccentTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
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
            this.colType.Items.AddRange(new object[] {
            "double",
            "float",
            "int",
            "long",
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
            // colLinks
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.colLinks.DefaultCellStyle = dataGridViewCellStyle3;
            this.colLinks.HeaderText = "Links";
            this.colLinks.Name = "colLinks";
            this.colLinks.ReadOnly = true;
            this.colLinks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLinks.Width = 60;
            // 
            // CtrlHookInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialCard1);
            this.Name = "CtrlHookInput";
            this.Size = new System.Drawing.Size(771, 424);
            this.contextMenuStrip1.ResumeLayout(false);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private System.Windows.Forms.DataGridView dataList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialRadioButton radSeekBar;
        private MaterialSkin.Controls.MaterialRadioButton radInput;
        private MaterialSkin.Controls.MaterialTextBox txtNameCheat;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private MaterialSkin.Controls.MaterialRadioButton radInputOnOff;
        private MaterialSkin.Controls.MaterialRadioButton radSeekBarToggle;
        private MaterialSkin.Controls.MaterialCheckbox chkMultiple;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOffset;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colField;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLinks;
    }
}
