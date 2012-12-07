using System;
using System.Linq;

namespace RingField {
    public class Zp : IField, IEquatable<Zp> {
        public bool Equals(Zp other) {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            return Order == other.Order;
        }

        public override int GetHashCode() {
            return Order;
        }

        public static bool operator ==(Zp left, Zp right) {
            return Equals(left, right);
        }

        public static bool operator !=(Zp left, Zp right) {
            return !Equals(left, right);
        }

        private int Order {
            get;
            set;
        }

        public override bool Equals(object obj) {
            return false;
        }

        public Zp(int order) {
            Order = order;
        }

        public int Add(params int[] xs) {
            int toret = xs.Sum();
            return toret % Order;
        }

        public int Multiply(params int[] xs) {
            int toret = xs.Aggregate(1, (current, x) => current * x);
            return toret % Order;
        }

        public int Subtract(int a, int b) {
            if(a >= b)
                return a - b;
            return a - b + Order;
        }
    }
}