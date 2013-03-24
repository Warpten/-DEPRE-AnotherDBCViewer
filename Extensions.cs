using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DBFilesClient.NET;

namespace MyDBCViewer.Extensions
{
    public static class Utils
    {
        public static string AsReflectionTypeIdentifier(this string str)
        {
            var ret = new List<char>(str.ToCharArray());
            for (int i = 0, s = ret.Count - 1; i < s; ++i)
                if (ret[i] == '-')
                    ret[i + 1] = (char)(ret[i + 1] - 32);

            // ReSharper disable CSharpWarnings::CS0642
            while (ret.Remove('-')) ;

            // ReSharper restore CSharpWarnings::CS0642
            return String.Join(String.Empty, ret.ToArray());
        }

        public static Type GetFormatType(this Assembly a, string typeString, params object[] args)
        {
            return a.GetType(String.Format(typeString, args));
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        public static void BoxError(string message, params object[] args)
        {
            MessageBox.Show(String.Format(message, args), @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static ToolStripItem LastItem(this ToolStripItemCollection list)
        {
            return list[list.Count - 1];
        }

        public static void WriteFormatString(this StreamWriter stream, string fmt, params object[] args)
        {
            stream.Write(String.Format(fmt, args));
        }

        public static T GetFirstRecord<T>(this DBCStorage<T> store) where T : class, new()
        {
            return store.Records.ToList()[0];
        }

        public static string Escape(this string str)
        {
            return str.Replace(@"""", @"\""");
        }
    }
}
