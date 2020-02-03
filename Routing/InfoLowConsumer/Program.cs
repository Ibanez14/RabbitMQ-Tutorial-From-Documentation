using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace InfoLowConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting to listen ...");

            // this connection will consume only info severity logs
            // howere LogPublihser can publish message with different severites like info, warning, error and whatever
            ConsumeMessage();
        }

        static void ConsumeMessage()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                  exchange: "direct_logs",
                                  routingKey: "info");

                var eventingBasicConsumer = new EventingBasicConsumer(channel);

                eventingBasicConsumer.Received += (sender, message) =>
                {
                     Console.ForegroundColor = ConsoleColor.Green;
                     Console.WriteLine(Encoding.UTF8.GetString(message.Body));
                     Console.ForegroundColor = ConsoleColor.White;
                };

                channel.BasicConsume(queueName, autoAck: true, eventingBasicConsumer);
                Console.WriteLine("Please press enter to exit");
                System.Console.ReadLine();
            }
        }
    }
}
