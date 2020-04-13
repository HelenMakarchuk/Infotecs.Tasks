using Magazine.Application.Pages;
using System.Windows;

namespace Magazine.Application
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            CurrentPage.Content = new LogInPage();
        }
    }
}
