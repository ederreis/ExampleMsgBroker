using BrokerEngine.ExtensionsMethods;
using BrokerEngine.Interfaces;
using BrokerEngine.Model;
using BrokerEngine.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BrokerEngine.Producer
{
	public class MessageBrokerProducer : RabbitMQBasic, IMessageBrokerProducer
	{
		public MessageBrokerProducer(IConfiguration configuration) : base(configuration) => Initialize();

		private void Initialize()
		{
			try
			{
				InitializeConnection();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				throw;
			}
		}

		public void Publish<TEntity>(TEntity entity) where TEntity : class
		{
			var entityAsString = JsonConvert.SerializeObject(entity);

			Publish(entityAsString);
		}

		public void Publish(string entityAsString)
		{
			var messageModel = new MessageModel(entityAsString);

			var messageModelAsString = JsonConvert.SerializeObject(messageModel);

			var entityBody = messageModelAsString.GetBytes();

			base.Channel.BasicPublish(exchange: string.Empty,
								routingKey: _brokerConfiguration.Queue.Name,
								basicProperties: null,
								body: entityBody);
		}
	}
}