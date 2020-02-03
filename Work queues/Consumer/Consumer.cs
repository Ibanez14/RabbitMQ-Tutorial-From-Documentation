using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            ConsumeMessageFromQueue("queue.tasks.5");
        }


        /// <summary>
        /// Code from Tutorial 1 "Hellow world" and Tutorial 2 in rabbimq docs
        /// </summary>
        /// <param name="queueName"></param>
        public static void ConsumeMessageFromQueue(string queueName)
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,// if true - queue will be recovered if Rabbit restarts, 
                                                    // but message won't, messages will if message.Persisten: true (see below)
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // this say to RabbiqMQ not to give more that one message
                // to the consumer until it ackowledge the message
                // so now, next message will be sent only when previoius message ware ackowledged
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, eventArgs) =>
                {
                    byte[] body = eventArgs.Body;
                    string message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Processing message: {message} \n" +
                                      $"Thread Id: {Thread.CurrentThread.ManagedThreadId}");

                    // manyally ackoledge delivery of message if autoAckoledge is set to false as below line 65
                    // if you don't ackoledge the message, next message won't be sent
                    // because channel.BasicQos set to get only one message per time see line 42

                    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                    Console.WriteLine("Message processed");
                };

                // if queue is not declared, exception can be thrown
                channel.BasicConsume(queue: queueName,
                                    autoAck: false,    // if true => once message is read it's marked as received, and RabbitMq delete it from queueue
                                                       //it's not always good, cause it doesnt mean that you processed it completeley
                                                       // Cause your server may stop in the middle of processing
                                                       // so it's better to set it to false and acknowledge it manually

                                    consumer: consumer);

                Console.WriteLine("Listening to channel... Press enter to exit");
                Console.ReadLine();
            }

            Console.WriteLine("Shutting down....");
        }

    }
}
