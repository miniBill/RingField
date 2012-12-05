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
    public partial class MainWindow : Window {
        private static int[] smallPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23 };

        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            int @char = smallPrimes[(int)charSlider.Value];
            int size = (int)rankSlider.Value;
            Field field = new Zp(@char);
            MatrixGenerator gen = new MatrixGenerator(field, size);
            curr = gen.ToList();
            zero = new List<Matrix>();
            zero.Add(Matrix.Zero(field, size));
            Filter(m => true);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            Filter(m => m.Invertible);
        }

        private void Filter(Func<Matrix, bool> lambda) {
            curr = curr.Where(lambda).ToList();
            results.Items.Clear();
            var views = curr.Concat(zero).OrderBy(m => m.NonzeroCount).Select(m => new MatrixView(m));
            results.Items.AddRange(views);
        }

        private List<Matrix> zero;
        private List<Matrix> curr;

        private void results_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var generators = results.SelectedItems.Cast<MatrixView>().Select(v => v.Matrix);
            List<Matrix> currgen = Generate(generators);
            possible.Items.Clear();
            possible.Items.AddRange(currgen.Select(m => new MatrixView(m) {
                MatrixBackground = curr.Contains(m) || m.IsZero ? Brushes.Lime : Brushes.Red
            }));
            if(curr.Concat(zero).ContainsAll(currgen)) {
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
            List<Matrix> lastgen = new List<Matrix>();
            List<Matrix> currgen = new List<Matrix>();
            currgen.AddRange(generators);
            while(!Equals(lastgen, currgen)) {
                var t = lastgen;
                lastgen = currgen;
                currgen = t;
                currgen.Clear();
                currgen.AddRange(lastgen);
                foreach(var a in lastgen)
                    foreach(var b in lastgen) {
                        var plus = a + b;
                        var prod = a * b;
                        if(!currgen.Contains(plus))
                            currgen.Add(plus);
                        if(!currgen.Contains(prod))
                            currgen.Add(prod);
                    }
            }
            return currgen;
        }

        private static bool Equals<T>(IEnumerable<T> a, IEnumerable<T> b) {
            return a.ContainsAll(b) && b.ContainsAll(a);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            curr = results.Items.Cast<MatrixView>().Where(v => v.MatrixBackground != Brushes.Red && !v.Matrix.IsZero).Select(v => v.Matrix).ToList();
            Filter(m => true);
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(charLabel != null)
                charLabel.Content = smallPrimes[(int)charSlider.Value];
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(rankLabel != null)
                rankLabel.Content = rankSlider.Value;
        }
    }
}
