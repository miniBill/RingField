using System.Collections.Generic;

namespace RingField {
    class MatrixGenerator : IEnumerable<Matrix> {
        private IField Field {
            get;
            set;
        }
        private readonly int _size;

        public MatrixGenerator(IField field, int size) {
            Field = field;
            _size = size;
        }

        public IEnumerator<Matrix> GetEnumerator() {
            var curr = new int[_size, _size];
            while(true) {
                yield return new Matrix(Field, (int[,])curr.Clone());
                if(!Increment(curr))
                    yield break;
            }
        }

        private bool Increment(int[,] curr) {
            for(int r = 0; r < _size; r++)
                for(int c = 0; c < _size; c++) {
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
