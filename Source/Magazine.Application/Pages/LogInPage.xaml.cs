using Magazine.Domain.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class LogInPage : Page
    {
        ILogInViewModel _viewModel;

        public LogInPage(ILogInViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        public event EventHandler<RoutedEventArgs> OnSignUp;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
        }

        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp.Invoke(sender, e);
        }

        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}