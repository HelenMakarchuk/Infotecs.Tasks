namespace Magazine.Domain.Contracts.Provider
{
    /// <summary>
    /// Интерфейс валидатора сущности "Статья" <see cref="ArticleValidateProvider"/>.
    /// </summary>
    public interface IArticleValidateProvider
    {
        void ValidateBody(string body);
        void ValidateTitle(string title);
    }
}
