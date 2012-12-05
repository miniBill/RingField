using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RingField {
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : UserControl {
        public MatrixView(Matrix matrix) {
            InitializeComponent();
            Matrix = matrix;
            text.Text = matrix.ToString();
        }

        public Matrix Matrix {
            get;
            private set;
        }

        public Brush MatrixBackground {
            get {
                return border.Background;
            }
            set {
                border.Background = value;
            }
        }
    }
}
