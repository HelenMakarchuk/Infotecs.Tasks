namespace Magazine.API.ClientCommunicationService.Contracts
{
    /// <summary>
    /// Событие сервиса сущности.
    /// </summary>
    public abstract class EntityServiceEvent
    {
        /// <summary>
        /// Название класса сервиса.
        /// </summary>
        public string ClassName { get; set; }
    }
}
