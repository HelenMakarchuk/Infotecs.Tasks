using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Newtonsoft.Json;
using Serilog;

namespace Infotecs.Magazine.API.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Комментарий".
    /// </summary>
    public class CommentService
    {
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public CommentService(IUnitOfWork unitOfWork,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Создание комментария.
        /// </summary>
        /// <param name="comment">Комментарий.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат комментария.</returns>
        public (Statuses status, string resultJson) Create(Comment comment)
        {
            var entry = _unitOfWork.CommentRepository.Add(comment);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        /// <summary>
        /// Обновление комментария.
        /// </summary>
        /// <param name="comment">Комментарий.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат комментария.</returns>
        public (Statuses status, string resultJson) Update(Comment comment)
        {
            var entry = _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        /// <summary>
        /// Удаление комментария.
        /// </summary>
        /// <param name="id">Идентификатор комментария.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат комментария.</returns>
        public (Statuses status, string resultJson) Delete(int id)
        {
            var entry = _unitOfWork.CommentRepository.Remove(id);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }
    }
}
