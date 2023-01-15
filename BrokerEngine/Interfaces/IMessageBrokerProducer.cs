namespace BrokerEngine.Interfaces
{
    public interface IMessageBrokerProducer
    {
		public void Publish<TEntity>(TEntity entity) where TEntity : class;

		public void Publish(string entityAsString);
	}
}