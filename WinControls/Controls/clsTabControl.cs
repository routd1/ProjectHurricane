using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Data;

namespace ICTEAS.WinForms.Controls
{
	/// <summary>
	/// Summary description for clsTabControl.
	/// </summary>
	public class clsTabControl	:	TabControl
	{
		private const int WM_LBUTTONDOWN = 513;

		private bool _ItemSelected = false;
		private int _ItemIndex = 0;

		public clsTabControl()
		{
			//this.DrawMode = TabDrawMode.OwnerDrawFixed; // INDRANIL
			
		}

		public void DrawRefresh()
		{
			_ItemSelected = true;
			for(int i=0; i < this.TabPages.Count; i++)
			{
				if(this.TabPages[i].Tag.ToString() != "0")
				{
					_ItemIndex = i;
					//this.DrawMode = TabDrawMode.OwnerDrawFixed; // INDRANIL
				}
			}
			_ItemSelected = false;
			this.Refresh();
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM_LBUTTONDOWN)
			{
				Point pt = new Point(m.LParam.ToInt32());
				for(int index=0; index < this.TabPages.Count; index++)
				{
					if(GetTabRect(index).Contains(pt)) 
					{
						if(TabPages[index].Tag.ToString() == "0")
						{
							base.WndProc(ref m);
						}
						//this.DrawMode = TabDrawMode.OwnerDrawFixed; // INDRANIL
						return;
					}
				}
			}
			//Keys keyData = (Keys)(int) m.WParam & Keys.KeyCode;
			base.WndProc (ref m);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			int index,currentIndex = 0;
			if(this.SelectedIndex != -1)
				currentIndex = this.SelectedIndex;

			if(e.KeyCode == Keys.Left && !(e.Alt && !e.Control))
			{
				for(index = currentIndex - 1 ; index >= 0; index--) 
				{
					if(TabPages[index].Tag.ToString().Trim() == "0")
					{
						this.SelectedIndex = index;
						//this.DrawMode = TabDrawMode.OwnerDrawFixed; // INDRANIL
						break;
					}
				}
				e.Handled = true;
			}
			else if(e.KeyCode == Keys.Right && !(e.Alt && !e.Control))
			{
				for(index = currentIndex + 1; index < TabPages.Count; index++)
				{
					if(TabPages[index].Tag.ToString().Trim() == "0")
					{
						this.SelectedIndex = index;
						//this.DrawMode = TabDrawMode.OwnerDrawFixed; // INDRANIL
						break;
					}
				}
				e.Handled = true;
			}
			base.OnKeyDown (e);
		}

	
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			int intOffsetLeft;
			int intOffsetTop;
			
			RectangleF r = RectangleF.FromLTRB(e.Bounds.Left,e.Bounds.Top,e.Bounds.Right,e.Bounds.Bottom);
			RectangleF r2;
			
			SolidBrush itemBrush = new SolidBrush(SystemColors.Control);
			Brush b;
			
			if(this.TabPages[e.Index].Tag.ToString() == "0")
				b = Brushes.Black;
			else
				b = Brushes.Gray;

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment =	StringAlignment.Center;

			Bitmap im;

			if(this.TabPages[e.Index].ImageIndex != -1)
			{
				im = (Bitmap)this.ImageList.Images[this.TabPages[e.Index].ImageIndex];
			}
			else
			{
				im = new Bitmap(10,10);
			}

			if(this.TabPages[e.Index].ImageIndex != -1)
			{
				r2 = new RectangleF(r.X + (im.Width / 2), r.Y, r.Width, r.Height);
			}
			else
			{
				r2 = new RectangleF(r.X,r.Y,r.Width,r.Height);
			}

			if(e.State == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(itemBrush,e.Bounds	);
				if(_ItemSelected)
				{
					b = Brushes.Gray;
					e.Graphics.DrawString(this.TabPages[_ItemIndex].Text,e.Font,b,r2,sf);
				}
				else
					e.Graphics.DrawString(this.TabPages[e.Index].Text,e.Font,b,r2,sf);
				intOffsetLeft = 5;
				intOffsetTop = 5;
			}
			else
			{
				if(_ItemSelected)
				{
					b = Brushes.Gray;
					e.Graphics.DrawString(this.TabPages[_ItemIndex].Text,e.Font,b,r2,sf);
				}
				else
					e.Graphics.DrawString(this.TabPages[e.Index].Text,e.Font,b,r2,sf);
				intOffsetLeft = 2;
				intOffsetTop = 2;
			}

			if(this.TabPages[e.Index].ImageIndex != -1)
			{
				this.ImageList.Draw(e.Graphics,Convert.ToInt32(r.Left) + intOffsetLeft,Convert.ToInt32(r.Top) + intOffsetTop,this.TabPages[e.Index].ImageIndex);
			}

			//base.OnDrawItem (e);
		}
	}
}
