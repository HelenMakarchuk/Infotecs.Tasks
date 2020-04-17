namespace Magazine.Application.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="NewArticleViewModel"/> для страницы создания новой статьи <see cref="NewArticlePage"/>.
    /// </summary>
    public interface INewArticleViewModel
    {
        void Save(string title, string body, int userId, byte[] teaser = null);
    }
}
