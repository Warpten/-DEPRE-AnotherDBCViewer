using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MyDBCViewer.Extensions
{
    public static class Utils
    {
        public static Type GetFormatType(this Assembly a, string typeString, params object[] args)
        {
            return a.GetType(String.Format(typeString, args));
        }

        public static void BoxError(string message, params object[] args)
        {
            MessageBox.Show(String.Format(message, args), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
