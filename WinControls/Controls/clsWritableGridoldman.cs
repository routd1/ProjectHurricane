using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using ICTEAS.WinForms.Common;
using ICTEAS.DataComponents.Custom;
using System.Globalization;


namespace ICTEAS.WinForms.Controls
{

	/// <summary>
	/// Summary description for clsWritableGrid.
	/// </summary> #define MAXSTRINLENGTH =1000;
	public class clsWritableGridoldman	:	System.Windows.Forms.DataGrid
	{


       


		
        private int _colReplacing = -1;
		private int intLoadRowCount=0;
		private int intTotalRowCount=0;
		private int intColCount=0;
		
		private const int MAXSTRLENGTH = 1000;
		private string strSQLProcName;
		private bool boolIsSQL = true;

		private string[] strColNames;
		private string[] strHeadings;
		private string[] strLangIDs;

        private System.Data.DataView dvSource;

        public MyGridTextBoxColumn[] dgTxt;
        public KeyTrapTextBox[] DataGridTxtBox;
        public DataGridComboBoxColumn[] dgComboBox;
        public DataGridDateTimePicker[] dgDateTimePicker;
        public DataGridButtonColumn[] dgButton;
		
		private DataGridTableStyle dgTabSt;
		
		private DataSet dsSource;
		private DataTable dtSource;

		private ArrayList arlOutParam;
        private ArrayList ProcParamNames = new ArrayList();
        private ArrayList ProcParamValues;
        private ArrayList ProcParamDir;

		private int intNumComboCols;
		private ArrayList arlComboCols;
		private string[][] arrComboValues;
		
		private int intNumDateTimeCols = 0;
		private int intNumButtonCols = 0;

		private string[] strProcOutValue;

		private int[] intDisableColumnNum, intReadOnlyColumnNum, intRightAlignColumnNum;
		private int[] intHiddenCols, intShowCols, intDateTimeColumnNum, intButtonColumnNum;
		
		private ArrayList arlNumericColumNum, arlNumericColumnType, arlNumericColIntSize, arlNumericColFloatSize;
        private ArrayList arlNumericColCurrency, arlStringColumsStrLength, arlStringColumnCollection;

		private int _toggleRow = -1, _toggleCol = -1;
		private ArrayList arlEnableColumns = new ArrayList();
		private ArrayList arlEnableRows = new ArrayList();

		private int noOfRowsToInspectForColumnAutoSizing = 15;
		private bool _boolPrivScreen = false;

		private Point pointInCell00;
        public int store_rowno=1;

		private string[] strLangID = new string[0];

		public clsWritableGridoldman()
		{
			intLoadRowCount = 0;
			intColCount = 0;
			intTotalRowCount = 0;
			
			this.RowHeaderWidth = 35;

			arlNumericColFloatSize = new ArrayList();
			arlNumericColIntSize = new ArrayList();
			arlNumericColumnType = new ArrayList();
			arlNumericColumNum = new ArrayList();
			arlNumericColCurrency = new ArrayList ();
			arlStringColumsStrLength = new ArrayList ();
			arlStringColumnCollection = new ArrayList ();
        }

        #region Replace in ReadOnly Column
        public void ReplaceInReadOnlyColumn(int RowNum, int ColNum, string ReplaceString)
        {
            //this.CurrentCell = new DataGridCell(RowNum,ColNum); // Infinite Loop
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            this.CurrentRowIndex = RowNum;

            _colReplacing = ColNum;
            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                string rep = "";
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];
                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    rep = m_decTemp.ToString(nmfi);

                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                }
                else
                    rep = ReplaceString;

                MyGridTextBoxColumn objGridTextCol = (MyGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[ColNum];
                //objGridTextCol.KeyTextBox.Modified=true;

                //objGridTextCol.KeyTextBox.TextChanged -= new EventHandler(TextBox_TextChanged);

                objGridTextCol.KeyTextBox.Text = rep;
                dvSource[RowNum][ColNum] = rep;

                //objGridTextCol.KeyTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
            }
            else if (CheckComboBoxColumn(ColNum, ref m_intColPos))
            {
                DataGridComboBoxColumn objGridComboCol = (DataGridComboBoxColumn)this.TableStyles[0].GridColumnStyles[ColNum];
                //objGridComboCol.TextBox.Modified = true;
                //objGridComboCol.TextBox.TextChanged+=new EventHandler(TextBox_TextChanged); 

                //objGridComboCol.TextBox.Text = ReplaceString;
                objGridComboCol.ColumnComboBox.Text = ReplaceString;

                dvSource[RowNum][ColNum] = ReplaceString;
            }
            else if (CheckDateTimeColumn(ColNum, ref m_intColPos))
            {
                DataGridDateTimePicker objGridDTPCol = (DataGridDateTimePicker)this.TableStyles[0].GridColumnStyles[ColNum];
                //objGridDTPCol.TextBox.Modified = true;
                //objGridDTPCol.TextBox.TextChanged += new EventHandler(TextBox_TextChanged);

                //objGridDTPCol.TextBox.Text = ReplaceString;
                objGridDTPCol.ColumnDateTimePicker.Text = ReplaceString;

                dvSource[RowNum][ColNum] = ReplaceString;
            }
            else
            {
                MyGridTextBoxColumn objGridTextCol = (MyGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[ColNum];
                //objGridTextCol.TextBox.Modified=true;

                //objGridTextCol.TextBox.Text = ReplaceString;            //Added - 11/03/2006 - Indranil
                //objGridTextCol.KeyTextBox.TextChanged -= new EventHandler(TextBox_TextChanged); 

                //objGridTextCol.TextBox.Text = ReplaceString;
                objGridTextCol.KeyTextBox.Text = ReplaceString;

                dvSource[RowNum][ColNum] = ReplaceString;

                //objGridTextCol.KeyTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
            }

            _colReplacing = -1;
            this.SetRowStatus(RowNum);
            this.SetTxtChange();
            dvSource.Table.AcceptChanges();
            this.Refresh();
        }
        #endregion

		#region textChange

		private bool boolTextChange = false;
        public void SetTxtChange()
        {
            boolTextChange = true;
        }
		public bool TxtChange
		{
			get{return boolTextChange;}
		}
		public void RefreshChange()
		{
			boolTextChange = false;
		}
		#endregion

		public string[] LangIDs
		{
			get
			{
				return strLangID;
			}
			set
			{
				strLangID = value;
			}
		}

		/// <summary>
		/// String Array that gives the translated column header names
		/// </summary>
		public string[] ColumnHeader
		{
			get
			{
				return strHeadings;
			}
		}

        /// <summary>
        /// RowStatus will return the status of the RowNumber -th row.
        /// If the Row is loaded and remains unchanged, it will return "0".
        /// If added, or added and then modified during the session, it will return "1".
        /// If loaded and then modified during the session, it will return "2"
        /// </summary>
        public string RowStatus(int RowNumber)
        {
            if (RowNumber >= 0 && dvSource != null && RowNumber < dvSource.Count)
                return Convert.ToString(dvSource[RowNumber][intColCount]);
            else
                return "0";
        }

        public void SetRowStatus(int RowNumber)
        {
            if (RowNumber >= 0 && dvSource != null && RowNumber < dvSource.Count)
            {
                orgStatus = dvSource[RowNumber][intColCount].ToString();
                if (orgStatus == "0" || orgStatus == "2")
                    newStatus = "2";
                else
                    newStatus = "1";
                dvSource[RowNumber][intColCount] = newStatus;
            }
        }


		/// <summary>
		/// This will return initial row count (ie. when loaded)
		/// </summary>
		public int InitialRowCount
		{
			get
			{
				return intLoadRowCount;
			}
		}

        public void RemoveSort()
        {
            if (dvSource != null)
            {
                dvSource.Sort = "";
                dvSource.Table.AcceptChanges();
            }
        }

