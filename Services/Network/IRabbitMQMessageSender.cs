using RentalMotors.MessageBus;

namespace RentalMotor.Api.Services.Network
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(IEnumerable<BaseMessage> messages, string queueName);
    }
}
