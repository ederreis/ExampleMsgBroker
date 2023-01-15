using BrokerEngine.Interfaces;

namespace BrokerEngine.Model
{
	public class MessageBrokerConfiguration : IMessageBrokerConfiguration
	{
		public MessageBrokerConfiguration() { }

		public EndPointConnection EndPointConnection { get; private set; } = null!;

		public Login Login { get; private set; } = null!;

		public MessageConfiguration Message { get; private set; } = null!;

		public QueueConfiguration Queue { get; private set; } = null!;
	}
}