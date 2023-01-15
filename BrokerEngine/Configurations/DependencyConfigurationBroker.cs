using BrokerEngine.Interfaces;
using BrokerEngine.Model;
using BrokerEngine.Producer;
using BrokerEngine.Receiver;
using Microsoft.Extensions.DependencyInjection;

namespace BrokerEngine.Configurations
{
	public static class DependencyConfigurationBroker
	{
		public static void Receiver(this IServiceCollection services)
		{
			services.AddSingleton<IMessageBrokerConfiguration, MessageBrokerConfiguration>();

			services.AddSingleton<IMessageBrokerReceiver, MessageBrokerReceiver>();
		}

		public static void ConfigureProducerScoped(this IServiceCollection services) 
		{
			services.AddScoped<IMessageBrokerProducer, MessageBrokerProducer>();

			services.AddScoped<IMessageBrokerConfiguration, MessageBrokerConfiguration>();
		}
		
		public static void ConfigureProducerTransient(this IServiceCollection services) 
		{
			services.AddTransient<IMessageBrokerProducer, MessageBrokerProducer>();

			services.AddTransient<IMessageBrokerConfiguration, MessageBrokerConfiguration>();
		}
	}
}
