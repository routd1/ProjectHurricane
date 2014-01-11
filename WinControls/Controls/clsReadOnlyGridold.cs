//using System;
//using System.Windows;
//using System.Windows.Forms;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using UniConvert;
//using ICTEAS.WinForms.Common;
//using ICTEAS.DataComponents.Custom;

//namespace ICTEAS.WinForms.Controls
//{
//    /// <summary>
//    /// Summary description for clsReadOnlyGrid.
//    /// </summary>	

//    public class clsReadOnlyGridold : System.Windows.Forms.DataGrid
//    {
//        //private clsSetResource objResource;
//        public bool SetDefault=true;

//        private bool m_DesignColor;
		
//        private bool boolLevelSearch = false;

//        private bool boolMutliLine = false;

//        private bool boolFirstRow = true;

//        private string strProcName = "";
//        private ArrayList ProcParamNames;
//        private ArrayList ProcParamValues;

//        private DataTable dtReadOnlyGrid;

//        public int intColCount;
//        public int intVisibleColumnCount;
//        private string strSQL;
//        private string[] strColNames;
//        public int RowCount;

//        private string[] strLangID = new string[0];

//        private int _MaxLevel = 0;
//        private int _CurrentLevel = 0;

//        private clsSearchGrid _dgThisSearchGrid;

//        private DataTable dtLevelData;

//        public DataGridTextBoxColumn[] dgText;
//        public TextBox[] tb;
		
//        private string strMsgID = "";

//        private string strLevelHier = "";
//        private string strValueHier = "";

//        private int _ReqColNo1 = -1;
//        private int _ReqColNo2 = -1;

//        private bool _DefaultSort = false;
//        private int _SortCol = 0;

//        private ArrayList arlHierarchyLevel;
//        private ArrayList arlHierarchyValue;
//        private ArrayList arlPrimKey;

//        private ContextMenu _ContxtMnu;
//        private MenuItem _mnuExportData;
//        frmHotSearch frmhot=null;
//        public new object this[int RowNumber,int ColNumber]
//        {
//            get
//            {
//                return ((DataTable)this.DataSource).Rows[RowNumber][ColNumber];
//            }
//            set
//            {
//                ((DataTable)this.DataSource).Rows[RowNumber][ColNumber] = value;
//            }
//        }

//        ////*****To set Font Size of Grid Data to regular 
//        ////*****(Design time Font Size of Data Grid to be bold)
//        public bool MultiLine
//        {
//            get{return boolMutliLine;}
//            set{boolMutliLine = value;}
//        }


//        public bool DefaultSort
//        {
//            get{return _DefaultSort;}
//            set{_DefaultSort = value;}
//        }


//        public int SortColumn
//        {
//            get{return _SortCol;}
//            set{_SortCol = value;}
//        }


//        public bool LevelSearch
//        {
//            get{return boolLevelSearch;}
//            set{boolLevelSearch = value;}
//        }


//        public int LevelHierarchyColumn1
//        {
//            get{return _ReqColNo1;}
//            set{_ReqColNo1 = value;}
//        }


//        public int LevelHierarchyColumn2
//        {
//            get{return _ReqColNo2;}
//            set{_ReqColNo2 = value;}
//        }


//        public string LevelHierarchy
//        {
//            get
//            {
//                if(arlHierarchyLevel != null)
//                {
//                    int i;
//                    strLevelHier = "";
//                    for(i=0; i < arlHierarchyLevel.Count; i++)
//                        strLevelHier += arlHierarchyLevel[i].ToString() + "--";
//                    if(strLevelHier != "")
//                        strLevelHier = strLevelHier.Substring(0,strLevelHier.Length-2);
//                }
//                else
//                    strLevelHier = "";
				
//                return strLevelHier;
//            }
//        }


//        public string ValueHierarchy
//        {
//            get
//            {
//                if(arlHierarchyValue != null)
//                {
//                    int i;
//                    strValueHier = "";
//                    for(i=0; i < arlHierarchyValue.Count; i++)
//                        strValueHier += arlHierarchyValue[i].ToString() + "--";
//                    if(strValueHier != "")
//                        strValueHier = strValueHier.Substring(0,strValueHier.Length-2);
//                }
//                else
//                    strValueHier = "";

//                return strValueHier;
//            }
//        }


//        public void RefreshRow()
//        {
//            boolFirstRow = true;
//            this.Refresh();
//        }


//        public clsSearchGrid AssociatedSearchGrid
//        {
//            set{ _dgThisSearchGrid = value; }
//            get{ return _dgThisSearchGrid; }
//        }


//        public string MessageID
//        {
//            get{return strMsgID;}
//            set{strMsgID = value;}
//        }


//        public int MaxLevel
//        {
//            get{return _MaxLevel;}
//        }


//        public int CurrentLevel
//        {
//            get{return _CurrentLevel;}
//        }


//        public string CurrentID
//        {
//            get
//            {
//                if(arlPrimKey == null)
//                    return "";
//                return arlPrimKey[_CurrentLevel-2].ToString();
//            }
//        }


//        public string[] LangIDs
//        {
//            get
//            {
//                return strLangID;
//            }
//            set
//            {
//                strLangID = value;
//            }
//        }


//        private void FormatGridCells(object sender, DataGridFormatCellEventArgs e)
//        {
//            e.TextFont = new Font(e.TextFont.Name, e.TextFont.Size, FontStyle.Regular);
//        }
		

//        protected override void OnDataSourceChanged(EventArgs e)
//        {
//            if(_DefaultSort)
//            {
//                ((DataTable)this.DataSource).DefaultView.Sort = ((DataTable)this.DataSource).Columns[_SortCol].ColumnName + " ASC ";
//            }

//            if( this.LevelSearch == true )
//            {
//                this.Tag = this.DataSource;
//            }

//            base.OnDataSourceChanged (e);
//        }


//        ////*****To disable Row Resizing of Data Grid

//        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) 
//        { 
//            try
//            {
//                DataGrid.HitTestInfo hti = this.HitTest(new Point(e.X, e.Y)); 
//                if(hti.Type == DataGrid.HitTestType.RowResize) 
//                { 
//                    return; //no baseclass call 
//                } 
//                if(e.Button != MouseButtons.Left)
//                    base.OnMouseMove(e); 
//                else
//                    return;
//            }
//            catch( Exception ex )
//            {
//                MessageBox.Show( ex.ToString() );
//            }
//        } 


//        ////*****To disable Row Resizing of Data Grid

//        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
//        {
//            try
//            {

               
//                DataGrid.HitTestInfo hti = this.HitTest(new Point(e.X, e.Y)); 
			
//                if(e.Button == MouseButtons.Right)
//                {
//                    base.OnMouseDown(e);
//                    return;
//                }

//                if(hti.Type == DataGrid.HitTestType.RowResize) 
//                { 
//                    return; //no baseclass call 
//                } 
			
//                if(boolMutliLine == false && (hti.Type == DataGrid.HitTestType.Cell || hti.Type == DataGrid.HitTestType.RowHeader))
//                {
//                    //added by Nilanjan 16/05/07
                   
