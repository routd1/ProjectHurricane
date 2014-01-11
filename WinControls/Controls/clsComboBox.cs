using System;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsComboBox.
	/// </summary>
	public class clsComboBox	:	System.Windows.Forms.ComboBox
	{
		private string strResName = "";
		private bool boolSetMultiLingual = true;

		private string[] strItemCollection;
		private bool boolTextChange = false;
		private bool _readOnly;

		public clsComboBox()
		{
			_readOnly = false;
		}

		public bool ReadOnly
		{
			get{return _readOnly;}
			set{_readOnly = value;}
		}

		public bool TxtChange
		{
			get{return boolTextChange;}
		}

		public void RefreshChange()
		{
			boolTextChange = false;
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			boolTextChange = true;
			base.OnSelectedIndexChanged (e);
		}

		public void StoreItems(params string[] ItemEntries)
		{
			strItemCollection = ItemEntries;
		}

		public bool MultiLingual
		{
			get{return boolSetMultiLingual;}
			set{boolSetMultiLingual = value;}
		}

		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}

		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)  
		{ 
			if(this.ReadOnly)	// && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete))
				e.Handled = true;
			else 
				base.OnKeyDown (e);
		}

		protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e) 
		{
			if(this.ReadOnly) 
				e.Handled = true;
			else
				base.OnKeyPress (e);
		}

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if ((m.Msg == 0x0201 || m.Msg == 0x0203) && _readOnly)
                this.Focus();
            else
                base.WndProc(ref m);

            //this.RefreshChange();
        }
	}
}
