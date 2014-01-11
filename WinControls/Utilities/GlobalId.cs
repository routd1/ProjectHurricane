using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace ICTEAS.WinForms.Utilities
{
    public class GlobalId
    {
        // Fields
        private int a = CommonHelper.NextId;

        // Properties
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Id
        {
            get
            {
                return this.a;
            }
        }

    }
}