//                        CurrencyManager cmrecent = (CurrencyManager)this.BindingContext[this.DataSource, this.DataMember];
//                        DataRowView drvrecent = (DataRowView)cmrecent.Current;
//                        DataView dvrecent = drvrecent.DataView;
//                        DataTable dtrecent = new DataTable();
//                        dtrecent = dvrecent.ToTable();

//                        if (hti.Type == DataGrid.HitTestType.RowHeader)
//                        {
//                            this.SetDataBinding(dtrecent, "");
//                        }
//                    int count = dtrecent.Rows.Count;
//                    for (int h = 0; h < count; h++)
//                    {
//                        if (IsSelected(h))
//                        {
//                            this.UnSelect(h);
//                        }
//                    }
//                    this.Select(hti.Row);
//                    this.CurrentRowIndex = hti.Row;
//                    return; 
//                    //end
                  
                    
//                }

//                if(hti.Type == DataGrid.HitTestType.Cell && boolMutliLine == true)
//                {
//                    if(hti.Row == 0)
//                    {
//                        if(boolFirstRow == false)
//                        {
//                            if(this.IsSelected(hti.Row))
//                            {
//                                this.UnSelect(hti.Row);
//                            }
//                            else
//                            {
//                                this.Select(hti.Row);
//                            }
//                        }
//                        else
//                        {
//                            boolFirstRow = false;
//                            this.Select(hti.Row);
//                        }
//                    }
//                    else
//                    {
//                        if(boolFirstRow && this.RowCount > 0)
//                        {
//                            this.UnSelect(0);
//                            boolFirstRow = false;
//                        }

//                        if(this.IsSelected(hti.Row))
//                        {
//                            this.UnSelect(hti.Row);
//                        }
//                        else
//                        {
//                            this.Select(hti.Row);
//                        }
//                    }
                   
//                }
//                else if (hti.Type == DataGrid.HitTestType.RowHeader && boolMutliLine == true)
//                {
//                    if (this.IsSelected(hti.Row))
//                    {
//                        this.UnSelect(hti.Row);
//                    }
//                    else
//                    {
//                        this.Select(hti.Row);
//                    }
//                }
//                else
//                {
//                    if (hti.Type == DataGrid.HitTestType.ColumnHeader && e.Button == MouseButtons.Left)
//                    {
//                        //added by Nilanjan 14/05/07
//                        CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource, this.DataMember];
//                        DataRowView drv = (DataRowView)cm.Current;
//                        DataView dv = drv.DataView;
//                        DataTable dt = dv.Table;
//                        this.SetDataBinding(dt, "");
//                        //end
//                    }
//                    base.OnMouseDown(e);
//                }
               
//            }
//            catch( Exception ex )
//            {
//                MessageBox.Show( ex.ToString() );
//            }
//        }


      

//        //added to implement multi level search 
//        //protected override void OnDoubleClick(EventArgs e)
//        //{
//        //    if (boolLevelSearch && this.CurrentLevel != this.MaxLevel)
//        //    {
//        //        if (this.CurrentLevel == 0)
//        //            this.IncreseCount();

//        //        this.IncreseCount();

//        //        this.ShowLevelData(this, this.CurrentLevel);
//        //        return;
//        //    }
//        //    else //added by Nilanjan 17/05/07
//        //    {
//        //        if (frmhot != null)
//        //        {
//        //            bool selectedflag = false;
//        //            DataTable dttest = (DataTable)this.DataSource;
//        //            for (int q = 0; q < dttest.Rows.Count; q++)
//        //            {
//        //                if (this.IsSelected(q))
//        //                {
//        //                    selectedflag = true;
//        //                }
//        //            }
//        //            if (selectedflag)
//        //            {
//        //                frmhot.SetReturn();
//        //            }
//        //        }
//        //        else
//        //        {
//        //            //MouseEventArgs me=(MouseEventArgs)e;
//        //            //MouseEventArgs ev = new MouseEventArgs(MouseButtons.Left, 2, me.X, me.Y, 0);
//        //            //this.OnMouseDown(ev);
//        //            //return;
//        //        }
//        //    } //end
//        //    base.OnDoubleClick(e);
//        //}
//        //

      


//        public clsReadOnlyGridold()
//        {
//            //objResource = new clsSetResource(this);	
	
//            ProcParamNames = new ArrayList();
//            ProcParamValues = new ArrayList();

//            ////***To define whether the Grid Color settings
//            ////***will be the one set at Desing time (m_DesignColor=true)
//            ////***or it will be set from clsColors (m_DesignColor=false)
//            m_DesignColor=false;
//        }		

//        public clsReadOnlyGridold(bool DesignColor)
//        {
//            ////***To define whether the Grid Color settings
//            ////***will be the one set at Desing time (m_DesignColor=true)
//            ////***or it will be set from clsColors (m_DesignColor=false)
//            m_DesignColor=DesignColor;
//        }

//        //added by Nilanjan 17/05/07
//        public clsReadOnlyGridold(frmHotSearch frmhot)
//        {
//            try
//            {
//                //objResource = new clsSetResource(this);	

//                ProcParamNames = new ArrayList();
//                ProcParamValues = new ArrayList();

//                ////***To define whether the Grid Color settings
//                ////***will be the one set at Desing time (m_DesignColor=true)
//                ////***or it will be set from clsColors (m_DesignColor=false)
//                m_DesignColor = false;
//                this.frmhot = frmhot;
//            }
//            catch(Exception exception)
//            {
//            }
//        }

//        private bool IsMaskCol(int ColNo)
//        {
//            if(dtReadOnlyGrid.Columns[ColNo].ColumnName.Length > 4)
//            {
//                if(dtReadOnlyGrid.Columns[ColNo].ColumnName.Substring(0,4) == "MASK")
//                    return true;
//            }
//            return false;
//        }


//        /// <summary>
//        /// This method is to set the Input/Output Parameters of Stored Procedure.
//        /// </summary>
//        /// <param name="ProcParamName">Procedure Pararmeter Name</param>
//        /// <param name="ProcParamVal">Procedure Parameter Value, If it is OUT then enter ""</param>
//        /// <param name="strIN_OUT">IN for Input Parameter, OUT for Output Parameter</param>
//        public void SetParam(string ProcParamName,string ProcParamVal)
//        {
//            ProcParamNames.Add(ProcParamName);
//            ProcParamValues.Add(ProcParamVal.Trim());
//        }



//        /// <summary>
//        /// Clear the Procedure Parametersvfor fresh entry
//        /// </summary>
//        public void ClearParams()
//        {
//            ProcParamNames.Clear();
//            ProcParamValues.Clear();
//        }


//        #region Access Datatable


//        /// <summary>
//        /// For Designing Grid from an already loaded DataTable
//        /// </summary>
//        /// <param name="tableToBeDesigned"></param>
//        /// <param name="strErr"></param>
//        /// <returns></returns>
//        //public bool DesignGridFromDataTable( DataTable tableToBeDesigned, ref string strErr )
//        //{
//        //    try
//        //    {
//        //        this.ReadOnly=true;
				
