using System.Collections.Generic;

namespace RingField {
    class MatrixGenerator : IEnumerable<Matrix> {
        public Field Field {
            get;
            private set;
        }
        private int size;

        public MatrixGenerator(Field field, int size) {
            Field = field;
            this.size = size;
        }

        public IEnumerator<Matrix> GetEnumerator() {
            int[,] curr = new int[size, size];
            while(true) {
                yield return new Matrix(Field, (int[,])curr.Clone());
                if(!Increment(curr))
                    yield break;
            }
        }

        private bool Increment(int[,] curr) {
            for(int r = 0; r < size; r++)
                for(int c = 0; c < size; c++) {
                    curr[r, c] = Field.Add(curr[r, c], 1);
                    if(curr[r, c] != 0)
                        return true;
                }
            return false;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
