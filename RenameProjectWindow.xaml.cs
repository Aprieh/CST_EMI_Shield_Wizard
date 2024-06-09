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
    
    public partial class RenameProjectWindow : Window
    {
        public string NewProjectName { get; private set; }

        public RenameProjectWindow(string currentName)
        {
            InitializeComponent();
            ProjectNameTextBox.Text = currentName;
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            NewProjectName = ProjectNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(NewProjectName))
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
