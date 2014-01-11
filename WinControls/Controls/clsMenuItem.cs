using System;

namespace ICTEAS.WinForms.Controls
{

    public enum Privileges { Create, Update, ViewOnly, Hidden, NotApplicable, Special, Create_Delete };

	/// <summary>
	/// Summary description for clsMenuItem.
		/// </summary>
	public class clsMenuItem : System.Windows.Forms.ToolStripMenuItem
	{
		private string _DBName = "";
		private string _MessageID = "";
		private Privileges m_Privlg = Privileges.ViewOnly;

		public clsMenuItem()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// This is to be set same as the clsMenuItem object name
		/// </summary>
		public string DBName
		{
			set
			{
				if( _DBName == "" )
				{
					_DBName = value;
				}
			}
			get
			{
				return _DBName;
			}
		}

		public string MessageID
		{
			set
			{
				_MessageID = value;
			}
			get
			{
				return _MessageID;
			}
		}

		public Privileges Privilege
		{
			get
			{
				return m_Privlg;
			}
            set
            {
                m_Privlg = value;
            }
		}


        private string _ScreenName = String.Empty;

        public string ScreenName
        {
            get { return _ScreenName; }
            set { _ScreenName = value; }
        }

	}
}
