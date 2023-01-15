using BrokerEngine.Interfaces;
using BrokerEngine.Model;
using Newtonsoft.Json;
using System.Text;

namespace BrokerEngine.ExtensionsMethods
{
	internal static class EncoderExtensions
	{
		private static Encoding Encoder => Encoding.UTF8;

		internal static byte[] GetBytes(this string value) => Encoder.GetBytes(value);

		internal static string GetString(this byte[] value) => Encoder.GetString(value);

		internal static Dictionary<string, object> ToRabbitMqArguments(this IQueueConfiguration configuration)
			=> new Dictionary<string, object> 
			{ 
				{ 
					"x-dead-letter-exchange", "DeadLetters" 
				}, 
				{ 
					"x-dead-letter-routing-key", configuration.Name 
				} 
			};

		internal static MessageReceiver ToMessageReceiver(this string stringAsEntity, ulong deliveryTag)
		{
			var messageEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(stringAsEntity , new JsonSerializerSettings()
			{
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			});

			if (messageEntity == null)
				throw new FieldAccessException();

			return new MessageReceiver(messageEntity, deliveryTag);
		}
	}
}
