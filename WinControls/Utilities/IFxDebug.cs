using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICTEAS.WinForms.Utilities
{
    public interface IFxDebug
    {
            // Methods
            void KryptonResetCounters();

            // Properties
            int KryptonLayoutCounter { get; }
            int KryptonPaintCounter { get; }
    }
}
