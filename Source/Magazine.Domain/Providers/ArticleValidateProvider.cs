using Magazine.Domain.Contracts.Provider;
using Serilog;
using System;

namespace Magazine.Domain.Providers
{
    public class ArticleValidateProvider : IArticleValidateProvider
    {
        ILogger _logger;

        public ArticleValidateProvider(ILogger logger)
        {
            _logger = logger;
        }

        public void ValidateBody(string body)
        {
            if (body.Length < 2000)
                throw new ArgumentException("Body must be at least 2000 characters");

            if (body.Length > 60000)
                throw new ArgumentException("Body maximum length exceeded");
        }

        public void ValidateTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is missing");

            if (title.Length > 80)
                throw new ArgumentException("Title maximum length exceeded");
        }
    }
}
