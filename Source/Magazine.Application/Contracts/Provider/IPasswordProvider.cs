namespace Magazine.Application.Contracts.Provider
{
    public interface IHashProvider
    {
        string GetSalt();
        string GetHash(string password, bool addSalt = true);
        public string GetHash(string password, string salt);
    }
}
