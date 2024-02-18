using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace COD4_Server_Browser
{
    public static class Extensions
    {
        public static byte[][] split(this byte[] composite, byte[] seperator)
        {
            bool found = false;

            int i = 0;
            for (; i < composite.Length - seperator.Length; i++)
            {
                var compositeview = new byte[seperator.Length];
                Array.Copy(composite, i, compositeview, 0, seperator.Length);

                found = compositeview.SequenceEqual(seperator);
                if (found) break;
            }

            if (found == false)
            {
                return null;
            }

            var component1length = i;
            var component1 = new byte[component1length];

            var component2length = composite.Length - seperator.Length - component1length;
            var component2 = new byte[component2length];
            var component2index = i + seperator.Length;

            Array.Copy(composite, 0, component1, 0, component1length);
            Array.Copy(composite, component2index, component2, 0, component2length);

            return new byte[][]
            {
        component1,
        component2
            };
        }

        public static byte[][] separate(this byte[] source, byte[] separator)
        {
            var parts = new List<byte[]>();
            var index = 0;
            byte[] part;
            for (var i = 0; i < source.Length; ++i)
            {
                if (Equals(source, separator, i))
                {
                    part = new byte[i - index];
                    Array.Copy(source, index, part, 0, part.Length);
                    parts.Add(part);
                    index = i + separator.Length;
                    i += separator.Length - 1;
                }
            }
            part = new byte[source.Length - index];
            Array.Copy(source, index, part, 0, part.Length);
            parts.Add(part);
            return parts.ToArray();
        }

        static bool Equals(byte[] source, byte[] separator, int index)
        {
            for (int i = 0; i < separator.Length; ++i)
                if (index + i >= source.Length || source[index + i] != separator[i])
                    return false;
            return true;
        }
    }
}
