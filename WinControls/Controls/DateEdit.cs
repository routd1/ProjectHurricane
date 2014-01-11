using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Drawing;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for DateEdit. The DateEdit control is designed to allow Null DateTime values
	///		to be entered by the user. 
	/// Notes: 
	///		1) DateTime.MinValue is reserved for use to represent NullValue. There are a couple of options
	///			regarding database handling. 
	///				a) Insert DateTime.NullValue into db and REMEMBER the null value (01/01/0001). 
	///				b) Convert any DateTime containing NullValue to a real null.
	///		2) InvalidDate event is fired when leaving the control with an invalid date. Use this to provide
	///			users an indication of the problem. Beep?, MessageBox?, change BackColor. ValidDate event
	///			can be used to signal good dates, or undo color changes
	///			
	///	Author: Oscar Bowyer 
	///	Revision History:
	///		Initial Release 5/22/2003
	/// </summary>
	public class DateEdit : TextBox
	{
		private string m_format = "__/__/____";		// display format (mask with input chars replaced by input char)
		private char m_inpChar = '_';
		private string m_regex = @"[0-9]";
		private int m_caret;
		private string userFormat = "dd/MM/yyyy";
		private Hashtable m_posNdx;
		private bool boolTextChange = false;

		// use MinValue as NullValue
		public static readonly DateTime NullValue = DateTime.MinValue;

		// events
		public delegate void InvalidDateEventHandler(object sender, EventArgs e);
		public delegate void ValidDateEventHandler(object sender, EventArgs e);

		public event InvalidDateEventHandler InvalidDate;
		public event ValidDateEventHandler ValidDate;

		public string DBValue
		{
			get
			{
				string ret;
				try
				{
					if( userFormat.ToUpper().IndexOf("DD") < 0 )
						ret = DateTime.ParseExact(this.Text,"HH:mm",null).ToString("HH:mm");
					else if( userFormat.ToUpper().IndexOf("HH") >= 0 )
						ret = DateTime.ParseExact(this.Text,"dd/MM/yyyy HH:mm",null).ToString("dd/MM/yyyy HH:mm");
					else
						ret = DateTime.ParseExact(this.Text,"dd/MM/yyyy",null).ToString("dd/MM/yyyy");
				}
				catch
				{
					ret = "";
				}

				return ret;
			}
		}

		public string Format
		{
			get
			{
				return userFormat;
			}
			set
			{
				base.Text = "";
				userFormat = value;
				BuildFormat();
				base.MaxLength = m_format.Length;
				BuildPosNdx();
				this.Select(0,1);
			}
		}

		public DateEdit()
		{
			// disable context menu since it bypasses Ctrl+V handler
			this.ContextMenu = new ContextMenu();
		}

		[Category("Behavior")]
		protected void OnInvalidDate(EventArgs e)
		{
			if(InvalidDate != null)
				InvalidDate(this, e);
		}

		[Category("Behavior")]
		protected void OnValidDate(EventArgs e)
		{
			if(ValidDate != null)
				ValidDate(this, e);
		}

		[Description("Sets the Input Char default '_'"), Category("Behavior"), 
		RefreshProperties(RefreshProperties.All)]
		public char InputChar
		{
			// default : ' '
			get{return m_inpChar;}
//			set
//			{
//				m_inpChar = value;
//				BuildFormat();
//			}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
//			get{return base.Text;}
			set{ /* ignore set */}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Multiline
		{
			get{return base.Multiline;}
			// ignore set
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int SelectionStart
		{
			get{return base.SelectionStart;}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int SelectionLength
		{
			get{return base.SelectionLength;}
		}


		public override int MaxLength
		{
			get{return base.MaxLength;}
		}


		public new bool ReadOnly
		{
			get{return base.ReadOnly;}
			set{base.ReadOnly = value;}
		}

		public bool TxtChange
		{
			get{return boolTextChange;}
		}

		public void RefreshChange()
		{
			boolTextChange = false;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
            try
            {
                if (this.ReadOnly && keyData != Keys.Tab)
                    return true;

                // NOTES: 
                //	1) break; causes warnings below
                //	2) m_caret tracks caret location, always the start of selected char
                //	3) code below is based on MaskedEdit, since our format is fixed
                //		there may be optimizations possible
                int strt = base.SelectionStart;
                int len = base.SelectionLength;
                int end = strt + base.SelectionLength - 1;
                string s = base.Text;
                int p;

                // handle startup, runs once
                if (strt < m_format.Length && m_format[strt] != m_inpChar)
                {
                    strt = Next(-1);
                    len = 1;
                }

                switch (keyData)
                {
                    case Keys.Shift | Keys.Delete:
                    case Keys.Shift | Keys.Back:
                        return true;

                    case Keys.Left:
                    case Keys.Up:
                        p = Prev(strt);
                        if (p != strt)
                        {
                            base.SelectionStart = p;
                            base.SelectionLength = 1;
                        }
                        m_caret = p;
                        return true;
                    case Keys.Left | Keys.Shift:
                    case Keys.Up | Keys.Shift:
                        if ((strt < m_caret) || (strt == m_caret && len <= 1))
                        {
                            // enlarge left
                            p = Prev(strt);
                            base.SelectionStart -= (strt - p);
                            base.SelectionLength = len + (strt - p);
                        }
                        else
                        {
                            // shrink right
                            base.SelectionLength = len - (end - Prev(end));
                        }
                        return true;
                    case Keys.Right:
                    case Keys.Down:
                        p = Next(strt);
                        if (p != strt)
                        {
                            base.SelectionStart = p;
                            base.SelectionLength = 1;
                        }
                        m_caret = p;
                        return true;
                    case Keys.Right | Keys.Shift:
                    case Keys.Down | Keys.Shift:
                        if (strt < m_caret)
                        {
                            // shrink left
                            p = Next(strt);
                            base.SelectionStart += (p - strt);
                            base.SelectionLength = len - (p - strt);
                        }
                        else if (strt == m_caret)
                        {
                            // enlarge right
                            p = Next(end);
                            base.SelectionLength = len + (p - end);
                        }
                        return true;
                    case Keys.Delete:
                        // delete selection, replace with input format
                        base.Text = s.Substring(0, strt) + m_format.Substring(strt, len) + s.Substring(strt + len);
                        base.SelectionStart = strt;
                        base.SelectionLength = 1;
                        m_caret = strt;
                        boolTextChange = true;
                        return true;
                    case Keys.Home:
                    case Keys.Left | Keys.Control:
                    case Keys.Home | Keys.Control:
                        base.SelectionStart = Next(-1);
                        base.SelectionLength = 1;
                        m_caret = base.SelectionStart;
                        return true;
                    case Keys.Home | Keys.Shift:
                        if (strt <= m_caret && len <= 1)
                        {
                            // enlarge left
                            p = Next(-1);
                            base.SelectionStart -= (strt - p);
                            base.SelectionLength = len + (strt - p);
                        }
                        else
                        {
                            // shrink right
                            p = Next(-1);
                            base.SelectionStart = p;
                            base.SelectionLength = (m_caret - p) + 1;
                        }
                        return true;
                    case Keys.End:
                    case Keys.Right | Keys.Control:
                    case Keys.End | Keys.Control:
                        base.SelectionStart = Prev(base.MaxLength);
                        base.SelectionLength = 1;
                        m_caret = base.SelectionStart;
                        return true;
                    case Keys.End | Keys.Shift:
                        if (strt < m_caret)
                        {
                            // shrink left
                            p = Prev(base.MaxLength);
                            base.SelectionStart = m_caret;
                            base.SelectionLength = (p - m_caret + 1);
                        }
                        else if (strt == m_caret)
                        {
                            // enlarge right
                            p = Prev(base.MaxLength);
                            base.SelectionLength = len + (p - end);
                        }
                        return true;
                    case Keys.V | Keys.Control:
                    case Keys.Insert | Keys.Shift:
                        // paste from clipboard
                        IDataObject iData = Clipboard.GetDataObject();

                        // assemble new text
                        string t = s.Substring(0, strt)
                            + (string)iData.GetData(DataFormats.Text)
                            + s.Substring(strt + len);

                        // check if data to be pasted is convertable to inputType
                        DateTime dt; ;
                        try
                        {
                            dt = DateTime.Parse(t);
                            base.Text = dt.ToString(userFormat);
                            boolTextChange = true;

                            // reset selection
                            base.SelectionStart = strt;
                            base.SelectionLength = len;
                        }
                        catch
                        {
                            // do nothing
                        }

                        return true;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			// reset selection to include input chars
			int strt = base.SelectionStart;
			int orig = strt;
			int len = base.SelectionLength;

			// reset selection start
			if(strt == base.MaxLength || m_format[strt] != m_inpChar)
			{
				// reset start
				if(Next(strt) == strt)
					strt = Prev(strt);
				else
					strt = Next(strt);

				base.SelectionStart = strt;
			}

			// reset selection length
			if(len < 1)
				base.SelectionLength = 1;
			else if(m_format[orig + len - 1] != m_inpChar)
			{
				len += Next(strt + len) - (strt + len);
				base.SelectionLength = len;
			}

			m_caret = strt;
			base.OnMouseUp(e);
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool IsValid
		{
			get
			{
				try
				{
					// null is valid
					if(base.Text == m_format)
						return true;

					DateTime ret = DateTime.ParseExact(base.Text,this.Format,null);
				}
				catch
				{
					return false;
				}

				return true;
			}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public DateTime Value
		{
			get
			{
				// TODO: What to return if Null not allowed and invalid value?
				//	a) error?
				//	b) Null?
				DateTime ret;
				try
				{
					ret = DateTime.ParseExact( this.Text, this.userFormat, null );
				}
				catch
				{
					ret = NullValue;
				}

				return ret;
			}
			set
			{
				if(value == NullValue)
					base.Text = m_format;
				else
				{
					base.Text = value.ToString(userFormat);	// TODO: must format using current culture!!!
					boolTextChange = true;
				}
                try
                {
                    string strText = base.Text;
                    DateTime dt = DateTime.ParseExact(strText, this.Format, null);
                    this.BackColor = SystemColors.Window;
                }
                catch
                {
                    //this.BackColor = Color.Red;
                }

				OnValidDate(EventArgs.Empty);
			}
		}


		protected override void OnLeave(EventArgs e)
		{
			// validate entry
			if( !this.ReadOnly )
			{
				try
				{
					string strText = base.Text;
					if( strText == m_format)
						return;
					DateTime dt = DateTime.ParseExact( strText, this.Format, null);
					OnValidDate(EventArgs.Empty);
					this.BackColor = SystemColors.Window;
					base.OnLeave(EventArgs.Empty);
				}
				catch
				{
					// fire InvalidDate event
					OnInvalidDate(EventArgs.Empty);
					this.BackColor = Color.Red;
					this.Focus();
                    this.boolTextChange = false;
					return;
				}
			}		
		}


		protected override void OnKeyPress(KeyPressEventArgs e)
		{
            try
            {
                int strt = base.SelectionStart;
                int len = base.SelectionLength;
                int p;

                if (char.IsNumber(e.KeyChar) == false)
                {
                    e.Handled = true;
                    return;
                }
                // Handle Backspace -> replace previous char with inpchar and select
                if (e.KeyChar == 0x08)
                {
                    string s = base.Text;
                    p = Prev(strt);
                    if (p != strt)
                    {
                        base.Text = s.Substring(0, p) + m_inpChar.ToString() + s.Substring(p + 1);
                        base.SelectionStart = p;
                        base.SelectionLength = 1;
                        boolTextChange = true;

                    }
                    m_caret = p;
                    e.Handled = true;
                    return;
                }

                if (e.KeyChar == 0x09)
                {
                    e.Handled = false;
                    base.OnKeyPress(e);
                }

                    // update display if valid char entered
                else if (IsValidChar(e.KeyChar, (int)m_posNdx[strt]))
                {
                    // assemble new text
                    string t = "";
                    t = base.Text.Substring(0, strt);
                    t += e.KeyChar.ToString();

                    if (strt + len != base.MaxLength)
                    {
                        if (len != 0)
                            t += m_format.Substring(strt + 1, len - 1);
                        else
                            t += m_format.Substring(strt + 1, len);
                        if ((strt + len) != 0)
                            t += base.Text.Substring(strt + len);
                        else
                            t += base.Text.Substring(1);
                    }
                    else
                    {
                        t += m_format.Substring(strt + 1);
                        base.Text = t;
                        OnLeave(e);
                    }

                    base.Text = t;
                    boolTextChange = true;

                    // select next input char
                    strt = Next(strt);
                    base.SelectionStart = strt;
                    m_caret = strt;
                    base.SelectionLength = 1;
                    e.Handled = true;

                }
            }
            catch
            {
 
            }
		}


		private bool IsValidChar(char input, int pos)
		{
			// validate input char against mask
			return Regex.IsMatch(input.ToString(), m_regex);
		}


		private int Prev(int startPos)
		{
			// return previous input char position
			// returns current position if no input chars to the left
			int strt = startPos;
			int ret = strt;

			while(strt > 0)
			{
				strt--;
				if(m_format[strt] == m_inpChar)
					return strt;
			}
			return ret;			
		}


		private int Next(int startPos)
		{
			// return next input char position
			// returns current position if no input chars to the left
			int strt = startPos;
			int ret = strt;
			
			while(strt < base.MaxLength - 1)
			{
				strt++;
				if(m_format[strt] == m_inpChar)
					return strt;
			}

			return ret;			
		}


		private void BuildFormat()
		{
			// this builds the m_format string based on current regional settings
			//	EN example "M/d/yyyy" with default input char '_' produces "__/__/____"

			try
			{

				m_format = "";
				Char[] temp = userFormat.ToCharArray();
				for( int i =0 ; i < userFormat.Length; i ++ )
				{
					if( temp[i] != '/' && temp[i] != ':' && temp[i] != ' ')
						m_format += InputChar.ToString();
					else
						m_format += temp[i];
				}
				base.Text = m_format;
			}
			catch
			{
				m_format = "00/00/0000";
				this.Format = "dd/MM/yyyy";
				base.Text = m_format;
			}
		}


		private void BuildPosNdx()
		{
			// used to build position translation map from mask string
			//	and input format
			string s = m_format;

			// reset index
			if(m_posNdx == null)
				m_posNdx = new Hashtable();
			else
				m_posNdx.Clear();

			int cnt = 0;

			for(int i = 0; i < s.Length; i++)
			{
				if(s[i] == m_inpChar)
					m_posNdx.Add(cnt, i);

				cnt++;
			}
		}
	}
}
