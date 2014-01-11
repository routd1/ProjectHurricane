// #######################################################
// ***Author			: Indranil 
// ***Date of Creation	: 22/07/2005
// ***Last Modified By	: Indranil
// ***Last Modified Date: 09/08/2005
// #######################################################

using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Auto-reduction Combo Box which is populated from a DataTable
	/// </summary>
	public class clsAutoComboBox	:	System.Windows.Forms.ComboBox
	{
		private string strResName = "";

		private string[,] arrStrSourceData;
		private bool blTxtChanged = false;
		private bool _readOnly;
		private bool blFireTxtChange = false;
		private string[] _retStr = {""};
		private bool _MatchFound = false;


		public clsAutoComboBox()
		{
			blTxtChanged = false;
			blFireTxtChange = false;
			_readOnly = false;
		}

		public bool MatchFound
		{
			get{ return _MatchFound; }
		}

		public bool ReadOnly
		{
			get{return _readOnly;}
			set{_readOnly = value;}
		}

		public string[] ReturnString
		{
			get{return _retStr;}
		}

		public bool TxtChange
		{
			get{return blTxtChanged;}
		}

		public void RefreshChange()
		{
			blTxtChanged = false;
		}

		/// <summary>
		/// Set the DataTable from which the Combo Box will be populated
		/// </summary>
		/// <param name="dtSource"></param>
		public void SetDataSource( System.Data.DataTable dtSource )
		{
			try
			{
				if( dtSource == null || dtSource.Rows.Count == 0 || dtSource.Columns.Count == 0 )
					return;

				base.Items.Clear();
				arrStrSourceData = new string[ dtSource.Rows.Count, dtSource.Columns.Count ];

				// Auto-sizing of Drop down list
				float width = 0; 
				int numRows = dtSource.Rows.Count; 
				Graphics g = Graphics.FromHwnd(this.Handle); 
				StringFormat sf = new StringFormat(StringFormat.GenericTypographic); 
				SizeF size; 

				for( int i = 0; i < numRows; i++ )
				{
					base.Items.Add( dtSource.Rows[i][0].ToString() );

					size = g.MeasureString( dtSource.Rows[i][0].ToString(), this.Font, 500, sf); 
					if(size.Width > width) 
						width = size.Width; 

					for( int j = 0; j < dtSource.Columns.Count; j++ )
					{
						arrStrSourceData[i,j] = dtSource.Rows[i][j].ToString();
					}
				}

				base.DropDownWidth = Convert.ToInt32( width );
				
				sf.Dispose();
				g.Dispose();
				dtSource.Dispose();
			}
			catch( Exception ex )
			{
				dtSource.Dispose();
				MessageBox.Show( ex.ToString() );
			}
		}


		protected override void OnEnter(EventArgs e)
		{
			if( _readOnly == false )
			{
				base.DroppedDown = true;
			}
			base.OnEnter (e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			try
			{
				base.OnTextChanged (e);

				if( blFireTxtChange )
				{
					blFireTxtChange = false;

					base.BeginUpdate();

					string strTypedtext = base.Text.ToUpper();

					base.Items.Clear();

					int iTotalRows = arrStrSourceData.GetLength(0);

					for( int i = 0; i < iTotalRows; i++ )
					{
						if( arrStrSourceData[i,0].ToUpper().IndexOf( strTypedtext ) == 0 )
						{
							base.Items.Add( arrStrSourceData[i,0] );
						}
					}

					base.Select( base.Text.Length , 0 );

					blTxtChanged = true;

					base.EndUpdate();
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
				base.EndUpdate();
			}
		}
	
		protected override void OnLeave(EventArgs e)
		{
			try
			{
				if( _readOnly == false )
				{
					string strBaseText = base.Text;

					_retStr = new string[ arrStrSourceData.GetLength(1) ];
					for( int i = 0; i < _retStr.Length; i++ )
						_retStr[i] = "";

					if( strBaseText != "" )
					{
						int iMatchingRow = base.FindStringExact( strBaseText, -1 );

						if( iMatchingRow > -1 )
						{
							for( int i = 0; i < arrStrSourceData.GetLength(0); i++ )
							{
								if( strBaseText == arrStrSourceData[i,0] )
								{
									for( int j = 0; j < _retStr.Length; j++ )
									{
										_retStr[j] = arrStrSourceData[i,j];
										_MatchFound = true;
									}
									break;
								}
							}
						}
						else
						{
							blFireTxtChange = false;
							_MatchFound = false;
							base.Text = "";
						}
					}

					base.OnLeave (e);
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

	
		protected override void OnSelectionChangeCommitted(EventArgs e)
		{
			blFireTxtChange = true;
			base.OnSelectionChangeCommitted (e);
			blTxtChanged = true;
		}



		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}

		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)  
		{ 
			try
			{
				if(this.ReadOnly)
				{
					e.Handled = true;
					return;
				}

				if( e.KeyData != Keys.Up && e.KeyData != Keys.Down 
					&& e.KeyData != Keys.PageUp && e.KeyData != Keys.PageDown)
					blFireTxtChange = true;
				else
					blFireTxtChange = false;

				base.OnKeyDown (e);

				if( Control.ModifierKeys != Keys.Shift )
					if( e.KeyData != Keys.Left
						&& e.KeyData != Keys.Right 
						&& e.KeyData != Keys.Delete 
						&& e.KeyData != Keys.Back ) 
						base.Select( base.Text.Length, 0 );
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

		protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e) 
		{
			if(this.ReadOnly) 
			{
				e.Handled = true;
				return;
			}
			else
				base.OnKeyPress (e);
		}

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if ((m.Msg == 0x0201 || m.Msg == 0x0203) && _readOnly)
                this.Focus();
            else
                base.WndProc(ref m);
        }
	}
}
