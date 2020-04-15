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

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
        }

        public void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SignUp(Login.Text, Password.Password);

            OnSignedUp.Invoke(sender, e);
        }
    }
}