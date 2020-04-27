using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Linq;

namespace Infotecs.Magazine.API.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService
    {
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public ArticleService(IUnitOfWork unitOfWork,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка статей.
        /// </summary>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат списка статей.</returns>
        public (Statuses status, string resultJson) Get()
        {
            var articles = _unitOfWork.ArticleRepository.Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();

            return (Statuses.Ok, JsonConvert.SerializeObject(articles));
        }

        /// <summary>
        /// Получение статьи по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор статьи.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат статьи.</returns>
        public (Statuses status, string resultJson) GetById(int id)
        {
            var article = _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.Account).SingleOrDefault(a => a.Id == id);

            return (Statuses.Ok, JsonConvert.SerializeObject(article, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize }));
        }

        /// <summary>
        /// Создание статьи.
        /// </summary>
        /// <param name="article">Статья.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат статьи.</returns>
        public (Statuses status, string resultJson) Create(Article article)
        {
            var dbArticle = _unitOfWork.ArticleRepository.SingleOrDefault(a => a.Title == article.Title);

            if (dbArticle != null)
                return (Statuses.Error, null);

            var entry = _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize }));
        }

        /// <summary>
        /// Обновление статьи.
        /// </summary>
        /// <param name="article">Статья.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат статьи.</returns>
        public (Statuses status, string resultJson) Update(Article article)
        {
            var entry = _unitOfWork.ArticleRepository.Update(article);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        /// <summary>
        /// Удаление статьи.
        /// </summary>
        /// <param name="id">Идентификатор статьи.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат статьи.</returns>
        public (Statuses status, string resultJson) Delete(int id)
        {
            var entry = _unitOfWork.ArticleRepository.Remove(id);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }
    }
}
