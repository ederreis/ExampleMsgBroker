using BrokerEngine.ExtensionsMethods;
using BrokerEngine.Interfaces;
using BrokerEngine.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BrokerEngine.RabbitMQ
{
	public abstract class RabbitMQBasic : IDisposable
	{
		public readonly IMessageBrokerConfiguration _brokerConfiguration;

		private ConnectionFactory? _factory;

		private readonly object _lockObject = new object();

		private IConnection? _connection;

		internal IModel Channel = null!;

		internal readonly MessageAcknowledge Ack;

		private bool WatcherConnectionHealthAlreadySetUp { get; set; } = false;

		public bool Disposed { get; private set; } = false;

		private bool IsConnected => _connection != null && _connection.IsOpen && !Disposed;

		protected RabbitMQBasic(IConfiguration configuration)
		{
			_brokerConfiguration = configuration.GetSection("MessageBrokerConfiguration").Get<MessageBrokerConfiguration>(options => options.BindNonPublicProperties = true)!;

			Ack = new MessageAcknowledge(_brokerConfiguration.Message);
		}

		#region Connection Area

		private void WatchConnectionHealth()
		{
			_connection!.ConnectionShutdown += ConnectionShoutDown;

			_connection!.CallbackException += CallbackExeption;

			_connection!.ConnectionBlocked += ConnectionBlocked;
		}

		private bool TryConnect()
		{
			lock (_lockObject)
			{
				if (IsConnected)
					return true;

				_connection = _factory?.CreateConnection();

				if (!WatcherConnectionHealthAlreadySetUp)
				{
					WatcherConnectionHealthAlreadySetUp = true;

					WatchConnectionHealth();
				}
			}

			return IsConnected;
		}

		public void InitializeConnection()
		{
			_factory = new ConnectionFactory()
			{
				HostName = _brokerConfiguration.EndPointConnection.Host,
				Port = _brokerConfiguration.EndPointConnection.Port,
				VirtualHost = _brokerConfiguration.EndPointConnection.VirtualHost,
				UserName = _brokerConfiguration.Login.UserName,
				Password = _brokerConfiguration.Login.Password,
				DispatchConsumersAsync = true
			};

			if (!TryConnect())
			{
				var exception = new Exception($"It was not possible to connect to {_brokerConfiguration.EndPointConnection.Host}:{_brokerConfiguration.EndPointConnection.Port} under the Virtual host '{_brokerConfiguration.EndPointConnection.VirtualHost}");

				throw exception;
			}

			Channel = _connection!.CreateModel();

			Channel.QueueDeclare(queue: _brokerConfiguration.Queue.Name,
									durable: true,
									exclusive: false,
									autoDelete: false,
									arguments: _brokerConfiguration.Queue.ToRabbitMqArguments());
		}

		#endregion
		
		#region Disposable Area

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (Disposed)
				return;

			Disposed = true;

			try
			{
				_connection!.ConnectionShutdown -= ConnectionShoutDown;

				_connection!.CallbackException -= CallbackExeption;

				_connection!.ConnectionBlocked -= ConnectionBlocked;

				if (Channel!.IsOpen)
					Channel!.Close();

				if (_connection!.IsOpen)
					_connection!.Close();

				Channel!.Dispose();

				_connection!.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Problem to dispose connection {ex.Message}.");
			}
		}

		#endregion

		#region Events Area

		private void ReconectScheme()
		{
			if (Disposed)
				return;

			TryConnect();
		}

		private void ConnectionShoutDown(object? sender, ShutdownEventArgs e) => ReconectScheme();

		private void CallbackExeption(object? sender, CallbackExceptionEventArgs e) => ReconectScheme();

		private void ConnectionBlocked(object? sender, ConnectionBlockedEventArgs e) => ReconectScheme();

		#endregion

		public struct MessageAcknowledge
		{
			public MessageAcknowledge(IMessageConfiguration messageConfiguration)
			{
				AutoAck = messageConfiguration.AutoAck;

				MultipleAck = false;

				ReQueue = true;
			}

			public bool AutoAck { get; }

			public bool MultipleAck { get; }

			public bool ReQueue { get; }
		}
	}
}