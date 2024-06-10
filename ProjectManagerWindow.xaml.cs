using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace CST_EMI_Shield_Wizard
{
    public partial class ProjectManagerWindow : Window
    {
        public ObservableCollection<Project> Projects { get; set; }
        public Project SelectedProject { get; private set; }

        public ProjectManagerWindow(ObservableCollection<Project> projects)
        {
            InitializeComponent();
            Projects = projects;
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
                SelectedProject = selectedProject;
            }
            else
            {
                ClearProjectInfo();
                SetProjectControlsState(false);
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            var answer = MessageBox.Show($"Проект \"{SelectedProject.ProjectName}\" выбран", "Выбор проекта", MessageBoxButton.OK);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ClearProjectInfo()
        {
            ProjectNameTextBox.Clear();
            CreationDateTextBox.Clear();
            ChangeDateTextBox.Clear();
        }

        private void SetProjectControlsState(bool isEnabled)
        {
            SelectSheild.IsEnabled = isEnabled;
            
        }
    }
}
