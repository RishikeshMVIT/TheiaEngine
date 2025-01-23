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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheiaEditor.GameProject.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for CreateProjectView.xaml
    /// </summary>
    public partial class CreateProjectView : UserControl
    {
        public CreateProjectView()
        {
            InitializeComponent();
        }

        private void OnCreate_Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as CreateProject;
            var projectpath = vm.CreateProjectFromTemplate(templateList.SelectedItem as ProjectTemplate);

            bool dialogResult = false;
            var win = Window.GetWindow(this);

            if (!string.IsNullOrEmpty(projectpath))
            {
                dialogResult = true;
            }

            win.DialogResult = dialogResult;
            win.Close();
        }
    }
}
