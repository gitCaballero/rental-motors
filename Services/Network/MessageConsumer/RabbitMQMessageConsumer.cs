using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalMotor.Api.Models.Responses;
using System.Text;
using System.Text.Json;

namespace RentalMotor.Api.Services.Network.MessageConsumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQMessageConsumer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "rentalmotorqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_chanel, evt) => 
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                var response = JsonSerializer.Deserialize<ResponseContractUserMotorModel>(content);
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("rentalmotorqueue", false, consumer);
            return Task.CompletedTask;
        }
    }
}
