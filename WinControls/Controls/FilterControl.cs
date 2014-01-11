using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{
    public partial class FilterControl : Form
    {
        public FilterControl(DataGridView basegrid)
        {
            InitializeComponent();
            string errmsg="";
            this.Location = new Point(basegrid.Location.X, basegrid.Location.Y - 50);
            this.Width = basegrid.Width;
            if (!clsSearchGrid1.DesignGrid(basegrid, ref errmsg))
            {
                MessageBox.Show(errmsg);
            }
            this.Visible = true;
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            clsSearchGrid1.setfulldata();
            base.OnClosing(e);
        }
    }
}