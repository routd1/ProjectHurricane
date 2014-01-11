using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ICTEAS.WinForms.Utilities
{
    public enum Privilege { Create, Update, ViewOnly, Hidden, NotApplicable, Special };

    public class FxMenuBar : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        

        #region Overridden From Object
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Overridden From Object

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FxMenuBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Name = "FxMenuBar";
            this.Size = new System.Drawing.Size(136, 40);
            this.Load += new System.EventHandler(this.FxMenuBar_Load);
            this.MouseLeave += new System.EventHandler(this.FxMenuBar_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FxMenuBar_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FxMenuBar_MouseMove);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FxMenuBar_MouseClick);
            this.Resize += new System.EventHandler(this.FxMenuBar_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        #region FxMenuBarButtons
        public class FxMenuBarButtons : CollectionBase
        {
            protected FxMenuBar parent;
            public FxMenuBar Parent
            {
                get{ return parent;}
            }

            internal FxMenuBarButtons(FxMenuBar parent) : base()
            {
                this.parent = parent;
            }

            public FxMenuBarButton this[int index]
            {
                get { return (FxMenuBarButton)List[index]; }
            }

            public void Add(FxMenuBarButton item)
            {
                if (List.Count == 0) Parent.SelectedButton = item;
                List.Add(item);
                item.Parent = this.Parent;
                Parent.ButtonlistChanged();
            }

            
            public FxMenuBarButton Add(string text, Image image)
            {
                FxMenuBarButton b = new FxMenuBarButton(this.parent);
                b.Text = text;
                b.Image = image;
                this.Add(b);
                return b;
            }

            public FxMenuBarButton Add(string text)
            {
                return this.Add(text, null);
            }

            public FxMenuBarButton Add()
            {
                return this.Add();
            }



            public void Remove(FxMenuBarButton button)
            {
                List.Remove(button);
                Parent.ButtonlistChanged();
            }

            public int IndexOf(object value)
            {
                return List.IndexOf(value);
            }
            #region handle CollectionBase events
            protected override void OnInsertComplete(int index, object value)
            {
                FxMenuBarButton b = (FxMenuBarButton)value;
                b.Parent = this.parent;
                Parent.ButtonlistChanged();
                base.OnInsertComplete(index, value);
            }

            protected override void OnSetComplete(int index, object oldValue, object newValue)
            {
                FxMenuBarButton b = (FxMenuBarButton)newValue;
                b.Parent = this.parent;
                Parent.ButtonlistChanged();
                base.OnSetComplete(index, oldValue, newValue);
            }

            protected override void OnClearComplete()
            {
                Parent.ButtonlistChanged();
                base.OnClearComplete();
            }
            #endregion handle CollectionBase events

        }
        #endregion FxMenuBarButtons

        #region FxMenuBar property definitions

       
        /// <summary>
        /// buttons contains the list of clickable OutlookBarButtons
        /// </summary>
        protected FxMenuBarButtons buttons;

        /// <summary>
        /// this variable remembers which button is currently selected
        /// </summary>
        protected FxMenuBarButton selectedButton = null;

        /// <summary>
        /// this variable remembers the button index over which the mouse is moving
        /// </summary>
        protected int hoveringButtonIndex = -1;

        /// <summary>
        /// property to set the buttonHeigt
        /// default is 30
        /// </summary>
        protected int buttonHeight;
        [Description("Specifies the height of each button on the OutlookBar"), Category("Layout")]
        public int ButtonHeight
        {
            get { return buttonHeight; }
            set
            {
                if (value > 18)
                    buttonHeight = value;
                else
                    buttonHeight = 18;
            }
        }

        protected Color gradientButtonDark = Color.FromArgb(178, 193, 140);
        [Description("Dark gradient color of the button"), Category("Appearance")]
        public Color GradientButtonNormalDark
        {
            get { return gradientButtonDark; }
            set { gradientButtonDark = value; }
        }

        protected Color gradientButtonLight = Color.FromArgb(234, 240, 207);
        [Description("Light gradient color of the button"), Category("Appearance")]
        public Color GradientButtonNormalLight
        {
            get { return gradientButtonLight; }
            set { gradientButtonLight = value; }
        }

        protected Color gradientButtonHoverDark = Color.FromArgb(247, 192, 91);
        [Description("Dark gradient color of the button when the mouse is moving over it"), Category("Appearance")]
        public Color GradientButtonHoverDark
        {
            get { return gradientButtonHoverDark; }
            set { gradientButtonHoverDark = value; }
        }

        protected Color gradientButtonHoverLight = Color.FromArgb(255, 255, 220);
        [Description("Light gradient color of the button when the mouse is moving over it"), Category("Appearance")]
        public Color GradientButtonHoverLight
        {
            get { return gradientButtonHoverLight; }
            set { gradientButtonHoverLight = value; }
        }

        protected Color gradientButtonSelectedDark = Color.FromArgb(239, 150, 21);
        [Description("Dark gradient color of the seleced button"), Category("Appearance")]
        public Color GradientButtonSelectedDark
        {
            get { return gradientButtonSelectedDark; }
            set { gradientButtonSelectedDark = value; }
        }

        protected Color gradientButtonSelectedLight = Color.FromArgb(251, 230, 148);
        [Description("Light gradient color of the seleced button"), Category("Appearance")]
        public Color GradientButtonSelectedLight
        {
            get { return gradientButtonSelectedLight; }
            set { gradientButtonSelectedLight = value; }
        }


        /// <summary>
        /// when a button is selected programatically, it must update the control
        /// and repaint the buttons
        /// </summary>
        [Browsable(false)]
        public FxMenuBarButton SelectedButton
        {
            get { return selectedButton; }
            set
            {
                // assign new selected button
                PaintSelectedButton(selectedButton, value);

                // assign new selected button
                selectedButton = value;
            }
        }

        /// <summary>
        /// readonly list of buttons
        /// </summary>
        //[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FxMenuBarButtons Buttons
        {
            get { return buttons; }
        }

        #endregion FxMenuBar property definitions

        #region FxMenuBar events
        [Serializable]
        public class ButtonClickEventArgs : MouseEventArgs
        {
            public ButtonClickEventArgs(FxMenuBarButton button, MouseEventArgs evt)
                : base(evt.Button, evt.Clicks, evt.X, evt.Y, evt.Delta)
            {
                SelectedButton = button;
            }

            public readonly FxMenuBarButton SelectedButton;
        }

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);

        public new event ButtonClickEventHandler Click;

        #endregion FxMenuBar events

        #region FxMenuBar functions

        public FxMenuBar()
        {
            InitializeComponent();
            buttons = new FxMenuBarButtons(this);
            buttonHeight = 30; // set default to 30
        }

        private void PaintSelectedButton(FxMenuBarButton prevButton, FxMenuBarButton newButton)
        {
            if (prevButton == newButton)
                return; // no change so return immediately

            int selIdx = -1;
            int valIdx = -1;
            
            // find the indexes of the previous and new button
            selIdx = buttons.IndexOf(prevButton);
            valIdx = buttons.IndexOf(newButton);

            // now reset selected button
            // mouse is leaving control, so unhighlight anythign that is highlighted
            Graphics g = Graphics.FromHwnd(this.Handle);
            if (selIdx >= 0)
                // un-highlight current hovering button
                buttons[selIdx].PaintButton(g, 1, selIdx * (buttonHeight + 1) + 1, false, false);

            if (valIdx >= 0)
                // highlight newly selected button
                buttons[valIdx].PaintButton(g, 1, valIdx * (buttonHeight + 1) + 1, true, false);
            g.Dispose();
        }

        /// <summary>
        /// returns the button given the coordinates relative to the Outlookbar control
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public FxMenuBarButton HitTest(int x, int y)
        {
            int index = (y - 1) / (buttonHeight + 1);
            if (index >= 0 && index < buttons.Count)
                return buttons[index];
            else
                return null;
        }

        /// <summary>
        /// this function will setup the control to cope with changes in the buttonlist 
        /// that is, addition and removal of buttons
        /// </summary>
        private void ButtonlistChanged()
        {
            if (!this.DesignMode) // only set sizes automatically at runtime
                this.MaximumSize = new Size(0, buttons.Count * (buttonHeight + 1) + 1);

            this.Invalidate();
        }

        #endregion FxMenuBar functions

        #region FxMenuBar control event handlers

        private void FxMenuBar_Load(object sender, EventArgs e)
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer |
                ControlStyles.Selectable |
                ControlStyles.UserMouse,
                true
                );
        }

        private void FxMenuBar_Paint(object sender, PaintEventArgs e)
        {
            int top = 1;
            foreach (FxMenuBarButton b in Buttons)
            {
                b.PaintButton(e.Graphics, 1, top, b.Equals(this.selectedButton), false);
                top += b.Height + 1;
            }
        }

        private void FxMenuBar_Click(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs)) return;

            // case to MouseEventArgs so position and mousebutton clicked can be used
            MouseEventArgs mea = (MouseEventArgs)e;

            // only continue if left mouse button was clicked
            if (mea.Button != MouseButtons.Left) return;

            int index = (mea.Y - 1) / (buttonHeight + 1);

            if (index < 0 || index >= buttons.Count)
                return;

            FxMenuBarButton button = buttons[index];
            if (button == null) return;
            if (!button.Enabled) return;

            // ok, all checks passed so assign the new selected button
            // and raise the event
            SelectedButton = button;

            ButtonClickEventArgs bce = new ButtonClickEventArgs(selectedButton, mea);
            if (Click != null) // only invoke on left mouse click
                Click.Invoke(this, bce);
        }


        private void FxMenuBar_MouseLeave(object sender, EventArgs e)
        {
            if (hoveringButtonIndex >= 0)
            {
                // so we need to change the hoveringButtonIndex to the new index
                Graphics g = Graphics.FromHwnd(this.Handle);
                FxMenuBarButton b1 = buttons[hoveringButtonIndex];

                // un-highlight current hovering button
                b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                hoveringButtonIndex = -1;
                g.Dispose();
            }
        }

        private void FxMenuBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                // determine over which button the mouse is moving
                int index = (e.Location.Y - 1) / (buttonHeight + 1);
                if (index >= 0 && index < buttons.Count)
                {
                    if (hoveringButtonIndex == index)
                        return; // nothing changed so we're done, current button stays highlighted

                    // so we need to change the hoveringButtonIndex to the new index
                    Graphics g = Graphics.FromHwnd(this.Handle);

                    if (hoveringButtonIndex >= 0)
                    {
                        FxMenuBarButton b1 = buttons[hoveringButtonIndex];

                        // un-highlight current hovering button
                        b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                    }

                    // highlight new hovering button
                    FxMenuBarButton b2 = buttons[index];
                    b2.PaintButton(g, 1, index * (buttonHeight + 1) + 1, b2.Equals(selectedButton), true);
                    hoveringButtonIndex = index; // set to new index
                    g.Dispose();

                }
                else
                {
                    // no hovering button, so un-highlight all.
                    if (hoveringButtonIndex >= 0)
                    {
                        // so we need to change the hoveringButtonIndex to the new index
                        Graphics g = Graphics.FromHwnd(this.Handle);
                        FxMenuBarButton b1 = buttons[hoveringButtonIndex];

                        // un-highlight current hovering button
                        b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                        hoveringButtonIndex = -1;
                        g.Dispose();
                    }
                }
            }
        }

        private void FxMenuBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (!(e is MouseEventArgs)) return;

            // case to MouseEventArgs so position and mousebutton clicked can be used
            MouseEventArgs mea = (MouseEventArgs)e;

            // only continue if left mouse button was clicked
            if (mea.Button != MouseButtons.Left) return;

            int index = (mea.Y - 1) / (buttonHeight + 1);

            if (index < 0 || index >= buttons.Count)
                return;

            FxMenuBarButton button = buttons[index];
            if (button == null) return;
            if (!button.Enabled) return;

            // ok, all checks passed so assign the new selected button
            // and raise the event
            SelectedButton = button;

            ButtonClickEventArgs bce = new ButtonClickEventArgs(selectedButton, mea);
            if (Click != null) // only invoke on left mouse click
                Click.Invoke(this, bce);
        }

        private bool isResizing = false;
        /// <summary>
        /// isResizing is used as a signal, so this method is not called recusively
        /// this prevents a stack overflow
        /// </summary>
        private void FxMenuBar_Resize(object sender, EventArgs e)
        {
            // only set sizes automatically at runtime
            if (!this.DesignMode)
            {
                if (!isResizing)
                {
                    isResizing = true;
                    if ((this.Height - 1) % (buttonHeight + 1) > 0)
                        this.Height = ((this.Height - 1) / (buttonHeight + 1)) * (buttonHeight + 1) + 1;
                    this.Invalidate();
                    isResizing = false;
                }
            }
        }

        #endregion FxMenuBar control event handlers

        
    }

    #region FxMenuBarButton
    public class FxMenuBarButton // : IComponent
    {
        private bool enabled = true;

        [Description("Indicates wether the button is enabled"), Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected Image image = null;
        [Description("The image that will be displayed on the button"), Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                parent.Invalidate();
            }
        }

        protected object tag = null;
        [Description("User-defined data to be associated with the button"), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public FxMenuBarButton()
        {
            this.parent = new FxMenuBar(); // set it to a dummy outlookbar control
            text = "";
        }

        public FxMenuBarButton(FxMenuBar parent)
        {
            this.parent = parent;
            text = "";
        }

        protected FxMenuBar parent;


        internal FxMenuBar Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected string text;
        [Description("The text that will be displayed on the button"), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                parent.Invalidate();
            }
        }

        protected int height;
        public int Height
        {
            get { return parent == null ? 30 : parent.ButtonHeight; }

        }

        public int Width
        {
            get { return parent == null ? 60 : parent.Width - 2; }
        }

        /// <summary>
        /// the outlook button will paint itself on its container (the OutlookBar)
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="IsSelected"></param>
        /// <param name="IsHovering"></param>
        public void PaintButton(Graphics graphics, int x, int y, bool IsSelected, bool IsHovering)
        {
            Brush br;
            Rectangle rect = new Rectangle(0, y, this.Width, this.Height);
            if (enabled)
            {
                if (IsSelected)
                    if (IsHovering)
                        br = new LinearGradientBrush(rect, parent.GradientButtonSelectedDark, parent.GradientButtonSelectedLight, 90f);
                    else
                        br = new LinearGradientBrush(rect, parent.GradientButtonSelectedLight, parent.GradientButtonSelectedDark, 90f);
                else
                    if (IsHovering)
                        br = new LinearGradientBrush(rect, parent.GradientButtonHoverLight, parent.GradientButtonHoverDark, 90f);
                    else
                        br = new LinearGradientBrush(rect, parent.GradientButtonNormalLight, parent.GradientButtonNormalDark, 90f);
            }
            else
                br = new LinearGradientBrush(rect, parent.GradientButtonNormalLight, parent.GradientButtonNormalDark, 90f);

            graphics.FillRectangle(br, x, y, this.Width, this.Height);
            br.Dispose();

            if (text.Length > 0)
                graphics.DrawString(this.Text, parent.Font, Brushes.Black, 36, y + this.Height / 2 - parent.Font.Height / 2);

            if (image != null)
            {
                graphics.DrawImage(image, 36 / 2 - image.Width / 2, y + this.Height / 2 - image.Height / 2, image.Width, image.Height);
            }
        }

        private string _DBName = "";
        private string _MessageID = "";
        private Privilege m_Privlg = Privilege.ViewOnly;

        /// <summary>
        /// This is to be set same as the clsMenuItem object name
        /// </summary>
        /// 
        [Description("Name of the menu button as stored in database."), Category("Language Conversion")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DBName
        {
            set { if (_DBName == "") { _DBName = value; } }
            get { return _DBName; }
        }

        [Description("Message Id for the menu button as stored in database."), Category("Language Conversion")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string MessageID
        {
            set { _MessageID = value; }
            get { return _MessageID; }
        }

        [Description("Menu Level priviledge for the menu button."), Category("Language Conversion")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Privilege Privilege
        {
            get { return m_Privlg; }
            set { m_Privlg = value; }
        }
    }
    #endregion
}
