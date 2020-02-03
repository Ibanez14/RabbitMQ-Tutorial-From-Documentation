using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbiqMQ
{
    class Sender
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter a message: ");
            var message = Console.ReadLine();

            while(message != "exit")
            {
                message += $" NO:{Guid.NewGuid().ToString()}";
                Tutorial2_SendMessageToConsumer("queue.tasks.5", message);

                Console.Write("Please enter a message: ");
                message = Console.ReadLine();
            }

            Console.WriteLine("Shutting down...");
        }



        /// <summary>
        /// Simple sending message to one queue. Tutorial 1 "Hello world" from rabbitmq docs
        /// When you call this method ensure you call the same Tutorial number method in Consumer console app
        /// </summary>
        public static void Tutorial2_SendMessageToConsumer(string queueName, string message)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";

            using (IConnection connection = connectionFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // queue will be created if doesn't exist
                channel.QueueDeclare(queue: queueName,
                                   durable: false, // if true - queue will be recovered if Rabbit restarts, 
                                                   // but message won't, messages will if message.Persisten: true (see below)
                                 exclusive: false,
                                autoDelete: false,
                                 arguments: null);

                var basicProps = channel.CreateBasicProperties();
                basicProps.Persistent = true; // if this true, means that messages will be persistent in disk
                // and in case rabbimq server suddenly stops, after restart all message will be recovered
                // BUT it will work only if queue and exchange are durable:true
                
                // By default message will be sent to the next consumer
                // this is called round-robbin

                channel.BasicPublish(exchange: "",
                                    routingKey: queueName, // if queue is not declared, exception may thrown
                                    basicProperties: basicProps /*null*/,  // also can set null, not necessary to set basicProperties
                                    body: Encoding.UTF8.GetBytes(message));

                Console.WriteLine("Message is sent");
            }
        }
    }
}
