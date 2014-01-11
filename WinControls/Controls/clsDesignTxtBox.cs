using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace ICTEAS.WinForms.Controls
{
	public class clsDesignTxtBox : clsTxtBox
	{
		const int WM_LBUTTONDOWN = 0x0201; 
		public clsDesignTxtBox()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		protected override void WndProc(ref System.Windows.Forms.Message m)  
		{ 
 
			if(m.Msg == WM_LBUTTONDOWN) 
 
				return; 
 
			base.WndProc(ref m); 
 
		} 
	}	

}
