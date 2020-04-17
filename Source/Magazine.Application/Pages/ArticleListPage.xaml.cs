using Magazine.Application.Contracts.Service;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница отображает список статей и контент выбранной статьи.
    /// </summary>
    public partial class ArticleListPage : Page
    {
        IArticleListViewModel _viewModel;
        IAuthenticationService _authenticationService;
        IArticleValidateProvider _validateProvider;
        ILogger _logger;

        public ArticleListPage(IArticleListViewModel viewModel,
                               IAuthenticationService authenticationService,
                               IArticleValidateProvider validateProvider,
                               ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _authenticationService = authenticationService;
            _validateProvider = validateProvider;
            _logger = logger;
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

            _viewModel.LoadData();
            DataContext = _viewModel;
        }

        void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            OnAddArticle.Invoke(sender, e);
        }

        void DeleteArticleButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this article?", "", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    _viewModel.DeleteSelectedArticle();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void EditArticleTitleButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            {
                try
                {
                    _validateProvider.ValidateTitle(TitleTextBox.Text);
                }
                catch (ArgumentException ex)
                {
                    ShowMessage(ex.Message);
                    return;
                }

                _viewModel.UpdateArticle();
                HideMessage();
            }

            TitleTextBox.Style = TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleTitleButton.Background = EditArticleTitleButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        private void EditArticleBodyButton_Click(object sender, RoutedEventArgs e)
        {
            if (BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            {
                try
                {
                    _validateProvider.ValidateBody(BodyTextBox.Text);
                }
                catch (ArgumentException ex)
                {
                    ShowMessage(ex.Message);
                    return;
                }

                _viewModel.UpdateArticle();
                HideMessage();
            }

            BodyTextBox.Style = BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleBodyButton.Background = EditArticleBodyButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            _authenticationService.LogOut();

            OnLoggedOut.Invoke(sender, e);
        }

        private void ArticleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleListBox.SelectedItem != null)
                _viewModel.LoadArticle(((Article)ArticleListBox.SelectedItem).Id);
        }

        void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 0, 0, 20);
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