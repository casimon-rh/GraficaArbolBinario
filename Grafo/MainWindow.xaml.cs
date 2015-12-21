using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker

namespace Grafo
{

    public partial class MainWindow : MetroWindow
    {
        List<LinePlainObject> lineas = new List<LinePlainObject>();
        List<PointPlainObject> puntos = new List<PointPlainObject>();
        Dictionary<int, Brush> colores = new Dictionary<int, Brush>();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);
            colores.Add(0, Brushes.Blue);
            colores.Add(1, Brushes.Red);
            colores.Add(2, Brushes.Purple);
            colores.Add(3, Brushes.Pink);
            colores.Add(4, Brushes.Yellow);
            colores.Add(5, Brushes.Green);
            colores.Add(6, Brushes.White);
            colores.Add(7, Brushes.Orange);
            colores.Add(8, Brushes.Crimson);
            colores.Add(9, Brushes.Gold);
            colores.Add(10, Brushes.Lavender);
            colores.Add(11, Brushes.LightSteelBlue);
            colores.Add(12, Brushes.Magenta);
            colores.Add(13, Brushes.Olive);
            colores.Add(14, Brushes.Salmon);
            colores.Add(15, Brushes.SpringGreen);
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {

            Config con = new Config();
            con.ShowDialog();
            lineas = con.rutina.Lineas ?? new List<LinePlainObject>(); 
            puntos = con.rutina.Puntos ?? new List<PointPlainObject>();
            foreach (var l in lineas)
                GeneraLinea(l.uno.x, l.dos.x, l.uno.y, l.dos.y, l.id1 + "-" + l.id2);
            GeneraPuntos(puntos.Select(x => x.x).ToList(), puntos.Select(y => y.y).ToList(), puntos.Select(id=>id.id.ToString()).ToList());
            Chart.Viewport.FitToView();
        }
        private void GeneraLinea(int x1, int x2, int y1, int y2, string id)
        {
            EnumerableDataSource<int> dsx = new EnumerableDataSource<int>(new List<int>() { x1, x2 });
            dsx.SetXMapping(x => x);
            EnumerableDataSource<int> dsy = new EnumerableDataSource<int>(new List<int>() { y1, y2 });
            dsy.SetYMapping(y => y);
            var cds = new CompositeDataSource(dsx, dsy);
            Chart.AddLineGraph(cds,
                new Pen(colores[(new Random()).Next(0, 15)], 2),
                new CirclePointMarker { Size = 10.0, Fill = colores[(new Random()).Next(0, 15)] },
                //new CirclePointMarker{ }
                new PenDescription(id));
        }
        private void GeneraPuntos(List<int> _x, List<int> _y, List<string> id)
        {
            EnumerableDataSource<int> dsx = new EnumerableDataSource<int>(_x);
            dsx.SetXMapping(x => x);
            EnumerableDataSource<int> dsy = new EnumerableDataSource<int>(_y);
            dsy.SetYMapping(y => y);
            var cds = new CompositeDataSource(dsx, dsy);
            Chart.AddLineGraph(cds,
                new Pen(Brushes.Transparent, 0),
                new CirclePointMarker { Size = 10.0, Fill = Brushes.White },
                new PenDescription("_"));
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Config c = new Config();
            c.Posicion = 1;
            c.ShowDialog();
            Chart.RemoveUserElements();
            lineas = c.rutina.Lineas ?? new List<LinePlainObject>();
            puntos = c.rutina.Puntos ?? new List<PointPlainObject>();
            foreach (var l in lineas)
                GeneraLinea(l.uno.x, l.dos.x, l.uno.y, l.dos.y, l.id1 + "-" + l.id2);
            GeneraPuntos(puntos.Select(x => x.x).ToList(), puntos.Select(y => y.y).ToList(), puntos.Select(id => id.id.ToString()).ToList());
            Chart.Viewport.FitToView();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Config c = new Config();
            c.Posicion = 2;
            c.ShowDialog();

        }

        private void FlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            Flyout ayuda = (this.Flyouts.Items.GetItemAt(0) as Flyout);
            (ayuda.IsOpen) = (!ayuda.IsOpen);
        }
    }
}
