using System;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsCheckBox.
	/// </summary>
	public class clsCheckBox	:	System.Windows.Forms.CheckBox
	{
		private string strMsgID = "";
		private string strResName="";

		private bool boolTextChange = false;

		public clsCheckBox()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool TxtChange
		{
			get{return boolTextChange;}
		}

		public void RefreshChange()
		{
			boolTextChange = false;
		}
		
		protected override void OnCheckedChanged(EventArgs e)
		{
			boolTextChange = true;
			base.OnCheckedChanged (e);
		}

		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}

		public string MessageID
		{
			get
			{
				return strMsgID;
			}
			set
			{
				strMsgID = value;
			}
		}
	}
}
