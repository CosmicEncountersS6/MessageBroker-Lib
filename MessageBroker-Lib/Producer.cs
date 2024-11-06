using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace MessageBroker_Lib
{
    public class Producer
    {        
        private readonly IModel _channel;
        public Producer(string host, Model.Queue queue)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = host };
            IConnection connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: queue.QueueName, durable: queue.IsDurable, exclusive: queue.IsExclusive, autoDelete: queue.ShouldAutoDelete, arguments: queue.Arguments);
        }

        public void Send(Dictionary<string, object> json)
        {
            string jsonString = JsonSerializer.Serialize(json);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);

            _channel.BasicPublish(exchange: string.Empty, routingKey: string.Empty, basicProperties: null, body: body);
        }
    }
}

