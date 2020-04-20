using Infotecs.Magazine.Application.Contracts.Page;
using Magazine.Domain.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница создания нового пользователя приложения.
    /// </summary>
    public partial class SignUpPage : Page, IPage
    {
        ISignUpViewModel _viewModel;

        public SignUpPage(ISignUpViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        /// <summary>
        /// Событие завершения регистрации нового пользователя приложения.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnSignedUp;

        /// <summary>
        /// Событие перехода на страницу аутентификации пользователя приложения.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnLogIn;

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события начала выполнения регистрации.
        /// </summary>
        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TrySignUp(Login.Text, Password.Password))
            {
                ShowMessage("User with the same login exists");
                return;
            }

            OnSignedUp.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события перехода на страницу аутентификации пользователя.
        /// </summary>
        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogIn.Invoke(sender, e);
        }


        public void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
        }


        public void HideMessage()
        {
            MessageBlock.Text = "";
            MessageBlock.Height = 0;
            MessageBlockBorder.Visibility = Visibility.Hidden;
            MessageBlockBorder.Margin = new Thickness(0);
        }

        public void SetData()
        {
            Login.Text = String.Empty;
            Password.Password = String.Empty;
            HideMessage();
        }
    }
}