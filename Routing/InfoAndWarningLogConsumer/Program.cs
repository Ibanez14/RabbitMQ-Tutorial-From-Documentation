using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace InfoAndWarningLogConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // will consume message with severity info and warning
            // so if publisher will send a message with info and warning severity our handler will take it
            ConsumeLog();
        }

        /// <summary>
        /// Consumes only log message with info & warning severity
        /// </summary>
        static void ConsumeLog()
        {
            var connectionFactory = new ConnectionFactory();

            using (IConnection connection = connectionFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                QueueDeclareOk queue = channel.QueueDeclare();

                channel.QueueBind(queue: queue.QueueName,
                                  exchange: "direct_logs",
                                  routingKey: "info");


                channel.QueueBind(queue: queue.QueueName,
                                  exchange: "direct_logs",
                                  routingKey: "warning");


                var eventingBasicConsumer = new EventingBasicConsumer(channel);

                eventingBasicConsumer.Received += (publisher, message) =>
                {
                      string data = Encoding.UTF8.GetString(message.Body);


                    if(message.RoutingKey == "info")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (message.RoutingKey == "warning")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                     Console.WriteLine(data);
                     Console.ForegroundColor = ConsoleColor.White;
                };

                channel.BasicConsume(queue.QueueName, autoAck: true, eventingBasicConsumer);

                Console.WriteLine("Press enter to exit");
                Console.Read();
            }

        }
    }
}
