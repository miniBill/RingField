namespace RingField {
    public class Zp : Field {
        public int Order {
            get;
            private set;
        }

        public Zp(int order) {
            Order = order;
        }

        public int Add(params int[] xs) {
            int toret = 0;
            foreach(int x in xs)
                toret += x;
            return toret % Order;
        }

        public int Multiply(params int[] xs) {
            int toret = 1;
            foreach(int x in xs)
                toret *= x;
            return toret % Order;
        }

        public int Subtract(int a, int b) {
            if(a >= b)
                return a - b;
            return a - b + Order;
        }
    }
}