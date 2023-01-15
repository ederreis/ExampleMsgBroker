using BrokerEngine.Configurations;
using ExampleMsgBroker.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSubscriber().Receiver();

builder.Build().Run();