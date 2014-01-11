using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsLabel.
	/// </summary>
	
	public enum LabelType
	{
		F2,
		Optional,
		Compulsary,
		NoChange,
        None
	};

    public enum PaintType
    {
        TypeI,
        TypeII,
        None
    };

    public enum BorderType
    {
        Left,
        Right,
        Top,
        Bottom,
        None
    };

	public class clsLabel : Label
	{
		private string m_strMsgID = "";
		private string strResName="";
		
		private LabelType lblType = LabelType.Optional;
        private ToolTip toolTip;
        private System.ComponentModel.IContainer components;

		private bool _defaultFont = true;

		public bool NormalFont
		{
			set{_defaultFont = value;}
			get{return _defaultFont;}
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

		public clsLabel()
		{
            this.BackColor = System.Drawing.Color.Blue;
        }
		
		public string ResourceName
		{
			get{return strResName;}
			set{strResName=value;}
		}

		public LabelType FieldType
		{
			get{return lblType;}
			set{lblType = value;}
		}

        private PaintType _paintType = PaintType.None;

        public PaintType TypeOfPaint
        {
            get { return _paintType; }
            set { _paintType = value; }
        }

        private BorderType _borderType = BorderType.None;
        public  BorderType TypeOfBorder
        {
            get { return _borderType; }
            set { _borderType = value; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Font f = new Font("Microsoft Sans Serif", 8.0f, FontStyle.Bold);
            if (_paintType == PaintType.None)
            {
 
            }
            else if (_paintType == PaintType.TypeI)
            {
                Color BackColor = Color.White;
                Color BackColor2 = Color.FromArgb(204, 204, 204);
                //LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), BackColor, BackColor2);
                //LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, BackColor, BackColor2, LinearGradientMode.Horizontal);
                Brush brush = new SolidBrush(Color.FromArgb(227,227,227));
                e.Graphics.FillRectangle(brush, ClientRectangle);
                Brush foreBrush = new SolidBrush(ForeColor);
                SizeF textSize = e.Graphics.MeasureString(Text, f);
                e.Graphics.DrawString(Text, f, foreBrush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2);
                brush.Dispose();
                foreBrush.Dispose();
            }
            else if (_paintType == PaintType.TypeII)
            {
                Color BackColor = Color.White;
                //Color BackColor2 = Color.FromArgb(84, 147, 191);
                Color BackColor2 = Color.FromArgb(146, 181, 202);
                //LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), BackColor, BackColor2);
                //LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, BackColor, BackColor2, LinearGradientMode.Horizontal);
                Brush brush = new SolidBrush(Color.FromArgb(200,215,230));
                
                e.Graphics.FillRectangle(brush, ClientRectangle);
                Brush foreBrush = new SolidBrush(ForeColor);
                SizeF textSize = e.Graphics.MeasureString(Text, f);
                e.Graphics.DrawString(Text, f, foreBrush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2);
                brush.Dispose();
                foreBrush.Dispose();
            }

            if (TypeOfBorder != BorderType.None)
            {
                e.Graphics.DrawLine(new Pen(Color.CadetBlue, 2.0f), new Point(this.ClientRectangle.Right, this.ClientRectangle.Top), new Point(this.ClientRectangle.Right, this.ClientRectangle.Bottom));
            }

        }

        private bool _ShowToolTips = false;
        public bool ShowToolTips
        {
            get { return _ShowToolTips; }
            set { _ShowToolTips = value; }
        }

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //    if (this.Text.ToString() != "" && ShowToolTips)
        //        toolTip.Show(this.Text, this.Parent);
        //}

        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //    if (this.Text.ToString() != "" && ShowToolTips)
        //        toolTip.Hide(this.Parent);
        //}

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ResumeLayout(false);

        }

	}
}