		/// <summary>
		/// This will return current row total.
		/// </summary>
		public int RowCount
		{
			get
			{
                if (dvSource == null)
                {
                    intTotalRowCount = 0;
                }
                else
                {
                    intTotalRowCount = dvSource.Count;
                }
				return intTotalRowCount;
			}
		}

		/// <summary>
		/// This will return total number of columns.
		/// </summary>
		public int ColumnCount
		{
			get
			{
				return intColCount;
			}
		}
		
		public bool IsPrivilegeScreen
		{
			get{return _boolPrivScreen;}
			set{_boolPrivScreen = value;}
		}

		/// <summary>
		/// This Property will return the executed Procedure's OUT values as string Array.
		/// </summary>
		public string[] ProcedureOUTvalues
		{
			get{return strProcOutValue;}
		}

		/// <summary>
		/// This Method is to set the Grid to execute Stored Procedure.
		/// </summary>
		public string StoredProcedureName
		{
			get{return strSQLProcName;}
			set
			{
                strSQLProcName = value;
                boolIsSQL = false;
                arlOutParam = new ArrayList();
                ProcParamNames = new ArrayList();
                ProcParamValues = new ArrayList();
                ProcParamDir = new ArrayList();
            }	
		}

		/// <summary>
		/// Enter the total number of combo box columns in the Datagrid.
		/// </summary>
		public int NumberOfComboColumns
		{
			get{return intNumComboCols;}
			set
			{
				intNumComboCols = value;
				arrComboValues = new string[intNumComboCols][];
			}
		}

		/// <summary>
		/// Set the Particular Combo Box Column in DataGrid
		/// </summary>
		/// <param name="ColumnNo">Combo Column Number in Datagrid as integer</param>
		/// <param name="ComboValues">Combo Box Values as string array</param>
		public void SetComboColumn(int ColumnNo,string[] ComboValues)
		{
			try
			{
				if(arlComboCols == null)
					arlComboCols = new ArrayList();

				arlComboCols.Add(ColumnNo);
				arrComboValues[arlComboCols.Count - 1] = ComboValues;
				
			}
			catch(Exception){ ; }
		}
		
		
		public int NumberOfRowsToInspectForColumnAutoSizing
		{
			get
			{
				return noOfRowsToInspectForColumnAutoSizing;
			}
			set
			{
				if (value > 15)
					noOfRowsToInspectForColumnAutoSizing = value;
				else
					noOfRowsToInspectForColumnAutoSizing = 15;
			}
		}

		
		private int FirstColumn()
		{
			if(intShowCols == null)
				return -1;

			return intShowCols[0];
		}

		private bool IsLastColumn(int ColNum)
		{
            if (ColNum >= intColCount)
				return true;
			
			return false;
		}

        private bool IsLastRow(int RowNum)
        {
            if (RowNum >= intTotalRowCount - 1)
                return true;

            return false;
        }

        private bool IsLastCell(int RowNum, int ColNum)
		{
            if (RowNum >= intTotalRowCount - 1 && ColNum >= intColCount)
                return true;

			return false;
		}

