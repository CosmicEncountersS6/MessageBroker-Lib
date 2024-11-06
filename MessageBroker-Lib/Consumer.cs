using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace MessageBroker_Lib
{
    public class Consumer
    {
        private readonly IModel _channel;
        public Consumer(string host, Model.Queue queue)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = host };
            IConnection connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: queue.QueueName, durable: queue.IsDurable, exclusive: queue.IsExclusive, autoDelete: queue.ShouldAutoDelete, arguments: queue.Arguments);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                string jsonString = ea.Body.ToString();
                Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                if (queue.Callback == null)
                    throw new ArgumentNullException(nameof(queue.Callback));

                queue.Callback.Invoke(json, _channel.CurrentQueue);
            };

            _channel.BasicConsume(queue: queue.QueueName, autoAck: true, consumer: consumer);
        }
    }
}
