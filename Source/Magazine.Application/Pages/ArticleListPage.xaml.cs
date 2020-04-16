using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Magazine.Application.Pages
{
    public partial class ArticleListPage : Page
    {
        IArticleListViewModel _viewModel;
        IAuthenticationService _authenticationService;

        public ArticleListPage(IArticleListViewModel viewModel,
                               IAuthenticationService authenticationService)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _authenticationService = authenticationService;
        }

        public event EventHandler<RoutedEventArgs> OnAddArticle;
        public event EventHandler<RoutedEventArgs> OnLoggedOut;

        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (!_authenticationService.IsLoggedIn)
            {
                OnLoggedOut.Invoke(sender, e);
                return;
            }

            DataContext = _viewModel;
        }

        void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            OnAddArticle.Invoke(sender, e);
        }

        void DeleteArticleButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditArticleTitleButton_Click(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Style = TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleTitleButton.Background = EditArticleTitleButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");

            throw new NotImplementedException();
        }

        private void EditArticleBodyButton_Click(object sender, RoutedEventArgs e)
        {
            BodyTextBox.Style = BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleBodyButton.Background = EditArticleBodyButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");

            throw new NotImplementedException();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.LogOut();

            OnLoggedOut.Invoke(sender, e);
        }
    }
}