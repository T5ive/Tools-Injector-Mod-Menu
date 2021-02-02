using System;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmFunction : Form
    {
        public FrmFunction()
        {
            InitializeComponent();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveRows();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowToDelete = dataList.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            dataList.Rows.RemoveAt(rowToDelete);
            dataList.ClearSelection();
        }

        private void RemoveRows()
        {
            try
            {
                if (dataList.RowCount <= 1 || dataList.CurrentCell.RowIndex == dataList.RowCount - 1) return;
                dataList.Rows.RemoveAt(dataList.SelectedCells[0].RowIndex);
                dataList.ClearSelection();
            }
            catch
            {
                //
            }
        }

        private void dataList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataList.HitTest(e.X, e.Y);
                dataList.ClearSelection();
                dataList.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}