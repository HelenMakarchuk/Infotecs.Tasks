using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using System;

namespace Magazine.Application.Services
{
    class AuthenticationService : IAuthenticationService
    {
        IHashProvider _hashProvider;
        IUnitOfWork _unitOfWork;
        User _user;

        public AuthenticationService(IUnitOfWork unitOfWork,
                                     IHashProvider hashProvider)
        {
            _unitOfWork = unitOfWork;
            _hashProvider = hashProvider;
        }

        public bool IsLoggedIn => _user != null;

        public bool TrySignUp(string login, string password)
        {
            if (_unitOfWork.UserRepository.FirstOrDefault(u => u.Login == login) != null)
                return false;

            var user = new User();
            user.Login = login;
            user.Salt = _hashProvider.GetSalt();
            user.Password = _hashProvider.GetHash(password, user.Salt);

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Commit();
            return true;
        }

        public bool TryLogIn(string login, string password)
        {
            var user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Login == login);

            if (user == null)
                return false;

            var dbHash = user.Password;
            var currentHash = _hashProvider.GetHash(password, user.Salt);

            if (StringComparer.Ordinal.Compare(currentHash, user.Password) != 0)
                return false;

            _user = user;
            return true;
        }

        public void LogOut()
        {
            _user = null;
        }
    }
}
