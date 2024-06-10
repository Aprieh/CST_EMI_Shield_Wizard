using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Linq;
using System.Windows;
using OxyPlot.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

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

        public double GetMaxAmplitude()
        {
            double maxAmplitude = 0;
            foreach (var point in DataPoints)
            {
                if (point.Y > maxAmplitude)
                {
                    maxAmplitude = point.Y;
                }
            }
            return maxAmplitude;
        }

        public void Parse(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            bool dataSection = false;

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
                else if (line.StartsWith("Data/Title"))
                {
                    dataSection = true;
                }
                else if (dataSection)
                {
                    var values = line.Split('\t');
                    if (values.Length == 2)
                    {
                        if (double.TryParse(values[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double x) &&
                            double.TryParse(values[1].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
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
        public PlotModel MagneticShieldingModel { get; private set; }
        public PlotModel ElectricShieldingModel { get; private set; }

        private LineSeries magneticLineSeries;
        private LineSeries electricLineSeries;
        private LineAnnotation magneticMaxAnnotation;
        private LineAnnotation electricMaxAnnotation;

        public GraphView()
        {
            InitializeComponent();
            DataContext = this;

            MagneticShieldingModel = new PlotModel { Title = "Магнитное воздействие" };
            ElectricShieldingModel = new PlotModel { Title = "Электрическое воздействие" };

            CreateGraphModels();
        }

        private void CreateGraphModels()
        {
            // Магнитная составляющая экранирования
            magneticLineSeries = new LineSeries
            {
                Title = "Магнитное воздействие",
                MarkerType = MarkerType.None,
                Color = OxyColors.Blue
            };
            MagneticShieldingModel.Series.Add(magneticLineSeries);

            magneticMaxAnnotation = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                LineStyle = LineStyle.Dash,
                Color = OxyColors.Red,
                Text = "Max",
                TextColor = OxyColors.Red,
                X = 0
            };
            MagneticShieldingModel.Annotations.Add(magneticMaxAnnotation);

            // Электрическая составляющая экранирования
            electricLineSeries = new LineSeries
            {
                Title = "Электрическое воздействие",
                MarkerType = MarkerType.None,
                Color = OxyColors.Green
            };
            ElectricShieldingModel.Series.Add(electricLineSeries);

            electricMaxAnnotation = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                LineStyle = LineStyle.Dash,
                Color = OxyColors.Red,
                Text = "Max",
                TextColor = OxyColors.Red,
                X = 0
            };
            ElectricShieldingModel.Annotations.Add(electricMaxAnnotation);

            // Подписи осей
            MagneticShieldingModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Частота (Гц)" });
            MagneticShieldingModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Ампер на метр" });

            ElectricShieldingModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Частота (Гц)" });
            ElectricShieldingModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Вольт на метр" });
        }

        public void LoadData(string magneticFilePath, string electricFilePath)
        {
            var magneticData = new DataParser();
            magneticData.Parse(magneticFilePath);
            UpdateGraph(MagneticShieldingModel, magneticLineSeries, magneticMaxAnnotation, magneticData.Title, magneticData.XLabel, magneticData.YLabel, magneticData.DataPoints);

            var electricData = new DataParser();
            electricData.Parse(electricFilePath);
            UpdateGraph(ElectricShieldingModel, electricLineSeries, electricMaxAnnotation, electricData.Title, electricData.XLabel, electricData.YLabel, electricData.DataPoints);
        }

        private void UpdateGraph(PlotModel plotModel, LineSeries lineSeries, LineAnnotation maxAnnotation, string title, string xLabel, string yLabel, List<DataPoint> dataPoints)
        {
            plotModel.Title = title;
            plotModel.Axes.First(axis => axis.Position == AxisPosition.Bottom).Title = xLabel;
            plotModel.Axes.First(axis => axis.Position == AxisPosition.Left).Title = yLabel;

            lineSeries.Points.Clear();
            lineSeries.Points.AddRange(dataPoints);

            var maxPoint = dataPoints.OrderByDescending(p => p.Y).First();
            maxAnnotation.X = maxPoint.X;

            plotModel.InvalidatePlot(true);
        }
    }
}
