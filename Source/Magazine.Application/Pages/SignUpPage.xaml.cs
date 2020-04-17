using Magazine.Domain.Contracts.ViewModel;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class SignUpPage : Page
    {
        ISignUpViewModel _viewModel;
        ILogger _logger;

        public SignUpPage(ISignUpViewModel viewModel,
                          ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _logger = logger;
        }

        public event EventHandler<RoutedEventArgs> OnSignedUp;
        public event EventHandler<RoutedEventArgs> OnLogIn;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            ClearData();
            DataContext = _viewModel;
        }

        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TrySignUp(Login.Text, Password.Password))
            {
                ShowMessage("User with the same login exists");
                return;
            }

            OnSignedUp.Invoke(sender, e);
        }

        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogIn.Invoke(sender, e);
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