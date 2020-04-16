using Magazine.Domain.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class SignUpPage : Page
    {
        ISignUpViewModel _viewModel;

        public SignUpPage(ISignUpViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        public event EventHandler<RoutedEventArgs> OnSignedUp;
        public event EventHandler<RoutedEventArgs> OnLogIn;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
        }

        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TrySignUp(Login.Text, Password.Password))
            {
                MessageBlock.Text = "User with the same login exists";
                MessageBlock.Height = Double.NaN;
                MessageBlockBorder.Visibility = Visibility.Visible;
                MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
                return;
            }

            OnSignedUp.Invoke(sender, e);
        }

        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogIn.Invoke(sender, e);
        }
    }
}