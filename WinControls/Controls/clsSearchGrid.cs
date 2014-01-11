using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using ICTEAS.WinForms.Common;
using System.Drawing.Drawing2D;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsSearchGrid.
	/// </summary>
	public class clsSearchGrid : System.Windows.Forms.DataGridView
	{

		public DataTable dtFullData;
		private DataTable dtFilterData;
        private DataTable dtSelected;
        ArrayList selectedrow = new ArrayList();
       
        DataGridViewTextBoxEditingControl editingcontrol;
        string[] strColNames, strHeadings;
		private DataGridView dgGrid;
        private Font captionfont;
        public string captiontext;
        public bool filterpresent=false;
        int intVisibleCount;

		private string strMsgID = "";

        frmHotSearch ftemp;
        private string[] strLangID = new string[0];
		public clsSearchGrid()
		{
			//
			// TODO: Add constructor logic here
			//
			//this.BackColor=Color.AliceBlue ;

            //Coloring
            this.EnableHeadersVisualStyles = true;
            //this.ColumnHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
            //this.DefaultCellStyle.ForeColor = Color.Red;
            //this.DefaultCellStyle.SelectionForeColor = Color.Green;
            //this.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.BackgroundColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            
            //THIS LINE IS ADDED SO THAT EACH CAN EDITED WITH A SINGLE CLICK
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            //***FOLLOWING LINES ARE ADDED TO REDUCE FLICKER OF THE COMPONENT
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //****************************************************************
            this.EnableHeadersVisualStyles = true;
            this.ScrollBars = ScrollBars.Both;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            //Alternating Row Default Style
            System.Windows.Forms.DataGridViewCellStyle alternatingCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            alternatingCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            alternatingCellStyle.BackColor = System.Drawing.Color.Transparent;
            alternatingCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            alternatingCellStyle.ForeColor = System.Drawing.Color.Black;
            alternatingCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            alternatingCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.AlternatingRowsDefaultCellStyle = alternatingCellStyle;

            //Column Header Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle columnHeaderCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            columnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            columnHeaderCellStyle.BackColor = System.Drawing.SystemColors.Control;
            columnHeaderCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            columnHeaderCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            columnHeaderCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            columnHeaderCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ColumnHeadersDefaultCellStyle = columnHeaderCellStyle;


            //Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle defaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            defaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            defaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            defaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            defaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            defaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            defaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            defaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DefaultCellStyle = defaultCellStyle;

            //
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.GridColor = System.Drawing.Color.CadetBlue;
            this.RowHeadersWidth = 30;
            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RowTemplate.Height = 16;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = Color.FromArgb(105,185,243);
		}
        public clsSearchGrid(frmHotSearch f)  //Nilanjan 9/05/07
        {
            //this.BackColor = Color.AliceBlue;
            //Coloring
            this.EnableHeadersVisualStyles = false;
            //this.ColumnHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.BackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.ForeColor = Color.SeaGreen;
            //this.DefaultCellStyle.ForeColor = Color.Red;
            //this.DefaultCellStyle.SelectionForeColor = Color.Green;
            //this.DefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.BackgroundColor = Color.Honeydew;
            //this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
            //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Honeydew;
            //
            ftemp = f;
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            //Changing For New Component
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            //***FOLLOWING LINES ARE ADDED TO REDUCE FLICKER OF THE COMPONENT
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //****************************************************************
            this.EnableHeadersVisualStyles = true;
            this.ScrollBars = ScrollBars.Both;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.NotSet;

            //Alternating Row Default Style
            System.Windows.Forms.DataGridViewCellStyle alternatingCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            alternatingCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            alternatingCellStyle.BackColor = System.Drawing.Color.Transparent;
            alternatingCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            alternatingCellStyle.ForeColor = System.Drawing.Color.Black;
            alternatingCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            alternatingCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.AlternatingRowsDefaultCellStyle = alternatingCellStyle;

            //Column Header Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle columnHeaderCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            columnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            columnHeaderCellStyle.BackColor = System.Drawing.SystemColors.Control;
            columnHeaderCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            columnHeaderCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            columnHeaderCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            columnHeaderCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ColumnHeadersDefaultCellStyle = columnHeaderCellStyle;


            //Default Cell Style
            System.Windows.Forms.DataGridViewCellStyle defaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            defaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            defaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            defaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            defaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            defaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            defaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            defaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DefaultCellStyle = defaultCellStyle;

            //
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.GridColor = System.Drawing.Color.CadetBlue;
            this.RowHeadersWidth = 30;
            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RowTemplate.Height = 16;
            this.ColumnHeadersHeight = 18;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = Color.FromArgb(105, 185, 243);

            
        }

        public  object this[int RowNumber, int ColNumber]
        {
            get
            {

                DataRowView v = ((DataRowView)this.BindingContext[this.DataSource, this.DataMember].Current);
                return v.DataView[RowNumber][ColNumber];

            }
            set
            {

                DataRowView v = ((DataRowView)this.BindingContext[this.DataSource, this.DataMember].Current);
                v.DataView[RowNumber][ColNumber] = value;
                v.EndEdit();
            }
        }

        //Properties from the past
        public Font CaptionFont
        {
            get
            {
                return (this.captionfont);
            }
            set
            {
                this.captionfont = value;
            }


        }


        public Font HeaderFont
        {
            get
            {
                return (this.ColumnHeadersDefaultCellStyle.Font);
            }
            set
            {
                this.ColumnHeadersDefaultCellStyle.Font = value;
            }

        }

        public Color HeaderForeColor
        {
            get
            {
                return (this.ColumnHeadersDefaultCellStyle.ForeColor);
            }
            set
            {
                this.ColumnHeadersDefaultCellStyle.ForeColor = value;
                
            }

        }


        public int RowHeaderWidth
        {
            get
            {
                return (this.RowHeadersWidth);
            }
            set
            {
                this.RowHeadersWidth = value;
            }

        }


        public string CaptionText
        {
            get
            {
                return (this.captiontext);
            }
            set
            {
                this.captiontext = value;

            }
        }
        public Color AlternatingBackColor
        {
            get
            {
                return (this.AlternatingRowsDefaultCellStyle.BackColor);
            }
            set
            {
                this.AlternatingRowsDefaultCellStyle.BackColor = value;
            }
        }

        //**************************

        //Methods from the past

        public void Select(int RowNumber)
        {
            try
            {
                this.Rows[RowNumber].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        //**********************
		public string MessageID
		{
			get{return strMsgID;}
			set{strMsgID = value;}
		}

		public DataTable FullData
		{
			set
			{ 
				if( value != null )
					dtFullData = value.Copy();
			}

			get
			{
				if( dtFullData != null )
					return dtFullData.Copy();
				else
					return null;
			}
		}

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            //THIS IS ADDED TO AVOID THE BLACKENING OF THE CELLS ON ENTER...DEBANJAN
            e.CellStyle.BackColor = Color.White;
            
            base.OnEditingControlShowing(e);
        }

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            //THIS IS ADDED TO AVOID THE BLACKENING OF THE CELLS ON ENTER...DEBANJAN
            this.CurrentCell.Style.BackColor = Color.White;
            this.CurrentCell.Style.ForeColor = Color.Black;
           // base.OnCellEnter(e);
        }
        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            //this.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Linen;
            this.CurrentCell.Style.ForeColor = Color.Black;
            base.OnCellLeave(e);
        }


		public bool DesignGrid(DataGridView dgRecords,ref string strErrMsg)
		{
			try
			{
                InitializeComponent();
				
                dgGrid = dgRecords;
                this.AutoGenerateColumns = false;
				if( ((Control)dgGrid).GetType().Name == "clsReadOnlyGrid" )
				{
					clsReadOnlyGrid dg = (clsReadOnlyGrid)dgGrid;
					dg.AssociatedSearchGrid = this;
                    this.strLangID = dg.LangIDs;
				}
                if (((Control)dgGrid).GetType().Name == "clsWritableGrid")
                {
                    clsWritableGrid dg = (clsWritableGrid)dgGrid;
                    dg.AssociatedSearchGrid = this;
                    this.strLangID = dg.LangIDs;
                }
                //BindingSource BSource = ((BindingSource)dgGrid.DataSource);
                //dtFullData = (DataTable)BSource.DataSource;
                dtFullData = ((DataTable)dgGrid.DataSource).Copy();
				
				////**** Creating a new Table Style to include the TextBoxes in the Datagrid

                object[] obj = new object[dtFullData.Columns.Count];
                for (int i = 0; i < obj.Length; i++)
                {
                    obj[i] = null;
                }
                DataTable dtSingleRow = dtFullData.Clone();
                dtSingleRow.Rows.Add(obj);
                dtSingleRow.AcceptChanges();
                dtSelected = dtSingleRow.Clone();
                this.DataSource = null;
                this.Refresh();
                this.DataSource = dtSingleRow;
                this.Columns.Clear();
                this.Refresh();
                this.SetColumns();

                for (int i = 0; i < dtFullData.Columns.Count; i++)
                {
                    if (dtFullData.Columns[i].ColumnName.StartsWith("MASK"))
                        continue;
                    DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                    col.DataPropertyName = dtSingleRow.Columns[i].ColumnName;
                    col.HeaderText = strHeadings[i];
                    col.ReadOnly = false;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                    this.Columns.Add(col);
                }


                CurrencyManager cm = (CurrencyManager)this.BindingContext[this.DataSource];

                ((DataView)cm.List).AllowNew = false;



                this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);

                dtSingleRow.Dispose();
                //this.Font = new Font(this.Font, FontStyle.Regular);
                return true;
			}
			catch(Exception ex)
			{
				strErrMsg = ex.Message + " - " + ex.Source;
				return false;
			}
		}


        




      

        void clsSearchGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //this.Refresh();
            //Application.DoEvents();
            //this.BackgroundColor = Color.White;
            //this.DefaultCellStyle.BackColor = Color.White;

                this.EnableHeadersVisualStyles = true;

            //this.CellPainting += new DataGridViewCellPaintingEventHandler(clsSearchGrid_CellPainting);
             editingcontrol=(DataGridViewTextBoxEditingControl) e.Control;
             editingcontrol.BackColor = Color.White;
           
             editingcontrol.BorderStyle = BorderStyle.None;
             editingcontrol.Dock = DockStyle.Fill;
             editingcontrol.PrepareEditingControlForEdit(false);
             
            
            editingcontrol.TextChanged += new EventHandler(editingcontrol_TextChanged);
          
           
        }

        void clsSearchGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           // e.CellStyle.BackColor = System.Drawing.Color.White;
        }



        string old = "";

        void editingcontrol_TextChanged(object sender, EventArgs e)
        {
            
            visibility_changed(null, null);
           
            editingcontrol.ForeColor = System.Drawing.SystemColors.ControlText;
            editingcontrol.BackColor = System.Drawing.Color.White;
            editingcontrol.BorderStyle = BorderStyle.Fixed3D;
            string s = editingcontrol.Text;
            if (s != old)
            {

                this.CurrentCell.Value = s;
                this.FilterData(s);
                old = s;
                editingcontrol.SelectionStart = s.Length;
            }
 
        }


    








        private void FilterData(string value)
        {
            try
            {
                string m_strFilter = "";
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                
                sBuilder = get_filter();
                m_strFilter = sBuilder.ToString();
                dtFilterData = new DataTable();
                DataRow[] dr;
                int index=get_actual_colum_index(this.CurrentCell.ColumnIndex);
                dr = dtFullData.Select(m_strFilter, dtFullData.Columns[index].ColumnName, DataViewRowState.CurrentRows);
                dtFilterData = dtFullData.Clone();
                dtFilterData.Rows.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    dtFilterData.ImportRow(dr[i]);
                }
                dgGrid.DataSource = dtFilterData;
                CurrencyManager heidi = (CurrencyManager)dgGrid.BindingContext[dgGrid.DataSource];
                //heidi.Position = -1;
                heidi.Refresh();

                dtFilterData.Dispose();
                filterpresent = true;
                dr = null;
                ((DataView)heidi.List).RowFilter = "";
                ((DataView)heidi.List).AllowNew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

		public void ClearGrid()
		{
			for( int i = 0; i < intVisibleCount; i++ )
			{
				this[i,0] = System.DBNull.Value;
			
			}
			CurrencyManager cm = (CurrencyManager)dgGrid.BindingContext[dgGrid.DataSource, dgGrid.DataMember];      
			((DataView)cm.List).RowFilter = "";
			cm = null;

            if (((Control)dgGrid).GetType().Name == "clsReadOnlyGrid")
                dgGrid.DataSource = dtFullData;
		}

   

		
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // clsSearchGrid
            // 
            //this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.MultiSelect = false;
            //dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            //dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            //dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            //dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            //dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            //dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //this.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            //this.DefaultCellStyle.BackColor = System.Drawing.Color.Linen;
            this.VisibleChanged += new System.EventHandler(this.visibility_changed);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }


        public int get_actual_colum_index(int index)
        {
            try
            {
               
                int exactcolumn = 0;
                int store = 0; ;
                for (int i = 0; i < dtFullData.Columns.Count; i++)
                {
                    if (dtFullData.Columns[i].ColumnName.StartsWith("MASK"))
                    {

                        continue;

                    }
                    else
                    {
                        if (exactcolumn == index)
                        {
                            store = i;
                            break;
                        }
                        exactcolumn++;
                    }
                }
                return store;

            }
            catch (Exception ex)
            {
                return -1;
            }

        }



        public void dataUpgrade(DataTable dt)
        {
            try
            {
                dtSelected.Rows.Clear();
                dtSelected.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    dtSelected.LoadDataRow(dr.ItemArray, true);
                }
                dtSelected.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }





        protected override void OnMouseDown(MouseEventArgs e)
        {
            HitTestInfo info = this.HitTest(e.X, e.Y);
            //this.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(clsSearchGrid_EditingControlShowing);
            visibility_changed(null, null);
            base.OnMouseDown(e);
            
        }
        
        //protected override void oncelle

        public bool check_if_filterpresent(int current)
        {
            try
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (i != current)
                    {
                        if (this[0, i] != String.Empty)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMPONENT ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return false;
            }
        }


        public System.Text.StringBuilder get_filter()
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                DataTable dt =(DataTable) this.DataSource;
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    int index = get_actual_colum_index(i);
                    string s=String.Empty;
                    Type p = ((object)this[0, index]).GetType();
                    if(p.ToString()=="System.String")
                    {
                        s = (string)this[0, index];
                    }
                     
                    if (s == String.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        if (!boolWildCardSearch)
                        {
                            sb.Append("(" + dt.Columns[index].ColumnName + " LIKE  '" + s + "%' ) AND ");
                        }
                        else
                        {
                            sb.Append("(" + dt.Columns[index].ColumnName + " LIKE  '%" + s + "%' ) AND ");
                            boolWildCardSearch = false;
                        }
                    }
                }
                if (sb.ToString() != "")
                {
                    sb.Replace(" AND ", "", sb.Length - 5, 5);
                }
                return sb;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        private bool SetColumns()
        {
            try
            {
                
                DataTable dtSource = (DataTable)this.DataSource;
                strColNames = new string[dtSource.Columns.Count];
                strHeadings = new string[dtSource.Columns.Count];
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    strColNames[i] = dtSource.Columns[i].ColumnName;
                    strHeadings[i] = strColNames[i].Replace("_", " ") + " ";
                }
                ChangeLanguage();
                return true;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }


        private void ChangeLanguage()
        {
            try
            {
                string strTmp = "";
                clsLanguage objLang = new clsLanguage();
                DataTable dtSource = (DataTable)this.DataSource;
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


        public void setfulldata()
        {
            try
            {
                dgGrid.DataSource = dtFullData;
                dgGrid.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void visibility_changed(object sender, EventArgs e)
        {
            try
            {
                this.EnableHeadersVisualStyles = true;

                //this.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
                //this.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                //Font f = new Font(this.DefaultCellStyle.Font.Name.ToString(),this.DefaultCellStyle.Font.Size, FontStyle.Bold);
                //this.ColumnHeadersDefaultCellStyle.Font = f;

                //this.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                ////
                //this.DefaultCellStyle.BackColor = System.Drawing.Color.Linen;
                ////
                //this.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Red;
                //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
                ////  this.BackgroundColor = System.Drawing.Color.Honeydew;
                //this.RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
                //this.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Green;
                //this.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Honeydew;
                //this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
                //this.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            catch (Exception ex)
            {
            }
        }

        public bool boolWildCardSearch = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //this.CurrentCell.Value = this.CurrentCell.Value.ToString() + keyData;
            //return base.ProcessCmdKey(ref msg, keyData);
            try
            {
                if (keyData == Keys.Enter)
                {
                    boolWildCardSearch = false;
                    FilterData(this.CurrentCell.Value.ToString());
                }
                else if ((keyData == ((Control.ModifierKeys & Keys.Control) | Keys.Enter)) && (keyData != Keys.Enter))
                {
                    boolWildCardSearch = true;
                    FilterData(this.CurrentCell.Value.ToString());
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
	}
}
