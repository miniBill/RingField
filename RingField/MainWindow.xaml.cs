using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RingField {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private static readonly int[] SmallPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23 };

        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            var @char = SmallPrimes[(int)charSlider.Value];
            var size = (int)rankSlider.Value;
            IField field = new Zp(@char);
            var gen = new MatrixGenerator(field, size);
            _curr = gen.ToList();
            _zero = new List<Matrix> { Matrix.Zero(field, size) };
            Filter(m => true);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            Filter(m => m.Invertible);
        }

        private void Filter(Func<Matrix, bool> lambda) {
            _curr = _curr.Where(lambda).ToList();
            results.Items.Clear();
            var views = _curr.Concat(_zero).OrderBy(m => m.NonzeroCount).Select(m => new MatrixView(m));
            results.Items.AddRange(views);
        }

        private List<Matrix> _zero;
        private List<Matrix> _curr;

        private void results_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var generators = results.SelectedItems.Cast<MatrixView>().Select(v => v.Matrix);
            List<Matrix> currgen = Generate(generators);
            possible.Items.Clear();
            possible.Items.AddRange(currgen.Select(m => new MatrixView(m) {
                MatrixBackground = _curr.Contains(m) || m.IsZero ? Brushes.Lime : Brushes.Red
            }));
            if(_curr.Concat(_zero).ContainsAll(currgen)) {
                possible.Background = Brushes.Lime;
                foreach(var item in results.SelectedItems.Cast<MatrixView>())
                    item.MatrixBackground = Brushes.Lime;
            }
            else {
                possible.Background = Brushes.Red;
                foreach(var item in results.SelectedItems.Cast<MatrixView>())
                    item.MatrixBackground = Brushes.Red;
            }
        }

        private static List<Matrix> Generate(IEnumerable<Matrix> generators) {
            var lastgen = new List<Matrix>();
            var currgen = new List<Matrix>();
            currgen.AddRange(generators);
            while(!Equals(lastgen, currgen)) {
                Swap(ref lastgen, ref currgen);
                currgen.Clear();
                currgen.AddRange(lastgen);
                foreach(var a in lastgen)
                    foreach(var b in lastgen) {
                        AddSum(a, b, currgen);
                        AddProduct(a, b, currgen);
                    }
            }
            return currgen;
        }

        private static void Swap(ref List<Matrix> lastgen, ref List<Matrix> currgen) {
            var t = lastgen;
            lastgen = currgen;
            currgen = t;
        }

        private static void AddProduct(Matrix a, Matrix b, List<Matrix> currgen) {
            var prod = a * b;
            if(!currgen.Contains(prod))
                currgen.Add(prod);
        }

        private static void AddSum(Matrix a, Matrix b, List<Matrix> currgen) {
            var plus = a + b;
            if(!currgen.Contains(plus))
                currgen.Add(plus);
        }

        private static bool Equals<T>(IEnumerable<T> a, IEnumerable<T> b) {
            var alist = a as IList<T> ?? a.ToList();
            var blist = b as IList<T> ?? b.ToList();
            return alist.ContainsAll(blist) && blist.ContainsAll(alist);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            var matrixViews = results.Items.Cast<MatrixView>();
            _curr = matrixViews.Where(v => !Equals(v.MatrixBackground, Brushes.Red) && !v.Matrix.IsZero).Select(v => v.Matrix).ToList();
            Filter(m => true);
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(charLabel != null)
                charLabel.Content = SmallPrimes[(int)charSlider.Value];
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(rankLabel != null)
                rankLabel.Content = rankSlider.Value;
        }
    }
}