//        //        FixContxtMnu();

//        //        if(boolLevelSearch==true)
//        //            this.CaptionVisible = true;
//        //        else
//        //            this.CaptionVisible = false;

//        //        strProcName = null;

//        //        intColCount=tableToBeDesigned.Columns.Count;
//        //        strColNames=new string[intColCount];
//        //        string[] strHeadings=new string[intColCount];
//        //        for(int i=0;i<intColCount;i++)
//        //        {
//        //            strColNames[i]= tableToBeDesigned.Columns[i].ColumnName;
//        //        }
			
//        //        ////**** Creating the Header text by replacing the Underscore(_)
//        //        ////**** Symbol with the Space ( ).

//        //        #region Header Text
//        //        for(int i=0;i<intColCount;i++)
//        //        {
//        //            string tmp="";
//        //            string[] m_strSplit=strColNames[i].Split(new char[] {'_'}, strColNames[i].Length);
//        //            for(int m=0;m<=m_strSplit.GetUpperBound(0);m++)
//        //                tmp = tmp.Trim() + " " + m_strSplit[m].Trim();
//        //            strHeadings[i] = tmp;

//        //            if(strLangID != null)
//        //            {
//        //                if(strLangID.Length >= intColCount)
//        //                {
//        //                    clsLanguage objLang = new clsLanguage();
							
//        //                    string strTmp = objLang.LanguageString(strLangID[i]);
//        //                    if(strTmp == "*****")
//        //                        strHeadings[i] = "** " + strHeadings[i]; 
//        //                    else
//        //                        strHeadings[i] = strTmp;

//        //                    objLang.Dispose();
//        //                }
//        //                else
//        //                {
//        //                    if(strLangID.Length > i)
//        //                    {
//        //                        clsLanguage objLang = new clsLanguage();
//        //                        string strTmp = objLang.LanguageString(strLangID[i]);
								
//        //                        if(strTmp == "*****")
//        //                            strHeadings[i] = "** " + strHeadings[i];
//        //                        else
//        //                            strHeadings[i] = strTmp;

//        //                        objLang.Dispose();
//        //                    }
//        //                    else
//        //                    {
//        //                        strHeadings[i] = "** " + strHeadings[i];
//        //                    }
//        //                }
//        //            }
//        //            else
//        //            {
//        //                strHeadings[i] = "** "+ strHeadings[i];
//        //            }
//        //        }
				
//        //        #endregion
//        //        ////**** Creating a new Table Style to include the TextBoxes in the Datagrid

//        //        DataGridTableStyle dgTabSt=new DataGridTableStyle();
//        //        // DataTable dbTab = new DataTable(tableToBeDesigned.TableName);
//        //        DataTable dbTab = tableToBeDesigned.Copy();
//        //        dgTabSt.MappingName=dbTab.TableName;
				
//        //        dgTabSt.PreferredRowHeight=15;

//        //        ////**** If the Default design is false then setting Colors for that Grid
				
//        //        if(SetDefault==true)
//        //        {
//        //            dgTabSt.HeaderBackColor=System.Drawing.Color.Linen;
//        //            dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//        //            dgTabSt.GridLineColor=System.Drawing.Color.Silver;
//        //        }
//        //        else
//        //        {
//        //            dgTabSt.HeaderBackColor=System.Drawing.Color.Lavender;
//        //            dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//        //            dgTabSt.GridLineColor=System.Drawing.Color.Silver;

//        //        }
				
//        //        ////**** To incorporate the Font the in DataGrid Cell.
				
//        //        FormatCellEventHandler handler=new FormatCellEventHandler(FormatGridCells);

//        //        ////**** To mask Certain Fields from the User Point of View.
//        //        ////**** If the Query Contains the KeyWord like 'MASK' initially in its aliasing name
//        //        ////**** Then that particular column will not be added to GridColumnStyle.

//        //        intVisibleColumnCount = 0;
//        //        for(int i=0;i<intColCount;i++)
//        //        {
//        //            if(strColNames[i].ToUpper().IndexOf("MASK") == 0 )
//        //                continue;
					
//        //            intVisibleColumnCount++;
//        //            MyGridColumn dgTBCol=new MyGridColumn(i);
//        //            dgTBCol.MappingName=strColNames[i];
//        //            dgTBCol.HeaderText=strHeadings[i];
//        //            dgTBCol.SetCellFormat+=handler;
//        //            dgTBCol.NullText = "";

//        //            dgTabSt.GridColumnStyles.Add(dgTBCol);
//        //            dgTBCol=null;
//        //        }
//        //        dgTabSt.ForeColor=System.Drawing.Color.Navy;
//        //        dgTabSt.BackColor=System.Drawing.Color.Lavender ;//LightCyan;

//        //        this.TableStyles.Clear();
//        //        this.TableStyles.Add(dgTabSt);
//        //        this.SetDataBinding( dbTab, "" );
				
//        //        RowCount=dbTab.Rows.Count;

//        //        dgText=new DataGridTextBoxColumn[intVisibleColumnCount];
//        //        tb=new TextBox[intVisibleColumnCount];
//        //        for(int i=0;i<intVisibleColumnCount;i++)
//        //        {
//        //            dgText[i]=(DataGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[i];
//        //            tb[i]=dgText[i].TextBox;
//        //            if(boolLevelSearch)
//        //            {
//        //                tb[i].KeyDown += new KeyEventHandler(F3KeyDown);
//        //            }
//        //        }

//        //        this.AllowSorting = true;

//        //        ////**** To AutoSize the Coloumn Width in the Datagrid
//        //        AutoSizeCol();
//        //        return true;
//        //    }
//        //    catch(Exception ex)
//        //    {
//        //        strErr = ex.Message + " - " + ex.Source;
//        //        return false;
//        //    }
//        //}



//        #endregion

//        /// <summary>
//        /// For Designing All Records Grid through Procedure
//        /// </summary>
//        /// <param name="strProcedureName"></param>
//        /// <param name="strErr"></param>
//        /// <returns></returns>
//        public bool DesignGrid(string strProcedureName,ref string strErr)
//        {
//            try
//            {
//                this.ReadOnly=true;
				
//                FixContxtMnu();

//                if(boolLevelSearch==true)
//                    this.CaptionVisible = true;
//                else
//                    this.CaptionVisible = false;

//                strProcName = strProcedureName;

//                Query objQuery = new Query();

//                if (ProcParamNames.Count > 0)
//                {
//                    string[] arrStrProcParamNames = new string[ProcParamNames.Count];
//                    string[] arrStrProcParamValues = new string[ProcParamNames.Count];
//                    for (int i = 0; i < ProcParamNames.Count; i++)
//                    {
//                        arrStrProcParamNames[i] = ProcParamNames[i].ToString();
//                        arrStrProcParamValues[i] = ProcParamValues[i].ToString();
//                    }
//                    objQuery.SetInputParameterNames(strProcedureName, arrStrProcParamNames);
//                    objQuery.SetInputParameterValues(arrStrProcParamValues);
//                }
//                objQuery.SetOutputParameterNames(strProcedureName, "CRITERIA");

