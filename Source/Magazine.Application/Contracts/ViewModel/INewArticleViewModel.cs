namespace Magazine.Application.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="NewArticleViewModel"/> для страницы создания новой статьи <see cref="NewArticlePage"/>.
    /// </summary>
    public interface INewArticleViewModel
    {
        public string Title { get; set; }
        public byte[] Teaser { get; set; }
        public string Body { get; set; }

        void CreateArticle();
        void SetTeaser();
    }
}
