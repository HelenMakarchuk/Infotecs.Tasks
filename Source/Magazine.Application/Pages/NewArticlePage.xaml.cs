using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class NewArticlePage : Page
    {
        INewArticleViewModel _viewModel;
        IAuthenticationService _authenticationService;

        public NewArticlePage(INewArticleViewModel viewModel,
                              IAuthenticationService authenticationService)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _authenticationService = authenticationService;
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
            throw new NotImplementedException();
        }
    }
}
