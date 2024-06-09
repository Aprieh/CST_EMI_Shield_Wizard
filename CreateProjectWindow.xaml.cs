using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для CreateProjectWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        public string ProjectName { get; private set; }

        public CreateProjectWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = ProjectNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                MessageBox.Show("Имя проекта не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
