using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsTxtBox.
	/// </summary>
	public class clsTxtBox : System.Windows.Forms.TextBox
	{
		
		#region Changing for Currency Implementation

		private bool _isCurrency = false;
		private int CommaPrec=3;
		private int IntegerPrecisionIncludedComma;
        private bool edited = false;
        private int noofcommas = 0;
        private bool semaphore = true;
        public int entrycount = 0;
        string strstoretext = "";
		public int GetNoofCommas()
		{
		
			int quotient;
			int remainder;
			int dividend = this.IntegerPrecision ;
			int divisor = this.CommaPrecision ;
			quotient = (int) dividend/divisor;
			remainder =   dividend % divisor;
			if(remainder == 0)
			return(	quotient -1);
			else return quotient ;
		}
		[Category("Currency"),Browsable(true),Description("Whether The TextBox will be used as Currency or not"),RefreshProperties (RefreshProperties.All)]
		public bool isCurrency
		{
			get 
			{ 
				return  _isCurrency;
			}
			set 
			{
				_isCurrency = value;
				if(_isCurrency)
				{
					if( this.CommaPrecision == -1 )
						this.CommaPrecision = 3;
					this.IntegerPrecisionIncludedComma = this.IntegerPrecision+GetNoofCommas();
				}
				
			}
		}

		[Category("Currency"),Browsable(true),Description("Set The Comma Precision"),RefreshProperties (RefreshProperties.All)]
		public int CommaPrecision
		{
			get 
			{
				if(isCurrency)
					return  CommaPrec;
				else return -1;
			}

			set
			{
				
				CommaPrec = value;
				this.isCurrency = true;
				OkString();
			}
		}

		[Category("Currency"),Browsable(true),Description("Returns Text Without Comma"),RefreshProperties (RefreshProperties.None)]
		public string TextCurrency
		{
			get 
			{
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                return (this.Text.Replace(nmfi.CurrencyGroupSeparator, ""));
			}
			set
			{
                if ((isCurrency) && (Type != TypeEnum.String))
                {
                    this.Text = value;
                    this.isCurrency = true;
                    if (!OkString())
                    {
                        //ignore moving off cell if bad date
                        this.Text = "";
                        return;
                    }
                }
			}
		}




        protected virtual void clsTxtBox_TextChanged(object sender, EventArgs e)
        {
            
            iAfterTxtChange = this.Text.Length;

            if ((isCurrency) && (Type != TypeEnum.String))
            {
                if (!OkString())
                {
                    //ignore moving off cell if bad date
                    return;
                }
                
            }

        }

		protected virtual void clsTxtBox_KeyUp(object sender,KeyEventArgs e)
		{
            if (!e.Shift && (e.KeyData != Keys.ShiftKey)
                && (e.KeyData != Keys.Left) && (e.KeyData  != Keys.Right)
                && (e.KeyData != Keys.Home) && (e.KeyData != Keys.End)
              )
			{
                //if ((isCurrency) && (Type != TypeEnum.String))
                //{
                //    edited = true;
                //    iAfterTxtChange = this.Text.Length;
                //    if (!OkString())
                //    {
                //        //ignore moving off cell if bad date
                //        return;
                //    }

                //}
			}
		}

		private string Execute(string DecimalNumber, int Precision)
		{
			try
			{
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
				string prefix = "";
                if (_NegativeSign && DecimalNumber.IndexOf(nmfi.NegativeSign) == 0)
				{
                    DecimalNumber = DecimalNumber.TrimStart(Convert.ToChar(nmfi.NegativeSign));
                    prefix = nmfi.NegativeSign;
				}
                if (DecimalNumber.Trim() == "")
                {
                    DecimalNumber = "0";
                }
                Decimal m_decTemp = Math.Round(Convert.ToDecimal(DecimalNumber, nmfi), Precision);
                //string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                //for(int k=0; k < Precision; k++)
                //    m_strLocal += "0";
				
                //m_strTemp += m_strLocal;
                string rep = m_decTemp.ToString(nmfi);
                string originalIntPortion;
                string originalDecPortion;
                int index = DecimalNumber.LastIndexOf(nmfi.NumberDecimalSeparator);
                
				if(index == -1)
				{

					originalIntPortion = DecimalNumber;
                    string m_strTemp = ""; ////SHASHWATA
                    m_strTemp = nmfi.NumberDecimalSeparator; ////SHASHWATA

                    for (int k = 0; k < Precision; k++)////SHASHWATA
                        m_strTemp += "0";////SHASHWATA

                    originalDecPortion = m_strTemp;////SHASHWATA

                    //originalDecPortion = nmfi.NumberDecimalSeparator + "0";
				}
				else
				{
					originalIntPortion = DecimalNumber .Substring (0,index);
                    //originalDecPortion = nmfi.NumberDecimalSeparator + DecimalNumber.Substring(index+1);
				}
                //return originalIntPortion + originalDecPortion;
                if (originalIntPortion == "") originalIntPortion = "0";

                string newIntPortion;
                index = rep.LastIndexOf(nmfi.NumberDecimalSeparator);
                if (index == -1)
                {

                    newIntPortion = rep;
                }
                else
                {
                    newIntPortion = rep.Substring(0, index);
                }

                if (newIntPortion == "") newIntPortion = "0";
                rep = prefix + rep.Replace(newIntPortion + nmfi.NumberDecimalSeparator, originalIntPortion + nmfi.NumberDecimalSeparator);
                return rep;
			}
			catch(SystemException)
			{
				return "";
			}
		}


		
		protected virtual void clsTxtBox_Leave(object sender,EventArgs e)
		{
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            //if (!this.Focused)
            //    return;
			if((isCurrency)&&(Type != TypeEnum .String ))
			{
				if(!OkString())
				{
					//ignore moving off cell if bad date
					return;
				}
			}
			if(this.Type ==TypeEnum.Float && this.AllowBlankInFloatValue == false)
			{
				bool txtch = this.TxtChange;
                this.Text = Execute(this.Text, this.DecimalPrecision);
				if(!txtch) this.RefreshChange();

			}
			else if(this.Type == TypeEnum.Integer)
			{
				bool txtch = this.TxtChange;
                if (this.Text == nmfi.NegativeSign)
					this.Text = "";
				if(!txtch) this.RefreshChange();

			}
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			blJustGotFocus = false;
			iBeforeTxtChange  = this.Text.Length;
			iSelectionStart = this.SelectionStart;
			iSelectionLength = this.SelectionLength;
			iAfterTxtChange = -1;

            if ((isCurrency) && (Type != TypeEnum.String))
            {
                if (!OkString())
                {
                    //ignore moving off cell if bad date
                    return;
                }
            }

			base.OnMouseDown(e);
		}

        public void ForceMouseDown()
        {
            blJustGotFocus = false;
            iBeforeTxtChange = this.Text.Length;
            iSelectionStart = this.SelectionStart;
            iSelectionLength = this.SelectionLength;
            iAfterTxtChange = -1;

            if ((isCurrency) && (Type != TypeEnum.String))
            {
                if (!OkString())
                {
                    //ignore moving off cell if bad date
                    return;
                }
            }
        }

		private bool OkString()
		{
            if (blJustGotFocus)
                return true;

			bool ok = true;
			string s = this.Text;
			
			try
			{
                if (edited)
                {
                   
                    int pos = this.SelectionStart;
                    this.entrycount = 1;
                    this.TextChanged -= new EventHandler(clsTxtBox_TextChanged);
                    this.Text=strInCommaFormat(s);
                    this.entrycount = 0;
                    edited = false;
                    this.TextChanged += new EventHandler(clsTxtBox_TextChanged);
                                
                    if (noofcommas == 0)
                    {
                        this.SelectionStart = pos;
                    }
                    else
                    {
                        this.SelectionStart = pos + noofcommas;
                    }
                    
                    
                    
                }

               
			}
			catch(Exception ex)
			{
				
				ok = false;
			}

			if(!isCurrency) return true;
			else
				return ok;
		}

		private string strInCommaFormat(string strParamTextinComma)
		{
            try
            {
               strstoretext=strParamTextinComma;
                noofcommas = 0;
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                string strTextinComma = strParamTextinComma;
                bool flag = false;
                string integerPart = "";
                string DecimalPart = "";

                int dotIndex = strTextinComma.LastIndexOf(Convert.ToChar(nmfi.NumberDecimalSeparator));

                if (dotIndex == -1)
                {
                    dotIndex = strTextinComma.Length;
                    DecimalPart = strTextinComma.Substring(dotIndex);
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    if (isCurrency && Type == TypeEnum.Float)
                    {
                        int dpres = this.DecimalPrecision;
                        string tmp = String.Empty;

                        for (int k = 0; k < dpres; k++)
                            tmp = tmp + "0";

                        DecimalPart = tmp;
                        flag = true;


                    }
                }
                else
                {


                    DecimalPart = strTextinComma.Substring(dotIndex + 1);
                    
                    if (this.DecimalPrecision < DecimalPart.Length)
                    {
                        DecimalPart=nmfi.NumberDecimalSeparator+DecimalPart;
                        decimal val = Convert.ToDecimal(DecimalPart);
                        int counter = 0;
                        int totalcount = DecimalPart.Length - this.DecimalPrecision;


                       
                        val = Math.Round(val, this.DecimalPrecision);
                        DecimalPart = val.ToString();
                        string[] pos = DecimalPart.Split(Convert.ToChar(nmfi.NumberDecimalSeparator));
                        DecimalPart = pos[1];

                    }
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    flag = true;
                }

                integerPart = integerPart.Replace(nmfi.NumberGroupSeparator, "");


                string prefix = "";
                if (_NegativeSign && integerPart.IndexOf(nmfi.NegativeSign) == 0)
                {
                    integerPart = integerPart.TrimStart(Convert.ToChar(nmfi.NegativeSign));
                    prefix = nmfi.NegativeSign;
                }
                int commasalreadyinintpart = this.find_no_occurences(strParamTextinComma, nmfi.NumberGroupSeparator);
                if (integerPart.Length >= this.CommaPrecision)
                {
                    int temp = integerPart.Length;
                    for (int k = this.CommaPrecision; k < temp; k += this.CommaPrecision)
                    {

                        integerPart = integerPart.Insert(temp - k, nmfi.NumberGroupSeparator);
                        noofcommas++;
                    }
                    noofcommas = noofcommas - commasalreadyinintpart;

                }

                if (flag)
                    strTextinComma = prefix + integerPart + nmfi.NumberDecimalSeparator + DecimalPart;
                else
                    strTextinComma = prefix + integerPart;
                edited = true;
                return strTextinComma;
            }
            catch (Exception ex)
            {
                return strstoretext;
            }
		}


        const int WM_KEYDOWN = 0x100;
        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_KEYDOWN && !OkString())
            {
                //ignore the key if bad date
                return true;
            }

            return base.ProcessKeyPreview(ref m);
        }

        protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
        {
        if ((isCurrency) && (Type != TypeEnum.String))
            {
                if (keyData == Keys.Tab)
                {
                    iAfterTxtChange = this.Text.Length;

                    if (!OkString())
                    {
                        //ignore the key if bad date
                        return true;
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }







		#endregion


        private bool _AllowBlankInFloatValue = false;

        [Browsable(true), Description("If set to true, Float Text Box can be left blank. Otherwise, it is set to .00"), RefreshProperties(RefreshProperties.All)]
        public bool AllowBlankInFloatValue
        {
            get
            {
                return _AllowBlankInFloatValue;
            }
            set
            {
                _AllowBlankInFloatValue = value;
            }
        }



		string strResName="";
		public enum TypeEnum
		{
			Integer,
			Float,
			String
		};

		private string m_strMsgID = "";
		private TypeEnum m_Type = TypeEnum.String;
		private int		 m_intDecimalPrecision = 2;
		private int		 m_intIntegerPrecision = 5;

		//user variable
		private int intDotIndex = -1;
		private bool _NegativeSign;

		private bool boolTextChange = false;

		public clsTxtBox()
		{
			_NegativeSign = false;
			this.KeyPress += new KeyPressEventHandler(KeyPressHandle);
			
			#region For Currency Implementation
			this.Leave +=new EventHandler(clsTxtBox_Leave);
			this.KeyUp +=new KeyEventHandler(clsTxtBox_KeyUp);
			this.TextChanged +=new EventHandler(clsTxtBox_TextChanged);

			#endregion

		}

		public bool TxtChange
		{
			get{return boolTextChange;}
		}

		public void RefreshChange()
		{
			boolTextChange = false;
		}

        protected override void OnTextChanged(EventArgs e)
        {

            if (this.entrycount == 1 )
            {
                this.entrycount = 0;
                return;
            }
            edited = true;
            boolTextChange = true;
            clsTxtBox_TextChanged(this, e);
            base.OnTextChanged(e);
        }

		public TypeEnum Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
			}
		}

		public bool AllowMinus
		{
			get{return _NegativeSign;}
			set{_NegativeSign = value;}
		}

		public string MessageID
		{
			get
			{
				return m_strMsgID;
			}
			set
			{
				m_strMsgID = value;
			}
		}
				
		public int DecimalPrecision
		{
			get
			{
				return m_intDecimalPrecision;
			}
			set
			{
				m_intDecimalPrecision = value;
                if (this.Type == TypeEnum.Float && this.AllowBlankInFloatValue == false)
                {
                    if (m_intDecimalPrecision > 0)
                    {
                        CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                        NumberFormatInfo nmfi = new NumberFormatInfo();
                        nmfi = ci.NumberFormat;
                        ci.ClearCachedData();
                        string m_strTemp;
                        if (this.Text.IndexOf(nmfi.NumberDecimalSeparator) >= 0)
                        {
                            Decimal m_decTemp = Math.Round(Convert.ToDecimal(this.Text, nmfi), m_intDecimalPrecision);
                            this.Text = m_decTemp.ToString();
                        }
                        else
                        {
                            m_strTemp = nmfi.NumberDecimalSeparator;
                            for (int i = 0; i < m_intDecimalPrecision; i++)
                                m_strTemp += "0";
                            this.Text += m_strTemp;
                        }
                    }
                }
			}
		}

		public int IntegerPrecision
		{
			get
			{
				return m_intIntegerPrecision;
			}
			set
			{
				m_intIntegerPrecision = value;
				IntegerPrecisionIncludedComma = m_intIntegerPrecision;
			}
		}

		/*
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			clsBCMessage objMessage = new clsBCMessage(this.m_strMsgID);

			if (objMessage.Message == "The message corresponding to message ID :  does not exist.")
				this.Text = "*" + this.Text + "*";
			else
				this.Text = objMessage.Message;

			objMessage.Dispose();

		}
*/
		bool blJustGotFocus = false;
		protected override void OnGotFocus(EventArgs e)
		{
            blJustGotFocus = true; 
			base.OnGotFocus (e);
		}

		int iBeforeTxtChange = 0, iSelectionStart = 0, iSelectionLength = 0, iAfterTxtChange = 0;
		protected override void OnKeyDown(KeyEventArgs e)
		{
            if (ICTEAS.WinForms.Common.clsCommonInfo.ProcessInProgress)
            {
                e.Handled = true;
                return;
            }
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            blJustGotFocus = false;
			blJustGotFocus = false;
			iBeforeTxtChange  = this.Text.Length;
			iSelectionStart = this.SelectionStart;
			iSelectionLength = this.SelectionLength;
			iAfterTxtChange = -1;

			if(e.KeyData == Keys.Delete)
			{
				if(m_Type == TypeEnum.String)
				{	
					base.OnKeyDown (e);
					return;
				}

				if( m_Type == TypeEnum.Integer )
				{
					if( _isCurrency )
					{
						if (this.SelectionStart > 0
							&& this.Text.Length > this.SelectionStart
                            && this.Text.Substring(this.SelectionStart, 1) == nmfi.NumberGroupSeparator
							)
						{
							iSelectionStart = this.SelectionStart = this.SelectionStart + 1;
							e.Handled = true;
							return;
						}
					}
					base.OnKeyDown(e);
					return;
				}


                if ((intDotIndex = this.Text.IndexOf(nmfi.NumberDecimalSeparator)) >= 0)
				{
					if(this.SelectionStart > intDotIndex)
					{
						if(this.SelectionStart >= intDotIndex + m_intDecimalPrecision + 1)
							e.Handled = true;
						else
						{
                            iSelectionLength = this.SelectionLength = 1;
                            this.SelectedText = "0";
							e.Handled = true;
						}
					}
					else if(this.SelectionStart == intDotIndex)
					{
                        iSelectionStart = this.SelectionStart = this.SelectionStart + 1;
						e.Handled = true;
					}
					else if( _isCurrency )
					{
                        if (this.SelectionStart > 0
                            && this.Text.Length > this.SelectionStart
                            && this.Text.Substring(this.SelectionStart, 1) == nmfi.NumberGroupSeparator
                            )
                        {
                            base.OnKeyDown(e);
                            iSelectionStart = this.SelectionStart = this.SelectionStart - 1;
                        }
                        else
                            iSelectionStart = this.SelectionStart + 1;
					}
					else
					{
						base.OnKeyDown(e);
						return;
					}
				}

			}
			else
				base.OnKeyDown(e);
            
		}


		protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{
            if (ICTEAS.WinForms.Common.clsCommonInfo.ProcessInProgress)
            {
                e.Handled = true;
            }
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
			if (m_Type == TypeEnum.String)
			{
				//e.Handled = false;
				base.OnKeyPress(e);
			}
			else
			{
                char[] arrDecimalSeperator = nmfi.NumberDecimalSeparator.ToCharArray();
                char chrDecimalSeperator = arrDecimalSeperator[0];
                if (char.IsNumber(e.KeyChar) || (e.KeyChar == 8) || (e.KeyChar == chrDecimalSeperator) || (e.KeyChar == 45))
				{
					if(e.KeyChar == 45)
					{
						if(_NegativeSign) 
						{
                            if (this.Text.IndexOf(nmfi.NegativeSign) == -1 && this.SelectionStart == 0)
							{
								base.OnKeyPress(e);
							}
							else
								e.Handled = true;
						}
						else
						{
							e.Handled = true;
						}	
					}
						//handle DOT
                    else if (e.KeyChar == chrDecimalSeperator)
                    {
                        if (m_Type == TypeEnum.Float)
                        {
                            //decimal allowed
                            if (this.Text.IndexOf(nmfi.NumberDecimalSeparator) == -1)
                            {
                                CutString();
                                //e.Handled = false;
                                base.OnKeyPress(e);
                            }
                            else
                            {
                                //already "." found
                                e.Handled = true;
                                iSelectionStart = this.SelectionStart = this.Text.IndexOf(nmfi.NumberDecimalSeparator) + 1;
                            }
                        }
                        else
                            e.Handled = true;
                    }
					else
					{
						//handle NUMBERs and BACKSPACE
						
						//allow backspace
						if (e.KeyChar == 8)
						{
							if(m_Type == TypeEnum.Float)
							{
                                if ((intDotIndex = this.Text.IndexOf(nmfi.NumberDecimalSeparator)) >= 0)
								{
                                    if (this.SelectionLength > 0)
                                    {
                                        string m_strTemp = this.SelectedText;
                                        int m_intTemp = m_strTemp.IndexOf(nmfi.NumberDecimalSeparator);
                                        if (m_intTemp >= 0)
                                        {
                                            this.SelectedText = "";
                                            //this.SelectionStart = this.SelectionStart;
                                        }
                                        else if (this.SelectionStart > intDotIndex)
                                        {
                                            int m_intSelectionStart = this.SelectionStart;
                                            m_strTemp = "";
                                            for (int i = 0; i < this.SelectionLength; i++)
                                                m_strTemp += "0";
                                            this.SelectedText = m_strTemp;

                                            iSelectionLength = m_strTemp.Length;
                                            iSelectionStart = this.SelectionStart = m_intSelectionStart - 1;
                                        }
                                        else
                                        {
                                            CutString();
                                            base.OnKeyPress(e);
                                        }
                                    }
                                    else if (this.SelectionStart >= intDotIndex + 1)
                                    {
                                        if (this.SelectionStart > intDotIndex + 1)
                                        {
                                            if (this.SelectionStart != 0)
                                            {
                                                iSelectionStart = this.SelectionStart = this.SelectionStart - 1;
                                                this.SelectionLength = 1;
                                                this.SelectedText = "0";
												if( !this.isCurrency )
													iSelectionStart = this.SelectionStart = this.SelectionStart - 1;
											}
                                            e.Handled = true;
                                        }
                                        else
                                        {
                                            if (this.SelectionStart != 0)
                                            {
                                                iSelectionStart = this.SelectionStart = this.SelectionStart - 1;
                                            }
                                            e.Handled = true;
                                        }
                                    }
                                    else
                                    {
                                        if (isCurrency && this.SelectionStart > 0 )
                                        {
                                            if (this.Text.Substring(this.SelectionStart - 1, 1) == nmfi.NumberGroupSeparator)
                                            {
                                                iSelectionStart = this.SelectionStart = this.SelectionStart - 2;
                                                iSelectionLength = this.SelectionLength = 2;
                                            }

                                        }
                                    }
								}
								else
								{
									CutString();
									base.OnKeyPress(e);
								}
							}

							// Integer
							else
							{
								if (isCurrency && this.SelectionStart > 0 )
								{
                                    if (this.Text.Substring(this.SelectionStart - 1, 1) == nmfi.NumberGroupSeparator)
									{
										iSelectionStart = this.SelectionStart = this.SelectionStart - 2;
										iSelectionLength = this.SelectionLength = 2;
									}
								}
								CutString();
								base.OnKeyPress(e);
							}
						}

						// Not backspace
						else
						{
							// Cannot replace selected text along with '.'
							// if the whole text is not selected
                            if (this.SelectedText.IndexOf(nmfi.NumberDecimalSeparator) > -1)
								if( this.SelectedText != this.Text )
								{
									this.SelectionLength = 0;
									e.Handled = true;
									return;
								}

							if (this.Text.Length <= this.MaxLength - 1)//m_intIntegerPrecision - 1)
							{
								//If Type is Float and . is already there
                                if ((intDotIndex = this.Text.IndexOf(nmfi.NumberDecimalSeparator)) >= 0)
								{
									string str = this.Text.TrimStart(Convert.ToChar(nmfi.NegativeSign)).Substring(0, intDotIndex).TrimEnd(Convert.ToChar(nmfi.NumberDecimalSeparator));
									if(_NegativeSign)
									{
                                        if (this.SelectionStart == 0 && this.Text.IndexOf(nmfi.NegativeSign) == 0 && this.SelectedText.IndexOf(nmfi.NegativeSign) < 0)
										{
											e.Handled = true;
											return;
										}
									}
									
									if(intDotIndex == 0)
									{
										if(this.SelectionStart <= intDotIndex + m_intDecimalPrecision && this.SelectionStart > intDotIndex)
										{
											iSelectionLength = this.SelectionLength = 1;
											base.OnKeyPress(e);
										}
										else if(this.SelectionStart == 0)
										{
											base.OnKeyPress(e);
										}
										else
											e.Handled = true;
									}
									else if ( str.Length >= IntegerPrecisionIncludedComma 
										&& this.SelectionStart < intDotIndex 
										&& this.SelectedText.Replace(nmfi.NumberGroupSeparator,"").Length < 1 )
									{
										e.Handled = true;
										return;
									}
									else if (str.Length < IntegerPrecisionIncludedComma && this.SelectionStart == intDotIndex)
									{
										CutString();
										base.OnKeyPress(e);
									}
									else if (str.Length >= IntegerPrecisionIncludedComma && this.SelectionStart == intDotIndex)
									{
										iSelectionStart = this.SelectionStart = intDotIndex + 1;
										if(this.DecimalPrecision == this.Text.Substring(intDotIndex+1).Length)
										{
											e.Handled = true;
										}
										
									}
									else if(this.Text.Substring(intDotIndex+1).Length >= m_intDecimalPrecision && this.SelectionStart > intDotIndex && this.SelectionStart < intDotIndex + m_intDecimalPrecision + 1)
									{
										int posn = this.SelectionStart;
										if(isCurrency)
										{
                                            if (this.Text.Substring(this.SelectionStart, 1) == nmfi.NumberGroupSeparator)
											{
												iSelectionStart  = this.SelectionStart = this.SelectionStart + 1;
												iSelectionLength = this.SelectionLength = 1;

												e.Handled = true;
												return;
											}
											
										}
										iSelectionLength = this.SelectionLength = 1;
										base.OnKeyPress(e);
									}
									else if(this.SelectionStart == intDotIndex + m_intDecimalPrecision + 1)
										e.Handled = true;
									else
									{
										CutString();
										base.OnKeyPress(e);
									}
								}

								// Eiher Integer or Float without '.'
								else if(this.Text.TrimStart(Convert.ToChar(nmfi.NegativeSign)).Length >= IntegerPrecisionIncludedComma)
								{
									if( this.SelectedText.Replace(nmfi.NumberGroupSeparator,"").Length < 1 )
										e.Handled = true;

									#region changed

									if((Type != TypeEnum .Integer))
									{
										this.Text += nmfi.NumberDecimalSeparator;
										iSelectionStart = this.SelectionStart = this.Text.Length;
									}
									
									#endregion
								}

								// Eiher Integer or Float without '.'
								else if(_NegativeSign)
								{
									if( this.SelectionStart == 0 && this.Text.IndexOf(nmfi.NegativeSign) == 0 && this.SelectedText.IndexOf(nmfi.NegativeSign) < 0 )
									{
										e.Handled = true;
										return;
									}
								}

								else
								{
									CutString();
									//e.Handled = false;
									base.OnKeyPress(e);
								}
							}

							// Exceeds MaxLength
							else
							{
								//Integer precesion in over::Check decimal precision
								//if dot is already there
								if ((intDotIndex = this.Text.IndexOf(nmfi.NumberDecimalSeparator)) >= 0)
								{
									string str = this.Text.Substring(intDotIndex+1);
									if(intDotIndex == 0)
									{
										base.OnKeyPress(e);
									}
                                    else if (this.Text.TrimStart(Convert.ToChar(nmfi.NegativeSign)).Substring(0, intDotIndex).Length >= IntegerPrecisionIncludedComma && this.SelectionStart == intDotIndex)
                                    {
                                        iSelectionStart = this.SelectionStart = intDotIndex + 1;
                                    }
                                    else
                                    {
                                        CutString();
                                        base.OnKeyPress(e);
                                    }
								}
								else
								{
									CutString();
									base.OnKeyPress(e);
								}
							}
						}
					}
				}
				
				// Not a numeric character. Don't allow
				else
					e.Handled = true;
			}
		}

		private void CutString()
		{
            if (this.Text.Length == this.MaxLength && this.SelectedText == "")
            {
                this.SelectionLength = 1;
                iSelectionLength = 1;
            }
			//this.Cut();
			//this.Text = this.Text.Remove(this.SelectionStart,this.SelectionLength);
		}
		
		private void CutString(KeyPressEventArgs e)
		{
            if (this.Text.Length == this.MaxLength && this.SelectedText == "")
            {
                this.SelectionLength = 1;
                iSelectionLength = 1;
            }
			if(this.SelectionLength > 0)
			{
				base.OnKeyPress(e);
				//CutString();
				//e.Handled = false;
			}
			else
				e.Handled = true;
		}

		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}

		private void KeyPressHandle(object sender, KeyPressEventArgs e)
		{
			if(m_Type == TypeEnum.Integer || m_Type == TypeEnum.Float)
                if (this.Text.Length == this.MaxLength && this.SelectedText == "")
                {
                    this.SelectionLength = 1;
                    iSelectionLength = 1;
                }
		}

        private int find_no_occurences(string input, string seperator)
        {
            try
            {
                char c = Convert.ToChar(seperator);
                char[] total = input.ToCharArray();
                int count=0;
                foreach (char m in total)
                {
                    if (c == m)
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
	}
}
