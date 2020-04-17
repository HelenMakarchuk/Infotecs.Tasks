using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class NewArticlePage : Page
    {
        INewArticleViewModel _viewModel;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public NewArticlePage(INewArticleViewModel viewModel,
                              IAuthenticationService authenticationService,
                              ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public event EventHandler<RoutedEventArgs> OnClosed;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (!_authenticationService.IsLoggedIn)
            {
                OnClosed.Invoke(sender, e);
                return;
            }

            DataContext = _viewModel;
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnClosed.Invoke(sender, e);
        }

        void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save(Title.Text, Body.Text, _authenticationService.User.Id);

            OnClosed.Invoke(sender, e);
        }

        void ChooseTeaserButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Next release feature !");
        }

        void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 5, 0, 5);
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
