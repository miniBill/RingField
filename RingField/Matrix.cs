using System;
using System.Text;

namespace RingField {
    public class Matrix : IEquatable<Matrix> {
        public bool Equals(Matrix other) {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            if(!Equals(Field, other.Field))
                return false;
            if(Rows != other.Rows || Columns != other.Columns)
                return false;
            for(int r=0;r<Rows;r++)
                for(int c=0;c<Columns;c++)
                    if(_values[r, c] != other._values[r, c])
                        return false;
            return true;
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((Matrix) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((_values != null ? _values.GetHashCode() : 0) * 397) ^ (Field != null ? Field.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Matrix left, Matrix right) {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix left, Matrix right) {
            return !Equals(left, right);
        }

        private IField Field {
            get;
            set;
        }

        private int Rows {
            get {
                return _values.GetLength(0);
            }
        }

        private int Columns {
            get {
                return _values.GetLength(1);
            }
        }

        private readonly int[,] _values;

        public Matrix(IField field, int[,] values) {
            Field = field;
            _values = values;
        }

        private int Get(int row, int column) {
            return _values[row, column];
        }

        public static Matrix operator *(Matrix a, Matrix b) {
            if(a.Columns != b.Rows || !Equals(a.Field, b.Field))
                throw new ArgumentException();
            var newvalues = new int[a.Rows, b.Columns];
            IField field = a.Field;
            for(int r = 0; r < a.Rows; r++)
                for(int c = 0; c < b.Columns; c++) {
                    int val = 0;
                    for(int j = 0; j < a.Columns; j++)
                        val = field.Add(val, field.Multiply(a.Get(r, j), b.Get(j, c)));
                    newvalues[r, c] = val;
                }
            return new Matrix(a.Field, newvalues);
        }

        public static Matrix operator +(Matrix a, Matrix b) {
            if(a.Rows != b.Rows || a.Columns != b.Columns || !Equals(a.Field, b.Field))
                throw new ArgumentException();
            var newvalues = new int[a.Rows, a.Columns];
            IField field = a.Field;
            for(int r = 0; r < a.Rows; r++)
                for(int c = 0; c < a.Columns; c++)
                    newvalues[r, c] = field.Add(a.Get(r, c), b.Get(r, c));
            return new Matrix(a.Field, newvalues);
        }

        public override string ToString() {
            var sb = new StringBuilder();
            for(int r = 0; r < Rows; r++) {
                for(int c = 0; c < Columns; c++) {
                    sb.Append(_values[r, c]);
                    if(c < Columns - 1)
                        sb.Append(' ');
                }
                if(r < Rows - 1)
                    sb.Append('\n');
            }
            return sb.ToString();
        }

        public bool Invertible {
            get {
                return Determinant != 0;
            }
        }

        private int Determinant {
            get {
                if(Rows != Columns || Rows == 0)
                    throw new ArgumentException();
                if(Rows == 1)
                    return _values[0, 0];
                if(Rows == 2)
                    return Field.Subtract(Field.Multiply(_values[0, 0], _values[1, 1]), Field.Multiply(_values[0, 1], _values[1, 0]));
                if(Rows == 3) {
                    int aei = Field.Multiply(_values[0, 0], _values[1, 1], _values[2, 2]);
                    int bfg = Field.Multiply(_values[0, 1], _values[1, 2], _values[2, 0]);
                    int cdh = Field.Multiply(_values[0, 2], _values[1, 0], _values[2, 1]);

                    int ceg = Field.Multiply(_values[0, 2], _values[1, 1], _values[2, 0]);
                    int bdi = Field.Multiply(_values[0, 1], _values[1, 0], _values[2, 2]);
                    int afh = Field.Multiply(_values[0, 0], _values[1, 2], _values[2, 1]);

                    int left = Field.Add(aei, bfg, cdh);
                    int right = Field.Add(ceg, bdi, afh);
                    return Field.Subtract(left, right);
                }
                throw new NotImplementedException();
            }
        }

        public bool IsZero {
            get {
                for(int r = 0; r < Rows; r++)
                    for(int c = 0; c < Columns; c++)
                        if(_values[r, c] != 0)
                            return false;
                return true;
            }
        }

        public static Matrix Zero(IField field, int size) {
            var idvalue = new int[size, size];
            return new Matrix(field, idvalue);
        }

        public int NonzeroCount {
            get {
                int toret = 0;
                for(int r = 0; r < Rows; r++)
                    for(int c = 0; c < Columns; c++)
                        if(_values[r, c] != 0)
                            toret++;
                return toret;
            }
        }
    }
}