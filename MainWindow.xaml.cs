using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


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
        public MainWindow()
        {
            InitializeComponent();
            PopulateMaterialComboBox();
            InitializeLayerDataGrid();

            InitSimulation.Click += CalculateEMIImpact_Click;
        }

        private void PopulateMaterialComboBox()
        {
            foreach (var material in materials)
            {
                materialComboBox.Items.Add(material);
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

        
    }
    public class LayerData
    {
        public string LayerName { get; set; }
        public string Material { get; set; }
        public string MinCoordinates { get; set; }
        public string MaxCoordinates { get; set; }
    }
}