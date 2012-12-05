namespace RingField {
    public interface Field {
        int Add(params int[] a);
        int Multiply(params int[] a);
        int Subtract(int a, int b);
    }
}