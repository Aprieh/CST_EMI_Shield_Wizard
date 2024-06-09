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
        public ObservableCollection<Project> Projects { get; set; }

        private CSTController cstController = new();
        private List<string> materials = new List<string> {
            "Alumina (96%) (lossy)", "Alumina (96%) (loss free)",
            "Copper (pure)", "Polycarbonate (lossy)", "Polyimide (lossy)",
            "Polyimide (loss free)", "Zinc", "Aluminum"
        };
        private List<string> spaceSettings = new List<string> {
            "Open", "Open (add space)"
        };
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            PopulateMaterialComboBox();
            InitializeLayerDataGrid();
            PopulateSpaceSetters();

            InitializeValidation();

            InitSimulation.Click += CalculateEMIImpact_Click;

            DataContext = this;
            LoadProjects();
        }

        private void LoadProjects()
        {
            Projects = new ObservableCollection<Project>
            {
                new Project { ProjectName = "Project A", CreationDate = new DateTime(2024, 5, 20, 10, 30, 0), LastModifiedDate = new DateTime(2024, 5, 20, 10, 30, 0) },
                new Project { ProjectName = "Project B", CreationDate = new DateTime(2024, 5, 21, 11, 45, 0), LastModifiedDate = new DateTime(2024, 5, 21, 11, 45, 0) },
                new Project { ProjectName = "Project C", CreationDate = new DateTime(2024, 5, 22, 14, 0, 0), LastModifiedDate = new DateTime(2024, 5, 22, 14, 0, 0) },
                new Project { ProjectName = "Project D", CreationDate = new DateTime(2024, 5, 23, 9, 15, 0), LastModifiedDate = new DateTime(2024, 5, 23, 9, 15, 0) },
                new Project { ProjectName = "Project E", CreationDate = new DateTime(2024, 5, 24, 16, 30, 0), LastModifiedDate = new DateTime(2024, 5, 24, 16, 30, 0) }
            };
            ProjectsList.ItemsSource = Projects;
        }

        private void ProjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                ProjectNameTextBox.Text = selectedProject.ProjectName;
                CreationDateTextBox.Text = selectedProject.CreationDate.ToString("dd.MM.yyyy HH:mm");
                ChangeDateTextBox.Text = selectedProject.LastModifiedDate.ToString("dd.MM.yyyy HH:mm");
            }
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var createProjectWindow = new CreateProjectWindow();
            if (createProjectWindow.ShowDialog() == true)
            {
                string newProjectName = createProjectWindow.ProjectName;
                if (Projects.Any(p => p.ProjectName == newProjectName))
                {
                    MessageBox.Show("Проект с таким именем уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var newProject = new Project
                    {
                        ProjectName = newProjectName,
                        CreationDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };
                    Projects.Add(newProject);
                }
            }
        }

        private void ChangeProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                selectedProject.LastModifiedDate = DateTime.Now;
                ProjectsList.Items.Refresh();
                ProjectsListView_SelectionChanged(null, null);
            }
        }

        private void RenameProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                //метод пока пустышка
                selectedProject.ProjectName = "Renamed Project";
                selectedProject.LastModifiedDate = DateTime.Now;
                ProjectsList.Items.Refresh();
                ProjectsListView_SelectionChanged(null, null);
            }
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                Projects.Remove(selectedProject);
                ProjectNameTextBox.Clear();
                CreationDateTextBox.Clear();
                ChangeDateTextBox.Clear();
            }
        }

        private void InitSimulation_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxesWithRandomValues();
        }

        private void UpdateTextBoxesWithRandomValues()
        {
            TopAmplitudeTextBox.Text = GenerateRandomValue(0, 1).ToString("F2");
            TopHTextBox.Text = GenerateRandomValue(0, 1).ToString("F2");
            TopETextBox.Text = GenerateRandomValue(0, 1).ToString("F2");
        }

        private double GenerateRandomValue(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
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
            try
            {
                string layerName = layerNameTextBox.Text;
                if (string.IsNullOrWhiteSpace(layerName))
                {
                    MessageBox.Show("Имя слоя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string material = materialComboBox.SelectedItem as string;
                if (material == null)
                {
                    MessageBox.Show("Выберите материал.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(xMinTextBox.Text, out double minX) ||
                    !double.TryParse(yMinTextBox.Text, out double minY) ||
                    !double.TryParse(zMinTextBox.Text, out double minZ) ||
                    !double.TryParse(xMaxTextBox.Text, out double maxX) ||
                    !double.TryParse(yMaxTextBox.Text, out double maxY) ||
                    !double.TryParse(zMaxTextBox.Text, out double maxZ))
                {
                    MessageBox.Show("Введите корректные числовые значения для координат.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var layers = layerDataGrid.ItemsSource as ObservableCollection<LayerData>;
                if (layers == null)
                {
                    layers = new ObservableCollection<LayerData>();
                    layerDataGrid.ItemsSource = layers;
                }

                layers.Add(new LayerData
                {
                    LayerName = layerName,
                    Material = material,
                    MinCoordinates = $"[{minX}, {minY}, {minZ}]",
                    MaxCoordinates = $"[{maxX}, {maxY}, {maxZ}]"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            var graphView = new GraphView();
            graphView.LoadData(@"C:\Files\AdvancedShield.txt", @"C:\Files\AdvancedShield.txt"); // Изменено: передаем два файла
            graphView.Show();
        }

        private void ShowGraphOfTheLoaded_Click(object sender, RoutedEventArgs e)
        {
            var graphView = new GraphView();
            graphView.LoadData(@"C:\Files\SimpleShield.txt", @"C:\Files\SimpleShield.txt"); // Изменено: передаем два файла
            graphView.Show();
        }

        private void CompareWithAbcent_Click(object sender, RoutedEventArgs e)
        {
            ShieldEfficacyHWithoutTextBox.Text = GenerateRandomValue(0, 1).ToString("F2");
            ShieldEfficacyEWithoutTextBox.Text = GenerateRandomValue(0, 1).ToString("F2");
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