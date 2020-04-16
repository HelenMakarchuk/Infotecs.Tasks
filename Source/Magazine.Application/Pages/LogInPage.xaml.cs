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
        public event EventHandler<RoutedEventArgs> OnLoggedIn;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
            //R1.Height = new GridLength() {  };
        }

        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TryLogIn(Login.Text, Password.Password))
            {
                MessageBlock.Text = "Incorrect login or password";
                MessageBlock.Height = Double.NaN;
                MessageBlockBorder.Visibility = Visibility.Visible;
                MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
                return;
            }

            OnLoggedIn.Invoke(sender, e);
        }

        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp.Invoke(sender, e);
        }
    }
}