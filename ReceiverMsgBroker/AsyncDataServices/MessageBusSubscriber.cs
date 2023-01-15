using BrokerEngine.Interfaces;
using BrokerEngine.Model;

namespace ExampleMsgBroker.AsyncDataServices
{
	public class MessageBusSubscriber : BackgroundService
	{
		private readonly IMessageBrokerReceiver _receiver;

		public MessageBusSubscriber(IMessageBrokerReceiver receiver) => _receiver = receiver;

		private void Setup()
			=> _receiver.MessageReceived += Receiver_MessageReceived;

		private void Receiver_MessageReceived(object? sender, MessageReceiver e) => Console.WriteLine(e.MessageBroadcasted);

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_receiver.Initialize();

			return base.StartAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			Setup();

			return Task.CompletedTask;
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			_receiver.DisposeBroker();

			return base.StopAsync(cancellationToken);
		}
	}
}