//                DataSet ds = objQuery.ExecuteQueryProcedure(strProcedureName);

//                dtReadOnlyGrid = ds.Tables[0].Copy();
//                ds.Dispose();

//                intColCount = dtReadOnlyGrid.Columns.Count;
//                strColNames = new string[intColCount];

//                for (int i = 0; i < intColCount; i++)
//                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;
				
//                string[] strHeadings=new string[intColCount];
			
//                ////**** Creating the Header text by replacing the Underscore(_)
//                ////**** Symbol with the Space ( ).

//                clsLanguage objLang = new clsLanguage();

//                for(int i=0;i<intColCount;i++)
//                {
//                    strHeadings[i] = strColNames[i].Replace( "_", " " );

//                    if( strLangID != null && strColNames[i].IndexOf( "MASK_" ) < 0 )
//                    {
//                        if(strLangID.Length >= intColCount)
//                        {
//                            string strTmp = objLang.LanguageString(strLangID[i]);
//                            if(strTmp == "*****")
//                                strHeadings[i] = "** " + strHeadings[i]; 
//                            else
//                                strHeadings[i] = strTmp;
//                        }
//                        else
//                        {
//                            if(strLangID.Length > i)
//                            {
//                                string strTmp = objLang.LanguageString(strLangID[i]);
								
//                                if(strTmp == "*****")
//                                    strHeadings[i] = "** " + strHeadings[i];
//                                else
//                                    strHeadings[i] = strTmp;
//                            }
//                            else
//                            {
//                                strHeadings[i] = "** " + strHeadings[i];
//                            }
//                        }
//                    }
//                    else
//                    {
//                        strHeadings[i] = "** "+ strHeadings[i];
//                    }
//                }

//                if( objLang != null )
//                    objLang.Dispose();
				
//                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid

//                DataGridTableStyle dgTabSt=new DataGridTableStyle();
//                dgTabSt.MappingName = dtReadOnlyGrid.TableName;
				
//                dgTabSt.PreferredRowHeight=15;

//                ////**** If the Default design is false then setting Colors for that Grid
				
//                if(SetDefault==true)
//                {
//                    dgTabSt.HeaderBackColor=System.Drawing.Color.Linen ;
//                    dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//                    dgTabSt.GridLineColor=System.Drawing.Color.Silver;
//                }
//                else
//                {
//                    dgTabSt.HeaderBackColor=System.Drawing.Color.LightSteelBlue ;
//                    dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//                    dgTabSt.GridLineColor=System.Drawing.Color.Silver;

//                }
				
//                ////**** To incorporate the Font the in DataGrid Cell.
				
//                FormatCellEventHandler handler=new FormatCellEventHandler(FormatGridCells);

//                intVisibleColumnCount = 0;
//                for(int i=0;i<intColCount;i++)
//                {
//                    if(strColNames[i].ToUpper().IndexOf("MASK") == 0 )
//                        continue;
					
//                    intVisibleColumnCount++;

//                    MyGridColumn dgTBCol=new MyGridColumn(i);
//                    dgTBCol.MappingName=strColNames[i];
//                    dgTBCol.HeaderText=strHeadings[i];
//                    dgTBCol.SetCellFormat+=handler;
//                    dgTBCol.NullText = "";
//                    dgTabSt.GridColumnStyles.Add(dgTBCol);
//                    dgTBCol=null;
//                }

//                dgTabSt.ForeColor=System.Drawing.Color.Navy;
//                dgTabSt.BackColor=System.Drawing.Color.Lavender   ;//.LightSteelBlue    ;//.LightCyan;

//                ////**** To mask Certain Fields from the User Point of View.
//                ////**** If the Query Contains the KeyWord like 'MASK' initially in its aliasing name
//                ////**** Then that particular column will be Masked.


//                this.TableStyles.Clear();			// INDRANIL
//                this.TableStyles.Add(dgTabSt);
//                this.SetDataBinding( dtReadOnlyGrid, "" );
				
//                RowCount=dtReadOnlyGrid.Rows.Count;

//                dgText=new DataGridTextBoxColumn[intVisibleColumnCount];
//                tb=new TextBox[intVisibleColumnCount];
//                for(int i=0;i<intVisibleColumnCount;i++)
//                {
//                    dgText[i]=(DataGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[i];
//                    tb[i]=dgText[i].TextBox;
//                    if(boolLevelSearch)
//                    {
//                        tb[i].KeyDown += new KeyEventHandler(F3KeyDown);
//                    }
//                }
//                this.AllowSorting = true;

//                ////**** To AutoSize the Coloumn Width in the Datagrid
//                AutoSizeCol();
//                return true;
//            }
//            catch(Exception ex)
//            {
//                strErr = ex.Message + " - " + ex.Source;
//                return false;
//            }
//        }


//        /// <summary>
//        /// For Designing An Empty Grid from an already designed Grid. e.g. Selected Records Grid in Hot Search
//        /// </summary>
//        /// <param name="dgRecords"></param>
//        /// <param name="strErrMsg"></param>
//        /// <returns></returns>
//        public bool DesignGrid( DataGrid dgOriginal, ref string strErrMsg )
//        {
//            try
//            {
//                this.ReadOnly = false;

//                FixContxtMnu();

//                if(boolLevelSearch == true)
//                    dgOriginal.CaptionVisible = true;
//                else
//                    dgOriginal.CaptionVisible = false;

//                DataTable dtOriginal = (DataTable) dgOriginal.DataSource;
				
//                dtReadOnlyGrid = dtOriginal.Clone();

//                DataGridTableStyle dgTabStyleOrg = dgOriginal.TableStyles[0];

//                intColCount = dtReadOnlyGrid.Columns.Count;
//                intVisibleColumnCount = dgTabStyleOrg.GridColumnStyles.Count;

//                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid
//                DataGridTableStyle dgTabSt=new DataGridTableStyle();
//                dgTabSt.MappingName = dtReadOnlyGrid.TableName;
				
//                dgTabSt.PreferredRowHeight=15;

//                ////**** If the Default design is false then setting Colors for that Grid

//                dgTabSt.HeaderBackColor=System.Drawing.Color.Linen ;//.Lavender;
//                dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//                dgTabSt.GridLineColor=System.Drawing.Color.Silver;
				
//                ////**** To incorporate the Font the in DataGrid Cell.
//                FormatCellEventHandler handler=new FormatCellEventHandler(FormatGridCells);
//                MyGridColumn.MultiLine = boolMutliLine;

//                for(int i=0;i<intVisibleColumnCount;i++)
//                {
//                    MyGridColumn dgTBCol=new MyGridColumn(i);
//                    dgTBCol.MappingName=dgTabStyleOrg.GridColumnStyles[i].MappingName;
//                    dgTBCol.HeaderText=dgTabStyleOrg.GridColumnStyles[i].HeaderText;
//                    dgTBCol.Width = dgTabStyleOrg.GridColumnStyles[i].Width;
//                    dgTBCol.SetCellFormat+=handler;
//                    dgTBCol.NullText = "";

