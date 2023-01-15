namespace BrokerEngine.Model
{
	public class MessageReceiver : MessageEntity
	{
		public MessageReceiver(MessageEntity messageEntity, ulong deliveryTag) : base(messageEntity.Id, messageEntity.MessageBroadcasted)
			=> DeliveryTag = deliveryTag;

		public ulong DeliveryTag { get; }
	}
}
