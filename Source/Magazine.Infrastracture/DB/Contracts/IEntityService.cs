using Infotecs.Magazine.Domain.Contracts.Entity;
using System.Linq;

namespace Infotecs.Magazine.Infrastracture.Contracts
{
    public interface IEntityService<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Получение списка сущностей.
        /// </summary>
        /// <returns>Возврат списка сущностей.</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Получение сущности по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Возврат сущности.</returns>
        TEntity Get(int id);

        /// <summary>
        /// Создание сущности.
        /// </summary>
        /// <param name="entity">Cущность.</param>
        /// <returns>Возврат сущности.</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Обновление сущности.
        /// </summary>
        /// <param name="entity">Cущность.</param>
        /// <returns>Возврат сущности.</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Удаление сущности.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Возврат сущности.</returns>
        TEntity Delete(int id);
    }
}
