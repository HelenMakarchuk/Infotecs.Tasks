using Infotecs.Magazine.Application.Contracts.Page;
using Magazine.Application.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница создания новой статьи.
    /// </summary>
    public partial class NewArticlePage : Page, IPage
    {
        INewArticleViewModel _viewModel;

        public NewArticlePage(INewArticleViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        /// <summary>
        /// Событие закрытия страницы.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnClosed;

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события отмены создания новой статьи.
        /// </summary>
        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnClosed.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события сохранения новой статьи.
        /// </summary>
        void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.CreateArticle();
            }
            catch (ArgumentException ex)
            {
                ShowMessage(ex.Message);
                return;
            }

            HideMessage();

            OnClosed.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события выбора картинки-тизера для статьи.
        /// </summary>
        void ChooseTeaserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SetTeaser();
            }
            catch (ArgumentNullException ex)
            {
                ShowMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Отображение сообщения пользователю.
        /// </summary>
        public void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 5, 0, 5);
        }

        /// <summary>
        /// Скрытие сообщения.
        /// </summary>
        public void HideMessage()
        {
            MessageBlock.Text = "";
            MessageBlock.Height = 0;
            MessageBlockBorder.Visibility = Visibility.Hidden;
            MessageBlockBorder.Margin = new Thickness(0);
        }

        public void SetData()
        {
            _viewModel.Title = String.Empty;
            _viewModel.Teaser = null;
            _viewModel.Body = String.Empty;
            HideMessage();
        }
    }
}
