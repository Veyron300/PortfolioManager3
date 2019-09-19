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
using PortfolioManager3.WPFUtility;
using PortfolioManager3.InstrumentDataFactory;
using System.Collections.ObjectModel;

namespace PortfolioManager3
{
    /// <summary>
    /// Interaction logic for AnalyticsView.xaml
    /// </summary>
    public partial class AnalyticsView : UserControl
    {

        internal IList<Analytic> HistoricalAnalyticSeries { get; set; } = new List<Analytic>();
        internal PointCollection HistoricalPointSeries { get; set; } = new PointCollection();
        
        public AnalyticsView()
        {
            InitializeComponent();
            returnGraph.DataContext = this;
            

            Mediator.Instance.PortfolioChanged += (s, e) =>
            {
                HistoricalAnalyticSeries.Clear();
                HistoricalPointSeries.Clear();
                returnGraph.Children.Clear();
                HistoricalAnalyticSeries = e.AnalyticList.ToList();
                
                if (HistoricalAnalyticSeries.Count > 0)
                {
                    DrawGraph();
                }

            };
        }

        private void DrawGraph()
        {
            double wxmin = -5;
            double wxmax = 65; //Convert.ToDouble(HistoricalAnalyticSeries.Count());
            double wymin = -30; //Convert.ToDouble(HistoricalAnalyticSeries.Min().CalculatedAnalytic) -5;  
            double wymax = 30; //Convert.ToDouble(HistoricalAnalyticSeries.Max().CalculatedAnalytic) + 5;
            const double xstep = 5;
            const double ystep = 10;

            const double dmargin = 10;
            double dxmin = dmargin;
            double dxmax = returnGraph.Width - dmargin;
            double dymin = dmargin;
            double dymax = returnGraph.Height - dmargin;

            // Prepare the transformation matrices.
            PrepareTransformations(
                wxmin, wxmax, wymin, wymax,
                dxmin, dxmax, dymax, dymin);

            // Get the tic mark lengths.
            Point p0 = DtoW(new Point(0, 0));
            Point p1 = DtoW(new Point(5, 5));
            double xtic = p1.X - p0.X;
            double ytic = p1.Y - p0.Y;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            p0 = new Point(wxmin, 0);
            p1 = new Point(wxmax, 0);
            xaxis_geom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));


            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            returnGraph.Children.Add(xaxis_path);

            // Make the Y axis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            p0 = new Point(0, wymin);
            p1 = new Point(0, wymax);
            xaxis_geom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (double y = -50; y <= wymax;  y += ystep)
            {
                // Add the tic mark.
                Point tic0 = WtoD(new Point(-xtic, y));
                Point tic1 = WtoD(new Point(xtic, y));
                xaxis_geom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's Y coordinate.
                DrawText(returnGraph, y.ToString(),
                    new Point(tic0.X - 10, tic0.Y), 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center);
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;
            returnGraph.Children.Add(yaxis_path);

            //Build Graph

            
            for (int i=0; i < HistoricalAnalyticSeries.Count(); i++)
            {
                
                Point p = new Point(i, Convert.ToDouble(HistoricalAnalyticSeries[i].CalculatedAnalytic));
                HistoricalPointSeries.Add(WtoD(p));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.DarkOrange;
            polyline.Points = HistoricalPointSeries;
            returnGraph.Children.Add(polyline);

            


        }

        // Prepare values for perform transformations.
        private Matrix WtoDMatrix, DtoWMatrix;
        private void PrepareTransformations(
            double wxmin, double wxmax, double wymin, double wymax,
            double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;
            WtoDMatrix.Translate(-wxmin, -wymin);

            double xscale = (dxmax - dxmin) / (wxmax - wxmin);
            double yscale = (dymax - dymin) / (wymax - wymin);
            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(dxmin, dymin);

            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point)
        {
            return WtoDMatrix.Transform(point);
        }

        // Transform a point from device to world coordinates.
        private Point DtoW(Point point)
        {
            return DtoWMatrix.Transform(point);
        }

        // Position a label at the indicated point.
        private void DrawText(Canvas can, string text, Point location,
            double font_size,
            HorizontalAlignment halign, VerticalAlignment valign)
        {
            // Make the label.
            Label label = new Label();
            label.Content = text;
            label.FontSize = font_size;
            can.Children.Add(label);

            // Position the label.
            label.Measure(new Size(double.MaxValue, double.MaxValue));

            double x = location.X;
            if (halign == HorizontalAlignment.Center)
                x -= label.DesiredSize.Width / 2;
            else if (halign == HorizontalAlignment.Right)
                x -= label.DesiredSize.Width;
            Canvas.SetLeft(label, x);

            double y = location.Y;
            if (valign == VerticalAlignment.Center)
                y -= label.DesiredSize.Height / 2;
            else if (valign == VerticalAlignment.Bottom)
                y -= label.DesiredSize.Height;
            Canvas.SetTop(label, y);
        }


        



    }
}
