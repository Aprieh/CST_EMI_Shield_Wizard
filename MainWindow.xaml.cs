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
            InitializeUIState();
            InitializeValidation();

            DataContext = this;
            LoadProjects();
        }

        private void InitializeUIState()
        {
            // Скрываем все вкладки при инициализации
            InterfaceTabs.Items.Remove(ProjectTab);
            InterfaceTabs.Items.Remove(ShieldTab);
            InterfaceTabs.Items.Remove(ImpactTab);
            InterfaceTabs.Items.Remove(ResultsTab);
        }

        private void ShowTabs(params TabItem[] tabs)
        {
            foreach (var tab in tabs)
            {
                if (!InterfaceTabs.Items.Contains(tab))
                {
                    InterfaceTabs.Items.Add(tab);
                }
            }
        }

        private void HideTabs(params TabItem[] tabs)
        {
            foreach (var tab in tabs)
            {
                if (InterfaceTabs.Items.Contains(tab))
                {
                    InterfaceTabs.Items.Remove(tab);
                }
            }
        }

        private void LoadProjects()
        {
            Projects = new ObservableCollection<Project>
            {
                new Project { ProjectName = "SimpleSchield", CreationDate = new DateTime(2024, 5, 20, 10, 30, 0), LastModifiedDate = new DateTime(2024, 5, 20, 10, 30, 0) },
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

                // Активируем кнопки манипуляции проектом
                SetProjectControlsState(true);
            }
            else
            {
                ClearProjectInfo();
                SetProjectControlsState(false);
            }
        }
        private void CreateProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateNewProject();
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewProject();
        }

        private void CreateNewProject()
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

                    //StatusBarText.Content = $"Проект '{newProjectName}' загружен";
                    ShowTabs(ShieldTab, ImpactTab);
                }
            }
        }
        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowTabs(ProjectTab);
            InterfaceTabs.SelectedItem = ProjectTab;

        }

        private void ChangeProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                selectedProject.LastModifiedDate = DateTime.Now;
                ProjectsList.Items.Refresh();
                ProjectsListView_SelectionChanged(null, null);

                // Имитация загрузки проекта
                MessageBox.Show($"Проект '{selectedProject.ProjectName}' загружен.", "Загрузка проекта", MessageBoxButton.OK, MessageBoxImage.Information);
                //StatusBarText.Content = $"Проект '{selectedProject.ProjectName}' загружен в {DateTime.Now:HH:mm:ss}";

                // Открываем вкладки "Экран" и "Воздействие"
                ShowTabs(ShieldTab, ImpactTab);
            }
        }

        private void RenameProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                var renameProjectWindow = new RenameProjectWindow(selectedProject.ProjectName);
                if (renameProjectWindow.ShowDialog() == true)
                {
                    string newProjectName = renameProjectWindow.NewProjectName;
                    if (Projects.Any(p => p.ProjectName == newProjectName))
                    {
                        MessageBox.Show("Проект с таким именем уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        selectedProject.ProjectName = newProjectName;
                        selectedProject.LastModifiedDate = DateTime.Now;
                        ProjectsList.Items.Refresh();
                        ProjectsListView_SelectionChanged(null, null);
                    }
                }
            }
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedItem is Project selectedProject)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить проект '{selectedProject.ProjectName}'?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Projects.Remove(selectedProject);
                    ClearProjectInfo();
                    SetProjectControlsState(false);
                }
            }
        }

        private void ClearProjectInfo()
        {
            ProjectNameTextBox.Clear();
            CreationDateTextBox.Clear();
            ChangeDateTextBox.Clear();
        }

        private void SetProjectControlsState(bool isEnabled)
        {
            ChangeProjectButton.IsEnabled = isEnabled;
            RenameProjectButton.IsEnabled = isEnabled;
            DeleteProjectButton.IsEnabled = isEnabled;
        }

        private void UpdateProjectControlsState()
        {
            SetProjectControlsState(ProjectsList.SelectedItem != null);
        }

        private void InitSimulation_Click(object sender, RoutedEventArgs e)
        {
            string resourcePathManyLayer = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ManyLayer.cst");
            string resourcePathBesEkrana = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "besEkrana.cst");

            try
            {
                dynamic cstApp = Activator.CreateInstance(Type.GetTypeFromProgID("CSTStudio.Application"));

                cstApp.OpenFile(resourcePathManyLayer);
                dynamic projectManyLayer = cstApp.Active3D;

                dynamic solverManyLayer = projectManyLayer.Solver;
                solverManyLayer.Start();

                cstApp.OpenFile(resourcePathBesEkrana);
                dynamic projectBesEkrana = cstApp.Active3D;

                dynamic solverBesEkrana = projectBesEkrana.Solver;
                solverBesEkrana.Start();

                MessageBox.Show("Расчеты успешно завершены.", "Запуск расчета", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске расчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ShowTabs(ResultsTab);
            InterfaceTabs.SelectedItem = ResultsTab;
            CompareWithAbcent();
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
            string eGraph = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerE.txt");
            string hGraph = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerH.txt");
            var graphView = new GraphView();
            graphView.LoadData(eGraph, hGraph);
            graphView.Show();
        }

        private string FormatAmplitude(double amplitude, string type)
        {
            string[] electricUnits = { "В/м", "мВ/м", "мкВ/м", "нВ/м" };
            string[] magneticUnits = { "А/м", "мА/м", "мкА/м", "нА/м" };
            string[] selectedUnits;
            double scaledAmplitude = amplitude;
            int unitIndex = 0;

            if (type == "electric")
            {
                selectedUnits = electricUnits;
            }
            else if (type == "magnetic")
            {
                selectedUnits = magneticUnits;
            }
            else
            {
                throw new ArgumentException("Unknown type specified. Valid types are 'electric' and 'magnetic'.");
            }

            
            while (scaledAmplitude < 1 && unitIndex < selectedUnits.Length - 1)
            {
                scaledAmplitude *= 1000;
                unitIndex++;
            }
            return $"{scaledAmplitude:F3} {selectedUnits[unitIndex]}";
        }

        // Пример использования метода в методах CompareWithAbcent и CompareWithCalculated

        private void CompareWithAbcent()
        {
            string magneticFilePathManyLayer = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerH.txt");
            string magneticFilePathBesEkrana = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "BesEkranaH.txt");

            string electricFilePathManyLayer = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerE.txt");
            string electricFilePathBesEkrana = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "BesEkranaE.txt");

            try
            {
                var magneticDataManyLayer = new DataParser();
                magneticDataManyLayer.Parse(magneticFilePathManyLayer);
                double maxMagneticAmplitudeManyLayer = magneticDataManyLayer.GetMaxAmplitude();

                var magneticDataBesEkrana = new DataParser();
                magneticDataBesEkrana.Parse(magneticFilePathBesEkrana);
                double maxMagneticAmplitudeBesEkrana = magneticDataBesEkrana.GetMaxAmplitude();

                var electricDataManyLayer = new DataParser();
                electricDataManyLayer.Parse(electricFilePathManyLayer);
                double maxElectricAmplitudeManyLayer = electricDataManyLayer.GetMaxAmplitude();

                var electricDataBesEkrana = new DataParser();
                electricDataBesEkrana.Parse(electricFilePathBesEkrana);
                double maxElectricAmplitudeBesEkrana = electricDataBesEkrana.GetMaxAmplitude();

                // Вычисление эффективности экранирования
                double seMagnetic = 20 * Math.Log10(maxMagneticAmplitudeBesEkrana / maxMagneticAmplitudeManyLayer);
                double seElectric = 20 * Math.Log10(maxElectricAmplitudeBesEkrana / maxElectricAmplitudeManyLayer);

                // Отображение результатов в текстовых полях
                ShieldEfficacyHWithoutTextBox.Text = seMagnetic.ToString("F10") + " дБ";
                ShieldEfficacyEWithoutTextBox.Text = seElectric.ToString("F10") + " дБ";

                // Отображение максимальных значений амплитуды с единицами измерения
                TopHTextBox.Text = FormatAmplitude(maxMagneticAmplitudeManyLayer, "magnetic");
                TopETextBox.Text = FormatAmplitude(maxElectricAmplitudeManyLayer, "electric");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CompareWithCalculated()
        {
            string magneticFilePathManyLayer = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerH.txt");
            string magneticFilePathSimpleShield = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "prostEkranH.txt");

            string electricFilePathManyLayer = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "ManyLayerE.txt");
            string electricFilePathSimpleShield = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Graphs", "prostEkranE.txt");

            try
            {
                var magneticDataManyLayer = new DataParser();
                magneticDataManyLayer.Parse(magneticFilePathManyLayer);
                double maxMagneticAmplitudeManyLayer = magneticDataManyLayer.GetMaxAmplitude();

                var magneticDataSimpleShield = new DataParser();
                magneticDataSimpleShield.Parse(magneticFilePathSimpleShield);
                double maxMagneticAmplitudeSimpleShield = magneticDataSimpleShield.GetMaxAmplitude();

                var electricDataManyLayer = new DataParser();
                electricDataManyLayer.Parse(electricFilePathManyLayer);
                double maxElectricAmplitudeManyLayer = electricDataManyLayer.GetMaxAmplitude();

                var electricDataSimpleShield = new DataParser();
                electricDataSimpleShield.Parse(electricFilePathSimpleShield);
                double maxElectricAmplitudeSimpleShield = electricDataSimpleShield.GetMaxAmplitude();

                // Вычисление эффективности экранирования
                double seMagnetic = 20 * Math.Log10(maxMagneticAmplitudeSimpleShield / maxMagneticAmplitudeManyLayer);
                double seElectric = 20 * Math.Log10(maxElectricAmplitudeSimpleShield / maxElectricAmplitudeManyLayer);

                TopAmplitudeExTextBox.Text = "100 В";
                // Отображение результатов в текстовых полях
                ShieldEfficacyHAnotherTextBox.Text = seMagnetic.ToString("F4") + " дБ";
                ShieldEfficacyEAnotherTextBox.Text = seElectric.ToString("F4") + " дБ";

                // Отображение максимальных значений амплитуды с единицами измерения
                TopHExTextBox.Text = FormatAmplitude(maxMagneticAmplitudeSimpleShield, "magnetic");
                TopEExTextBox.Text = FormatAmplitude(maxElectricAmplitudeSimpleShield, "electric");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadShield_Click(object sender, RoutedEventArgs e)
        {
            var shieldSelectionWindow = new ProjectManagerWindow(Projects);
            if (shieldSelectionWindow.ShowDialog() == true)
            {
                CompareWithCalculated();
            }
        }
    }
}