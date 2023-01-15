using Newtonsoft.Json;

namespace BrokerEngine.Model
{
	public abstract class MessageEntity
	{
		protected MessageEntity(string messageBroadcasted) 
		{ 
			Id = Guid.NewGuid();

			MessageBroadcasted = messageBroadcasted;
		}

		protected MessageEntity(Guid id, string messageBroadcasted) : this(messageBroadcasted) =>	Id = id;

		[JsonProperty]
		public Guid Id { get; private set; }

		[JsonProperty]
		public string MessageBroadcasted { get; private set; }
	}
}
