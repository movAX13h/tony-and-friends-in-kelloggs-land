using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Kelloggs.Tool
{
    static class Extensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        // Find first occurance of a byte pattern in byte array:
        public static int Locate(this byte[] ba, byte[] pattern, int start = 0)
        {
            if (start < 0)
                throw new Exception("Parameter start must not be negative.");
            for (int i = start; i + pattern.Length <= ba.Length; ++i)
            {
                bool isMatch = true;
                for (int j = 0; j < pattern.Length; ++j)
                {
                    if (ba[i + j] != pattern[j])
                    {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                    return i;
            }
            return -1;
        }

        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }

    }
}
