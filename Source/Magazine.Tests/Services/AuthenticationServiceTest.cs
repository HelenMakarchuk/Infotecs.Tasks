using Autofac;
using Infotecs.Magazine.Tests;
using Magazine.Application.Contracts.Service;
using System;
using Xunit;

namespace Magazine.Tests.Services
{
    /// <summary>
    /// Тест сервиса аутентификации <see cref="AuthenticationService"/>.
    /// </summary>
    public class AuthenticationServiceTest : IClassFixture<TestFixture>, IDisposable
    {
        TestFixture _testFixture;
        ILifetimeScope _containerScope;
        IAuthenticationService _authenticationService;

        /// <summary>
        /// Выполнение перед каждым тестом.
        /// </summary>
        public AuthenticationServiceTest(TestFixture testFixture)
        {
            _testFixture = testFixture;
            _containerScope = _testFixture.Container.BeginLifetimeScope();
            _authenticationService = _containerScope.Resolve<IAuthenticationService>();
        }

        /// <summary>
        /// Выполнение после каждого теста.
        /// </summary>
        public void Dispose()
        {
            _containerScope.Dispose();
        }

        /// <summary>
        /// Возврат False при попытке регистрации нового пользователя с существующим логином.
        /// </summary>
        /// <param name="login">Логин.</param>
        [Theory]
        [InlineData("login")]
        [InlineData("login@infotecs.com")]
        public void TrySignUp_LoginExists_ReturnFalse(string login)
        {
            // Arrange
            var firstSignUp = _authenticationService.TrySignUp(login, "SomePassword");

            // Act
            var secondSignUp = _authenticationService.TrySignUp(login, "AnotherPassword");

            // Assert
            Assert.True(firstSignUp);
            Assert.False(secondSignUp);
        }

        /// <summary>
        /// Возврат False при аутентификации с некорректным логином.
        /// </summary>
        /// <param name="signUpLogin">Логин при регистрации.</param>
        /// <param name="logInLogin">Логин при аутентификации.</param>
        [Theory]
        [InlineData("login", "logIn")]
        [InlineData("login", " login")]
        [InlineData("SomeLogin", "AnotherLogin")]
        [InlineData("login_à", "login__a")]
        public void TryLogIn_LoginDoesntMatch_ReturnFalse(string signUpLogin, string logInLogin)
        {
            // Arrange
            var signUpResult = _authenticationService.TrySignUp(signUpLogin, "password");

            // Act
            var logInResult = _authenticationService.TryLogIn(logInLogin, "password");

            // Assert
            Assert.True(signUpResult);
            Assert.False(logInResult);
        }

        /// <summary>
        /// Возврат False при аутентификации с некорректным паролем.
        /// </summary>
        /// <param name="signUpPassword">Пароль при регистрации.</param>
        /// <param name="logInPassword">Пароль при аутентификации.</param>
        [Theory]
        [InlineData("password", "passWord")]
        [InlineData("password", " password")]
        [InlineData("some password", "another password")]
        [InlineData("*_à", "*_a")]
        public void TryLogIn_PasswordDoesntMatch_ReturnFalse(string signUpPassword, string logInPassword)
        {
            // Arrange
            var signUpResult = _authenticationService.TrySignUp("login", signUpPassword);

            // Act
            var logInResult = _authenticationService.TryLogIn("login", logInPassword);

            // Assert
            Assert.True(signUpResult);
            Assert.False(logInResult);
        }

        /// <summary>
        /// Возврат True при аутентификации с корректным логином и паролем.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        [Theory]
        [InlineData("login", "password")]
        [InlineData("login@infotecs.com", "*_(^%dsfsdf#$ghj@!")]
        public void TryLogIn_CorrectLoginAndPassword_ReturnTrue(string login, string password)
        {
            // Arrange
            var signUpResult = _authenticationService.TrySignUp(login, password);

            // Act
            var logInResult = _authenticationService.TryLogIn(login, password);

            // Assert
            Assert.True(signUpResult);
            Assert.True(logInResult);
        }
    }
}
