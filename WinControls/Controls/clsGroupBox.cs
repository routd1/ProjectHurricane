using System;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsGroupBox.
	/// </summary>
	public class clsGroupBox	:	System.Windows.Forms.GroupBox
	{
		private string strMsgID = "";

		public clsGroupBox()
		{
			//
			// TODO: Add constructor logic here
			//
           // this.BackColor = System.Drawing.Color.Honeydew;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // clsGroupBox
            // 
           // this.BackColor = System.Drawing.Color.Honeydew;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.painting);
            this.ResumeLayout(false);

        }

        private void painting(object sender, System.Windows.Forms.PaintEventArgs e)
        {
          //  this.BackColor = System.Drawing.Color.Honeydew;
        }
	}
}
