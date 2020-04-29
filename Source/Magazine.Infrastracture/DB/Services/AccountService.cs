using Infotecs.Magazine.Domain.Providers;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Newtonsoft.Json;
using Serilog;
using System;

namespace Infotecs.Magazine.Infrastracture.DB.Services
{
    /// <summary>
    /// Сервис обработки операций CRUD для сущности "Аккаунт".
    /// </summary>
    public class AccountService
    {
        IUnitOfWork _unitOfWork;
        HashProvider _hashProvider;
        ILogger _logger;

        public AccountService(IUnitOfWork unitOfWork,
                              HashProvider hashProvider,
                              ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _hashProvider = hashProvider;
            _logger = logger;
        }

        /// <summary>
        /// Создание аккаунта.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат аккаунта.</returns>
        public (Statuses status, string resultJson) Create(Account account)
        {
            if (_unitOfWork.AccountRepository.SingleOrDefault(u => u.Login == account.Login) != null)
                return (Statuses.Error, null);

            account.Salt = _hashProvider.GetSalt();
            account.Password = _hashProvider.GetHash(account.Password, account.Salt);

            var entry = _unitOfWork.AccountRepository.Add(account);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        /// <summary>
        /// Обновление аккаунта.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат аккаунта.</returns>
        public (Statuses status, string resultJson) Update(Account account)
        {
            var entry = _unitOfWork.AccountRepository.Update(account);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        /// <summary>
        /// Удаление аккаунта.
        /// </summary>
        /// <param name="id">Идентификатор аккаунта.</param>
        /// <returns>Возврат статуса выполнения операции.</returns>
        /// <returns>Возврат аккаунта.</returns>
        public (Statuses status, string resultJson) Delete(int id)
        {
            var entry = _unitOfWork.AccountRepository.Remove(id);
            _unitOfWork.Commit();

            return (Statuses.Ok, JsonConvert.SerializeObject(entry.Entity));
        }

        public (Statuses status, string resultJson) Get(Account account)
        {
            var dbAccount = _unitOfWork.AccountRepository.SingleOrDefault(u => u.Login == account.Login);

            if (dbAccount == null)
                return (Statuses.Error, null);

            var password = _hashProvider.GetHash(account.Password, dbAccount.Salt);

            if (StringComparer.Ordinal.Compare(password, dbAccount.Password) != 0)
                return (Statuses.Error, null);

            return (Statuses.Ok, JsonConvert.SerializeObject(dbAccount));
        }
    }
}