//                    dgTabSt.GridColumnStyles.Add(dgTBCol);
//                    dgTBCol=null;
//                }

//                dgTabStyleOrg.Dispose();

//                dgTabSt.ForeColor=System.Drawing.Color.Navy;
//                dgTabSt.BackColor=System.Drawing.Color.Lavender ;//.LightCyan;

//                ////**** To mask Certain Fields from the User Point of View.
//                ////**** If the Query Contains the KeyWord like 'MASK' initially in its aliasing name
//                ////**** Then that particular column will be Masked.

//                this.TableStyles.Clear();
//                this.TableStyles.Add(dgTabSt);

//                if(_DefaultSort)
//                    dtReadOnlyGrid.DefaultView.Sort = dtReadOnlyGrid.Columns[_SortCol].ColumnName + " ASC ";
//                this.SetDataBinding( dtReadOnlyGrid, "" );
				
				
//                int RowCount = dtReadOnlyGrid.Rows.Count;

//                dgText	= new DataGridTextBoxColumn[intVisibleColumnCount];
//                tb		= new TextBox[intVisibleColumnCount];
				
//                for(int i=0;i<intVisibleColumnCount;i++)
//                {
//                    dgText[i] = (DataGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[i];
//                    tb[i] = dgText[i].TextBox;
//                    if(boolLevelSearch)
//                    {
//                        tb[i].KeyDown += new KeyEventHandler(F3KeyDown);
//                    }
//                }

//                this.AllowSorting = true;

//                // This is to block the append new row with * symbol in the grid.
////				CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource, this.DataMember];      
////				((DataView)cm.List).AllowNew = false; 
				
//                return true;
//            }
//            catch(Exception ex)
//            {
//                strErrMsg = ex.Message + " - " + ex.Source;
//                return false;
//            }
//        }


//        private void FixContxtMnu()
//        {
//            this._ContxtMnu = new ContextMenu();
//            this._mnuExportData = new MenuItem();
//            this._ContxtMnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this._mnuExportData});
//            this._mnuExportData.Index = 0;
//            this._mnuExportData.Text = "Export Data";
//            this.ContextMenu = _ContxtMnu;
//            this._mnuExportData.Click += new System.EventHandler(this.mnuExportData_Click);
//        }


//        private void mnuExportData_Click(object sender, System.EventArgs e)
//        {
//            string m_strErr = "";
			
//            try
//            {
//                SaveFileDialog dlg = new SaveFileDialog();
//                dlg.Title = "Save File As";
//                dlg.InitialDirectory = @"C:\";
//                dlg.Filter = "Text Files (*.txt)|*.txt|Comma Separated Text (*.csv)|*.csv|Microsoft Excel (*.xls)|*.xls";
//                if(dlg.ShowDialog() == DialogResult.OK)
//                {
//                    if(dlg.FilterIndex == 1)
//                    {
//                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
//                        DataTable m_dtTemp = (DataTable) this.DataSource;
//                        if(cnv.CreateTextFile(m_dtTemp,true,ref m_strErr) == false)
//                        {
//                            MessageBox.Show(m_strErr);
//                            return;
//                        }
//                    }
//                    else if(dlg.FilterIndex == 2)
//                    {
//                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
//                        DataTable m_dtTemp = (DataTable) this.DataSource;
//                        if(cnv.CreateTextFile(m_dtTemp,false,ref m_strErr) == false)
//                        {
//                            MessageBox.Show(m_strErr);
//                            return;
//                        }
//                    }
//                    else if(dlg.FilterIndex == 3)
//                    {
//                        cnvTag cnv = new cnvTag(dlg.FileName.Trim());
//                        if(cnv.CreateExcelFile((DataTable)this.DataSource,ref m_strErr) == false)
//                        {
//                            MessageBox.Show(m_strErr);
//                            return;
//                        }
//                    }
//                    dlg.Dispose();
//                    MessageBox.Show("Data Exported Successfully.");
//                }
//            }
//            catch(Exception ex)
//            {
//                m_strErr = ex.Source + " - " + ex.Message;
//                MessageBox.Show(m_strErr);
//                return;
//            }
//        }


//        /// <summary>
//        /// For Designing Grid through a SQL
//        /// </summary>
//        /// <param name="dgParam"></param>
//        /// <param name="sqlParam"></param>
//        /// <param name="strErr"></param>
//        /// <param name="SearchGrid"></param>
//        /// <returns></returns>
//        public bool DesignGridFromSQL( string sqlParam, ref string strErr )
//        {			
//            try
//            {
//                FixContxtMnu();

//                if(boolLevelSearch == true)
//                    this.CaptionVisible = true;
//                else
//                    this.CaptionVisible = false;

//                strSQL=sqlParam;
				
//                ////***If the Grid is a single row Search Grid (SearchGrid=true),
//                ////***then it is Editable, else it is ReadOnly.

//                Query objQuery = new Query();

//                DataSet ds = objQuery.ExecuteQueryCommand(strSQL);

//                dtReadOnlyGrid = ds.Tables[0].Copy();
//                dtReadOnlyGrid.TableName = "Table";
//                ds.Dispose();

//                intColCount = dtReadOnlyGrid.Columns.Count;
//                strColNames = new string[intColCount];

//                for (int i = 0; i < intColCount; i++)
//                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

//                this.ReadOnly=true;

//                ///***Displaying data from a Table to a Grid and aligning the Columns***
//                intColCount=dtReadOnlyGrid.Columns.Count;
//                strColNames=new string[intColCount];

//                for(int i=0;i<intColCount;i++)
//                    strColNames[i]=dtReadOnlyGrid.Columns[i].ColumnName;

//                string[] strHeadings=new string[intColCount];
			
//                ////**** Creating the Header text by replacing the Underscore(_)
//                ////**** Symbol with the Space ( ).

//                clsLanguage objLang = new clsLanguage();

//                for(int i=0;i<intColCount;i++)
//                {
//                    strHeadings[i] = strColNames[i].Replace( "_", " " );
					
//                    if( strLangID != null && strColNames[i].IndexOf( "MASK_" ) < 0 )
//                    {
//                        if(strLangID.Length >= intColCount)
//                        {
//                            string strTmp = objLang.LanguageString(strLangID[i]);
//                            if(strTmp == "*****")
//                                strHeadings[i] = "** " + strHeadings[i]; 
//                            else
//                                strHeadings[i] = strTmp;
//                        }
//                        else
//                        {
//                            if(strLangID.Length > i)
//                            {
//                                string strTmp = objLang.LanguageString(strLangID[i]);
								
//                                if(strTmp == "*****")
//                                    strHeadings[i] = "** " + strHeadings[i];
//                                else
//                                    strHeadings[i] = strTmp;
//                            }
//                            else
//                            {
//                                strHeadings[i] = "** " + strHeadings[i];
//                            }
//                        }
//                    }
//                    else
//                    {
//                        strHeadings[i] = "** "+ strHeadings[i];
//                    }
//                }

//                if( objLang != null )
//                    objLang.Dispose();
				
