using System;
using System.Text;

namespace RingField {
    public class Matrix {
        public Field Field {
            get;
            private set;
        }

        public int Rows {
            get {
                return values.GetLength(0);
            }
        }

        public int Columns {
            get {
                return values.GetLength(1);
            }
        }

        private int[,] values;

        public Matrix(Field field, int[,] values) {
            Field = field;
            this.values = values;
        }

        public int Get(int row, int column) {
            return values[row, column];
        }

        public static Matrix operator *(Matrix a, Matrix b) {
            if(a.Columns != b.Rows || a.Field != b.Field)
                throw new ArgumentException();
            int[,] newvalues = new int[a.Rows, b.Columns];
            Field field = a.Field;
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
            if(a.Rows != b.Rows || a.Columns != b.Columns || a.Field != b.Field)
                throw new ArgumentException();
            int[,] newvalues = new int[a.Rows, a.Columns];
            Field field = a.Field;
            for(int r = 0; r < a.Rows; r++)
                for(int c = 0; c < a.Columns; c++)
                    newvalues[r, c] = field.Add(a.Get(r, c), b.Get(r, c));
            return new Matrix(a.Field, newvalues);
        }

        public override bool Equals(object obj) {
            Matrix other = obj as Matrix;
            if(other == null)
                return false;
            if(other.Rows != Rows || other.Columns != Columns)
                return false;
            for(int r = 0; r < Rows; r++)
                for(int c = 0; c < Columns; c++)
                    if(values[r, c] != other.values[r, c])
                        return false;
            return true;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            for(int r = 0; r < Rows; r++) {
                for(int c = 0; c < Columns; c++) {
                    sb.Append(values[r, c]);
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

        public int Determinant {
            get {
                if(Rows != Columns || Rows == 0)
                    throw new ArgumentException();
                if(Rows == 1)
                    return values[0, 0];
                if(Rows == 2)
                    return Field.Subtract(Field.Multiply(values[0, 0], values[1, 1]), Field.Multiply(values[0, 1], values[1, 0]));
                if(Rows == 3) {
                    int aei = Field.Multiply(values[0, 0], values[1, 1], values[2, 2]);
                    int bfg = Field.Multiply(values[0, 1], values[1, 2], values[2, 0]);
                    int cdh = Field.Multiply(values[0, 2], values[1, 0], values[2, 1]);

                    int ceg = Field.Multiply(values[0, 2], values[1, 1], values[2, 0]);
                    int bdi = Field.Multiply(values[0, 1], values[1, 0], values[2, 2]);
                    int afh = Field.Multiply(values[0, 0], values[1, 2], values[2, 1]);

                    int left = Field.Add(aei, bfg, cdh);
                    int right = Field.Add(ceg, bdi, afh);
                    return Field.Subtract(left, right);
                }
                throw new NotImplementedException();
            }
        }

        public static Matrix Identity(Field field, int size) {
            if(size == 0)
                throw new ArgumentOutOfRangeException();
            int[,] idvalue = new int[size, size];
            for(int j = 0; j < size; j++)
                idvalue[j, j] = 1;
            return new Matrix(field, idvalue);
        }

        public bool IsIdentity {
            get {
                for(int r = 0; r < Rows; r++)
                    for(int c = 0; c < Columns; c++)
                        if((r == c && values[r, c] != 1) || (r != c && values[r, c] != 0))
                            return false;
                return true;
            }
        }

        public bool IsZero {
            get {
                for(int r = 0; r < Rows; r++)
                    for(int c = 0; c < Columns; c++)
                        if(values[r, c] != 0)
                            return false;
                return true;
            }
        }

        public static Matrix Zero(Field field, int size) {
            int[,] idvalue = new int[size, size];
            return new Matrix(field, idvalue);
        }

        public int NonzeroCount {
            get {
                int toret = 0;
                for(int r = 0; r < Rows; r++)
                    for(int c = 0; c < Columns; c++)
                        if(values[r, c] != 0)
                            toret++;
                return toret;
            }
        }
    }
}