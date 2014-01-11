using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

namespace ICTEAS.WinForms.Helpers
{
    public class GradientPanel : Panel
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
        }

        #endregion

        public GradientPanel()
        {
            InitializeComponent();
        }

        public GradientPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        //Members
        private System.Drawing.Color mStartColor;
        private System.Drawing.Color mEndColor;
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

        private LinearGradientMode _GradientMode = LinearGradientMode.Vertical;
        public LinearGradientMode GradientMode
        {
            get { return _GradientMode; }
            set { _GradientMode = value; }
        }

        private void PaintGradient()
        {
            System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
            //gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0),
            //new Point(this.Width, this.Height), mStartColor, mEndColor);
            gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, mStartColor, mEndColor, GradientMode);

            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gradBrush, new Rectangle(0, 0, this.Width, this.Height));
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