//                ////**** Creating a new Table Style to include the TextBoxes in the Datagrid
//                DataGridTableStyle dgTabSt=new DataGridTableStyle();

//                dgTabSt.MappingName = dtReadOnlyGrid.TableName;
				
//                dgTabSt.PreferredRowHeight=15;

//                if(SetDefault==true)
//                {
//                    dgTabSt.HeaderBackColor=System.Drawing.Color.Linen;
//                    dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//                    dgTabSt.GridLineColor=System.Drawing.Color.Silver;
//                }
//                else
//                {
//                    dgTabSt.HeaderBackColor=System.Drawing.Color.Linen ;//.AliceBlue ;//Lavender;
//                    dgTabSt.HeaderForeColor=System.Drawing.Color.Navy;
//                    dgTabSt.GridLineColor=System.Drawing.Color.Silver;
//                }

				
				
//                FormatCellEventHandler handler=new FormatCellEventHandler(FormatGridCells);

//                MyGridColumn.MultiLine = boolMutliLine;

//                intVisibleColumnCount = 0;
//                for(int i=0;i<intColCount;i++)
//                {
//                    if(strColNames[i].ToUpper().IndexOf("MASK") == 0 )
//                        continue;
					
//                    intVisibleColumnCount++;

//                    MyGridColumn dgTBCol=new MyGridColumn(i);
//                    dgTBCol.MappingName=strColNames[i];
//                    dgTBCol.HeaderText=strHeadings[i];
//                    dgTBCol.SetCellFormat+=handler;
//                    dgTBCol.NullText = "";

//                    dgTabSt.GridColumnStyles.Add(dgTBCol);
//                    dgTBCol=null;
//                }
//                dgTabSt.ForeColor=System.Drawing.Color.Navy;
//                dgTabSt.BackColor=System.Drawing.Color.Lavender  ;//LightCyan;
//                dgTabSt.AlternatingBackColor = System.Drawing.Color.White;

//                this.TableStyles.Clear();			// Indranil

//                this.TableStyles.Add(dgTabSt);

//                if(_DefaultSort)
//                    dtReadOnlyGrid.DefaultView.Sort = dtReadOnlyGrid.Columns[_SortCol].ColumnName + " ASC ";

//                this.SetDataBinding( dtReadOnlyGrid, "" );
				
//                RowCount = dtReadOnlyGrid.Rows.Count;

//                dgText=new DataGridTextBoxColumn[intVisibleColumnCount];
//                tb=new TextBox[intVisibleColumnCount];
//                for( int i = 0; i < intVisibleColumnCount; i++ )
//                {
//                    dgText[i]=(DataGridTextBoxColumn)this.TableStyles[0].GridColumnStyles[i];
//                    tb[i]=dgText[i].TextBox;
//                    if(boolLevelSearch)
//                    {
//                        tb[i].KeyDown += new KeyEventHandler(F3KeyDown);
//                    }
//                }
				
//                ////**** To AutoSize the Coloumn Width in the Datagrid
//                AutoSizeCol();
//                if(boolLevelSearch == true)
//                {
//                    this.KeyDown += new KeyEventHandler(BackSpaceKeyDown);
//                    arlHierarchyLevel = new ArrayList();
//                    arlHierarchyValue = new ArrayList();
//                    arlPrimKey = new ArrayList();
//                }
//                this.AllowSorting = true;

//                return true;
//            }
//            catch(Exception ex)
//            {
//                strErr = ex.Message + " - " + ex.Source;
//                return false;
//            }
//        }


//        public void IncreseCount()
//        {
//            _CurrentLevel++;
//        }

		
//        private void DecreaseCount()
//        {
//            _CurrentLevel--;
//        }

		
//        private void ShowLevelData(int CountVal)
//        {
//            dtLevelData = FilterLevelExtra(CountVal);
			
//            this.CaptionVisible = true;
//            this.CaptionText = _CurrentLevel + "/" + _MaxLevel;
//            this.SetDataBinding( dtLevelData, "" );
//            this.AssociatedSearchGrid.FullData = dtLevelData;
//            this.Tag = dtLevelData.Copy();
//            dtLevelData.Dispose();
//        }
		
		
//        private DataGrid dgSearchLevel;

//        //public void ShowLevelData(clsReadOnlyGrid dgParam,int CountVal)
//        //{
//        //    dgSearchLevel = dgParam;

//        //    dtLevelData = FilterLevel(CountVal);

//        //    dgParam.CaptionVisible = true;
//        //    if(_CurrentLevel != 0)
//        //        dgParam.CaptionText = _CurrentLevel + "/" + _MaxLevel;
//        //    else
//        //        dgParam.CaptionText = "1/" + _MaxLevel;

//        //    if( dtLevelData != null )
//        //    {
//        //        dgParam.SetDataBinding( dtLevelData, "" );

//        //        dgParam.AssociatedSearchGrid.FullData = dtLevelData;

//        //        dgParam.Tag = dtLevelData.Copy();

//        //        dtLevelData.Dispose();
//        //    }
//        //}

		
//        private DataTable FilterLevelExtra(int CountVal)
//        {
//            FindMaxLevel();

//            _CurrentLevel = CountVal;

//            if(_CurrentLevel < 1)
//                _CurrentLevel = 1;
//            else if(_CurrentLevel > _MaxLevel)
//                _CurrentLevel = _MaxLevel;

//            int i;
//            string str = "";

//            DataTable dtFilt = dtReadOnlyGrid.Clone();

//            for(i=0; i < dtReadOnlyGrid.Columns.Count - 1; i++)
//            {
//                if(_CurrentLevel == 1)
//                {
//                    if(i == 0)
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + _CurrentLevel + "' AND ";
//                    else
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' AND ";

//                }
//                else
//                {
//                    if(i==0)
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + _CurrentLevel + "' AND ";
//                    else if(i==2 && arlPrimKey != null)
//                    {
//                        if(arlPrimKey.Count >= _CurrentLevel-2)
//                            str += "("+ dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + arlPrimKey[_CurrentLevel-2].ToString()  + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
//                        else
//                            str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' AND ";
//                    }
//                    else
//                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
//                }
//            }

//            if(_CurrentLevel == 1)
//            {
//                if(i==0)
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + _CurrentLevel + "' ";
//                else
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' ";
//            }
//            else
//            {
//                if(i==0)
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + _CurrentLevel + "' ";
//                else if(i==2 && arlPrimKey != null)
//                {
//                    if(arlPrimKey.Count >= _CurrentLevel-2)
//                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + arlPrimKey[_CurrentLevel-2].ToString()  + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
//                    else
//                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
//                }
//                else
//                    str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
//            }
//            DataRow[] drTemp = dtReadOnlyGrid.Select(str,dtReadOnlyGrid.Columns[CurrentCell.ColumnNumber].ColumnName,DataViewRowState.OriginalRows);

//            for(i=0; i < drTemp.Length; i++)
//                dtFilt.ImportRow(drTemp[i]);

//            return dtFilt;
//        }

		
//        private DataTable FilterLevel(int CountVal)
//        {
//            FindMaxLevel();
			
