using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ICTEAS.WinForms.Utilities
{
    public class CommonHelper : GlobalId
    {
        // Fields
        private const int a = 0x10;
        private const int b = 0x11;
        private const int c = 0x12;
        private static readonly Padding d = new Padding(-1);
        private static int e = 0x3e8;
        private static DateTime f = new DateTime(0x7d0, 1, 1);
        private static PropertyInfo g;
        private static MethodInfo h;
        private static NullContentValues i;
        private static Point j = new Point(0x7fffffff, 0x7fffffff);
        private static Rectangle k = new Rectangle(0x7fffffff, 0x7fffffff, 0, 0);

        // Methods
        private CommonHelper()
        {
        }

        public static void AddControlToParent(Control parent, Control c)
        {
            if (c.Parent != null)
            {
                RemoveControlFromParent(c);
            }
            if (parent.Controls is KryptonControlCollection)
            {
                ((KryptonControlCollection)parent.Controls).AddInternal(c);
            }
            else
            {
                parent.Controls.Add(c);
            }
        }

        public static Rectangle ApplyPadding(VisualOrientation orientation, Rectangle rect, Padding padding)
        {
            if (!padding.Equals(InheritPadding))
            {
                switch (orientation)
                {
                    case VisualOrientation.Top:
                        rect = new Rectangle(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
                        return rect;

                    case VisualOrientation.Bottom:
                        rect = new Rectangle(rect.X + padding.Right, rect.Y + padding.Bottom, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
                        return rect;

                    case VisualOrientation.Left:
                        rect = new Rectangle(rect.X + padding.Top, rect.Y + padding.Right, rect.Width - padding.Vertical, rect.Height - padding.Horizontal);
                        return rect;

                    case VisualOrientation.Right:
                        rect = new Rectangle(rect.X + padding.Bottom, rect.Y + padding.Left, rect.Width - padding.Vertical, rect.Height - padding.Horizontal);
                        return rect;
                }
            }
            return rect;
        }

        public static Size ApplyPadding(VisualOrientation orientation, Size size, Padding padding)
        {
            if (!padding.Equals(InheritPadding))
            {
                switch (orientation)
                {
                    case VisualOrientation.Top:
                    case VisualOrientation.Bottom:
                        size.Width += padding.Horizontal;
                        size.Height += padding.Vertical;
                        return size;

                    case VisualOrientation.Left:
                    case VisualOrientation.Right:
                        size.Width += padding.Vertical;
                        size.Height += padding.Horizontal;
                        return size;
                }
            }
            return size;
        }

        public static Rectangle ApplyPadding(Orientation orientation, Rectangle rect, Padding padding)
        {
            if (!padding.Equals(InheritPadding))
            {
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        rect.X += padding.Left;
                        rect.Width -= padding.Horizontal;
                        rect.Y += padding.Top;
                        rect.Height -= padding.Vertical;
                        return rect;

                    case Orientation.Vertical:
                        rect.X += padding.Top;
                        rect.Width -= padding.Vertical;
                        rect.Y += padding.Right;
                        rect.Height -= padding.Horizontal;
                        return rect;
                }
            }
            return rect;
        }

        public static Size ApplyPadding(Orientation orientation, Size size, Padding padding)
        {
            if (!padding.Equals(InheritPadding))
            {
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        size.Width += padding.Horizontal;
                        size.Height += padding.Vertical;
                        return size;

                    case Orientation.Vertical:
                        size.Width += padding.Vertical;
                        size.Height += padding.Horizontal;
                        return size;
                }
            }
            return size;
        }

        public static Color BlackenColor(Color color1, float percentR, float percentG, float percentB)
        {
            int red = (int)(color1.R * percentR);
            int green = (int)(color1.G * percentG);
            int blue = (int)(color1.B * percentB);
            if (red < 0)
            {
                red = 0;
            }
            if (red > 0xff)
            {
                red = 0xff;
            }
            if (green < 0)
            {
                green = 0;
            }
            if (green > 0xff)
            {
                green = 0xff;
            }
            if (blue < 0)
            {
                blue = 0;
            }
            if (blue > 0xff)
            {
                blue = 0xff;
            }
            return Color.FromArgb(red, green, blue);
        }

        public static PaletteButtonStyle ButtonStyleToPalette(ButtonStyle style)
        {
            switch (style)
            {
                case ButtonStyle.Standalone:
                    return PaletteButtonStyle.Standalone;

                case ButtonStyle.Alternate:
                    return PaletteButtonStyle.Alternate;

                case ButtonStyle.LowProfile:
                    return PaletteButtonStyle.LowProfile;

                case ButtonStyle.ButtonSpec:
                    return PaletteButtonStyle.ButtonSpec;

                case ButtonStyle.Cluster:
                    return PaletteButtonStyle.Cluster;

                case ButtonStyle.NavigatorStack:
                    return PaletteButtonStyle.NavigatorStack;

                case ButtonStyle.NavigatorMini:
                    return PaletteButtonStyle.NavigatorMini;

                case ButtonStyle.InputControl:
                    return PaletteButtonStyle.InputControl;

                case ButtonStyle.ListItem:
                    return PaletteButtonStyle.ListItem;

                case ButtonStyle.Form:
                    return PaletteButtonStyle.Form;

                case ButtonStyle.Custom1:
                    return PaletteButtonStyle.Custom1;

                case ButtonStyle.Custom2:
                    return PaletteButtonStyle.Custom2;

                case ButtonStyle.Custom3:
                    return PaletteButtonStyle.Custom3;
            }
            return PaletteButtonStyle.Standalone;
        }

        public static bool CheckContextMenuForShortcut(ContextMenuStrip cms, ref Message msg, Keys keyData)
        {
            int num = 0x11;
            if (cms != null)
            {
                if (g == null)
                {
                    g = typeof(ToolStrip).GetProperty(a("爠䬢䨤唦崨䠪堬嬮䈰", num), BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);
                    h = typeof(ToolStripMenuItem).GetMethod(a("焠儢䨤䐦䰨堪帬氮尰圲縴制䀸", num), BindingFlags.NonPublic | BindingFlags.Instance);
                }
                Hashtable hashtable = (Hashtable)g.GetValue(cms, null);
                ToolStripMenuItem item = (ToolStripMenuItem)hashtable[keyData];
                if (item != null)
                {
                    object obj2 = h.Invoke(item, new object[] { (Message)msg, keyData });
                    if (obj2 != null)
                    {
                        return (bool)obj2;
                    }
                }
            }
            return false;
        }

        public static Color ColorToBlackAndWhite(Color color)
        {
            int red = (int)(((color.R * 0.3f) + (color.G * 0.59f)) + (color.B * 0.11f));
            return Color.FromArgb(red, red, red);
        }

        public static PaletteContentStyle ContentStyleFromLabelStyle(LabelStyle style)
        {
            switch (style)
            {
                case LabelStyle.NormalControl:
                    return PaletteContentStyle.LabelNormalControl;

                case LabelStyle.TitleControl:
                    return PaletteContentStyle.LabelTitleControl;

                case LabelStyle.NormalPanel:
                    return PaletteContentStyle.LabelNormalPanel;

                case LabelStyle.TitlePanel:
                    return PaletteContentStyle.LabelTitlePanel;

                case LabelStyle.ToolTip:
                    return PaletteContentStyle.LabelToolTip;

                case LabelStyle.SuperTip:
                    return PaletteContentStyle.LabelSuperTip;

                case LabelStyle.KeyTip:
                    return PaletteContentStyle.LabelKeyTip;

                case LabelStyle.Custom1:
                    return PaletteContentStyle.LabelCustom1;

                case LabelStyle.Custom2:
                    return PaletteContentStyle.LabelCustom2;

                case LabelStyle.Custom3:
                    return PaletteContentStyle.LabelCustom3;
            }
            return PaletteContentStyle.LabelNormalPanel;
        }

        public static Control GetControlWithFocus(Control control)
        {
            if (control.Focused)
            {
                return control;
            }
            foreach (Control control2 in control.Controls)
            {
                if (control2.ContainsFocus)
                {
                    return GetControlWithFocus(control2);
                }
            }
            return null;
        }

        public static bool GetRightToLeftLayout(Control control)
        {
            bool rightToLeftLayout = false;
            if (control != null)
            {
                Form form = control.FindForm();
                if (form != null)
                {
                    rightToLeftLayout = form.RightToLeftLayout;
                }
            }
            return rightToLeftLayout;
        }

        public static Padding GetWindowBorders(CreateParams cp)
        {
            ip.b b = new ip.b();
            b.a = 0;
            b.c = 0;
            b.b = 0;
            b.d = 0;
            ip.AdjustWindowRectEx(ref b, cp.Style, false, cp.ExStyle);
            return new Padding(-b.a, -b.b, b.c, b.d);
        }

        [DebuggerStepThrough]
        public static bool HasABorder(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.All) != PaletteDrawBorders.None);
        }

        [DebuggerStepThrough]
        public static bool HasAllBorders(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.All) == PaletteDrawBorders.All);
        }

        [DebuggerStepThrough]
        public static bool HasBottomBorder(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.Bottom) == PaletteDrawBorders.Bottom);
        }

        [DebuggerStepThrough]
        public static bool HasLeftBorder(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.Left) == PaletteDrawBorders.Left);
        }

        [DebuggerStepThrough]
        public static bool HasNoBorders(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.All) == PaletteDrawBorders.None);
        }

        [DebuggerStepThrough]
        public static bool HasOneBorder(PaletteDrawBorders borders)
        {
            PaletteDrawBorders borders2 = borders & PaletteDrawBorders.All;
            if (((borders2 != PaletteDrawBorders.Top) && (borders2 != PaletteDrawBorders.Bottom)) && (borders2 != PaletteDrawBorders.Left))
            {
                return (borders2 == PaletteDrawBorders.Right);
            }
            return true;
        }

        [DebuggerStepThrough]
        public static bool HasRightBorder(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.Right) == PaletteDrawBorders.Right);
        }

        [DebuggerStepThrough]
        public static bool HasTopBorder(PaletteDrawBorders borders)
        {
            return ((borders & PaletteDrawBorders.Top) == PaletteDrawBorders.Top);
        }

        public static bool IsFormMaximized(Form f)
        {
            return ((ip.GetWindowLong(f.Handle, -16) & 0x1000000) != 0);
        }

        public static bool IsFormMinimized(Form f)
        {
            return ((ip.GetWindowLong(f.Handle, -16) & 0x20000000) != 0);
        }

        [DebuggerStepThrough]
        public static bool IsOverrideState(PaletteState state)
        {
            return ((state & PaletteState.Override) == PaletteState.Override);
        }

        [DebuggerStepThrough]
        public static bool IsOverrideStateExclude(PaletteState state, PaletteState exclude)
        {
            return ((state != exclude) && IsOverrideState(state));
        }

        public static Color MergeColors(Color color1, float percent1, Color color2, float percent2)
        {
            return MergeColors(color1, percent1, color2, percent2, Color.Empty, 0f);
        }

        public static Color MergeColors(Color color1, float percent1, Color color2, float percent2, Color color3, float percent3)
        {
            int red = (int)(((color1.R * percent1) + (color2.R * percent2)) + (color3.R * percent3));
            int green = (int)(((color1.G * percent1) + (color2.G * percent2)) + (color3.G * percent3));
            int blue = (int)(((color1.B * percent1) + (color2.B * percent2)) + (color3.B * percent3));
            if (red < 0)
            {
                red = 0;
            }
            if (red > 0xff)
            {
                red = 0xff;
            }
            if (green < 0)
            {
                green = 0;
            }
            if (green > 0xff)
            {
                green = 0xff;
            }
            if (blue < 0)
            {
                blue = 0;
            }
            if (blue > 0xff)
            {
                blue = 0xff;
            }
            return Color.FromArgb(red, green, blue);
        }

        public static PaletteDrawBorders OrientateDrawBorders(PaletteDrawBorders borders, VisualOrientation orientation)
        {
            if (orientation == VisualOrientation.Top)
            {
                return borders;
            }
            if ((borders == PaletteDrawBorders.All) || (borders == PaletteDrawBorders.None))
            {
                return borders;
            }
            PaletteDrawBorders none = PaletteDrawBorders.None;
            switch (orientation)
            {
                case VisualOrientation.Bottom:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    return none;

                case VisualOrientation.Left:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    return none;

                case VisualOrientation.Right:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    return none;
            }
            return none;
        }

        public static Padding OrientatePadding(VisualOrientation orientation, Padding padding)
        {
            switch (orientation)
            {
                case VisualOrientation.Top:
                    return padding;

                case VisualOrientation.Bottom:
                    return new Padding(padding.Right, padding.Bottom, padding.Left, padding.Top);

                case VisualOrientation.Left:
                    return new Padding(padding.Top, padding.Right, padding.Bottom, padding.Left);

                case VisualOrientation.Right:
                    return new Padding(padding.Bottom, padding.Left, padding.Top, padding.Right);
            }
            return padding;
        }

        public static TextRenderingHint PaletteTextHintToRenderingHint(PaletteTextHint hint)
        {
            switch (hint)
            {
                case PaletteTextHint.AntiAlias:
                    return TextRenderingHint.AntiAlias;

                case PaletteTextHint.AntiAliasGridFit:
                    return TextRenderingHint.AntiAliasGridFit;

                case PaletteTextHint.ClearTypeGridFit:
                    return TextRenderingHint.ClearTypeGridFit;

                case PaletteTextHint.SingleBitPerPixel:
                    return TextRenderingHint.SingleBitPerPixel;

                case PaletteTextHint.SingleBitPerPixelGridFit:
                    return TextRenderingHint.SingleBitPerPixelGridFit;

                case PaletteTextHint.SystemDefault:
                    return TextRenderingHint.SystemDefault;
            }
            return TextRenderingHint.SystemDefault;
        }

        public static object PerformOperation(Operation op, object parameter)
        {
            using (ModalWaitDialog dialog = new ModalWaitDialog())
            {
                et et = new et(op, parameter);
                new Thread(new ThreadStart(et.f)).Start();
                goto Label_0035;
            Label_0028:
                Thread.Sleep(0x19);
                dialog.UpdateDialog();
            Label_0035:
                switch (et.i())
                {
                    case 1:
                        return et.d();

                    case 2:
                        throw et.h();

                    case 0:
                        goto Label_0028;
                }
                return null;
            }
        }

        public static Rectangle RealClientRectangle(IntPtr handle)
        {
            ip.b b = new ip.b();
            ip.GetWindowRect(handle, ref b);
            return new Rectangle(0, 0, b.c - b.a, b.d - b.b);
        }

        public static void RemoveControlFromParent(Control c)
        {
            if (c.Parent != null)
            {
                if (c.Parent.Controls is KryptonControlCollection)
                {
                    ((KryptonControlCollection)c.Parent.Controls).RemoveInternal(c);
                }
                else
                {
                    c.Parent.Controls.Remove(c);
                }
            }
        }

        public static PaletteDrawBorders ReverseOrientateDrawBorders(PaletteDrawBorders borders, VisualOrientation orientation)
        {
            if (orientation == VisualOrientation.Top)
            {
                return borders;
            }
            if ((borders == PaletteDrawBorders.All) || (borders == PaletteDrawBorders.None))
            {
                return borders;
            }
            PaletteDrawBorders none = PaletteDrawBorders.None;
            switch (orientation)
            {
                case VisualOrientation.Bottom:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    return none;

                case VisualOrientation.Left:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    return none;

                case VisualOrientation.Right:
                    if (HasTopBorder(borders))
                    {
                        none |= PaletteDrawBorders.Left;
                    }
                    if (HasBottomBorder(borders))
                    {
                        none |= PaletteDrawBorders.Right;
                    }
                    if (HasLeftBorder(borders))
                    {
                        none |= PaletteDrawBorders.Bottom;
                    }
                    if (HasRightBorder(borders))
                    {
                        none |= PaletteDrawBorders.Top;
                    }
                    return none;
            }
            return none;
        }

        public static GraphicsPath RoundedRectanglePath(Rectangle rect, int rounding)
        {
            GraphicsPath path = new GraphicsPath();
            rounding = Math.Min(rounding, Math.Min((int)(rect.Width / 2), (int)(rect.Height / 2)) - rounding);
            if (rounding <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            RectangleF ef = new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
            int num = rounding * 2;
            path.AddArc(ef.Left, ef.Top, (float)num, (float)num, 180f, 90f);
            path.AddArc(ef.Right - num, ef.Top, (float)num, (float)num, 270f, 90f);
            path.AddArc(ef.Right - num, ef.Bottom - num, (float)num, (float)num, 0f, 90f);
            path.AddArc(ef.Left, ef.Bottom - num, (float)num, (float)num, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        [DebuggerStepThrough]
        public static void SwapRectangleSizes(ref Rectangle rect)
        {
            int width = rect.Width;
            rect.Width = rect.Height;
            rect.Height = width;
        }

        public static bool ValidContextMenuStrip(ContextMenuStrip cms)
        {
            return ((cms != null) && (cms.Items.Count > 0));
        }

        //public static bool ValidKryptonContextMenu(KryptonContextMenu kcm)
        //{
        //    return ((kcm != null) && (kcm.Items.Count > 0));
        //}

        public static Orientation VisualToOrientation(VisualOrientation orientation)
        {
            switch (orientation)
            {
                case VisualOrientation.Top:
                case VisualOrientation.Bottom:
                    return Orientation.Vertical;

                case VisualOrientation.Left:
                case VisualOrientation.Right:
                    return Orientation.Horizontal;
            }
            return Orientation.Vertical;
        }

        public static Color WhitenColor(Color color1, float percentR, float percentG, float percentB)
        {
            int red = (int)(((float)color1.R) / percentR);
            int green = (int)(((float)color1.G) / percentG);
            int blue = (int)(((float)color1.B) / percentB);
            if (red < 0)
            {
                red = 0;
            }
            if (red > 0xff)
            {
                red = 0xff;
            }
            if (green < 0)
            {
                green = 0;
            }
            if (green > 0xff)
            {
                green = 0xff;
            }
            if (blue < 0)
            {
                blue = 0;
            }
            if (blue > 0xff)
            {
                blue = 0xff;
            }
            return Color.FromArgb(red, green, blue);
        }

        // Properties
        public static Padding InheritPadding
        {
            [DebuggerStepThrough]
            get
            {
                return d;
            }
        }

        public static bool IsAltKeyPressed
        {
            [DebuggerStepThrough]
            get
            {
                return ((ip.GetKeyState(0x12) & 0x8000) != 0);
            }
        }

        public static bool IsCtrlKeyPressed
        {
            [DebuggerStepThrough]
            get
            {
                return ((ip.GetKeyState(0x11) & 0x8000) != 0);
            }
        }

        public static bool IsShiftKeyPressed
        {
            [DebuggerStepThrough]
            get
            {
                return ((ip.GetKeyState(0x10) & 0x8000) != 0);
            }
        }

        public static int NextId
        {
            [DebuggerStepThrough]
            get
            {
                return e++;
            }
        }

        public static IContentValues NullContentValues
        {
            get
            {
                if (i == null)
                {
                    i = new NullContentValues();
                }
                return i;
            }
        }

        public static Point NullPoint
        {
            [DebuggerStepThrough]
            get
            {
                return j;
            }
        }

        public static Rectangle NullRectangle
        {
            [DebuggerStepThrough]
            get
            {
                return k;
            }
        }

        public static string UniqueString
        {
            get
            {
                int num = 14;
                ip.g g = new ip.g();
                ip.CoCreateGuid(ref g);
                return string.Format(a("攝ဟᠡ簣ሥ唧儩ᴫᐭ栯ر䤳䴵਷9搻਽㴿㥁睃籅၇繉ㅋ㕍恏桑౓払╗⅙浛摝㡟噡ᥣᵥ婧偩㑫婭൯ॱ䝳䱵⁷乹Ż", num), new object[] { g.a, g.b, g.c, g.d, g.e, g.f, g.g, g.h });
            }
        }

    }

    public class NullContentValues : IContentValues
    {
        // Methods
        public virtual Image GetImage(PaletteState state)
        {
            return null;
        }

        public virtual Color GetImageTransparentColor(PaletteState state)
        {
            return Color.Empty;
        }

        public virtual string GetLongText()
        {
            return string.Empty;
        }

        public virtual string GetShortText()
        {
            return string.Empty;
        }
    }

    public enum VisualOrientation
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [TypeConverter(typeof(gv))]
    public enum PaletteButtonStyle
    {
        Inherit,
        Standalone,
        Alternate,
        LowProfile,
        ButtonSpec,
        Cluster,
        NavigatorStack,
        NavigatorMini,
        InputControl,
        ListItem,
        Form,
        Custom1,
        Custom2,
        Custom3
    }

    [TypeConverter(typeof(dn))]
    public enum ButtonStyle
    {
        Standalone,
        Alternate,
        LowProfile,
        ButtonSpec,
        Cluster,
        Gallery,
        NavigatorStack,
        NavigatorMini,
        InputControl,
        ListItem,
        Form,
        Custom1,
        Custom2,
        Custom3
    }

    [TypeConverter(typeof(eh))]
    public enum PaletteContentStyle
    {
        ButtonStandalone,
        ButtonAlternate,
        ButtonLowProfile,
        ButtonButtonSpec,
        ButtonCluster,
        ButtonGallery,
        ButtonNavigatorStack,
        ButtonNavigatorMini,
        ButtonInputControl,
        ButtonListItem,
        ButtonForm,
        ButtonCustom1,
        ButtonCustom2,
        ButtonCustom3,
        ContextMenuHeading,
        ContextMenuItemImage,
        ContextMenuItemTextStandard,
        ContextMenuItemTextAlternate,
        ContextMenuItemShortcutText,
        GridHeaderColumnList,
        GridHeaderRowList,
        GridDataCellList,
        GridHeaderColumnSheet,
        GridHeaderRowSheet,
        GridDataCellSheet,
        GridHeaderColumnCustom1,
        GridHeaderRowCustom1,
        GridDataCellCustom1,
        HeaderPrimary,
        HeaderSecondary,
        HeaderForm,
        HeaderCustom1,
        HeaderCustom2,
        LabelNormalControl,
        LabelTitleControl,
        LabelNormalPanel,
        LabelTitlePanel,
        LabelToolTip,
        LabelSuperTip,
        LabelKeyTip,
        LabelCustom1,
        LabelCustom2,
        LabelCustom3,
        TabHighProfile,
        TabStandardProfile,
        TabLowProfile,
        TabOneNote,
        TabCustom1,
        TabCustom2,
        TabCustom3,
        InputControlStandalone,
        InputControlRibbon,
        InputControlCustom1
    }

    [TypeConverter(typeof(it))]
    public enum LabelStyle
    {
        NormalControl,
        TitleControl,
        NormalPanel,
        TitlePanel,
        ToolTip,
        SuperTip,
        KeyTip,
        Custom1,
        Custom2,
        Custom3
    }
    [TypeConverter(typeof(ct)), Flags]
    public enum PaletteDrawBorders
    {
        None,
        Top,
        Bottom,
        TopBottom,
        Left,
        TopLeft,
        BottomLeft,
        TopBottomLeft,
        Right,
        TopRight,
        BottomRight,
        TopBottomRight,
        LeftRight,
        TopLeftRight,
        BottomLeftRight,
        All,
        Inherit
    }

    public enum TextRenderingHint
    {
        SystemDefault,
        SingleBitPerPixelGridFit,
        SingleBitPerPixel,
        AntiAliasGridFit,
        AntiAlias,
        ClearTypeGridFit
    }

    public enum PaletteTextHint
    {
        AntiAlias = 0,
        AntiAliasGridFit = 1,
        ClearTypeGridFit = 2,
        Inherit = -1,
        SingleBitPerPixel = 3,
        SingleBitPerPixelGridFit = 4,
        SystemDefault = 5
    }

    public delegate object Operation(object parameter);



 




 




}
