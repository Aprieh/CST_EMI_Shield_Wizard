using OxyPlot.Series;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CST_EMI_Shield_Wizard
{
    public partial class MainWindow : Window
    {
        private CSTController cstController = new();
        private List<string> materials = new List<string> {
            "Alumina (96%) (lossy)", "Alumina (96%) (loss free)",
            "Copper (pure)", "Polycarbonate (lossy)", "Polyimide (lossy)",
            "Polyimide (loss free)", "Zinc", "Aluminum"
        };
        private List<string> spaceSettings = new List<string> {
            "Open", "Open (add space)"
        };
        public MainWindow()
        {
            InitializeComponent();
            PopulateMaterialComboBox();
            InitializeLayerDataGrid();
            PopulateSpaceSetters();

            InitializeValidation();

            InitSimulation.Click += CalculateEMIImpact_Click;
        }

        private void PopulateMaterialComboBox()
        {
            foreach (var material in materials)
            {
                materialComboBox.Items.Add(material);
            }
        }
        private void PopulateSpaceSetters()
        {
            foreach (var setting in spaceSettings)
            {
                xMinBndComboBox.Items.Add(setting);
                yMinBndComboBox.Items.Add(setting);
                zMinBndComboBox.Items.Add(setting);
                xMaxBndComboBox.Items.Add(setting);
                yMaxBndComboBox.Items.Add(setting);
                zMaxBndComboBox.Items.Add(setting);
            }
        }

        private void InitializeValidation()
        {
            xMinTextBox.TextChanged += ValidateTextBox;
            yMinTextBox.TextChanged += ValidateTextBox;
            zMinTextBox.TextChanged += ValidateTextBox;
            xMaxTextBox.TextChanged += ValidateTextBox;
            yMaxTextBox.TextChanged += ValidateTextBox;
            zMaxTextBox.TextChanged += ValidateTextBox;
            probeXPosTextBox.TextChanged += ValidateTextBox;
            probeYPosTextBox.TextChanged += ValidateTextBox;
            probeZPosTextBox.TextChanged += ValidateTextBox;
            xNormalTextBox.TextChanged += ValidateTextBox;
            yNormalTextBox.TextChanged += ValidateTextBox;
            zNormalTextBox.TextChanged += ValidateTextBox;
            xElectricFieldTextBox.TextChanged += ValidateTextBox;
            yElectricFieldTextBox.TextChanged += ValidateTextBox;
            zElectricFieldTextBox.TextChanged += ValidateTextBox;
        }
        private void ValidateTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                bool isValid = double.TryParse(textBox.Text, out double result);
                if (!isValid)
                {
                    textBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 204)); // Светло-желтый цвет
                    textBox.Tag = "Invalid";
                }
                else
                {
                    textBox.Background = Brushes.White;
                    textBox.Tag = null;
                }
            }
        }
        private void InitializeLayerDataGrid()
        {
            layerDataGrid.ItemsSource = new ObservableCollection<LayerData>();
        }

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            string layerName = layerNameTextBox.Text;
            string material = materialComboBox.SelectedItem as string;

            double minX = Convert.ToDouble(xMinTextBox.Text);
            double minY = Convert.ToDouble(yMinTextBox.Text);
            double minZ = Convert.ToDouble(zMinTextBox.Text);
            double maxX = Convert.ToDouble(xMaxTextBox.Text);
            double maxY = Convert.ToDouble(yMaxTextBox.Text);
            double maxZ = Convert.ToDouble(zMaxTextBox.Text);

            var layers = layerDataGrid.ItemsSource as ObservableCollection<LayerData>;
            layers.Add(new LayerData
            {
                LayerName = layerName,
                Material = material,
                MinCoordinates = $"[{minX}, {minY}, {minZ}]",
                MaxCoordinates = $"[{maxX}, {maxY}, {maxZ}]"
            });
        }

        private void DeleteSelectedLayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (layerDataGrid.SelectedItem != null)
            {
                var layers = layerDataGrid.ItemsSource as ObservableCollection<LayerData>;
                layers.Remove(layerDataGrid.SelectedItem as LayerData);
            }
            else
            {
                MessageBox.Show("No row selected for deletion", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void LayerDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
        private void FeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FeedbackWindow feedbackWindow = new FeedbackWindow();
            feedbackWindow.ShowDialog();
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            ProjectCreator projectCreator = new ProjectCreator();
            if (projectCreator.ShowDialog() == true)
            {
                MessageBox.Show("Project creation dialog finished successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
        private void RestoreSize_Click(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.CanResize;
            WindowState = WindowState.Normal;
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void CSTLaunch_Click(object sender, RoutedEventArgs e)
        {
            LaunchCST();
        }
        //доработать
        private void CalculateEMIImpact_Click(object sender, RoutedEventArgs e)
        {
            LaunchCST();
        }
        private void LaunchCST()
        {
            try
            {
                cstController.LaunchCST();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowGraphOfTheCalculated_Click(object sender, RoutedEventArgs e)
        {
            var graphView = new GraphView
            {
                PlotModel = CreateCalculatedGraph()
            };
            graphView.Show();
        }

        private void ShowGraphOfTheLoaded_Click(object sender, RoutedEventArgs e)
        {
            var graphView = new GraphView
            {
                PlotModel = CreateLoadedGraph()
            };
            graphView.Show();
        }

        private PlotModel CreateCalculatedGraph()
        {
            var plotModel = new PlotModel { Title = "Calculated Graph" };
            var lineSeries = new LineSeries
            {
                Title = "Calculated Data",
                MarkerType = MarkerType.Circle
            };

            // Добавьте здесь данные для графика рассчитанных данных
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 20));
            lineSeries.Points.Add(new DataPoint(20, 10));
            lineSeries.Points.Add(new DataPoint(30, 40));

            plotModel.Series.Add(lineSeries);
            return plotModel;
        }

        private PlotModel CreateLoadedGraph()
        {
            var plotModel = new PlotModel { Title = "Loaded Graph" };
            var lineSeries = new LineSeries
            {
                Title = "Loaded Data",
                MarkerType = MarkerType.Circle
            };

            // Добавьте здесь данные для графика загруженных данных
            lineSeries.Points.Add(new DataPoint(0, 10));
            lineSeries.Points.Add(new DataPoint(10, 30));
            lineSeries.Points.Add(new DataPoint(20, 20));
            lineSeries.Points.Add(new DataPoint(30, 50));

            plotModel.Series.Add(lineSeries);
            return plotModel;
        }
    }
    public class LayerData
    {
        public string LayerName { get; set; }
        public string Material { get; set; }
        public string MinCoordinates { get; set; }
        public string MaxCoordinates { get; set; }
    }
}