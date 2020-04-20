using Infotecs.Magazine.Application.Contracts.Page;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница отображает список статей и контент выбранной статьи.
    /// </summary>
    public partial class ArticleListPage : Page, IPage
    {
        IArticleListViewModel _viewModel;

        public ArticleListPage(IArticleListViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        /// <summary>
        /// Событие создания новой статьи.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnAddArticle;

        /// <summary>
        /// Событие выхода текущего пользователя из приложения и перехода на страницу аутентификации.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnLogOut;

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события перехода на страницу создания новой статьи.
        /// </summary>
        void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            OnAddArticle.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события удаления текущей статьи.
        /// </summary>
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

        /// <summary>
        /// Обработчик события редактирования заголовка статьи.
        /// </summary>
        void EditArticleTitleButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            {
                try
                {
                    _viewModel.UpdateArticle();
                }
                catch (ArgumentException ex)
                {
                    ShowMessage(ex.Message);
                    return;
                }

                HideMessage();
            }

            TitleTextBox.Style = TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleTitleButton.Background = EditArticleTitleButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        /// <summary>
        /// Обработчик события редактирования картинки-тизера статьи.
        /// </summary>
        private void EditArticleTeaserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SetTeaser();
                _viewModel.UpdateArticle();
            }
            catch (ArgumentException ex)
            {
                ShowMessage(ex.Message);
                return;
            }

            HideMessage();
        }

        /// <summary>
        /// Обработчик события редактирования контента статьи.
        /// </summary>
        void EditArticleBodyButton_Click(object sender, RoutedEventArgs e)
        {
            if (BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            {
                try
                {
                    _viewModel.UpdateArticle();
                }
                catch (ArgumentException ex)
                {
                    ShowMessage(ex.Message);
                    return;
                }

                HideMessage();
            }

            BodyTextBox.Style = BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            EditArticleBodyButton.Background = EditArticleBodyButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        /// <summary>
        /// Обработчик события выхода текущего пользователя из приложения и перехода на страницу аутентификации.
        /// </summary>
        void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogOut.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события выбора статьи из списка статей.
        /// </summary>
        void ArticleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleListBox.SelectedItem != null)
                _viewModel.LoadArticle(((Article)ArticleListBox.SelectedItem).Id);
        }

        public void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 0, 0, 20);
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
            _viewModel.LoadData();
            HideMessage();
        }
    }
}