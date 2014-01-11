//~~~~~~~~~~~~~~~~~~~~~ File Information ~~~~~~~~~~~~~~~~~~~~~~
//------This class is taken from a sample project in http://www.codeprojct.com/ 
//------Article Name: 'SplitButton: an XP style dropdown split button'
//------Article Author: Gladstone
//------Webpage Link: http://www.codeproject.com/KB/buttons/SplitButton.aspx
//------The original work had no explicit licence attached to it.
//------Last Modified By:   Debanjan Routh
//------Last Modified On:   23/03/2009

namespace SMETool.Controls
{
    #region "           Namespaces          "
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using System.Drawing;
    using System.ComponentModel;
    #endregion "           Namespaces          "

    /// <summary>
    /// 
    /// </summary>
    public class SplitButton : Button
    {
        #region "           Variables               "
        /// <summary>
        /// 
        /// </summary>
        private PushButtonState _state;
        /// <summary>
        /// 
        /// </summary>
        private const int PushButtonWidth = 14;
        /// <summary>
        /// 
        /// </summary>
        private static int BorderSize = SystemInformation.Border3DSize.Width * 2;
        /// <summary>
        /// 
        /// </summary>
        private bool skipNextOpen = false;
        /// <summary>
        /// 
        /// </summary>
        private Rectangle dropDownRectangle = new Rectangle();
        /// <summary>
        /// 
        /// </summary>
        private bool showSplit = true;
        #endregion "           Variables               "

        #region "           Ctor            "
        /// <summary>
        /// 
        /// </summary>
        public SplitButton()
        {
            this.AutoSize = true;
        }
        #endregion "           Ctor            "

        #region "               Properties          "
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool ShowSplit
        {
            set
            {
                if (value != showSplit)
                {
                    showSplit = value;
                    Invalidate();
                    if (this.Parent != null)
                    {
                        this.Parent.PerformLayout();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private PushButtonState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (!_state.Equals(value))
                {
                    _state = value;
                    Invalidate();
                }
            }
        }
        #endregion "               Properties          "

        #region "           Methods         "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proposedSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            if (showSplit && !string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + PushButtonWidth > preferredSize.Width)
            {
                return preferredSize + new Size(PushButtonWidth + BorderSize * 2, 0);
            }
            return preferredSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData.Equals(Keys.Down) && showSplit)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnGotFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (showSplit)
            {
                if (kevent.KeyCode.Equals(Keys.Down))
                {
                    ShowContextMenuStrip();
                }
                else if (kevent.KeyCode.Equals(Keys.Space) && kevent.Modifiers == Keys.None)
                {
                    State = PushButtonState.Pressed;
                }
            }

            base.OnKeyDown(kevent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (kevent.KeyCode.Equals(Keys.Space))
            {
                if (Control.MouseButtons == MouseButtons.None)
                {
                    State = PushButtonState.Normal;
                }
            }
            base.OnKeyUp(kevent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnLostFocus(e);
                return;
            }
            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Normal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseDown(e);
                return;
            }

            if (dropDownRectangle.Contains(e.Location))
            {
                ShowContextMenuStrip();
            }
            else
            {
                State = PushButtonState.Pressed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseEnter(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Hot;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseLeave(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                if (Focused)
                {
                    State = PushButtonState.Default;
                }
                else
                {
                    State = PushButtonState.Normal;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (!showSplit)
            {
                base.OnMouseUp(mevent);
                return;
            }

            if (ContextMenuStrip == null || !ContextMenuStrip.Visible)
            {
                SetButtonDrawState();
                if (Bounds.Contains(Parent.PointToClient(Cursor.Position)) && !dropDownRectangle.Contains(mevent.Location))
                {
                    OnClick(new EventArgs());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (!Enabled)
                return;
            if (!showSplit)
            {
                return;
            }

            Graphics g = pevent.Graphics;
            Rectangle bounds = this.ClientRectangle;

            // draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles)
            {
                Rectangle backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);
                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // button renderer doesnt draw the black frame when themes are off =(
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);

            }
            else
            {
                ButtonRenderer.DrawButton(g, bounds, State);
            }
            // calculate the current dropdown rectangle.
            dropDownRectangle = new Rectangle(bounds.Right - PushButtonWidth - 1, BorderSize, PushButtonWidth, bounds.Height - BorderSize * 2);

            int internalBorder = BorderSize;
            Rectangle focusRect =
                new Rectangle(internalBorder,
                              internalBorder,
                              bounds.Width - dropDownRectangle.Width - internalBorder,
                              bounds.Height - (internalBorder * 2));

            bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles);

            if (RightToLeft == RightToLeft.Yes)
            {
                dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = dropDownRectangle.Right;
                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Left + PushButtonWidth, BorderSize, bounds.Left + PushButtonWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Left + PushButtonWidth + 1, BorderSize, bounds.Left + PushButtonWidth + 1, bounds.Bottom - BorderSize);
                }
            }
            else
            {
                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Right - PushButtonWidth, BorderSize, bounds.Right - PushButtonWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Right - PushButtonWidth - 1, BorderSize, bounds.Right - PushButtonWidth - 1, bounds.Bottom - BorderSize);
                }

            }

            // Draw an arrow in the correct location 
            PaintArrow(g, dropDownRectangle);

            // Figure out how to draw the text
            TextFormatFlags formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            // If we dont' use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
            if (!UseMnemonic)
            {
                formatFlags = formatFlags | TextFormatFlags.NoPrefix;
            }
            else if (!ShowKeyboardCues)
            {
                formatFlags = formatFlags | TextFormatFlags.HidePrefix;
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                TextRenderer.DrawText(g, Text, Font, focusRect, SystemColors.ControlText, formatFlags);
            }

            // draw the focus rectangle.

            if (State != PushButtonState.Pressed && Focused)
            {
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="dropDownRect"></param>
        private void PaintArrow(Graphics g, Rectangle dropDownRect)
        {
            Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            //if the width is odd - favor pushing it over one pixel right.
            middle.X += (dropDownRect.Width % 2);

            Point[] arrow = new Point[] { new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1), new Point(middle.X, middle.Y + 2) };

            g.FillPolygon(SystemBrushes.ControlText, arrow);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowContextMenuStrip()
        {
            if (skipNextOpen)
            {
                // we were called because we're closing the context menu strip
                // when clicking the dropdown button.
                skipNextOpen = false;
                return;
            }
            State = PushButtonState.Pressed;

            if (ContextMenuStrip != null)
            {
                ContextMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
                ContextMenuStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            if (cms != null)
            {
                cms.Closing -= new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
            }

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                skipNextOpen = (dropDownRectangle.Contains(this.PointToClient(Cursor.Position)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetButtonDrawState()
        {
            if (Bounds.Contains(Parent.PointToClient(Cursor.Position)))
            {
                State = PushButtonState.Hot;
            }
            else if (Focused)
            {
                State = PushButtonState.Default;
            }
            else
            {
                State = PushButtonState.Normal;
            }
        }
        #endregion "           Methods         "
    }

}


