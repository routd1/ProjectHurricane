using System;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsButton.
	/// </summary>
	public class clsButton	:	System.Windows.Forms.Button
	{
        //protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs mevent)
        //{
        //    if (ICTEAS.WinForms.Common.clsCommonInfo.ProcessInProgress)
        //    {
        //        return;
        //    }
        //    base.OnMouseDown(mevent);
        //}
		private string strMsgID = "";
		private string strResName="";

		public clsButton()
		{
			//
			// TODO: Add constructor logic here
			//
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
