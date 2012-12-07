namespace RingField {
    public interface IField {
        int Add(params int[] a);
        int Multiply(params int[] a);
        int Subtract(int a, int b);
    }
}