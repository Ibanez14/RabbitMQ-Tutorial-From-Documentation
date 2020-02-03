using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer start to listen...");
            Consume();
        }


        public static void Consume()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";

            using (IConnection conn = connectionFactory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                // 1 => Declare a queue
                // New Fresh queue will be created with generated name
                // when we supply no parameters to QueueDeclare() 
                // we create a non-durable, exclusive, autodelete queue with a generated name:
                var queueName = channel.QueueDeclare().QueueName;

                // 2 => Bind a queue to exchange
                // That relationship between exchange and a queue is called a binding.
                // We say to exchange to send messages to our queue via queue binding
                channel.QueueBind(queue: queueName, exchange: "Logs", routingKey: "");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (publisher, data) =>
                {
                    string message  = Encoding.UTF8.GetString(data.Body);
                    Console.WriteLine($"Message consumed: {message} at {DateTime.Now}");
                };

                // and Remember!
                // queueu will be deleted after you disconnect
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine("Press enter to exit");
                Console.Read();
            }
        }
    }
}
