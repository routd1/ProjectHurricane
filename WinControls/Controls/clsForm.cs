using System;
using System.Windows.Forms;
using System.Drawing;
using ICTEAS.WinForms.Common;
using ICTEAS.WinForms.Docking;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using ICTEAS.WinForms.Controls;
using ICTEASToolKit.Utilities;
namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsForm.
	/// </summary>
	public class clsForm : System.Windows.Forms.Form ,IDockContent 
	{

		private string strMsgID;
		private bool boolTextChange = false;

		private bool _ReportForm;
		private static bool _TranslateMDIMenu = true;
		private static int _InputLang = 1;
		private Privileges m_Privlg = Privileges.ViewOnly;

		public clsForm()
		{
			Application.DoEvents();
			_ReportForm = false;

            m_dockHandler = new DockContentHandler(this, new GetPersistStringCallback(GetPersistString));
            m_dockHandler.DockStateChanged += new EventHandler(DockHandler_DockStateChanged);
		}

		/// <summary>
		/// This property is for TVDMDI
		/// </summary>
		public bool ReportForm
		{
			get{return _ReportForm;}
			set{_ReportForm = value;}
		}

		public string MessageID
		{
			get{return strMsgID;}
			set{strMsgID = value;}
		}

		public static bool TranslateMDIMenu
		{
			get{return _TranslateMDIMenu;}
			set{_TranslateMDIMenu = value;}
		}

		// Indranil
		public static int InputLangID
		{
			get{return _InputLang;}
			set{_InputLang = value;}
		}
		
		public Privileges FormPrivilege
		{
			get
			{
				return m_Privlg;
			}
		}

		public void SetPrivilege( Privileges value )
		{
			m_Privlg = value;
		}

		public bool TxtChange
		{
			get
			{
				FindControl4txtChange(this);
				return boolTextChange;
			}
            set
            {
                boolTextChange = value;
            }

		}

		public void RefreshChange()
		{
			FindControl4Refresh(this);
			boolTextChange = false;
		}


        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
        }

		protected override void OnLoad(EventArgs e)
		{
			this.Text = GetStringOnID(strMsgID);
            this.TabText = GetStringOnID(strMsgID);
            if (clsProfile.DoEnableProfiling)
            {
                using (DesignHelper helper = new DesignHelper())
                {
                    helper.DoProfileBasedDesign(this);
                }
            }

			RecursivelyFindControl(this);

			base.OnLoad (e);
		}

		private string GetStringOnID(string strID)
		{
			clsLanguage objMessage = new clsLanguage();
			string strTemp = objMessage.LanguageString(strID);
			objMessage.Dispose(); 
			objMessage = null;
			if (strTemp == "*****")
				return "** " + this.Text ;
			else
				return strTemp;
		}

		private string GetStringVal(string str)
		{
			string strTemp = "";
			clsLanguage objLang = new clsLanguage();
			strTemp = objLang.LangEngString(str,true);
			objLang.Dispose();

			if(strTemp == "*****")
				return "** " + str;
			else
				return strTemp;
		}

		private string GetMenuTextByID(string MsgID, string strMenuText)
		{
			string strTemp = "";
			clsLanguage objLang = new clsLanguage();
			strTemp = objLang.LanguageString(MsgID);
			objLang.Dispose();

			if(strTemp == "*****")
			{
				if( strMenuText.IndexOf("** ") == -1 )
					strMenuText = "** " + strMenuText;
				return strMenuText;
			}
			else
				return strTemp;
		}



		/// <summary>
		/// Retrieve messages for control
		/// </summary>
		/// <param name="objControl"></param>
		private void SetMessageForControl(System.Windows.Forms.Control objControl,string strMsgID)
		{
			clsLanguage objMessage;

			if(strMsgID.Trim() == "")
			{
				if(objControl.Text.Trim() != "")
				{
                    objControl.Text = "** " + objControl.Text;
				}
				return;
			}

			objMessage = new clsLanguage();

			string strTemp = objMessage.LanguageString(strMsgID);

			if (strTemp == "*****")
			{
                objControl.ForeColor = System.Drawing.Color.Orange;
                objControl.Text = "**" + objControl.Text;
			}
			else
			{
                objControl.Text = strTemp;
			}

			objMessage.Dispose(); 
			objMessage = null;
		}

        //private void SetMessageForMenuBar(FxMenuBar _MenuBar)
        //{
        //    clsLanguage objMessage;

        //    if (strMsgID.Trim() == "")
        //    {
        //        if (objControl.Text.Trim() != "")
        //        {
        //            objControl.Text = "** " + objControl.Text;
        //        }
        //        return;
        //    }

        //    objMessage = new clsLanguage();

        //    foreach (FxMenuBarButton button in _MenuBar.Buttons)
        //    {
        //        string strMessage = button.MessageID;
        //    }

        //    string strTemp = objMessage.LanguageString(strMsgID);

        //    if (strTemp == "*****")
        //    {
        //        objControl.ForeColor = System.Drawing.Color.Orange;
        //        objControl.Text = "**" + objControl.Text;
        //    }
        //    else
        //    {
        //        objControl.Text = strTemp;
        //    }

        //    objMessage.Dispose();
        //    objMessage = null;
        //}

		private void RecursivelyFindControl(System.Windows.Forms.Control objControl)
		{
			//this control contains no other controls.
			//so terminate it and call 
			if (objControl.Controls.Count == 0)
			{
				//check that it's label control
				if (objControl.GetType().Name.ToString() == "clsLabel")
				{
					clsLabel objTmp = (clsLabel) objControl;
                    //SetColor(objTmp,objTmp.FieldType);
					SetMessageForControl(objTmp,objTmp.MessageID);
				}
				else if (objControl.GetType().Name.ToString() == "clsComboBox")
				{
					clsComboBox objTmp = (clsComboBox) objControl;
					if(objTmp.MultiLingual == true)
						SetMessage4ComboItems(objTmp);
				}
				else if (objControl.GetType().Name.ToString() == "clsRadioButton")
				{
					clsRadioButton objTmp = (clsRadioButton) objControl;
					SetMessageForControl(objControl,objTmp.MessageID);
				}
				else if (objControl.GetType().Name.ToString() == "clsCheckBox")
				{
					clsCheckBox objTmp = (clsCheckBox) objControl;
					SetMessageForControl(objControl,objTmp.MessageID);
				}
				else if (objControl.GetType().Name.ToString() == "clsButton")
				{
					clsButton objTmp = (clsButton) objControl;
					SetMessageForControl(objControl,objTmp.MessageID);
				}
                
			}
			else
			{
				if (objControl.GetType().Name.ToString() == "clsGroupBox")
				{
					clsGroupBox objTmp = (clsGroupBox) objControl;
					SetMessageForControl(objControl,objTmp.MessageID);
				}
				else if (objControl.GetType().Name == "TabControl" || objControl.GetType().Name == "clsTabControl")
				{
					TabControl objTmp = (TabControl) objControl;
					for(int index = 0; index < objTmp.TabPages.Count; index++)
					{
						string strTranslation = GetStringVal( objTmp.TabPages[index].Text );
						if( strTranslation == "*****")
							objTmp.TabPages[index].Text = "** " + objTmp.TabPages[index].Text;
						else
							objTmp.TabPages[index].Text = strTranslation;
					}
				}
                else if (objControl.GetType().Name == "FxTabControl")
                {
                    FxTabControl objTmp = (FxTabControl)objControl;
                    foreach (clsTabControlItem _item in objTmp.Items)
                    {
                        string strTranslation = GetStringVal(_item.Title.ToString());
                        if (strTranslation == "")
                            _item.Title = "" + _item.Title.ToString();
                        else
                            _item.Title = strTranslation;
                    }
                }
                else if (objControl.GetType().Name.ToString() == "Expando")
                {
                    Expando objExpando = (Expando)objControl;
                    string s = objExpando.MessageId;
                    s = s == null ? "" : s;
                    SetMessageForControl(objExpando, s);
                }
				foreach(System.Windows.Forms.Control objChildControl in objControl.Controls)
				{
					RecursivelyFindControl(objChildControl);
				}
			}
		}

		private void FindControl4Refresh(Control ctrl)
		{
			if(ctrl.Controls.Count == 0)
			{
				if(ctrl.GetType().Name.ToString() == "clsTxtBox")
				{
					clsTxtBox objTxt = (clsTxtBox) ctrl;
					objTxt.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "clsComboBox")
				{
					clsComboBox objCombo = (clsComboBox) ctrl;
					objCombo.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "clsAutoComboBox")
				{
					clsAutoComboBox objCombo = (clsAutoComboBox) ctrl;
					objCombo.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "clsDateTimePicker")
				{
					clsDateTimePicker objDtp = (clsDateTimePicker) ctrl;
					objDtp.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "clsCheckBox")
				{
					clsCheckBox objChkBox = (clsCheckBox) ctrl;
					objChkBox.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "clsRadioButton")
				{
					clsRadioButton objRadioBtn = (clsRadioButton) ctrl;
					objRadioBtn.RefreshChange();
				}
				else if(ctrl.GetType().Name.ToString() == "DateEdit")
				{
					DateEdit objDateEdit = (DateEdit) ctrl;
					objDateEdit.RefreshChange();
				}
			}
			else
			{
				if(ctrl.GetType().Name.ToString() == "clsWritableGrid")
				{
					clsWritableGrid objWritableGrid = (clsWritableGrid) ctrl;
					objWritableGrid.RefreshChange ();
				}
				else
				foreach(Control childCtrl in ctrl.Controls)
				{
					FindControl4Refresh(childCtrl);
				}
			}
		}

		private void FindControl4txtChange(Control ctrl)
		{
			if(boolTextChange == true)
				return;
			
			if(ctrl.Controls.Count == 0)
			{
				if(ctrl.GetType().Name.ToString() == "clsTxtBox")
				{
					clsTxtBox objTxt = (clsTxtBox) ctrl;
					boolTextChange = objTxt.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "clsComboBox")
				{
					clsComboBox objCombo = (clsComboBox) ctrl;
					boolTextChange = objCombo.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "clsAutoComboBox")
				{
					clsAutoComboBox objCombo = (clsAutoComboBox) ctrl;
					boolTextChange = objCombo.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "clsDateTimePicker")
				{
					clsDateTimePicker objDtp = (clsDateTimePicker) ctrl;
					boolTextChange = objDtp.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "clsCheckBox")
				{
					clsCheckBox objChkBox = (clsCheckBox) ctrl;
					boolTextChange = objChkBox.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "clsRadioButton")
				{
					clsRadioButton objRadioBtn = (clsRadioButton) ctrl;
					boolTextChange = objRadioBtn.TxtChange;
				}
				else if(ctrl.GetType().Name.ToString() == "DateEdit")
				{
					DateEdit objDateEdit = (DateEdit) ctrl;
					boolTextChange = objDateEdit.TxtChange;
				}
				
			}
			else
			{
				if(ctrl.GetType().Name.ToString() == "clsWritableGrid")
				{
					clsWritableGrid objWritableGrid = (clsWritableGrid) ctrl;
					boolTextChange = objWritableGrid.TxtChange;
				}
				else
                {
                    if (ctrl.GetType().Name.ToString() == "clsReadOnlyGrid")
                    {

                        boolTextChange = false;
                    }
                    else
                    {
					foreach(Control childCtrl in ctrl.Controls)
					{
						FindControl4txtChange(childCtrl);
					}
                    }
                }
			}
		}

		private void SetMessage4ComboItems(clsComboBox objCombo)
		{
			try
			{
				string[] local0 = new string[objCombo.Items.Count];

				for(int i=0; i < objCombo.Items.Count;i++)
					local0[i] = objCombo.Items[i].ToString();

				objCombo.StoreItems(local0);
				
				objCombo.Items.Clear();

				for(int i=0; i < local0.Length; i++)
				{
					clsLanguage objLang = new clsLanguage();
					string m_strTemp = objLang.LangEngString(local0[i]);
					
					if(m_strTemp != "*****")
						objCombo.Items.Add(m_strTemp);
					else
						objCombo.Items.Add("** " + local0[i]);

					objLang.Dispose();
				}
            }
			catch(Exception)
			{
				return;
			}
		}

		private void SetColor(Control objCntrl,LabelType lblType)
		{
			if(lblType == LabelType.F2)
			{
				objCntrl.ForeColor = System.Drawing.Color.Maroon;
				if(((clsLabel)objCntrl).NormalFont)
					objCntrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			}
			else if(lblType == LabelType.Compulsary)
			{
				objCntrl.ForeColor = System.Drawing.Color.Maroon;
				if(((clsLabel)objCntrl).NormalFont)
					objCntrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			}
			else if(lblType == LabelType.Optional)
			{
				objCntrl.ForeColor = System.Drawing.Color.Black;
				if(((clsLabel)objCntrl).NormalFont)
					objCntrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            }
            else if (lblType == LabelType.None)
            {
                objCntrl.ForeColor = System.Drawing.Color.White;
                if (((clsLabel)objCntrl).NormalFont)
                    objCntrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            }
			else
			{
				if(((clsLabel)objCntrl).NormalFont)
					objCntrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			}
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsForm));
			// 
			// clsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "clsForm";

		}


        //Profile Based Designing*********************************************
        private void DoRecursiveDesign(System.Windows.Forms.Control objControl)
        {
            //if(objControl.Controls.Count
        }


        //**********************************************************************************************
        //Docking Window Codes Starts From Here*********************************************************
        //**********************************************************************************************


        private DockContentHandler m_dockHandler = null;
        [Browsable(false)]
        public DockContentHandler DockHandler
        {
            get { return m_dockHandler; }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_AllowEndUserDocking_Description")]
        [DefaultValue(true)]
        public bool AllowEndUserDocking
        {
            get { return DockHandler.AllowEndUserDocking; }
            set { DockHandler.AllowEndUserDocking = value; }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_DockAreas_Description")]
        [DefaultValue(DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom | DockAreas.Document | DockAreas.Float)]
        public DockAreas DockAreas
        {
            get { return DockHandler.DockAreas; }
            set { DockHandler.DockAreas = value; }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_AutoHidePortion_Description")]
        [DefaultValue(0.25)]
        public double AutoHidePortion
        {
            get { return DockHandler.AutoHidePortion; }
            set { DockHandler.AutoHidePortion = value; }
        }

        [Localizable(true)]
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_TabText_Description")]
        [DefaultValue(null)]
        public string TabText
        {
            get { return DockHandler.TabText; }
            set { DockHandler.TabText = value; }
        }
        private bool ShouldSerializeTabText()
        {
            return (DockHandler.TabText != null);
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_CloseButton_Description")]
        [DefaultValue(true)]
        public bool CloseButton
        {
            get { return DockHandler.CloseButton; }
            set { DockHandler.CloseButton = value; }
        }

        [Browsable(false)]
        public DockPanel DockPanel
        {
            get { return DockHandler.DockPanel; }
            set { DockHandler.DockPanel = value; }
        }

        [Browsable(false)]
        public DockState DockState
        {
            get { return DockHandler.DockState; }
            set { DockHandler.DockState = value; }
        }

        [Browsable(false)]
        public DockPane Pane
        {
            get { return DockHandler.Pane; }
            set { DockHandler.Pane = value; }
        }

        [Browsable(false)]
        public bool IsHidden
        {
            get { return DockHandler.IsHidden; }
            set { DockHandler.IsHidden = value; }
        }

        [Browsable(false)]
        public DockState VisibleState
        {
            get { return DockHandler.VisibleState; }
            set { DockHandler.VisibleState = value; }
        }

        [Browsable(false)]
        public bool IsFloat
        {
            get { return DockHandler.IsFloat; }
            set { DockHandler.IsFloat = value; }
        }

        [Browsable(false)]
        public DockPane PanelPane
        {
            get { return DockHandler.PanelPane; }
            set { DockHandler.PanelPane = value; }
        }

        [Browsable(false)]
        public DockPane FloatPane
        {
            get { return DockHandler.FloatPane; }
            set { DockHandler.FloatPane = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual string GetPersistString()
        {
            return GetType().ToString();
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_HideOnClose_Description")]
        [DefaultValue(false)]
        public bool HideOnClose
        {
            get { return DockHandler.HideOnClose; }
            set { DockHandler.HideOnClose = value; }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_ShowHint_Description")]
        [DefaultValue(DockState.Unknown)]
        public DockState ShowHint
        {
            get { return DockHandler.ShowHint; }
            set { DockHandler.ShowHint = value; }
        }

        [Browsable(false)]
        public bool IsActivated
        {
            get { return DockHandler.IsActivated; }
        }

        public bool IsDockStateValid(DockState dockState)
        {
            return DockHandler.IsDockStateValid(dockState);
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_TabPageContextMenu_Description")]
        [DefaultValue(null)]
        public ContextMenu TabPageContextMenu
        {
            get { return DockHandler.TabPageContextMenu; }
            set { DockHandler.TabPageContextMenu = value; }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockContent_TabPageContextMenuStrip_Description")]
        [DefaultValue(null)]
        public ContextMenuStrip TabPageContextMenuStrip
        {
            get { return DockHandler.TabPageContextMenuStrip; }
            set { DockHandler.TabPageContextMenuStrip = value; }
        }

        [Localizable(true)]
        [Category("Appearance")]
        [LocalizedDescription("DockContent_ToolTipText_Description")]
        [DefaultValue(null)]
        public string ToolTipText
        {
            get { return DockHandler.ToolTipText; }
            set { DockHandler.ToolTipText = value; }
        }

        public new void Activate()
        {
            DockHandler.Activate();
        }

        public new void Hide()
        {
            DockHandler.Hide();
        }

        public new void Show()
        {
            DockHandler.Show();
        }

        public void Show(DockPanel dockPanel)
        {
            DockHandler.Show(dockPanel);

            if (this.ToolTipText==null)
                this.ToolTipText = this.TabText.ToString();
            if (this.TabText.Length > 26)
            {
                string s = this.TabText.Substring(0, 26);
                s += "...";
                this.TabText = s;
            }
        }

        public void Show(DockPanel dockPanel, DockState dockState)
        {
            DockHandler.Show(dockPanel, dockState);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
        public void Show(DockPanel dockPanel, Rectangle floatWindowBounds)
        {
            DockHandler.Show(dockPanel, floatWindowBounds);
        }

        public void Show(DockPane pane, IDockContent beforeContent)
        {
            DockHandler.Show(pane, beforeContent);
        }

        public void Show(DockPane previousPane, DockAlignment alignment, double proportion)
        {
            DockHandler.Show(previousPane, alignment, proportion);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
        public void FloatAt(Rectangle floatWindowBounds)
        {
            DockHandler.FloatAt(floatWindowBounds);
        }

        public void DockTo(DockPane paneTo, DockStyle dockStyle, int contentIndex)
        {
            DockHandler.DockTo(paneTo, dockStyle, contentIndex);
        }

        public void DockTo(DockPanel panel, DockStyle dockStyle)
        {
            DockHandler.DockTo(panel, dockStyle);
        }

        #region Events
        private void DockHandler_DockStateChanged(object sender, EventArgs e)
        {
            OnDockStateChanged(e);
        }

        private static readonly object DockStateChangedEvent = new object();
        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("Pane_DockStateChanged_Description")]
        public event EventHandler DockStateChanged
        {
            add { Events.AddHandler(DockStateChangedEvent, value); }
            remove { Events.RemoveHandler(DockStateChangedEvent, value); }
        }
        protected virtual void OnDockStateChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[DockStateChangedEvent];
            if (handler != null)
                handler(this, e);
        }
        #endregion
	}
}
