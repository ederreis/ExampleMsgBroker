using BrokerEngine.Interfaces;

namespace BrokerEngine.Model
{
	public class EndPointConnection : IEndPointConnection
	{
		public EndPointConnection() { }

		public EndPointConnection(string host, int port, string virtualHost)
		{
			Host = host;

			Port = port;

			VirtualHost = virtualHost;
		}

		public string Host { get; private set; } = null!;

		public int Port { get; private set; } = 5672;

		public string VirtualHost { get; private set; } = null!;
	}
}