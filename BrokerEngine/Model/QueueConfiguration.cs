using BrokerEngine.Interfaces;

namespace BrokerEngine.Model
{
	public class QueueConfiguration : IQueueConfiguration
	{
		public QueueConfiguration() { }

		public QueueConfiguration(string name) => Name = name;

		public string Name { get; private set; } = null!;
	}
}