//----------------------------------------------------------------------------------
// - Author			   - Pham Minh Tri
// - Last Updated      - 19/Nov/2003
//----------------------------------------------------------------------------------
// - ICTEAS.WinForms.Controls:        - Nullable DateTimePicker
// - Version:          - 1.0
// - Description:      - A datetimepicker that allow null value.
//----------------------------------------------------------------------------------

using System;
using System.Windows.Forms;  

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for DateTimePicker.
	/// </summary>
	public class clsDateTimePicker : System.Windows.Forms.DateTimePicker   
	{
		private DateTimePickerFormat oldFormat = DateTimePickerFormat.Long;
		private string oldCustomFormat = null;
		private bool bIsNull = false;
		private string strResName ="";

		private bool boolTextChange = false;
		private bool _readOnly;

		public clsDateTimePicker() : base()
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

		protected override void OnValueChanged(EventArgs eventargs)
		{
			boolTextChange = true;
			base.OnValueChanged (eventargs);
		}

		public new DateTime Value 
		{
			get 
			{
				if (bIsNull)
					return DateTime.MinValue;
				else
					return base.Value;
			}
			set 
			{
				if (value == DateTime.MinValue)
				{
					if (bIsNull == false) 
					{
						oldFormat = this.Format;
						oldCustomFormat = this.CustomFormat;
						bIsNull = true;
					}

					this.Format = DateTimePickerFormat.Custom;
					this.CustomFormat = " ";
				}
				else 
				{
					if (bIsNull) 
					{
						this.Format = oldFormat;
						this.CustomFormat = oldCustomFormat;
						bIsNull = false;
					}
					base.Value = value;
				}
			}
		}

		protected override void OnCloseUp(EventArgs eventargs)
		{
			if (Control.MouseButtons == MouseButtons.None) 
			{
				if (bIsNull) 
				{
					this.Format = oldFormat;
					this.CustomFormat = oldCustomFormat;
					bIsNull = false;
				}
			}
			base.OnCloseUp (eventargs);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(this.ReadOnly && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete))
				e.Handled = true;
			else 
			{
				base.OnKeyDown (e);

				if (e.KeyCode == Keys.Delete)
					this.Value = DateTime.MinValue; 
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if(this.ReadOnly) 
				e.Handled = true;
			else
				base.OnKeyPress (e);
		}

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x0201 || m.Msg == 0x0203 ) && (_readOnly || ICTEAS.WinForms.Common.clsCommonInfo.ProcessInProgress ))
                this.Focus();
            else
                base.WndProc(ref m);
        }

		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}
	}
}
