using RabbitMQ.Client;
using System;
using System.Text;

namespace LogPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter a log message: ");
            var message = Console.ReadLine();
            Console.WriteLine("Please enter severity");
            var severity = Console.ReadLine();

            while (message != "exit")
            {
                SendLogMessage(message, severity);
                Console.Write("Please enter a log message: ");
                message = Console.ReadLine();
                Console.WriteLine("Please enter severity");
                severity = Console.ReadLine();
            }

            Console.WriteLine("Shutting down...");
        }


        // Assume that 'severity' can be one of 'info', 'warning', 'error'.
        static void SendLogMessage(string message, string severity)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";


            using (IConnection conn = connectionFactory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: severity,
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(message));

            }

        }
    }



}
