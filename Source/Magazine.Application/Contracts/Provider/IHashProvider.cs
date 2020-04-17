namespace Magazine.Application.Contracts.Provider
{
    /// <summary>
    /// Интерфейс генератора хеша.
    /// </summary>
    public interface IHashProvider
    {
        string GetSalt();
        string GetHash(string password, bool addSalt = true);
        public string GetHash(string password, string salt);
    }
}