//            if(CountVal > _MaxLevel)
//            {
//                _CurrentLevel = _MaxLevel;
//                return (DataTable) dgSearchLevel.Tag;
//            }
			
//            DataTable dtFilt = dtReadOnlyGrid.Clone();

//            DataTable dtTemp = (DataTable) dgSearchLevel.Tag;

//            if(dtTemp != null && _ReqColNo1 != -1)
//            {
//                if(arlHierarchyLevel != null )
//                {
//                    if(arlHierarchyLevel.Count <= _CurrentLevel-2)
//                        arlHierarchyLevel.Add(dtTemp.Rows[CurrentRowIndex][_ReqColNo1].ToString());
//                    else if( _CurrentLevel >= 2 )
//                        arlHierarchyLevel[_CurrentLevel-2] = dtTemp.Rows[CurrentRowIndex][_ReqColNo1].ToString();
//                }
//            }

//            if(dtTemp != null && _ReqColNo2 != -1)
//            {
//                if(arlHierarchyValue != null )
//                {
//                    if(arlHierarchyValue.Count <= _CurrentLevel-2 )
//                        arlHierarchyValue.Add(dtTemp.Rows[CurrentRowIndex][_ReqColNo2].ToString());
//                    else if( _CurrentLevel >= 2 )
//                        arlHierarchyValue[_CurrentLevel-2] = dtTemp.Rows[CurrentRowIndex][_ReqColNo2].ToString();
//                }
//            }

//            if(dtTemp != null && arlPrimKey != null)
//            {
//                if(arlPrimKey.Count < MaxLevel )
//                {
//                    if(arlPrimKey.Count <= _CurrentLevel-2 )
//                        arlPrimKey.Add(dtTemp.Rows[CurrentRowIndex][1].ToString());
//                    else if( _CurrentLevel >= 2 )
//                        arlPrimKey[_CurrentLevel-2] = dtTemp.Rows[CurrentRowIndex][1].ToString();
//                }
//            }

//            string str = "";
//            int i;
//            // All except the last column
//            for(i=0 ; i < dtReadOnlyGrid.Columns.Count - 1; i++)
//            {
//                if(CountVal == 1)
//                {
//                    if(i==0)
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' AND ";
//                    else
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' AND ";
//                }
//                else
//                {
//                    if(i==0)
//                        str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' AND ";
//                    else if(i == 2)
//                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + dtTemp.Rows[CurrentRowIndex][1].ToString() + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
//                    else
//                        str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') AND ";
//                }
//            }

//            // For the last column, so that there is no trailing AND
//            if(CountVal == 1)
//            {
//                if(i==0)
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' ";
//                else
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '' ";
//            }
//            else
//            {
//                if(i==0)
//                    str += dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + CountVal + "' ";
//                else if(i==2)
//                    str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '" + dtTemp.Rows[CurrentRowIndex][1].ToString() + "' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
//                else
//                    str += "(" + dtReadOnlyGrid.Columns[i].ColumnName + " LIKE '%' OR Isnull(" + dtReadOnlyGrid.Columns[i].ColumnName + ",'') = '') ";
//            }

//            DataRow[] drTemp = dtReadOnlyGrid.Select(str,dtReadOnlyGrid.Columns[CurrentCell.ColumnNumber].ColumnName,DataViewRowState.OriginalRows);

//            for(i=0; i < drTemp.Length; i++)
//                dtFilt.ImportRow(drTemp[i]);

//            if( dtTemp != null )
//                dtTemp.Dispose();

//            return dtFilt;
//        }
		
		
//        private void BackSpaceKeyDown(object sender, KeyEventArgs e)
//        {
//            if(e.KeyData != Keys.Back)
//                return;

//            --_CurrentLevel;
//            ShowLevelData(_CurrentLevel);
//        }

		
//        //private void F3KeyDown(object sender,KeyEventArgs e)
//        //{
//        //    if(e.KeyData != Keys.F3 && e.KeyData != Keys.Back)
//        //        return;

//        //    if(e.KeyData == Keys.F3)
//        //    {
//        //        if(_CurrentLevel == 0)
//        //            _CurrentLevel = 1;

//        //        _CurrentLevel++;
//        //        ShowLevelData(this,_CurrentLevel);
//        //    }
//        //    else
//        //    {
//        //        _CurrentLevel--;
//        //        ShowLevelData(_CurrentLevel);
//        //    }
//        //}

//        private void ENterKeyDown(object sender,KeyEventArgs e)
//        {
//            if(e.KeyData == Keys.Enter )
//            {
				
//            }
			
//        }


//        private void FindMaxLevel()
//        {
//            int temp = 0, temp1 = 0;
			
//            for(int i=0; i < dtReadOnlyGrid.Rows.Count; i++)
//            {
//                temp1 = Convert.ToInt32(dtReadOnlyGrid.Rows[i][0].ToString());
//                if(temp1 > temp)
//                    temp = temp1;
//            }

//            _MaxLevel = temp;
//        }


//        /// <summary>
//        /// Populating the already designed Grid with an SQL Query.
//        /// </summary>
//        /// <param name="sqlParam"></param>
//        /// <param name="strErr"></param>
//        /// <returns></returns>
//        public bool PopulateGrid(string sqlParam, ref string strErr)
//        {			
//            try
//            {
//                this.ReadOnly=true;
				
//                strSQL=sqlParam;

//                ///***Displaying data from a Table to a Grid and aligning the Columns***
//                Query objQuery = new Query();

//                DataSet ds = objQuery.ExecuteQueryCommand(strSQL);

//                dtReadOnlyGrid = ds.Tables[0].Copy();
//                ds.Dispose();

//                intColCount = dtReadOnlyGrid.Columns.Count;
//                strColNames = new string[intColCount];

//                for (int i = 0; i < intColCount; i++)
//                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

//                this.SetDataBinding(dtReadOnlyGrid, "");
                
//                AutoSizeCol();
				
//                RowCount=dtReadOnlyGrid.Rows.Count;
				
//                return true;
//            }
//            catch(Exception ex)
//            {
//                strErr = ex.Message + " - " + ex.Source;
//                return false;
//            }
//        }

//        //to implement sailing navigation by Nilanjan 18/05/07
//        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
//        {
            
//            return base.ProcessCmdKey(ref msg, keyData);
//        }


//        /// <summary>
//        /// Populating the already designed Grid with a Procedure.
//        /// </summary>
//        /// <param name="strErr"></param>
//        /// <returns></returns>
//        public bool PopulateGrid(ref string strErr)
//        {
//            try
//            {
//                Query objQuery = new Query();

//                string[] arrStrProcParamNames = new string[ProcParamNames.Count];
//                string[] arrStrProcParamValues = new string[ProcParamNames.Count];
//                for (int i = 0; i < ProcParamNames.Count; i++)
//                {
//                    arrStrProcParamNames[i] = ProcParamNames[i].ToString();
//                    arrStrProcParamValues[i] = ProcParamValues[i].ToString();
//                }
//                objQuery.SetInputParameterNames(strProcName, arrStrProcParamNames);
//                objQuery.SetInputParameterValues(arrStrProcParamValues);
//                objQuery.SetOutputParameterNames(strProcName, "CRITERIA");

