using BrokerEngine.Interfaces;

namespace BrokerEngine.Model
{
	public class Login : ILogin
	{
		public Login() { }

		public Login(string userName, string password)
		{
			UserName = userName;

			Password = password;
		}

		public string UserName { get; private set; } = null!;

		public string Password { get; private set; } = null!;
	}
}