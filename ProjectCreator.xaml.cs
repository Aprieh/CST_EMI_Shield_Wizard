using System.IO;
using System.Windows;
using System.Windows.Shapes;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace CST_EMI_Shield_Wizard
{
    public partial class ProjectCreator : Window
    {
        public ProjectCreator()
        {
            InitializeComponent();
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = NameInputTextBox.Text;
            string savePath = PathInputTextBox.Text;

            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(savePath))
            {
                MessageBox.Show("Введите имя проекта и путь для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CreateProject(projectName, savePath))
            {
                MessageBox.Show("Проект успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Не удалось создать проект.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelProjectButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void FileSearchButton_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    PathInputTextBox.Text = dialog.FileName;
                }
            }
            Activate();
        }

        private bool CreateProject(string projectName, string savePath)
        {
            try
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string filePath = System.IO.Path.Combine(savePath, projectName + ".txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Project Name: {projectName}");
                    writer.WriteLine("Configuration settings go here.");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
