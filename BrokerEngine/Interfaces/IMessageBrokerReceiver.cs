using BrokerEngine.Model;

namespace BrokerEngine.Interfaces
{
	public interface IMessageBrokerReceiver
	{
		public void Initialize();

		public void DisposeBroker();

		public event EventHandler<MessageReceiver>? MessageReceived;

		public void Acknowledge(MessageReceiver messageModel);

		public void NotAcknowledge(MessageReceiver messageModel);
	}
}
