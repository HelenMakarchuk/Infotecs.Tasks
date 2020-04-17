using Magazine.Domain.Contracts.ViewModel;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница входа в приложение.
    /// </summary>
    public partial class LogInPage : Page
    {
        ILogInViewModel _viewModel;
        ILogger _logger;

        public LogInPage(ILogInViewModel viewModel,
                         ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _logger = logger;
        }

        public event EventHandler<RoutedEventArgs> OnSignUp;
        public event EventHandler<RoutedEventArgs> OnLoggedIn;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            ClearData();
            DataContext = _viewModel;
        }

        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TryLogIn(Login.Text, Password.Password))
            {
                ShowMessage("Incorrect login or password");
                return;
            }

            OnLoggedIn.Invoke(sender, e);
        }

        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp.Invoke(sender, e);
        }

        void ClearData()
        {
            Login.Text = "";
            Password.Password = "";
            HideMessage();
        }

        void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
        }

        void HideMessage()
        {
            MessageBlock.Text = "";
            MessageBlock.Height = 0;
            MessageBlockBorder.Visibility = Visibility.Hidden;
            MessageBlockBorder.Margin = new Thickness(0);
        }
    }
}