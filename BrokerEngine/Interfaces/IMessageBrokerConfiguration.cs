using BrokerEngine.Model;

namespace BrokerEngine.Interfaces
{
    public interface IMessageBrokerConfiguration
    {
        public EndPointConnection EndPointConnection { get; }

        public Login Login { get; }

        public MessageConfiguration Message { get; }

        public QueueConfiguration Queue { get; }
    }

    public interface IEndPointConnection
    {
		public string Host { get; }

		public int Port { get; }

		public string VirtualHost { get; }
	}

	public interface ILogin
	{
		public string UserName { get; }

		public string Password { get; }
	}

    public interface IMessageConfiguration
	{
        public bool AutoAck { get; }
    }

    public interface IQueueConfiguration
    {
        public string Name { get; }
    }
}
