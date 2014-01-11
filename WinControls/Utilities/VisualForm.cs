using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace ICTEAS.WinForms.Utilities
{
    public abstract class VisualForm : Form //,IFxDebug
    {
        //// Fields
        //private static bool a;
        //private bool b;
        //private bool c;
        //private bool d;
        //private bool e;
        //private bool f;
        //private bool g;
        //private bool h;
        //private bool i;
        //private bool j;
        //private bool k;
        //private int l;
        //private int m;
        //private int n;
        //private ViewBase o;
        //private IKryptonComposition p;
        //private IPalette q;
        //private IPalette r;
        //private IRenderer s;
        //private PaletteMode t;
        //private PaletteRedirect u;
        //private NeedPaintHandler v;
        //private ViewManager w;
        //private IntPtr x;

        //// Events
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler ApplyCustomChromeChanged;

        //[Category("Property Changed"), Description("Occurs when the value of the Palette property is changed.")]
        //public event EventHandler PaletteChanged;

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler WindowActiveChanged;

        //// Methods
        //static VisualForm()
        //{
        //    try
        //    {
        //        a = VisualStyleInformation.IsEnabledByUser && !string.IsNullOrEmpty(VisualStyleInformation.ColorScheme);
        //    }
        //    catch
        //    {
        //    }
        //}

        //public VisualForm()
        //{
        //    base.SetStyle(ControlStyles.ResizeRedraw, true);
        //    this.x = ip.CreateCompatibleDC(IntPtr.Zero);
        //    this.v = new NeedPaintHandler(this.OnNeedPaint);
        //    this.q = null;
        //    this.a(KryptonManager.CurrentGlobalPalette);
        //    this.t = PaletteMode.Global;
        //    this.i = true;
        //    this.l = 30;
        //    this.u = this.CreateRedirector();
        //    KryptonManager.GlobalPaletteChanged += new EventHandler(this.b);
        //    SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.a);
        //}

        //private void a()
        //{
        //    if (!this.h)
        //    {
        //        this.h = true;
        //        bool flag = ((!base.DesignMode && base.TopLevel) && (this.ApplyCustomChrome && this.AllowComposition)) && DWM.IsCompositionEnabled;
        //        if (this.ApplyComposition != flag)
        //        {
        //            this.g = flag;
        //            if (this.Composition != null)
        //            {
        //                this.Composition.CompVisible = this.g;
        //                this.Composition.CompOwnerForm = this;
        //            }
        //            DWM.ExtendFrameIntoClientArea(base.Handle, new Padding(0, this.g ? this.l : 0, 0, 0));
        //            if (this.ApplyCustomChrome)
        //            {
        //                this.a(false);
        //                this.a(true);
        //            }
        //        }
        //        this.h = false;
        //    }
        //}

        //private void a(IPalette A_0)
        //{
        //    if (A_0 != this.r)
        //    {
        //        if (this.r != null)
        //        {
        //            this.r.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(this.OnNeedPaint);
        //            this.r.ButtonSpecChanged -= new EventHandler(this.OnButtonSpecChanged);
        //            this.r.AllowFormChromeChanged -= new EventHandler(this.OnAllowFormChromeChanged);
        //            this.r.BasePaletteChanged -= new EventHandler(this.a);
        //            this.r.BaseRendererChanged -= new EventHandler(this.a);
        //        }
        //        this.r = A_0;
        //        this.s = this.r.GetRenderer();
        //        if (this.r != null)
        //        {
        //            this.r.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(this.OnNeedPaint);
        //            this.r.ButtonSpecChanged += new EventHandler(this.OnButtonSpecChanged);
        //            this.r.AllowFormChromeChanged += new EventHandler(this.OnAllowFormChromeChanged);
        //            this.r.BasePaletteChanged += new EventHandler(this.a);
        //            this.r.BaseRendererChanged += new EventHandler(this.a);
        //        }
        //    }
        //}

        //private void a(object A_0, UserPreferenceChangedEventArgs A_1)
        //{
        //    switch (A_1.Category)
        //    {
        //        case UserPreferenceCategory.Window:
        //        case UserPreferenceCategory.VisualStyle:
        //            this.PerformNeedPaint(true);
        //            break;

        //        case UserPreferenceCategory.Locale:
        //            break;

        //        default:
        //            return;
        //    }
        //}

        //private void a(object A_0, EventArgs A_1)
        //{
        //    this.s = this.r.GetRenderer();
        //}

        //private bool b()
        //{
        //    return (this.PaletteMode != PaletteMode.Global);
        //}

        //private void b(object A_0, EventArgs A_1)
        //{
        //    if (this.PaletteMode == PaletteMode.Global)
        //    {
        //        this.q = null;
        //        this.a(KryptonManager.CurrentGlobalPalette);
        //        this.Redirector.Target = this.r;
        //        this.OnNeedPaint(this.Palette, new NeedLayoutEventArgs(true));
        //    }
        //}

        //protected virtual PaletteRedirect CreateRedirector()
        //{
        //    return new PaletteRedirect(this.r);
        //}

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        //public ToolStripRenderer CreateToolStripRenderer()
        //{
        //    return this.Renderer.RenderToolStrip(this.GetResolvedPalette());
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    this.k = true;
        //    if (disposing)
        //    {
        //        if (this.r != null)
        //        {
        //            this.r.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(this.OnNeedPaint);
        //            this.r.ButtonSpecChanged -= new EventHandler(this.OnButtonSpecChanged);
        //            this.r.AllowFormChromeChanged -= new EventHandler(this.OnAllowFormChromeChanged);
        //            this.r.BasePaletteChanged -= new EventHandler(this.a);
        //            this.r.BaseRendererChanged -= new EventHandler(this.a);
        //        }
        //        KryptonManager.GlobalPaletteChanged -= new EventHandler(this.b);
        //        SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.a);
        //    }
        //    base.Dispose(disposing);
        //    if (this.ViewManager != null)
        //    {
        //        this.ViewManager.Dispose();
        //    }
        //    if (this.x != IntPtr.Zero)
        //    {
        //        ip.DeleteDC(this.x);
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        //public IPalette GetResolvedPalette()
        //{
        //    return this.r;
        //}

        //protected void InvalidateNonClient()
        //{
        //    this.InvalidateNonClient(Rectangle.Empty, true);
        //}

        //protected void InvalidateNonClient(Rectangle invalidRect)
        //{
        //    this.InvalidateNonClient(invalidRect, true);
        //}

        //protected void InvalidateNonClient(Rectangle invalidRect, bool excludeClientArea)
        //{
        //    if ((!base.IsDisposed && !base.Disposing) && base.IsHandleCreated)
        //    {
        //        if (invalidRect.IsEmpty)
        //        {
        //            Padding realWindowBorders = this.RealWindowBorders;
        //            Rectangle realWindowRectangle = this.RealWindowRectangle;
        //            invalidRect = new Rectangle(-realWindowBorders.Left, -realWindowBorders.Top, realWindowRectangle.Width, realWindowRectangle.Height);
        //        }
        //        using (Region region = new Region(invalidRect))
        //        {
        //            if (excludeClientArea)
        //            {
        //                region.Exclude(base.ClientRectangle);
        //            }
        //            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
        //            {
        //                IntPtr hrgn = region.GetHrgn(graphics);
        //                ip.RedrawWindow(base.Handle, IntPtr.Zero, hrgn, 0x501);
        //                ip.DeleteObject(hrgn);
        //            }
        //        }
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void KryptonResetCounters()
        //{
        //    this.ViewManager.ResetCounters();
        //}

        //protected override void OnActivated(EventArgs e)
        //{
        //    this.WindowActive = true;
        //    base.OnActivated(e);
        //}

        //protected virtual void OnAllowFormChromeChanged(object sender, EventArgs e)
        //{
        //}

        //protected virtual void OnApplyCustomChromeChanged(EventArgs e)
        //{
        //    if (this.z != null)
        //    {
        //        this.z(this, e);
        //    }
        //}

        //protected virtual void OnButtonSpecChanged(object sender, EventArgs e)
        //{
        //}

        //protected virtual bool OnCompWM_NCHITTEST(ref Message m)
        //{
        //    IntPtr ptr;
        //    ip.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, out ptr);
        //    m.Result = ptr;
        //    if (m.Result == IntPtr.Zero)
        //    {
        //        this.DefWndProc(ref m);
        //    }
        //    if ((m.Result == ((IntPtr)2)) || (m.Result == ((IntPtr)1)))
        //    {
        //        Point screenPt = new Point(m.LParam.ToInt32());
        //        Point pt = this.ScreenToWindow(screenPt);
        //        m.Result = this.WindowChromeHitTest(pt, true);
        //    }
        //    return true;
        //}

        //protected override void OnDeactivate(EventArgs e)
        //{
        //    this.WindowActive = false;
        //    base.OnDeactivate(e);
        //}

        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    try
        //    {
        //        ip.DisableProcessWindowsGhosting();
        //    }
        //    catch
        //    {
        //    }
        //    base.OnHandleCreated(e);
        //}

        //protected virtual void OnNeedPaint(object sender, NeedLayoutEventArgs e)
        //{
        //    int num = 14;
        //    if (e == null)
        //    {
        //        throw new ArgumentNullException(a("笝", num));
        //    }
        //    if (this.ApplyCustomChrome)
        //    {
        //        if (this.ApplyComposition)
        //        {
        //            this.p.CompNeedPaint(e.NeedLayout);
        //        }
        //        else
        //        {
        //            if (e.NeedLayout)
        //            {
        //                this.i = true;
        //            }
        //            this.InvalidateNonClient();
        //        }
        //    }
        //}

        //protected virtual void OnNonClientPaint(IntPtr hWnd)
        //{
        //    Rectangle realWindowRectangle = this.RealWindowRectangle;
        //    if ((realWindowRectangle.Width > 0) && (realWindowRectangle.Height > 0))
        //    {
        //        IntPtr windowDC = ip.GetWindowDC(base.Handle);
        //        if (windowDC != IntPtr.Zero)
        //        {
        //            try
        //            {
        //                IntPtr ptr2 = ip.CreateCompatibleBitmap(windowDC, realWindowRectangle.Width, realWindowRectangle.Height);
        //                if (ptr2 != IntPtr.Zero)
        //                {
        //                    try
        //                    {
        //                        Padding realWindowBorders = this.RealWindowBorders;
        //                        Rectangle rectangle2 = new Rectangle(realWindowBorders.Left, realWindowBorders.Top, realWindowRectangle.Width - realWindowBorders.Horizontal, realWindowRectangle.Height - realWindowBorders.Vertical);
        //                        bool flag = CommonHelper.IsFormMinimized(this);
        //                        if (flag || ((rectangle2.Width > 0) && (rectangle2.Height > 0)))
        //                        {
        //                            if (!flag)
        //                            {
        //                                ip.ExcludeClipRect(windowDC, rectangle2.Left, rectangle2.Top, rectangle2.Right, rectangle2.Bottom);
        //                            }
        //                            ip.SelectObject(this.x, ptr2);
        //                            using (Graphics graphics = Graphics.FromHdc(this.x))
        //                            {
        //                                this.WindowChromePaint(graphics, realWindowRectangle);
        //                            }
        //                            ip.BitBlt(windowDC, 0, 0, realWindowRectangle.Width, realWindowRectangle.Height, this.x, 0, 0, 0xcc0020);
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        ip.DeleteObject(ptr2);
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                ip.ReleaseDC(base.Handle, windowDC);
        //            }
        //        }
        //    }
        //    this.n++;
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    if (this.ApplyCustomChrome && this.ApplyComposition)
        //    {
        //        Rectangle rect = new Rectangle(0, 0, base.Width, this.l);
        //        e.Graphics.FillRectangle(Brushes.Black, rect);
        //        e.Graphics.SetClip(rect, CombineMode.Exclude);
        //    }
        //    base.OnPaintBackground(e);
        //}

        //protected virtual bool OnPaintNonClient(ref Message m)
        //{
        //    this.DefWndProc(ref m);
        //    this.InvalidateNonClient();
        //    return true;
        //}

        //protected virtual void OnPaletteChanged(EventArgs e)
        //{
        //    this.Redirector.Target = this.r;
        //    this.OnNeedPaint(this.Palette, new NeedLayoutEventArgs(true));
        //    if (this.y != null)
        //    {
        //        this.y(this, e);
        //    }
        //}

        //protected override void OnResize(EventArgs e)
        //{
        //    this.ResumePaint();
        //    base.OnResize(e);
        //    if (this.ApplyCustomChrome && ((base.MdiParent == null) || !CommonHelper.IsFormMaximized(this)))
        //    {
        //        this.PerformNeedPaint(true);
        //    }
        //    this.SuspendPaint();
        //}

        //protected virtual void OnWindowActiveChanged()
        //{
        //    if (this.aa != null)
        //    {
        //        this.aa(this, EventArgs.Empty);
        //    }
        //}

        //protected virtual bool OnWM_LBUTTONUP(ref Message m)
        //{
        //    this.j = false;
        //    base.Capture = false;
        //    this.o = null;
        //    this.d = false;
        //    Point p = new Point(m.LParam.ToInt32());
        //    Point screenPt = base.PointToScreen(p);
        //    Point rawPt = this.ScreenToWindow(screenPt);
        //    this.ViewManager.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, rawPt.X, rawPt.Y, 0), rawPt);
        //    this.ViewManager.MouseLeave(EventArgs.Empty);
        //    this.InvalidateNonClient();
        //    return true;
        //}

        //protected virtual bool OnWM_MOUSEMOVE(ref Message m)
        //{
        //    Point p = new Point(m.LParam.ToInt32());
        //    Point screenPt = base.PointToScreen(p);
        //    Point pt = this.ScreenToWindow(screenPt);
        //    this.WindowChromeNonClientMouseMove(pt);
        //    return true;
        //}

        //protected virtual bool OnWM_NCACTIVATE(ref Message m)
        //{
        //    this.WindowActive = m.WParam == ((IntPtr)1);
        //    if (!this.ApplyComposition)
        //    {
        //        if ((base.MdiParent == null) || this.b)
        //        {
        //            m.Result = (IntPtr)1;
        //            return true;
        //        }
        //        this.b = true;
        //    }
        //    return false;
        //}

        //protected virtual bool OnWM_NCCALCSIZE(ref Message m)
        //{
        //    if (m.WParam != IntPtr.Zero)
        //    {
        //        Padding empty;
        //        if (base.FormBorderStyle == FormBorderStyle.None)
        //        {
        //            empty = Padding.Empty;
        //        }
        //        else
        //        {
        //            empty = this.RealWindowBorders;
        //        }
        //        ip.n lParam = (ip.n)m.GetLParam(typeof(ip.n));
        //        if (this.ApplyComposition)
        //        {
        //            empty.Top = 0;
        //        }
        //        lParam.a.a += empty.Left;
        //        lParam.a.b += empty.Top;
        //        lParam.a.c -= empty.Right;
        //        lParam.a.d -= empty.Bottom;
        //        Marshal.StructureToPtr(lParam, m.LParam, false);
        //    }
        //    return true;
        //}

        //protected virtual bool OnWM_NCHITTEST(ref Message m)
        //{
        //    Point screenPt = new Point(m.LParam.ToInt32());
        //    Point pt = this.ScreenToWindow(screenPt);
        //    m.Result = this.WindowChromeHitTest(pt, false);
        //    return true;
        //}

        //protected virtual bool OnWM_NCLBUTTONDBLCLK(ref Message m)
        //{
        //    Point screenPt = new Point(m.LParam.ToInt32());
        //    Point pt = this.ScreenToWindow(screenPt);
        //    ViewBase base2 = this.ViewManager.Root.ViewFromPoint(pt);
        //    return ((base2 != null) && (base2.FindMouseController() != null));
        //}

        //protected virtual bool OnWM_NCLBUTTONDOWN(ref Message m)
        //{
        //    Point screenPt = new Point(m.LParam.ToInt32());
        //    Point pt = this.ScreenToWindow(screenPt);
        //    if (this.ApplyComposition)
        //    {
        //        pt.X -= this.RealWindowBorders.Left;
        //    }
        //    return this.WindowChromeLeftMouseDown(pt);
        //}

        //protected virtual bool OnWM_NCLBUTTONUP(ref Message m)
        //{
        //    Point screenPt = new Point(m.LParam.ToInt32());
        //    Point pt = this.ScreenToWindow(screenPt);
        //    if (this.ApplyComposition)
        //    {
        //        pt.X -= this.RealWindowBorders.Left;
        //    }
        //    return this.WindowChromeLeftMouseUp(pt);
        //}

        //protected virtual bool OnWM_NCMOUSELEAVE(ref Message m)
        //{
        //    this.d = false;
        //    this.WindowChromeMouseLeave();
        //    m.Result = IntPtr.Zero;
        //    this.InvalidateNonClient();
        //    return true;
        //}

        //protected virtual bool OnWM_NCMOUSEMOVE(ref Message m)
        //{
        //    Point screenPt = new Point(m.LParam.ToInt32());
        //    Point pt = this.ScreenToWindow(screenPt);
        //    if (this.ApplyComposition)
        //    {
        //        pt.X -= this.RealWindowBorders.Left;
        //    }
        //    this.WindowChromeNonClientMouseMove(pt);
        //    if (!this.d)
        //    {
        //        ip.m m2 = new ip.m();
        //        m2.a = (uint)Marshal.SizeOf(typeof(ip.m));
        //        m2.d = 100;
        //        m2.b = 0x12;
        //        m2.c = base.Handle;
        //        ip.TrackMouseEvent(ref m2);
        //        this.d = true;
        //    }
        //    m.Result = IntPtr.Zero;
        //    return true;
        //}

        //protected virtual bool OnWM_NCPAINT(ref Message m)
        //{
        //    if (!this.k)
        //    {
        //        this.OnNonClientPaint(m.HWnd);
        //    }
        //    m.Result = (IntPtr)1;
        //    return true;
        //}

        //[EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        //public void PerformNeedPaint(bool needLayout)
        //{
        //    this.OnNeedPaint(this, new NeedLayoutEventArgs(needLayout));
        //}

        //public void RecalcNonClient()
        //{
        //    if ((!base.IsDisposed && !base.Disposing) && base.IsHandleCreated)
        //    {
        //        ip.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x237);
        //    }
        //}

        //public void RedrawNonClient()
        //{
        //    this.InvalidateNonClient(Rectangle.Empty, true);
        //}

        //public void ResetPalette()
        //{
        //    this.PaletteMode = PaletteMode.Global;
        //}

        //public void ResetPaletteMode()
        //{
        //    this.PaletteMode = PaletteMode.Global;
        //}

        //protected virtual void ResumePaint()
        //{
        //    this.m--;
        //}

        //protected Point ScreenToWindow(Point screenPt)
        //{
        //    Point point = base.PointToClient(screenPt);
        //    Padding realWindowBorders = this.RealWindowBorders;
        //    point.Offset(realWindowBorders.Left, this.ApplyComposition ? 0 : realWindowBorders.Top);
        //    return point;
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void SendSysCommand(int sysCommand)
        //{
        //    this.SendSysCommand(sysCommand, IntPtr.Zero);
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void SendSysCommand(int sysCommand, IntPtr lParam)
        //{
        //    ip.SendMessage(base.Handle, 0x112, (IntPtr)sysCommand, lParam);
        //}

        //protected void StartCapture(ViewBase element)
        //{
        //    base.Capture = true;
        //    this.j = true;
        //    this.o = element;
        //}

        //protected virtual void SuspendPaint()
        //{
        //    this.m++;
        //}

        //public virtual void WindowChromeCompositionLayout(ViewLayoutContext context, Rectangle compRect)
        //{
        //}

        //public virtual void WindowChromeCompositionPaint(RenderContext context)
        //{
        //}

        //protected virtual void WindowChromeEnd()
        //{
        //}

        //protected virtual IntPtr WindowChromeHitTest(Point pt, bool composition)
        //{
        //    return (IntPtr)1;
        //}

        //protected virtual bool WindowChromeLeftMouseDown(Point pt)
        //{
        //    this.ViewManager.MouseDown(new MouseEventArgs(MouseButtons.Left, 1, pt.X, pt.Y, 0), pt);
        //    if (this.ViewManager.ActiveView != null)
        //    {
        //        IMouseController controller = this.ViewManager.ActiveView.FindMouseController();
        //        if (controller != null)
        //        {
        //            return controller.IgnoreVisualFormLeftButtonDown;
        //        }
        //    }
        //    return false;
        //}

        //protected virtual bool WindowChromeLeftMouseUp(Point pt)
        //{
        //    this.ViewManager.MouseUp(new MouseEventArgs(MouseButtons.Left, 0, pt.X, pt.Y, 0), pt);
        //    return false;
        //}

        //protected virtual void WindowChromeMouseLeave()
        //{
        //    this.ViewManager.MouseLeave(EventArgs.Empty);
        //}

        //protected virtual void WindowChromeNonClientMouseMove(Point pt)
        //{
        //    this.ViewManager.MouseMove(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0), pt);
        //}

        //protected virtual void WindowChromePaint(Graphics g, Rectangle bounds)
        //{
        //}

        //protected virtual void WindowChromeStart()
        //{
        //}

        //protected override void WndProc(ref Message m)
        //{
        //    bool flag = false;
        //    if (((m.Msg == 0x83) && a) && ((base.MdiParent == null) || this.ApplyCustomChrome))
        //    {
        //        flag = this.OnWM_NCCALCSIZE(ref m);
        //    }
        //    if ((this.ApplyCustomChrome && !base.IsDisposed) && !base.Disposing)
        //    {
        //        switch (m.Msg)
        //        {
        //            case 12:
        //            case 0x53:
        //            case 0x116:
        //                flag = this.OnPaintNonClient(ref m);
        //                break;

        //            case 0x84:
        //                if (!this.ApplyComposition)
        //                {
        //                    flag = this.OnWM_NCHITTEST(ref m);
        //                    break;
        //                }
        //                flag = this.OnCompWM_NCHITTEST(ref m);
        //                break;

        //            case 0x85:
        //                if (!this.ApplyComposition)
        //                {
        //                    if (this.m > 0)
        //                    {
        //                        flag = true;
        //                        break;
        //                    }
        //                    flag = this.OnWM_NCPAINT(ref m);
        //                }
        //                break;

        //            case 0x86:
        //                flag = this.OnWM_NCACTIVATE(ref m);
        //                break;

        //            case 160:
        //                flag = this.OnWM_NCMOUSEMOVE(ref m);
        //                break;

        //            case 0xa1:
        //                flag = this.OnWM_NCLBUTTONDOWN(ref m);
        //                break;

        //            case 0xa2:
        //                flag = this.OnWM_NCLBUTTONUP(ref m);
        //                break;

        //            case 0xa3:
        //                flag = this.OnWM_NCLBUTTONDBLCLK(ref m);
        //                break;

        //            case 0xae:
        //                flag = true;
        //                break;

        //            case 0x112:
        //                if (((int)m.WParam) != 0xf100)
        //                {
        //                    flag = this.OnPaintNonClient(ref m);
        //                }
        //                break;

        //            case 0x200:
        //                if (this.j)
        //                {
        //                    flag = this.OnWM_MOUSEMOVE(ref m);
        //                }
        //                break;

        //            case 0x202:
        //                if (this.j)
        //                {
        //                    flag = this.OnWM_LBUTTONUP(ref m);
        //                }
        //                break;

        //            case 0x2a2:
        //                if (!this.j)
        //                {
        //                    flag = this.OnWM_NCMOUSELEAVE(ref m);
        //                }
        //                if (this.ApplyComposition)
        //                {
        //                    this.p.CompNeedPaint(true);
        //                }
        //                break;
        //        }
        //    }
        //    if (!flag)
        //    {
        //        base.WndProc(ref m);
        //    }
        //}

        //// Properties
        //[EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //public bool AllowComposition
        //{
        //    get
        //    {
        //        return this.f;
        //    }
        //    set
        //    {
        //        if (this.f != value)
        //        {
        //            this.f = value;
        //            if (this.ApplyCustomChrome)
        //            {
        //                this.a();
        //            }
        //        }
        //    }
        //}

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ApplyComposition
        //{
        //    get
        //    {
        //        return this.g;
        //    }
        //}

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool ApplyCustomChrome
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.e;
        //    }
        //    internal set
        //    {
        //        if (this.e != value)
        //        {
        //            bool e = this.e;
        //            this.e = value;
        //            if (this.e)
        //            {
        //                try
        //                {
        //                    this.e = false;
        //                    if (ip.IsAppThemed() && ip.IsThemeActive())
        //                    {
        //                        this.e = true;
        //                        this.a();
        //                        if (!this.ApplyComposition)
        //                        {
        //                            ip.SetWindowTheme(base.Handle, "", "");
        //                        }
        //                        else
        //                        {
        //                            ip.SetWindowTheme(base.Handle, null, null);
        //                        }
        //                        this.WindowChromeStart();
        //                    }
        //                }
        //                catch
        //                {
        //                    this.e = false;
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    this.a();
        //                    ip.SetWindowTheme(base.Handle, null, null);
        //                    this.WindowChromeEnd();
        //                }
        //                catch
        //                {
        //                }
        //            }
        //            if (this.e != e)
        //            {
        //                this.OnApplyCustomChromeChanged(EventArgs.Empty);
        //            }
        //        }
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //public IKryptonComposition Composition
        //{
        //    get
        //    {
        //        return this.p;
        //    }
        //    set
        //    {
        //        this.p = value;
        //    }
        //}

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int KryptonLayoutCounter
        //{
        //    get
        //    {
        //        return this.ViewManager.LayoutCounter;
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int KryptonPaintCounter
        //{
        //    get
        //    {
        //        return this.ViewManager.PaintCounter;
        //    }
        //}

        //protected bool NeedLayout
        //{
        //    get
        //    {
        //        return this.i;
        //    }
        //    set
        //    {
        //        this.i = value;
        //    }
        //}

        //protected NeedPaintHandler NeedPaintDelegate
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.v;
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //public int PaintCount
        //{
        //    get
        //    {
        //        return this.n;
        //    }
        //}

        //[Category("Visuals"), DefaultValue((string)null), Description("Custom palette applied to drawing.")]
        //public IPalette Palette
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.q;
        //    }
        //    set
        //    {
        //        if (this.q != value)
        //        {
        //            IPalette q = this.q;
        //            this.a(value);
        //            if (value == null)
        //            {
        //                this.t = PaletteMode.Global;
        //                this.q = null;
        //                this.a(KryptonManager.GetPaletteForMode(this.t));
        //            }
        //            else
        //            {
        //                this.q = value;
        //                this.t = PaletteMode.Custom;
        //            }
        //            if (q != this.q)
        //            {
        //                this.OnPaletteChanged(EventArgs.Empty);
        //                base.PerformLayout();
        //            }
        //        }
        //    }
        //}

        //[Description("Palette applied to drawing."), Category("Visuals"), DefaultValue(typeof(PaletteMode), "Global")]
        //public PaletteMode PaletteMode
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.t;
        //    }
        //    set
        //    {
        //        if (this.t != value)
        //        {
        //            if (value != PaletteMode.Custom)
        //            {
        //                this.t = value;
        //                this.q = null;
        //                this.a(KryptonManager.GetPaletteForMode(this.t));
        //                this.OnPaletteChanged(EventArgs.Empty);
        //                base.PerformLayout();
        //            }
        //        }
        //    }
        //}

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        //public Padding RealWindowBorders
        //{
        //    get
        //    {
        //        return CommonHelper.GetWindowBorders(this.CreateParams);
        //    }
        //}

        //protected Rectangle RealWindowRectangle
        //{
        //    get
        //    {
        //        ip.b b = new ip.b();
        //        ip.GetWindowRect(base.Handle, ref b);
        //        return new Rectangle(0, 0, b.c - b.a, b.d - b.b);
        //    }
        //}

        //protected PaletteRedirect Redirector
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.u;
        //    }
        //}

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        //public IRenderer Renderer
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.s;
        //    }
        //}

        //protected ViewManager ViewManager
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return this.w;
        //    }
        //    set
        //    {
        //        this.w = value;
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        //public bool WindowActive
        //{
        //    get
        //    {
        //        return this.c;
        //    }
        //    set
        //    {
        //        if (this.c != value)
        //        {
        //            this.c = value;
        //            this.OnWindowActiveChanged();
        //        }
        //    }
        //}

    }
}
