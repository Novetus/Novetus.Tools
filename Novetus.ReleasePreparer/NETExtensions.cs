using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace Novetus.Core
{
    public static class NetExtensions
    {
        #region Extensions
        //This code was brought to you by:
        //https://stackoverflow.com/questions/101265/why-is-there-no-foreach-extension-method-on-ienumerable

        #region IEnumerable Extensions
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }
        #endregion

        #endregion
    }
}