		public void FocusCell(int RowNum, int ColNum)
		{
            if(this.CurrentCell.RowNumber == RowNum && this.CurrentCell.ColumnNumber == ColNum)
                this.CurrentCell = new DataGridCell(RowNum,this.ColumnCount - 1);
			this.CurrentCell = new DataGridCell(RowNum,ColNum);
		}


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{

            this.Refresh();
            if(CurrentRowIndex >= 0)
			{
				if(this.IsRowSelected())
				{
					if(keyData == Keys.Delete)
						return true;
                  
				}
                //added by Nilanjan
                if (keyData == Keys.Right || keyData == Keys.Left)
                {
                    int col1 = this.CurrentCell.ColumnNumber;
                    if (_DisableImmdCols != null)
                    {
                        if (CheckDisableImmediate(col1))
                        {

                            return false;

                        }
                    }
                    else
                    {
                        if (intDisableColumnNum != null)
                        {
                            for (int w = 0; w < intDisableColumnNum.Length; w++)
                            {
                                if (intDisableColumnNum[w] == col1)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (keyData == Keys.Up || keyData == Keys.Down)
                {
                    if (this.store_rowno == 0)
                    {
                        return false;
                    }
                    int col1 = this.CurrentCell.ColumnNumber;
                    int row1 = this.CurrentCell.RowNumber;
                    if (row1 == 0)
                    {
                        this.store_rowno = 1;
                    }
                    else
                    {
                        this.store_rowno = row1;
                    }
                    if (_DisableImmdCols != null)
                    {
                        if (CheckDisableImmediate(col1))
                        {

                            return false;

                        }
                    }
                    else
                    {
                        if (intDisableColumnNum != null)
                        {
                            for (int w = 0; w < intDisableColumnNum.Length; w++)
                            {
                                if (intDisableColumnNum[w] == col1)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    dvSource.Table.Rows[row1][col1] = this.newValue;
                }
                
                //
			}
            
			return base.ProcessCmdKey (ref msg, keyData);
		}

		private bool IsRowSelected()
		{
			if(this.RowCount < 0)
				return false;

			for(int i=0; i < RowCount; i++)
			{
				if(this.IsSelected(i))
					return true;
			}

			return false;
		}

		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) 
		{ 
			DataGrid.HitTestInfo hti = this.HitTest(new Point(e.X, e.Y)); 
			if(hti.Type == DataGrid.HitTestType.RowResize) 
			{ 
				return; //no baseclass call 
                //base.OnAutoSizeChanged(e);
			} 
			if(e.Button != MouseButtons.Left)
				base.OnMouseMove(e); 
			
			else
				return;

		} 

		public delegate void ColumnHeaderEventHandler(object sender, MouseEventArgs e,HitTestInfo htI);
		public event ColumnHeaderEventHandler ColumnHeaderClick;
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid.HitTestInfo hti = this.HitTest(new Point(e.X, e.Y)); 

			this.Refresh();

			if(hti.Type == DataGrid.HitTestType.RowResize) 
			{ 
				if(e.Button != MouseButtons.Left)
					base.OnMouseDown(e);
				else
					return;
			}
            

            if (hti.Type == DataGrid.HitTestType.ColumnHeader && e.Button == MouseButtons.Right) 
			{
                if (ColumnHeaderClick != null)
                {
                    ColumnHeaderClick(this, e, hti);
                    return;
                }
			}

           
            
            base.OnMouseDown(e);
		}

		/// <summary>
		/// This Method is to Set the Numeric Columns in the Grid.
		/// </summary>
		/// <param name="ColumnNumber">Enter the column Number</param>
		/// <param name="ColumnType">The Column Type (Integer,Float,String)</param>
		/// <param name="IntegerPrecision">Enter the number of digits as integer</param>
		/// <param name="DecimalPrecision">Enter the number of digits as decimal</param>
		/// <returns>True if Numeric Columns are set</returns>
		/// 


		private int getMaxStrLength(int col)
		{
			for(int i=0;i<arlStringColumnCollection.Count;i++)
			{
				if(arlStringColumnCollection[i].ToString () == col.ToString ())
				{
					return Convert.ToInt32(arlStringColumsStrLength[i].ToString());
				}
			}
			return MAXSTRLENGTH;
		}
		public bool SetStringColumn(int ColumnNumber,int MaxLength)
		{
			try
			{
				arlStringColumnCollection.Add(ColumnNumber);
				arlStringColumsStrLength.Add(MaxLength);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		public bool SetNumericColumn(int ColumnNumber,clsTxtBox.TypeEnum ColumnType,int IntegerPrecision, int DecimalPrecision)
		{
			try
			{
				arlNumericColumNum.Add(ColumnNumber);
				arlNumericColumnType.Add(ColumnType);
				arlNumericColIntSize.Add(IntegerPrecision);
				arlNumericColFloatSize.Add(DecimalPrecision);
				arlNumericColCurrency .Add (false);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		public bool SetNumericColumn(int ColumnNumber,clsTxtBox.TypeEnum ColumnType,int IntegerPrecision, int DecimalPrecision,bool isCurrency)
		{
			try
			{
				arlNumericColumNum.Add(ColumnNumber);
				arlNumericColumnType.Add(ColumnType);
				arlNumericColIntSize.Add(IntegerPrecision);
				arlNumericColFloatSize.Add(DecimalPrecision);
				arlNumericColCurrency .Add (isCurrency);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		
		public bool SetRefreshColumns()
		{
			try
			{
				arlNumericColumNum.Clear();
				arlNumericColumnType.Clear();
				arlNumericColIntSize.Clear();
				arlNumericColFloatSize.Clear();
				arlNumericColCurrency .Clear();
				intDisableColumnNum=null;
				intReadOnlyColumnNum= null; 
				intRightAlignColumnNum= null;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Set the column numbers you want to disable when grid is loaded.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set Multiple column numbers</param>
		/// <returns>True if it sets properly</returns>
		public bool SetDisableColumns(params int[] ColumnNums)
		{
			try
			{
				intDisableColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the column numbers you want to make read only when grid is loaded.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set the muliple column numbers</param>
		/// <returns>True if it sets properly</returns>
		public bool SetReadOnlyColumns(params int[] ColumnNums)
		{
			try
			{
				intReadOnlyColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the column numbers you want to make the text right aligned.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set the multiple column numbers</param>
		/// <returns>True if it sets properly</returns>
		public bool SetRightAlignColumns(params int[] ColumnNums)
		{
			try
			{
				intRightAlignColumnNum = ColumnNums;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the column numbers you want to show Date Time Picker.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set the multiple column numbers ',' separated</param>
		/// <returns>True if it sets properly</returns>
		public bool SetDateTimeColumns(params int[] ColumnNums)
		{
			try
			{
				intDateTimeColumnNum = ColumnNums;
				intNumDateTimeCols = ColumnNums.Length;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the column numbers you want to show Button.
		/// Column number starts from 0 (including MASK columns)
		/// </summary>
		/// <param name="ColumnNums">Set the multiple column numbers ',' separated</param>
		/// <returns>True if it sets properly</returns>
		public bool SetButtonColumns(params int[] ColumnNums)
		{
			try
			{
				intButtonColumnNum = ColumnNums;
				intNumButtonCols = ColumnNums.Length;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Set the Language ID / Message ID for the Grid Column Header text.
		/// </summary>
		/// <param name="LangIDs">Set multiple Message IDs one by one.</param>
		/// <returns>True if it sets properly</returns>
		public bool SetLangIDs(params string[] LangIDs)
		{
			try
			{
				strLangIDs = LangIDs;
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// This method is to set the Input/Output Parameters of Stored Procedure.
		/// </summary>
		/// <param name="ProcParamName">Procedure Pararmeter Name</param>
		/// <param name="ProcParamVal">Procedure Parameter Value, If it is OUT then enter ""</param>
		/// <param name="strIN_OUT">IN for Input Parameter, OUT for Output Parameter</param>
		public void SetParam(string ProcParamName,string ProcParamVal,string strIN_OUT)
		{
            if (strIN_OUT.ToUpper() == "IN")
            {
                ProcParamNames.Add(ProcParamName);
                ProcParamValues.Add(ProcParamVal.Trim());
            }
            else
            {
                ProcParamNames.Add(ProcParamName);
                ProcParamValues.Add(null);
                arlOutParam.Add(ProcParamName);
            }
        }

		/// <summary>
		/// This Method is to Design the Grid with Stored Procedure.
		/// </summary>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True if Datagrid is design correctly.</returns>
		public bool DesignGrid(ref string strErrMsg)
		{
			try
			{
				this.ReadOnly = false;
				
				if(ExecuteProcedure(strSQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}
				
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		protected override void OnDataSourceChanged(EventArgs e)
		{
			dtSource = (DataTable)this.DataSource;						//INDRANIL
			
			if( dtSource != null )										//INDRANIL
			{
                dvSource = dtSource.DefaultView;
                dvSource.ListChanged += new ListChangedEventHandler(dvSource_ListChanged);
                dvSource.AllowNew = false;
                intTotalRowCount = dvSource.Count;					//INDRANIL

			}

			base.OnDataSourceChanged (e);
		}

		/// <summary>
		/// This is the method to design the writable grid against the SQL Query.
		/// </summary>
		/// <param name="SQLProcName">Enter SQL query</param>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True if it executed properly.</returns>
		public bool DesignGrid(string SQLProcName,ref string strErrMsg)
		{
			try
			{
				this.ReadOnly = false;
				
				strSQLProcName = SQLProcName;
				boolIsSQL = true;

				if(ExecuteProcedure(SQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + " - " + ex.Source;
				return false;
			}
		}



		/// <summary>
		///This method is used to refresh the grid values.
		/// </summary>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True if it refreshed successfully.</returns>
		
		public bool PopulateGrid(ref string strErrMsg)
		{
			try
			{
				if(ExecuteProcedure(strSQLProcName,boolIsSQL,ref strErrMsg) == false)
				{
					return false;
				}
				if(SetColumns(ref strErrMsg) == false)
				{
					return false;
				}
				if(DesignTableStyle(ref strErrMsg) == false)
				{
					return false;
				}

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		private bool ExecuteProcedure(string SQLProcName,bool isSQL,ref string strErrMsg)
		{
			try
			{
                Query objQuery = new Query();

                if (!isSQL)
                {
                    string[] arrStrProcParamNames = new string[0];
                    string[] arrStrProcParamValues = new string[0];
                    if (ProcParamNames.Count - arlOutParam.Count > 0)
                    {
                        arrStrProcParamNames = new string[ProcParamNames.Count - arlOutParam.Count];
                        arrStrProcParamValues = new string[ProcParamNames.Count - arlOutParam.Count];
                        for (int i = 0; i < ProcParamNames.Count; i++)
                        {
                            if (ProcParamValues[i] != null)
                            {
                                arrStrProcParamNames[i] = ProcParamNames[i].ToString();
                                arrStrProcParamValues[i] = ProcParamValues[i].ToString();
                            }
                        }
                        objQuery.SetInputParameterNames(SQLProcName, arrStrProcParamNames);
                        objQuery.SetInputParameterValues(arrStrProcParamValues);
                    }

                    arrStrProcParamNames = new string[arlOutParam.Count];
                    arrStrProcParamValues = new string[arlOutParam.Count];
                    int iOutParamIndex = 0;
                    for (int i = 0; i < ProcParamNames.Count; i++)
                    {
                        if (ProcParamValues[i] == null)
                        {
                            arrStrProcParamNames[iOutParamIndex++] = ProcParamNames[i].ToString();
                        }
                    }
                    objQuery.SetOutputParameterNames(SQLProcName, arrStrProcParamNames);

                    dsSource = objQuery.ExecuteQueryProcedure(SQLProcName);
                }
                else
                {
                    dsSource = objQuery.ExecuteQueryCommand(SQLProcName);
                }

                dtSource = dsSource.Tables[0].Copy();
                dsSource.Dispose();

                intColCount = dtSource.Columns.Count;
                strColNames = new string[intColCount];

                for (int i = 0; i < intColCount; i++)
                    strColNames[i] = dtSource.Columns[i].ColumnName;


                if (isSQL == false)
                {
                    strProcOutValue = new string[arlOutParam.Count];
                    for (int i = 0; i < arlOutParam.Count; i++)
                    {
                        strProcOutValue[i] = objQuery.GetOutputParameterValue(arlOutParam[i].ToString());
                    }
                }


                dtSource = dsSource.Tables[0];

                dvSource = dtSource.DefaultView;
                dvSource.ListChanged += new ListChangedEventHandler(dvSource_ListChanged);
                dvSource.AllowNew = false;
                intTotalRowCount = dvSource.Count;					//INDRANIL

                dtSource.Columns.Add("MASK_LOAD_COL_VALUE");
				
				for(int i=0; i<dtSource.Rows.Count; i++)
                    dtSource.Rows[i]["MASK_LOAD_COL_VALUE"] = "0";

				intColCount = dtSource.Columns.Count - 1;
				
				MyGridTextBoxColumn._RowCount = dtSource.Rows.Count;

				ChangeDataTable();

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + " - " + ex.Source;
				return false;
			}
		}

		private void ChangeDataTable()
        {
            #region commented code
            //try//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //{//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    int m_intColPos = -1;//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    for (int i = 0; i < dvSource.Count; i++)//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    {//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //        if (CheckNumericColumn(i, ref m_intColPos))//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //        {//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //            clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //            if (objEnum == clsTxtBox.TypeEnum.Float)//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //            {//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                int m_intFloatSize = (int)arlNumericColFloatSize[m_intColPos];//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                string m_strTemp = "0.";//This Area Was Uncommented Considering code of 26/12/2006 3:51PM

            //                for (int k = 0; k < m_intFloatSize; k++)//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                    m_strTemp += "0";//This Area Was Uncommented Considering code of 26/12/2006 3:51PM

            //                for (int j = 0; j < dvSource.Count; j++)//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                {//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                    if (dvSource[j][i].ToString() != "")//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                    {//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        decimal m_decTemp = Math.Round(Convert.ToDecimal(dvSource[j][i].ToString()), m_intFloatSize);//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        string rep = m_decTemp.ToString("###" + m_strTemp + ";");//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        string strFloat = dvSource[j][i].ToString();//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        int index = strFloat.LastIndexOf(".");//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        if (index == -1) index = 0;//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        string originalIntPortion = strFloat.Substring(0, index);//This Area Was Uncommented Considering code of 26/12/2006 3:51PM

            //                        if (originalIntPortion == "") originalIntPortion = "0";//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        index = rep.LastIndexOf(".");//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        if (index == -1) index = 0;//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        string nextIntPortion = rep.Substring(0, index);//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        if (nextIntPortion == "") nextIntPortion = "0";//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        rep = rep.Replace(nextIntPortion, originalIntPortion);//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                        dvSource[j][i] = rep;//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                    }//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //                }//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //            }//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //        }//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    }//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    dvSource.Table.AcceptChanges();//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //}//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //catch (Exception ex)//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //{//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    MessageBox.Show(ex.Source + " - " + ex.Message);//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //    return;//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
            //}//This Area Was Uncommented Considering code of 26/12/2006 3:51PM
#endregion

            try
            {
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                int m_intColPos = -1;
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    if (CheckNumericColumn(i, ref m_intColPos))
                    {
                        clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];
                        if (objEnum == clsTxtBox.TypeEnum.Float)
                        {
                            int m_intFloatSize = (int)arlNumericColFloatSize[m_intColPos];
                            for (int j = 0; j < dvSource.Count; j++)
                            {
                               // changed on 14/05/07 by Nilanjan (inclusion of group seperarors in wrong location
                                if (dvSource[j][i].ToString() != "")
                                {

                                    string prefix = "";
                                    string strFloat = dvSource[j][i].ToString();
                                    strFloat = strFloat.Replace(".", nmfi.NumberDecimalSeparator);
                                    decimal m_decTemp = Math.Round(Convert.ToDecimal(strFloat, nmfi), m_intFloatSize);
                                    string rep = m_decTemp.ToString(nmfi);
                                    int groupsize = nmfi.NumberGroupSizes[0];
                                    int dotIndex = rep.LastIndexOf(Convert.ToChar(nmfi.NumberDecimalSeparator));
                                    string DecimalPart = "", integerPart;
                                    if (dotIndex == -1)
                                    {
                                        dotIndex = rep.Length;
                                        DecimalPart = rep.Substring(dotIndex);
                                        integerPart = rep.Substring(0, dotIndex);

                                        int dpres = nmfi.NumberDecimalDigits;
                                        string tmp = String.Empty;

                                        for (int k = 0; k < dpres; k++)
                                            tmp = tmp + "0";

                                        DecimalPart = tmp;


                                    }
                                    else
                                    {

                                        DecimalPart = rep.Substring(dotIndex+1);
                                        integerPart = rep.Substring(0, dotIndex);
                                        
                                        if (integerPart.Contains(nmfi.NegativeSign))
                                        {
                                            prefix = "-";
                                            integerPart = integerPart.Remove(0, 1);
                                        }

                                        if (integerPart.Length >= nmfi.NumberDecimalDigits)
                                        {
                                            int temp = integerPart.Length;
                                            for (int k = groupsize; k < temp; k += groupsize)
                                            {

                                                integerPart = integerPart.Insert(temp - k, nmfi.NumberGroupSeparator);

                                            }
                                        }

                                        rep = prefix + integerPart + nmfi.NumberDecimalSeparator + DecimalPart;


                                    }
                                    //.......

                                    dvSource[j][i] = rep;
                                }
                            }
                        }
                    }
                    
                }
                dvSource.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + " - " + ex.Message);
                return;
            }
		}

		private bool SetColumns(ref string strErrMsg)
		{
			try
			{
				strColNames = new string[dtSource.Columns.Count];
				strHeadings = new string[dtSource.Columns.Count];

				for(int i=0; i < dtSource.Columns.Count; i++)
				{
					strColNames[i] = dtSource.Columns[i].ColumnName;
					strHeadings[i] = strColNames[i].Replace("_"," ") + " ";
				}
				ChangeLanguage();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message; 
				return false;
			}
		}

		private void ChangeLanguage()
		{
			try
			{
                string strTmp = "";
                clsLanguage objLang = new clsLanguage();
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    if (strLangID[i] != null)
                    {
                        strTmp = objLang.LanguageString(strLangID[i]);

                        if (strTmp != "*****")
                            strHeadings[i] = strTmp + " ";
                    }
                }
                objLang.Dispose();
            }
            catch (Exception ex)
			{
				string test = ex.Message;
			}
		}

		#region Enabling or Disabling Cell
		ArrayList cellDisableList = new ArrayList();
//		public void SetCellDisable(int row,int col)
//		{
//			DataGridCellDisableEnableEventArgs temp = new DataGridCellDisableEnableEventArgs(row,col);
//			cellDisableList.Add(temp);
//
//		}


		#endregion


		//changing cell color
		ArrayList cellColorList = new ArrayList ();
		public void SetCellColor(int row,int col,Brush brus)
		{
			
			DataGridCellColorEventArgs temp = new DataGridCellColorEventArgs (row,col,brus);
			cellColorList.Add (temp);
			

		}

		public void SetRowColor(int row,Brush brus)
		{
			
			for(int i=0;i<ColumnCount ;i++)
			{
				DataGridCellColorEventArgs temp = new DataGridCellColorEventArgs (row,i,brus);
				cellColorList.Add (temp);
				
			}
		}
		public void RemoveCellColor(int row,int col)
		{
			
			for(int i=0;i<cellColorList.Count;i++)
			{
				DataGridCellColorEventArgs t =(DataGridCellColorEventArgs) cellColorList[i];
				if((t.Row == row)&&(t.Column == col))
				{
					cellColorList.RemoveAt (i);
					i--;
				}
			}
		}

		public void RemoveRowColor(int row)
		{
			for(int i=0;i<cellColorList.Count;i++)
			{
				DataGridCellColorEventArgs t =(DataGridCellColorEventArgs) cellColorList[i];
				if(t.Row == row)
				{
					cellColorList.RemoveAt (i);
					i--;
				}
			}
		}

		private void dgTBCol_CheckCellColor(object sender,DataGridCellColorEventArgs e)
		{
			if(cellColorList .Count != 0)
			{
				for(int k=0;k<cellColorList.Count ;k++)
				{
					DataGridCellColorEventArgs  objTemp =(DataGridCellColorEventArgs) cellColorList[k];
					if((e.Column ==objTemp.Column  )&&(e.Row ==objTemp.Row))
					{
						e.EnableColor = objTemp.EnableColor ;
					}
					objTemp = null;
				}
			}
			
		}

		#region Changing for Individual Column Sizing
		ArrayList listOfColumnsForResizing= new ArrayList ();
		public void SetWidthOfColumn(int ColoumnIndex,int Width)
		{
			colWidth objColWidth = new colWidth(ColoumnIndex,Width);
			listOfColumnsForResizing.Add (objColWidth);
		}

		#endregion

		#region for row status

		//private int lastEditedRow = -1;
		//private int lastEditedCol = -1;

        private string GetNewValue(int col, object sender)
		{
			if(CheckComboBoxColumn(col))
			{

                DataGridComboBoxColumn tbc = (DataGridComboBoxColumn)sender;
				
				if(tbc != null)
				{
					
					return tbc.TextBox.Text;
				}
			}
			else if(CheckDateTimeColumn(col))
			{
                DataGridDateTimePicker tbc = (DataGridDateTimePicker)sender;
				if(tbc != null && tbc.ColumnDateTimePicker.TxtChange)
				{
					return tbc.TextBox.Text;
				}


			}
			else
			{

                TextBox tb = (TextBox)sender;
				if(tb != null)
				{
					return tb.Text;
				}
			}

			return null;
		}

        string oldValue = "", newValue = "", orgStatus = "", newStatus = "";
		private void TextBox_TextChanged(object sender, EventArgs e)
		{
            try
            {
                this.Refresh();
                DataGridCell dgcc = this.CurrentCell;
                //TextBox tb = null;

                if (_colReplacing == -1)
                {
                    _colReplacing = dgcc.ColumnNumber;
                }

                newValue = GetNewValue(_colReplacing, sender);

                if (newValue != null)
                {
                    oldValue = this[dgcc].ToString();
                    orgStatus = dvSource[dgcc.RowNumber][intColCount].ToString();
                    
                    //newValue = tb.Text;
                    //if ((dgcc.RowNumber != lastEditedRow || _colReplacing != lastEditedCol) && orgStatus == "0")
                    //{
                    if (newValue != oldValue)
                    {
                        if(orgStatus == "0")
                            dvSource[dgcc.RowNumber][intColCount] = "2"; //newStatus;
                            //dvSource[dgcc.RowNumber][dgcc.ColumnNumber] 
                        //if (orgStatus == "0" || orgStatus == "2")
                        //    newStatus = "2";
                        //else
                        //    newStatus = "1";

                        boolTextChange = true;

                        //lastEditedRow = dgcc.RowNumber;
                        //lastEditedCol = _colReplacing;
                    }
                    //}
                }

                _colReplacing = -1;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
		}



		#endregion

		private bool DesignTableStyle(ref string strErrMsg)
		{
			try
			{
				string m_strMaskCols = "", m_strShowCols = "";

				dgTabSt=new DataGridTableStyle();
				dgTabSt.MappingName=dtSource.TableName;
				
				dgTabSt.PreferredRowHeight=15;

				////**** If the Default design is false then setting Colors for that Grid
				
				dgTabSt.HeaderBackColor=System.Drawing.Color.Lavender;
				dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
				dgTabSt.GridLineColor=System.Drawing.Color.Silver;
				
				for(int i=0; i < dtSource.Columns.Count; i++)
				{
					int local0 = 0;

					if(CheckComboBoxColumn(i))
					{
						DataGridComboBoxColumn dgCBCol = new DataGridComboBoxColumn(i);

						dgCBCol.MappingName = strColNames[i];
						dgCBCol.HeaderText = strHeadings[i];
                        dgCBCol.Alignment = HorizontalAlignment.Center; //Added by Nilanjan 14/05/07
						dgCBCol.ColumnComboBox.DataSource = GetComboValue(i);
						arlEnableColumns = new ArrayList();
						arlEnableRows = new ArrayList();

						if(CheckReadOnlyColumn(i))	
						{
							dgCBCol.ReadOnly = true;
							dgCBCol.TextBox.TabStop = false;
						}
						
						dgCBCol.CheckCellEnabled += new EnableCellEventHandler(SetEnableValues);
						//changing color

						dgCBCol.CheckCellColor +=new EnableCellColorEventHandler(dgTBCol_CheckCellColor);
						dgCBCol.TextChangedCombo  += new EventHandler(TextBox_TextChanged);
                        //dgCBCol.TextBox.TextChanged+=new EventHandler(TextBox_TextChanged);  //added By Nilanjan 14/05/07

						dgTabSt.GridColumnStyles.Add(dgCBCol);
						
						dgCBCol.Dispose();
					}
					else if(CheckDateTimeColumn(i))
					{
						DataGridDateTimePicker dgDTPCol = new DataGridDateTimePicker(i);
						
						dgDTPCol.MappingName = strColNames[i];
						dgDTPCol.HeaderText = strHeadings[i];

						if(CheckReadOnlyColumn(i))	
						{
							dgDTPCol.ReadOnly = true;
							dgDTPCol.TextBox .TabStop = false;
						}

						dgDTPCol.CheckCellEnabled += new EnableCellEventHandler(SetEnableValues);
						//changing color

						dgDTPCol.CheckCellColor +=new EnableCellColorEventHandler(dgTBCol_CheckCellColor);
						dgDTPCol.TextChangedDTP +=new EventHandler(TextBox_TextChanged);

						
						dgTabSt.GridColumnStyles.Add(dgDTPCol);
						dgDTPCol.Dispose();
					}
					else if(CheckButtonColumn(i))
					{
						DataGridButtonColumn dgBtnCol = new DataGridButtonColumn(i);

						dgBtnCol.MappingName = strColNames[i];
						dgBtnCol.HeaderText = strHeadings[i];
						
						dgTabSt.GridColumnStyles.Add(dgBtnCol);
						dgBtnCol.Dispose();
					}
					else
					{
						MyGridTextBoxColumn dgTBCol=new MyGridTextBoxColumn(i);
					
						dgTBCol.MappingName=strColNames[i];
						dgTBCol.HeaderText=strHeadings[i];
						
						if(_boolPrivScreen)
						{
							delegateGetColorRowCol d = new delegateGetColorRowCol(MyGetColorRowCol);
							dgTBCol.GetColorRowCol = d;
						}

						if(CheckReadOnlyColumn(i))	
						{
							dgTBCol.ReadOnly = true;
							dgTBCol.TextBox.TabStop = false;
						}
					
                        if(CheckRightAlignColumn(i))
                            dgTBCol.Alignment = HorizontalAlignment.Right; 
						if(CheckNumericColumn(i,ref local0))
						{
							dgTBCol.TextType = TxtType(arlNumericColumnType[local0].ToString());
							dgTBCol.IntPrecision = Convert.ToInt32(arlNumericColIntSize[local0].ToString());
							dgTBCol.DecPrecision = Convert.ToInt32(arlNumericColFloatSize[local0].ToString());
							dgTBCol.IsCurrency  = Convert.ToBoolean (arlNumericColCurrency[local0].ToString());
							
						}
						else
						{
							dgTBCol.TextType = clsTxtBox.TypeEnum.String;
							dgTBCol.TextBox.MaxLength = getMaxStrLength(i); 
						}
						dgTBCol.CheckCellEnabled += new EnableCellEventHandler(SetEnableValues);
						//changing color

						dgTBCol.CheckCellColor +=new EnableCellColorEventHandler(dgTBCol_CheckCellColor);
						dgTBCol .KeyTextBox.TextChanged +=new EventHandler(TextBox_TextChanged);

						dgTabSt.GridColumnStyles.Add(dgTBCol);
						dgTBCol.Dispose();
					}
				}

				for(int i=0; i < dtSource.Columns.Count; i++)
				{
					if(strColNames[i].Length >= 4)
					{
						if(strColNames[i].Substring(0,4).ToUpper()=="MASK")
						{
							m_strMaskCols += i.ToString() + ",";
							dgTabSt.GridColumnStyles[i].Width=0;
						}
						else
						{
							m_strShowCols += i.ToString() + ",";
						}
					}
					dgTabSt.GridColumnStyles[i].NullText = "";
				}
			

				#region changing for ColSizing

				for(int i=0;i<listOfColumnsForResizing .Count ;i++)
				{
					colWidth obj = (colWidth)listOfColumnsForResizing[i];
					dgTabSt.GridColumnStyles[obj.ColumnIndex].Width=obj.ColumnWidth;

				}
				
			

				#endregion


				if(m_strMaskCols.Trim() != "")
				{
					m_strMaskCols = m_strMaskCols.Substring(0,m_strMaskCols.Length-1);
					string[] local0 = m_strMaskCols.Split(',');
					
					intHiddenCols = new int[local0.Length];

					for(int i=0;i<local0.Length;i++)
						intHiddenCols[i] = Convert.ToInt32(local0[i]);

					Array.Sort(intHiddenCols);
				}

				if(m_strShowCols.Trim() != "")
				{
					m_strShowCols = m_strShowCols.Substring(0,m_strShowCols.Length-1);
					string[] local0 = m_strShowCols.Split(',');

					intShowCols = new int[local0.Length];

					for(int i=0;i<local0.Length;i++)
						intShowCols[i] = Convert.ToInt32(local0[i]);

					Array.Sort(intShowCols);
				}

				this.TableStyles.Clear();
				this.TableStyles.Add(dgTabSt);

				this.SetDataBinding( dtSource, "" );					//INDRANIL
				this.Refresh();

				intLoadRowCount = dtSource.Rows.Count;
				intTotalRowCount = intLoadRowCount;

				dgTxt=new MyGridTextBoxColumn[dtSource.Columns.Count];
				DataGridTxtBox=new KeyTrapTextBox[dtSource.Columns.Count];
				dgComboBox = new DataGridComboBoxColumn[intNumComboCols];
				dgDateTimePicker = new DataGridDateTimePicker[intNumDateTimeCols];
				dgButton = new DataGridButtonColumn[intNumButtonCols];

				int m_intComboCount = 0, m_intDTPCount = 0, m_intBtnCount = 0;

				for(int i=0;i<dtSource.Columns.Count;i++)
				{
					if(CheckComboBoxColumn(i))
					{
						dgComboBox[m_intComboCount] =(DataGridComboBoxColumn)this.TableStyles[0].GridColumnStyles[i];
						++m_intComboCount;
					}
					else if(CheckDateTimeColumn(i))
					{
						dgDateTimePicker[m_intDTPCount] = (DataGridDateTimePicker)this.TableStyles[0].GridColumnStyles[i];
						++m_intDTPCount;
					}
					else if(CheckButtonColumn(i))
					{
						dgButton[m_intBtnCount] = (DataGridButtonColumn) this.TableStyles[0].GridColumnStyles[i];
						++m_intBtnCount;
					}
					else
					{
						dgTxt[i]=(MyGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[i];
						DataGridTxtBox[i]= dgTxt[i].KeyTextBox;
					}
				}

                dvSource = ((DataTable)this.DataSource).DefaultView;
                dvSource.ListChanged += new ListChangedEventHandler(dvSource_ListChanged);

                this.DoubleBuffered = true;
				
				this.Paint += new PaintEventHandler(RowHeaderPaint);
				
				if(this.RowCount > 0)
					pointInCell00 = new Point(this.GetCellBounds(0,0).X + 4, this.GetCellBounds(0,0).Y + 4);

				this.Scroll += new EventHandler(HorizontalScroll);

                //this.AllowSorting = true;

				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		private void HorizontalScroll(object sender, EventArgs e)
		{
			this.Select();
		}

		private int TopRow()
		{
			if(pointInCell00.ToString() == null)
				pointInCell00 = new Point(this.GetCellBounds(0,0).X + 4, this.GetCellBounds(0,0).Y + 4);
			
			DataGrid.HitTestInfo hti = this.HitTest(pointInCell00);
			return hti.Row;
		}


		private void RowHeaderPaint(object sender,PaintEventArgs e)
		{
			try
			{
				if(this.RowCount <= 0)
					return;

				int row = TopRow(); 
			
				if(row < 0)
					row = 0;

				int yDelta = this.GetCellBounds(row,0).Height + 1; 
				int y = this.GetCellBounds(row,0).Top + 2; 

				while(y < this.Height - yDelta && row < this.RowCount ) 
				{ 
					string text = string.Format("  {0}", row + 1); 
					e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.DarkBlue),12, y); 
					y += yDelta; 
					row++; 
				} 
			}
			catch(Exception ex )
			{
				string err = ex.ToString();
			}
		}

		/// <summary>
		///To enable the columns in the datagrid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void SetEnableValues(object sender, DataGridEnableEventArgs e)
		{
			if(CheckDisableColumn(e.Column) && e.Row < intLoadRowCount)
			{
				e.EnableValue = false;
			}
			else
			{
				e.EnableValue = true;
			}

			#region For Cell Enabling and Disabling


//			for(int i=0; i < cellDisableList.Count; i++)
//			{
//				DataGridCellDisableEnableEventArgs obj = (DataGridCellDisableEnableEventArgs)cellDisableList[i];
//
//				if((obj.Column == e.Column) && ( obj.Row == e.Row))
//				{
//					e.EnableValue = false;
//				}
//			}
//			

			#endregion
			
		}

		/// <summary>
		///Replace the current datagrid cell with ReplaceString.
		/// </summary>
		/// <param name="ReplaceString">String to be replaced.</param>

		public void ReplaceValue(string ReplaceString)
		{
            dvSource[this.CurrentRowIndex][this.CurrentCell.ColumnNumber] = ReplaceString;
            dvSource.Table.AcceptChanges();
            this.Refresh();
		}

		/// <summary>
		///Replace the datagrid cell with ReplaceString.
		/// </summary>
		/// <param name="RowNum">Datagrid Row Number</param>
		/// <param name="ColNum">Datagrid Column Number</param>
		/// <param name="ReplaceString">String to be replaced.</param>
		public void ReplaceValue(int RowNum, int ColNum,string ReplaceString)
		{
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            NumberFormatInfo nmfi = new NumberFormatInfo();
            nmfi = ci.NumberFormat;
            ci.ClearCachedData();
            int m_intColPos = -1;
            if (CheckNumericColumn(ColNum, ref m_intColPos))
            {
                clsTxtBox.TypeEnum objEnum = (clsTxtBox.TypeEnum)arlNumericColumnType[m_intColPos];


                if (objEnum == clsTxtBox.TypeEnum.Float)
                {
                    #region Float
                    if (ReplaceString == "")
                        ReplaceString = "0";
                    int m_intPrecision = (int)arlNumericColFloatSize[m_intColPos];

                    Decimal m_decTemp = Math.Round(Convert.ToDecimal(ReplaceString, nmfi), m_intPrecision);
                    string m_strTemp = "0", m_strLocal = nmfi.NumberDecimalSeparator;

                    for (int k = 0; k < m_intPrecision; k++)
                        m_strLocal += "0";

                    m_strTemp += m_strLocal;
                    //string rep = m_decTemp.ToString("###" + m_strTemp + ";",nmfi); //###" + m_strTemp + ";"+ m_strLocal);
                    string rep = m_decTemp.ToString(nmfi); //###" + m_strTemp + ";"+ m_strLocal);

                    //int ind = ReplaceString.LastIndexOf(".");//Change On 19/1/2007
                    int ind = ReplaceString.LastIndexOf(nmfi.NumberDecimalSeparator);
                    if (ind < 0)
                    {
                    }
                    else
                    {
                        string originalIntPortion = ReplaceString.Substring(0, ind);
                        if (originalIntPortion == "") originalIntPortion = "0";
                        string nextIntPortion = rep.Substring(0, rep.LastIndexOf(nmfi.NumberDecimalSeparator));
                        if (nextIntPortion == "") nextIntPortion = "0";
                        rep = rep.Replace(nextIntPortion, originalIntPortion);
                    }
                    dvSource[RowNum][ColNum] = rep;

                    #endregion
                }

                else
                {
                    dvSource[RowNum][ColNum] = ReplaceString;
                }
            }
            else
            {
                dvSource[RowNum][ColNum] = ReplaceString;
            }
            dvSource.Table.AcceptChanges();
            this.Refresh();
		}
		/// <summary>
		///Replace the datagrid cell with ReplaceString.
		/// </summary>
		/// <param name="ColNum">Datagrid Column Number</param>
		/// <param name="ReplaceString">String to be replaced.</param>
		public void ReplaceValue(int ColNum,string ReplaceString)
		{
			if(this.CurrentRowIndex == -1)
				return;
            dvSource[this.CurrentRowIndex][ColNum] = ReplaceString;
            dvSource.Table.AcceptChanges();
            this.Refresh();
		}

		private bool CheckNumericColumn(int ColumnNum,ref int ColPosition)
		{
			for(int i=0; i < arlNumericColumNum.Count; i++)
			{
				if(arlNumericColumNum[i].ToString() == ColumnNum.ToString())
				{
					ColPosition = i;
					return true;
				}
			}
			ColPosition = -1;
			return false;
		}

		private bool CheckComboBoxColumn(int ColumnNum,ref int Pos)
		{
			Pos = -1;
			if(arlComboCols == null)
				return false;
			
			for(int i=0; i < arlComboCols.Count; i++)
			{
				if(arlComboCols[i].ToString() == ColumnNum.ToString())
				{
					Pos = i;
					return true;
				}
			}

			return false;
		}

		private bool CheckComboBoxColumn(int ColumnNum)
		{
			if(arlComboCols == null)
				return false;
			
			for(int i=0; i < arlComboCols.Count; i++)
			{
				if(arlComboCols[i].ToString() == ColumnNum.ToString())
					return true;
			}

			return false;
		}

		private bool CheckButtonColumn(int ColumnNum)
		{
			if(intButtonColumnNum == null)
				return false;

			for(int i=0; i < intButtonColumnNum.Length; i++)
			{
				if(intButtonColumnNum[i] == ColumnNum)
					return true;
			}

			return false;
		}
	
		private bool CheckDateTimeColumn(int ColumnNum, ref int Pos)
		{
			Pos = -1;
			if(intDateTimeColumnNum == null)
				return false;

			for(int i=0; i < intDateTimeColumnNum.Length; i++)
			{
				if(intDateTimeColumnNum[i] == ColumnNum)
				{
					Pos = i;
					return true;
				}
			}

			return false;
		}
	
		private bool CheckDateTimeColumn(int ColumnNum)
		{
			if(intDateTimeColumnNum == null)
				return false;

			for(int i=0; i < intDateTimeColumnNum.Length; i++)
				if(intDateTimeColumnNum[i] == ColumnNum)
					return true;

			return false;
		}

		private bool CheckReadOnlyColumn(int ColumnNum)
		{
			if(intReadOnlyColumnNum == null)
				return false;

			for(int i=0; i < intReadOnlyColumnNum.Length; i++)
				if(intReadOnlyColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private bool CheckRightAlignColumn(int ColumnNum)
		{
			if(intRightAlignColumnNum == null)
				return false;

			for(int i=0; i < intRightAlignColumnNum.Length; i++)
				if(intRightAlignColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private bool CheckDisableColumn(int ColumnNum)
		{
			if(intDisableColumnNum == null)
				return false;

			for(int i=0; i < intDisableColumnNum.Length; i++)
				if(intDisableColumnNum[i] == ColumnNum)
					return true;
			
			return false;		
		}

		private clsTxtBox.TypeEnum TxtType(string txtType)
		{
			if(txtType == clsTxtBox.TypeEnum.Integer.ToString())
				return clsTxtBox.TypeEnum.Integer;
			else if(txtType == clsTxtBox.TypeEnum.Float.ToString())
				return clsTxtBox.TypeEnum.Float;
			else
				return clsTxtBox.TypeEnum.String;
		}

		private string[] GetComboValue(int ColumnNum)
		{
			if(arlComboCols == null)
				return null;

			for(int i=0; i < arlComboCols.Count; i++)
			{
				if(arlComboCols[i].ToString() == ColumnNum.ToString())
				{
					return arrComboValues[i];
				}
			}

			return null;
		}

		private Color MyGetColorRowCol(int row, int col)
		{
			Color c = Color.Black;
			if(col != 2)
				return c;
			
			string m_strTemp = this.dvSource[row][3].ToString();
			if(m_strTemp != "1")
				c = Color.Blue;
			else
				c = Color.Red;

			m_strTemp = this.dvSource[row][5].ToString();

			if(m_strTemp.ToUpper() == "NA")
				c = Color.DarkGray;

			return c;
		}

		/// <summary>
		///Remove the selected items from datagrid.
		/// </summary>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True is removed successfully.</returns>
		public bool Remove(ref string strErrMsg)
		{
			try
			{
                ArrayList intNums = new ArrayList();

                for (int i = 0; i < dvSource.Count; i++)
                {
                    if (this.IsSelected(i))
                    {
                        intNums.Add(i);
                    }
                }

                if (intNums.Count > 0)
                {
                    intNums.Sort();
                    intNums.Reverse();

                    for (int i = 0; i < intNums.Count; i++)
                    {
                        dvSource.Delete(Convert.ToInt32(intNums[i]));
                    }
                }

                dvSource.Table.AcceptChanges();
                this.Refresh();

                return true;
            }
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		private bool IsFirstCell()
		{
			if(this.CurrentCell.RowNumber == 0 && this.CurrentCell.ColumnNumber == 0)
				return true;

			return false;
		}


		/// <summary>
		///Remove the row from datagrid.
		/// </summary>
		/// <param name="RowNum">The Row Number</param>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True if it removed.</returns>
		public bool Remove(int RowNum,ref string strErrMsg)
		{
			try
			{
                if ((dvSource.Count - 1) >= RowNum)
                {
                    dvSource.Delete(RowNum);
                    dvSource.Table.AcceptChanges();
                }
				else
				{
					strErrMsg = "The Row Number is not present in the datagrid.";
					return false;
				}
				
				this.Refresh();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		public bool RemoveAll(ref string strErrMsg)
		{
			try
			{
				strErrMsg = "";

				if( dtSource != null )
				{
					dtSource.Clear();

                    dtSource.AcceptChanges();

                    dvSource = dtSource.DefaultView;
                    dvSource.ListChanged += new ListChangedEventHandler(dvSource_ListChanged);
                    dvSource.AllowNew = false;
                }

				intTotalRowCount = 0;
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + "-" + ex.Source;
				return false;
			}
		}


		/// <summary>
		///Add a new row to the datagrid.
		/// </summary>
		/// <param name="strErrMsg">Error Message</param>
		/// <returns>True if row is added.</returns>
		public bool Add(ref string strErrMsg)
		{
			try
			{
				object[] obj = new object[dtSource.Columns.Count];
				for(int i=0; i < obj.Length; i++)
				{
					if(i != obj.Length - 1)
						obj[i] = null;
					else
						obj[i] = "1";
				}

                dvSource.Sort = "";

                dtSource.Rows.Add(obj);
                
                dvSource.Table.AcceptChanges();
				intTotalRowCount++;
			
				this.Refresh();
				return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Source + " - " + ex.Message;
				return false;
			}
		}

		public void DisableColumn(int RowNum,int ColNo)
		{
			int m_intPos = -1;
			_toggleCol = ColNo;
			_toggleRow = RowNum;
			int i = CheckDisableColumn(RowNum,ColNo);
			if( i != -1)
			{
				arlEnableColumns.RemoveAt(i);
				arlEnableRows.RemoveAt(i);
			}
			if(CheckComboBoxColumn(ColNo,ref m_intPos))
				dgComboBox[m_intPos].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
			else if(CheckDateTimeColumn(ColNo,ref m_intPos))
				dgDateTimePicker[m_intPos].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
			else
				dgTxt[ColNo].CheckCellEnabled += new EnableCellEventHandler(DisableCell);
			this.Refresh();
		}

		private int CheckDisableColumn(int RowNum,int ColNo)
		{
			for(int i=0; i < arlEnableColumns.Count; i++)
			{
				if(arlEnableColumns[i].ToString() == ColNo.ToString())
				{
					if(arlEnableRows[i].ToString() == RowNum.ToString())
						return i;
				}
			}
			return -1;
		}

		public void EnableColumn(int RowNum, int ColNo)
		{
			int m_intPos = -1;
			_toggleCol = ColNo;
			_toggleRow = RowNum;
			if(CheckDisableColumn(ColNo))
			{
				if(CheckEnableCols(RowNum,ColNo) == false)
				{
					arlEnableColumns.Add(ColNo);
					arlEnableRows.Add(RowNum);
				}
			}
			if(CheckComboBoxColumn(ColNo,ref m_intPos))
				dgComboBox[m_intPos].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
			else if(CheckDateTimeColumn(ColNo,ref m_intPos))
				dgDateTimePicker[m_intPos].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
			else
				dgTxt[ColNo].CheckCellEnabled += new EnableCellEventHandler(EnableCell);
			this.Refresh();
		}

		private int[] _DisableImmdCols;
		private int[] _EnableImmdCols;

		public void EnableImmediate(params int[] ColNum)
		{
			try
			{
				_EnableImmdCols = ColNum;
                

				for(int i=0; i < ColNum.Length ; i++)
				{
					if(CheckComboBoxColumn(ColNum[i]))
						dgComboBox[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(EnableImmediateCols);
					else if(CheckDateTimeColumn(ColNum[i]))
						dgDateTimePicker[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(EnableImmediateCols);
					else
						dgTxt[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(EnableImmediateCols);
				}

				this.Refresh();
			}
			catch(Exception)
			{
				return;
			}
		}

		public void DisableImmediate(params int[] ColNum)
		{
			try
			{
				_DisableImmdCols = ColNum;

				for(int i=0; i < ColNum.Length ; i++)
				{
					if(CheckComboBoxColumn(ColNum[i]))
						dgComboBox[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(DisableImmediateCols);
					else if(CheckDateTimeColumn(ColNum[i]))
						dgDateTimePicker[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(DisableImmediateCols);
					else
						dgTxt[ColNum[i]].CheckCellEnabled += new EnableCellEventHandler(DisableImmediateCols);
				}

                this.Refresh();
			}
			catch(Exception)
			{
				return;
			}
		}
		
		private bool CheckDisableImmediate(int ColNum)
		{
			for(int i=0; i < _DisableImmdCols.Length ; i++)
			{
				if(_DisableImmdCols[i] == ColNum)
					return true;
			}
			
			return false;
		}

		private bool CheckEnableImmediate(int ColNum)
		{
			for(int i=0; i < _EnableImmdCols.Length ; i++)
			{
				if(_EnableImmdCols[i] == ColNum)
					return true;
			}
			return false;
		}

		private void DisableImmediateCols(object sender, DataGridEnableEventArgs e)
		{
			if(CheckDisableImmediate(e.Column))
			{
				e.EnableValue = false;
			}
			else
			{
				e.EnableValue = true;
			}
		}	

		private void EnableImmediateCols(object sender, DataGridEnableEventArgs e)
		{
			if(CheckEnableImmediate(e.Column))
			{
				e.EnableValue = true;
			}
		}

		private bool CheckEnableCols(int RowNum,int ColNo)
		{
			for(int i=0; i < arlEnableColumns.Count; i++)
			{
				if(arlEnableColumns[i].ToString() == ColNo.ToString())
				{
					if(arlEnableRows[i].ToString() == RowNum.ToString())
						return true;
				}
			}
			return false;
		}

		private void DisableCell(object sender,DataGridEnableEventArgs e)
		{
			if((e.Column == _toggleCol && e.Row == _toggleRow) || (CheckEnableCols(e.Row,e.Column) == false))
				e.EnableValue = false;
			else
			{
				e.EnableValue = true;
			}
		}

		private void EnableCell(object sender,DataGridEnableEventArgs e)
		{
			if((e.Column == _toggleCol && e.Row == _toggleRow) || CheckEnableCols(e.Row,e.Column))
				e.EnableValue = true;
		}

		/// <summary>
		/// Autosize the DataGrid Cell
		/// </summary>
		public void AutoSizeCol() 
		{ 
			DataTable dt = ((DataTable)(this.DataSource));
			
            int intDgSize;
			int intTotalSize=0;

			if(dt.Rows.Count > 0)
				intDgSize = this.Width-70;
			else
				intDgSize = this.Width-150;

			for(int col=0 ; col<intColCount ; col++)
			{
				float width = 0; 
				int numRows = ((DataTable)(this.DataSource)).Rows.Count;
				Graphics g = Graphics.FromHwnd(this.Handle); 
				StringFormat sf = new StringFormat(StringFormat.GenericTypographic); 
				SizeF size; 

				//// To find the approximate length of the Rows
				//// Displayed in the Datagrid by checking the First 15 Rows length.
				if(this.TableStyles[0].GridColumnStyles[col].Width != 0)
				{
					for(int i = 0; i < numRows && i < noOfRowsToInspectForColumnAutoSizing; ++ i) 
					{ 
						size = g.MeasureString(this[i,col].ToString(), this.Font, 500, sf); 
						if(size.Width > width) 
							width = size.Width; 
					} 

					//set calculated width by above 15-rec-found technique
					this.TableStyles[0].GridColumnStyles[col].Width = (int) width + 8; // 8 is for leading and trailing padding 

					//calculate the size column header text
					size=g.MeasureString(this.TableStyles[0].GridColumnStyles[col].HeaderText.ToString(),this.Font,500,sf);
					int m_intTextLen=(int)size.Width;
					
					//compare with column data text
					int m_intDataLen=this.TableStyles[0].GridColumnStyles[col].Width;
					if(m_intTextLen > m_intDataLen)
						this.TableStyles[0].GridColumnStyles[col].Width = m_intTextLen + 8;
					intTotalSize += this.TableStyles[0].GridColumnStyles[col].Width;
				}

				//dispose graphics
				g.Dispose();

			}

			////**** If total width of the Datagrid is less than the Width occupied by Columns
			////**** Then resize the Column width to match Datagrid Width.

			if(intTotalSize < intDgSize )
			{
				for(int i=0; i < intColCount ; i++)
					if(this.TableStyles[0].GridColumnStyles[i].Width != 0)
						this.TableStyles[0].GridColumnStyles[i].Width = this.TableStyles[0].GridColumnStyles[i].Width + (intDgSize-intTotalSize)/intColCount;
			}
		}


        private void dvSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            intTotalRowCount = dvSource.Count;
            MyGridTextBoxColumn._RowCount = dvSource.Count;
        }

	}



	public class colWidth
	{
		private int colIndex;
		private int width;
		public colWidth(int ColumnIndex,int ColumnWidth)
		{
			colIndex = ColumnIndex ;
			width = ColumnWidth;
		}
		public int ColumnIndex
		{
			get { return colIndex;}
			set{ colIndex = value;}
		}
		public int ColumnWidth
		{
			get { return width;}
			set{ width = value;}
		}
	}
}
