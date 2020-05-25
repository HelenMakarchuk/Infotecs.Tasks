using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Domain.Entities;
using System;
using System.Text.RegularExpressions;

namespace Infotecs.Magazine.Domain.Providers
{
    /// <summary>
    /// Валидатор сущности "Аккаунт".
    /// </summary>
    public class AccountValidateProvider : IValidateProvider<Account>
    {
        /// <summary>
        /// Валидация аккаунта.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        public void Validate(Account account)
        {
            ValidateLogin(account.Login);
            ValidatePassword(account.Password);
        }

        /// <summary>
        /// Валидация логина.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <exception cref="ArgumentException">
        /// Логин не указан.
        /// </exception>
        public void ValidateLogin(string login)
        {
            if (String.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login missed");
        }

        public void ValidatePassword(string password)
        {
            // Минимальная длина пароля
            var minLength = 6;

            // Максимальная длина пароля
            var maxLength = 15;

            // 0 или более символов предыдущего типа
            var zeroOrMore = "*";

            // 1 или более символов предыдущего типа
            var oneOrMore = "+";

            // Содержит 1 или более цифр
            var hasNumber = $@"(?=.{zeroOrMore}[0-9]{oneOrMore})";

            // Содержит 1 или более букв верхнего регистра
            var hasUpperChar = $@"(?=.{zeroOrMore}[A-Z]{oneOrMore})";

            // Содержит 1 или более букв нижнего регистра
            var hasLowerChar = $@"(?=.{zeroOrMore}[a-z]{oneOrMore})";

            // Количество символов от MinLength до MaxLength
            var hasRequiredCount = $@"(?=.{{{minLength},{maxLength}}})";

            // Содержит 1 или более специальных символов
            var hasSpecialCharacters = $@"(?=.{zeroOrMore}[ _\-+=<>|@#$%^&*(){{}}\[\].,:;!?]{oneOrMore})";

            var regex = new Regex(hasNumber + hasUpperChar + hasLowerChar + hasRequiredCount + hasSpecialCharacters);

            if (!regex.IsMatch(password))
                throw new ArgumentException($"Password must be {minLength}-{maxLength} characters long and contain at least one upper character, lower character, number, special character.");
        }
    }
}
