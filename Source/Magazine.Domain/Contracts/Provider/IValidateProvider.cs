using Infotecs.Magazine.Domain.Contracts.Entity;

namespace Infotecs.Magazine.Domain.Contracts.Provider
{
    /// <summary>
    /// Интерфейс валидатора сущности БД.
    /// </summary>
    public interface IValidateProvider<T> where T : IEntity
    {
        void Validate(T entity);
    }
}
