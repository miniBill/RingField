using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingField {
    public static class ListExtensions {
        public static String ToFullString<T>(this IEnumerable<T> list) {
            StringBuilder sb = new StringBuilder();
            foreach(var item in list) {
                sb.Append(item);
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> haystack, IEnumerable<T> needles) {
            foreach(var needle in needles) 
                if(!haystack.Contains(needle))
                    return false;
            return true;
        }
    }
}
