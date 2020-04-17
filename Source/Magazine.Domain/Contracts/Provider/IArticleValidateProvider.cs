namespace Magazine.Domain.Contracts.Provider
{
    public interface IArticleValidateProvider
    {
        void ValidateBody(string body);
        void ValidateTitle(string title);
    }
}
