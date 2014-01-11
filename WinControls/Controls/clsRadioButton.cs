using System;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsRadioButton.
	/// </summary>
	public class clsRadioButton	:	System.Windows.Forms.RadioButton
	{
		private string strResName="";
		private string strMsgID = "";

		private bool boolTextChange = false;

		public clsRadioButton()
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
