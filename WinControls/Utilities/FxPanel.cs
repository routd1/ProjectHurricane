#region Author Info.
///@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
/// ````````````````Debanjan Routh````````````````````````
///@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

namespace ICTEAS.WinForms.Utilities
{
    public class FxPanel : Panel
    {
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
        /// 
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
			components.Dispose();
        }

        #endregion

        public FxPanel()
        {
            InitializeComponent();
        }

        public FxPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        //Members
        private System.Drawing.Color mStartColor;
        private System.Drawing.Color mEndColor;
        private LinearGradientMode _GradientStyle = LinearGradientMode.Horizontal;

        public LinearGradientMode GradientStyle
        {
            get { return _GradientStyle; }
            set { _GradientStyle = value; PaintGradient(); }
        }

        public System.Drawing.Color PageStartColor
        {
            get
            {
                return mStartColor;
            }
            set
            {
                mStartColor = value;
                PaintGradient();
            }
        }


        public System.Drawing.Color PageEndColor
        {
            get
            {
                return mEndColor;
            }
            set
            {
                mEndColor = value;
                PaintGradient();
            }
        }

        

        private void PaintGradient()
        {
            try
            {
                if (this.ClientRectangle.Width == 0 && this.ClientRectangle.Height == 0)
                    return;
                System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
                //gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0),
                //new Point(this.Width, this.Height), mStartColor, mEndColor);
                //gradBrush.gr
                gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, mStartColor, mEndColor, GradientStyle);
                Bitmap bmp = new Bitmap(this.Width, this.Height);

                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(gradBrush, new Rectangle(0, 0, this.Width, this.Height));
                this.BackgroundImage = bmp;
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch
            {
 
            }
        }
    }
}
