using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ICTEAS.WinForms.Utilities
{
    public partial class FxTextBox : TextBox
    {
        public FxTextBox()
        {
            InitializeComponent();
        }


        #region "     Designer.cs     "
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #endregion

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        /// <param name="g">The Graphics region to draw the prompt on</param>
        public void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            flags = flags | TextFormatFlags.HorizontalCenter;
            rect.Offset(0, 1);

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, "Enter text to filter data.", this.Font, rect, Color.FromArgb(9, 19, 205), this.BackColor, flags);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.Text.Length == 0)
            {
                DrawTextPrompt(e.Graphics);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (this.Text.Length == 0)
            {
                DrawTextPrompt(this.CreateGraphics());
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this.Text.Length == 0)
            {
                DrawTextPrompt(this.CreateGraphics());
            }
        }
       
    }
}
