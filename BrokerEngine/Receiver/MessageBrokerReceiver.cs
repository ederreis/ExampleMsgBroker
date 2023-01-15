using BrokerEngine.ExtensionsMethods;
using BrokerEngine.Interfaces;
using BrokerEngine.Model;
using BrokerEngine.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using NotBase = RabbitMQ.Client.Events;

namespace BrokerEngine.Receiver
{
	public class MessageBrokerReceiver : RabbitMQBasic, IMessageBrokerReceiver
	{
		public MessageBrokerReceiver(IConfiguration configuration) : base(configuration) { }

		public void Initialize()
		{
			try
			{
				InitializeConnection();

				Listener();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				throw;
			}
		}

		public void DisposeBroker() => Dispose();

		private void Listener()
		{
			base.Channel.BasicQos(0, 3, false);

			var consumer = new NotBase.AsyncEventingBasicConsumer(base.Channel);

			consumer.Received += Consumer_Received;

			base.Channel.BasicConsume(queue: _brokerConfiguration.Queue.Name,
								autoAck: Ack.AutoAck,
								consumer: consumer);
		}

		private Task Consumer_Received(object sender, NotBase.BasicDeliverEventArgs @event)
		{
			var body = @event.Body.ToArray();

			var notificationMessage = body.GetString();

			var messageBody = notificationMessage.ToMessageReceiver(@event.DeliveryTag);

			OnMessageReceived(messageBody);

			return Task.CompletedTask;
		}

		public void Acknowledge(MessageReceiver messageModel)
		{
			if (Ack.AutoAck)
				throw new InvalidOperationException($"--> You can't acknowledge or not acknowledge the message because the variable {nameof(Ack.AutoAck)} is set to true");

			Channel.BasicAck(messageModel.DeliveryTag, Ack.MultipleAck);
		}

		public void NotAcknowledge(MessageReceiver messageModel)
		{
			if (Ack.AutoAck)
				throw new InvalidOperationException($"--> You can't acknowledge or not acknowledge the message because the variable {nameof(Ack.AutoAck)} is set to true");

			base.Channel.BasicNack(messageModel.DeliveryTag, Ack.MultipleAck, Ack.ReQueue);
		}

		public event EventHandler<MessageReceiver>? MessageReceived;

		protected virtual void OnMessageReceived(MessageReceiver message)
			=> MessageReceived?.Invoke(this, message);
	}
}