//                DataSet ds = objQuery.ExecuteQueryProcedure(strProcName);

//                dtReadOnlyGrid = ds.Tables[0].Copy();
//                ds.Dispose();

//                intColCount = dtReadOnlyGrid.Columns.Count;
//                strColNames = new string[intColCount];

//                for (int i = 0; i < intColCount; i++)
//                    strColNames[i] = dtReadOnlyGrid.Columns[i].ColumnName;

//                this.SetDataBinding(dtReadOnlyGrid, "");

//                //Debasish - Autosizing after refresh
//                AutoSizeCol();

//                RowCount = dtReadOnlyGrid.Rows.Count;
//                return true;
//            }
//            catch(Exception ex)
//            {
//                strErr = ex.Message + " - " + ex.Source;
//                return false;
//            }
//        }


//        public void AutoSizeCol() 
//        { 
//            int intDgSize,intTotalSize=0;
//            intDgSize=this.Width-70;
//            string rowText = "", colName = "";
//            DataTable dtSource = ((DataTable)this.DataSource);
//            for(int col=0;col<intVisibleColumnCount;col++)
//            {
//                colName = this.TableStyles[0].GridColumnStyles[col].MappingName;

//                float width = 0; 
//                int numRows = ((DataTable) this.DataSource).Rows.Count; 
//                Graphics g = Graphics.FromHwnd(this.Handle); 
//                StringFormat sf = new StringFormat(StringFormat.GenericTypographic); 
//                SizeF size; 

//                ////**** To find the approximate length of the Rows
//                ////**** Displayed in the Datagrid by checking the First 15 Rows length.
//                if(this.TableStyles[0].GridColumnStyles[col].Width != 0)
//                {
//                    for(int i = 0; i < numRows; ++ i) 
//                    { 
//                        rowText = dtSource.Rows[i][colName].ToString();
//                        size = g.MeasureString( rowText, this.Font, 500, sf ); 
//                        if(size.Width > width) 
//                            width = size.Width; 
//                    } 
//                    size=g.MeasureString( this.TableStyles[0].GridColumnStyles[col].HeaderText.ToString(),this.Font,500,sf);
//                    int m_intTextLen=(int)size.Width;
//                    g.Dispose();
//                    this.TableStyles[0].GridColumnStyles[col].Width = (int) width + 8; // 8 is for leading and trailing padding 
//                    int m_intDataLen=this.TableStyles[0].GridColumnStyles[col].Width;
//                    if(m_intTextLen > m_intDataLen)
//                        this.TableStyles[0].GridColumnStyles[col].Width = m_intTextLen + 8;
//                    intTotalSize += this.TableStyles[0].GridColumnStyles[col].Width;
//                }
//            }

//            ////**** If total width of the Datagrid is less than the Width occupied by Columns
//            ////**** Then resize the Column width to match Datagrid Width.

//            if(intTotalSize < intDgSize )
//            {
//                for(int i=0; i < intVisibleColumnCount ; i++)
//                        this.TableStyles[0].GridColumnStyles[i].Width = this.TableStyles[0].GridColumnStyles[i].Width + (intDgSize-intTotalSize)/intColCount;
//            }

//            dtSource.Dispose();

//        }


//    }
	

//    ////****  To set the Datagrid Text Box Column with the regular Font
//    ////****  MyGridColumn is inherited from DataGridTextBoxColumn
//    ////****  In MyGridColumn the Paint Method (Protected) is over rided.

//    public class MyGridColumn : DataGridTextBoxColumn
//    {
//        //In the handler, set the EnableValue to true or false, depending upon the row & col
		
//        private int SelectedRow = -1; 
//        private static bool boolMultiLine;
        
//        public bool SelectLine = true;

//        public static bool MultiLine
//        {
//            set
//            {
//                boolMultiLine = value;
//            }
//        }


//        public event FormatCellEventHandler SetCellFormat;
		
//        private int _col;

//        public MyGridColumn(int col)
//        {
//            _col = col;
//        }

//        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
//        {
//            DataGridFormatCellEventArgs e = new DataGridFormatCellEventArgs(rowNum, this._col, this.DataGridTableStyle.DataGrid.Font, backBrush, foreBrush);
//            if(SetCellFormat != null)
//            {
//                SetCellFormat(this, e);
//            }
//            if(e.UseBaseClassDrawing)
//                base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
//            else
//            {
//                g.FillRectangle(e.BackBrush, bounds);
//                g.DrawString(this.GetColumnValueAtRow(source, rowNum).ToString(), e.TextFont, e.ForeBrush, bounds.X, bounds.Y);
//            }
//            if(e.TextFont != this.DataGridTableStyle.DataGrid.Font)
//                e.TextFont.Dispose();
//        }

//        protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
//        {
//            if (!readOnly)
//            {
//                if (boolMultiLine == false)
//                {
//                    if (SelectedRow > -1 && SelectedRow < source.List.Count + 1)
//                    {
//                        if (SelectedRow > this.DataGridTableStyle.DataGrid.VisibleRowCount - 1)
//                            SelectedRow = -1;
//                        else
//                            this.DataGridTableStyle.DataGrid.UnSelect(SelectedRow);
//                    }
//                    SelectedRow = rowNum;
//                    this.DataGridTableStyle.DataGrid.Select(SelectedRow);
//                }
//                else
//                {

//                    SelectedRow = rowNum;
//                    this.DataGridTableStyle.DataGrid.Select(SelectedRow);
//                }

//                try
//                {

//                    base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
//                }
//                catch
//                {
//                }
//            }
            

//        }
		
//    }

//    public delegate void FormatCellEventHandler(object sender, DataGridFormatCellEventArgs e);

//    public class DataGridFormatCellEventArgs : EventArgs
//    {
//        private int _column;
//        private int _row;
//        private Font _font;
//        private Brush _backBrush;
//        private Brush _foreBrush;
//        private bool _useBaseClassDrawing;

//        public DataGridFormatCellEventArgs(int row, int col, Font font1, Brush backBrush, Brush foreBrush)
//        {
//            _row = row;
//            _column = col;
//            _font = font1;
//            _backBrush = backBrush;
//            _foreBrush = foreBrush;
//            _useBaseClassDrawing = false;
//        }

//        public int Column
//        {
//            get{ return _column;}
//            set{ _column = value;}
//        }
//        public int Row
//        {
//            get{ return _row;}
//            set{ _row = value;}
//        }
//        public Font TextFont
//        {
//            get{ return _font;}
//            set{ _font = value;}
//        }

//        public Brush BackBrush
//        {
//            get{ return _backBrush;}
//            set{ _backBrush = value;}
//        }
//        public Brush ForeBrush
//        {
//            get{ return _foreBrush;}
//            set{ _foreBrush = value;}
//        }
//        public bool UseBaseClassDrawing
//        {
//            get{ return _useBaseClassDrawing;}
//            set{ _useBaseClassDrawing = value;}
//        }
//    }
//}
