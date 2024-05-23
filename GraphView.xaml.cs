using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Linq;
using System.Windows;
using OxyPlot.Annotations;

namespace CST_EMI_Shield_Wizard
{
    /// <summary>
    /// Логика взаимодействия для GraphView.xaml
    /// </summary>
    public partial class GraphView : Window
    {
        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get => plotModel;
            set
            {
                plotModel = value;
                DataContext = this;
                plotView.Model = plotModel;
            }
        }
        private LineSeries lineSeries;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private LineAnnotation maxAnnotation;

        public GraphView()
        {
            InitializeComponent();
            PlotModel = new PlotModel { Title = "Sample Graph" };
            CreateGraph();
        }

        private void CreateGraph()
        {
            lineSeries = new LineSeries
            {
                Title = "Line Series",
                MarkerType = MarkerType.Circle
            };

            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 18));
            lineSeries.Points.Add(new DataPoint(20, 12));
            lineSeries.Points.Add(new DataPoint(30, 8));
            lineSeries.Points.Add(new DataPoint(40, 15));

            PlotModel.Series.Add(lineSeries);

            xAxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 50 };
            yAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 20 };

            PlotModel.Axes.Add(xAxis);
            PlotModel.Axes.Add(yAxis);

            maxAnnotation = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                LineStyle = LineStyle.Dash,
                Color = OxyColors.Red,
                Text = "Max",
                TextColor = OxyColors.Red,
                X = lineSeries.Points.First().X
            };
            PlotModel.Annotations.Add(maxAnnotation);

            PlotModel.InvalidatePlot(true);
        }

        private void MoveSliderToMaxValue(object sender, RoutedEventArgs e)
        {
            if (lineSeries == null || lineSeries.Points.Count == 0)
                return;

            var maxPoint = lineSeries.Points.OrderByDescending(p => p.Y).First();
            maxAnnotation.X = maxPoint.X;
            PlotModel.InvalidatePlot(true);
        }
    }
}
