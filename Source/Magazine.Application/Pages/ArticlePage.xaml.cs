using Magazine.Domain.Contracts.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    public partial class ArticlePage : Page
    {
        IArticleViewModel _viewModel;

        public ArticlePage(IArticleViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
        }

        void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void EditArticleButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void DeleteArticleButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}