namespace RingField {
    public interface IField {
        int Add(params int[] addends);
        int Multiply(params int[] factors);
        int Subtract(int left, int right);
    }
}