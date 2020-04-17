using Autofac;
using Core.DI;
using Magazine.Application.Contracts.Service;
using Magazine.Tests.DI;
using Xunit;

namespace Magazine.Tests.Services
{
	/// <summary>
	/// Тест сервиса аутентификации <see cref="AuthenticationService"/>
	/// </summary>
	public class AuthenticationServiceTest
	{
		IContainer _container;
		IAuthenticationService _authenticationService;

		public AuthenticationServiceTest()
		{
			_container = AutofacConfig.Configure(new TestModule());
			_authenticationService = _container.Resolve<IAuthenticationService>();
		}

		[Theory]
		[InlineData("SameLogin", "SameLogin", false)]
		[InlineData("SomeLogin", "AnotherLogin", true)]
		public void TrySignUp_Theory(string existedLogin, string newLogin, bool expected)
		{
			using (var scope = _container.BeginLifetimeScope())
			{
				// Arrange
				var firstSignUp = _authenticationService.TrySignUp(existedLogin, "SomePassword");

				// Act
				var secondSignUp = _authenticationService.TrySignUp(newLogin, "AnotherPassword");

				// Assert
				Assert.True(firstSignUp);
				Assert.Equal(expected, secondSignUp);
			}
		}
	}
}
