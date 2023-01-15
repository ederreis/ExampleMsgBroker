using BrokerEngine.Interfaces;

namespace BrokerEngine.Model
{
	public class MessageConfiguration : IMessageConfiguration
	{
		public MessageConfiguration() { }

		public MessageConfiguration(bool autoAck) => AutoAck = autoAck;

		public bool AutoAck { get; private set; }
	}
}