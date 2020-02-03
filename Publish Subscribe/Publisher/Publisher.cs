using System.Text;
using System.Security.Authentication.ExtendedProtection;
using System;
using RabbitMQ.Client;

namespace Publisher
{
    class Publisher
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter a message: ");
            var message = Console.ReadLine();

            while (message != "exit")
            {
                SendMessage(message);

                Console.Write("Please enter a message: ");
                message = Console.ReadLine();
            }

            Console.WriteLine("Shutting down...");
        }


        public static void SendMessage(string message)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";

            using (IConnection connection = connectionFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // Exchange is fanout
                // so it will broadcast all messages it receives
                // to all queues it knows
                channel.ExchangeDeclare(exchange: "Logs", ExchangeType.Fanout);


                channel.BasicPublish(exchange: "Logs", 
                                      routingKey: "",
                                     basicProperties: null, 
                                     body: Encoding.ASCII.GetBytes(message));

                // Write in a conolse message we published
                Console.WriteLine(string.Concat(message, " ", "(Message has been publisher)"));
            }
        }
    }
}
