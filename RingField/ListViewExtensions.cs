using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RingField {
    public static class ListViewExtensions {
        public static void AddRange(this ItemCollection items, IEnumerable<Object> toadd) {
            foreach(var obj in toadd)
                items.Add(obj);
        }
    }
}
