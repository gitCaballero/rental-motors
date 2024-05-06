using RabbitMQ.Client;
using RentalMotor.Api.Models.Responses;
using RentalMotors.MessageBus;
using System.Text;
using System.Text.Json;

namespace RentalMotor.Api.Services.Network.MessageSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName = "localhost";
        private readonly string _password = "guest";
        private readonly string _userName = "guest";
        private IConnection _connection;

        public void SendMessage(IEnumerable<BaseMessage> messages, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _userName,
            };
            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            foreach (var message in messages)
            {
                byte[] body = GetMessageAssByteArray(message);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        private static byte[] GetMessageAssByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize((ResponseContractUserMotorModel)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}