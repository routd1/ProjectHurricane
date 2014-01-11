using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
namespace ICTEAS.WinForms.Utilities
{
    public interface IContentValues
    {
        // Methods
        Image GetImage(PaletteState state);
        Color GetImageTransparentColor(PaletteState state);
        string GetLongText();
        string GetShortText();

    }

    [Flags]
    public enum PaletteState
    {
        Checked = 0x1000,
        CheckedNormal = 0x1002,
        CheckedPressed = 0x1008,
        CheckedTracking = 0x1004,
        Context = 0x2000,
        ContextCheckedNormal = 0x2008,
        ContextCheckedTracking = 0x2010,
        ContextNormal = 0x2002,
        ContextTracking = 0x2004,
        Disabled = 1,
        FocusOverride = 0x100001,
        LinkNotVisitedOverride = 0x100008,
        LinkPressedOverride = 0x100010,
        LinkVisitedOverride = 0x100004,
        Normal = 2,
        NormalDefaultOverride = 0x100002,
        Override = 0x100000,
        Pressed = 8,
        Tracking = 4
    }


}
