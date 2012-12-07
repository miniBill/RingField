using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingField {
    public static class ListExtensions {
        public static String ToFullString<T>(this IEnumerable<T> list) {
            var sb = new StringBuilder();
            foreach(var item in list) {
                sb.Append(item);
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> haystack, IEnumerable<T> needles) {
            var listed = haystack as IList<T> ?? haystack.ToList();
            return needles.All(listed.Contains);
        }
    }
}
