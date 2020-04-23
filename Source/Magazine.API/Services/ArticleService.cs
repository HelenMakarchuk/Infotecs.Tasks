using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Newtonsoft.Json;

namespace Infotecs.Magazine.API.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Статья".
    /// </summary>
    public class ArticleService
    {
        IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Создание статьи.
        /// </summary>
        /// <param name="article">Статья.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат статьи.</returns>
        public (Statuses status, string resultJson) Create(Article article)
        {
            var entry = _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
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
