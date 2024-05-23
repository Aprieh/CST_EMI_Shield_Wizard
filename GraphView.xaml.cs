using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Linq;
using System.Windows;
using OxyPlot.Annotations;
using System.Collections.Generic;
using System.IO;

namespace CST_EMI_Shield_Wizard
{

    public class DataParser
    {
        public string Title { get; private set; }
        public string XLabel { get; private set; }
        public string YLabel { get; private set; }
        public List<DataPoint> DataPoints { get; private set; }

        public DataParser()
        {
            DataPoints = new List<DataPoint>();
        }

        public void Parse(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (line.StartsWith("Title"))
                {
                    Title = line.Split('=')[1].Trim();
                }
                else if (line.StartsWith("Xlabel"))
                {
                    XLabel = line.Split('=')[1].Trim();
                }
                else if (line.StartsWith("Ylabel"))
                {
                    YLabel = line.Split('=')[1].Trim();
                }
                else if (!line.StartsWith("Curve") && !line.StartsWith("Filename") && !line.StartsWith("Npoints") &&
                         !line.StartsWith("Type") && !line.StartsWith("Subtype") && !line.StartsWith("Result type") &&
                         !line.StartsWith("View type") && !line.StartsWith("Plot type") && !line.StartsWith("Data/Title") &&
                         !line.StartsWith("X/Label") && !line.StartsWith("X/Unit") && !line.StartsWith("Y/Label") &&
                         !line.StartsWith("Y/Unit") && !line.StartsWith("Y/Logfactor") && !line.StartsWith("Plot/wrapHeuristics"))
                {
                    var values = line.Split('\t');
                    if (values.Length == 2)
                    {
                        if (double.TryParse(values[0].Replace(',', '.'), out double x) &&
                            double.TryParse(values[1].Replace(',', '.'), out double y))
                        {
                            DataPoints.Add(new DataPoint(x, y));
                        }
                    }
                }
            }
        }

    }

    public partial class GraphView : Window
    {
        public PlotModel PlotModel { get; private set; }
        private LineSeries lineSeries;
        private LineAnnotation maxAnnotation;

        public GraphView()
        {
            InitializeComponent();
            DataContext = this;
            PlotModel = new PlotModel { Title = "Sample Graph" };
            CreateGraph();
        }

        public void UpdateGraph(string title, string xLabel, string yLabel, List<DataPoint> dataPoints)
        {
            PlotModel.Title = title;
            PlotModel.Axes.Clear();
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xLabel });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = yLabel });

            lineSeries.Points.Clear();
            lineSeries.Points.AddRange(dataPoints);

            var maxPoint = dataPoints.OrderByDescending(p => p.Y).First();
            maxAnnotation.X = maxPoint.X;
            PlotModel.InvalidatePlot(true);
        }

        private void CreateGraph()
        {
            lineSeries = new LineSeries
            {
                Title = "Line Series",
                MarkerType = MarkerType.Circle
            };
            PlotModel.Series.Add(lineSeries);

            maxAnnotation = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                LineStyle = LineStyle.Dash,
                Color = OxyColors.Red,
                Text = "Max",
                TextColor = OxyColors.Red,
                X = 0
            };
            PlotModel.Annotations.Add(maxAnnotation);

            PlotModel.InvalidatePlot(true);
        }

        private void MoveToMax_Click(object sender, RoutedEventArgs e)
        {
            if (lineSeries.Points.Count == 0) return;
            var maxPoint = lineSeries.Points.OrderByDescending(p => p.Y).First();
            maxAnnotation.X = maxPoint.X;
            PlotModel.InvalidatePlot(true);
        }
    }
}
