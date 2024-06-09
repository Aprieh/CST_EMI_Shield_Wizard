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

                // Имитация загрузки проекта
                MessageBox.Show($"Проект '{selectedProject.ProjectName}' загружен.", "Загрузка проекта", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
