using BrokerEngine.Configurations;

namespace ExampleMsgBroker.AsyncDataServices
{
	public static class DependencyInjectionBroker
	{
		public static IServiceCollection ConfigureSubscriber(this IServiceCollection services) => services.AddHostedService<MessageBusSubscriber>();

	}
}
