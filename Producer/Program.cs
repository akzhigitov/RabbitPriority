using System;
using Contract;
using EasyNetQ;
using EasyNetQ.Topology;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var advancedBus = RabbitHutch.CreateBus().Advanced;

            var exchange = advancedBus.ExchangeDeclare(QueueNames.PriorityQueue + ".exchange", ExchangeType.Topic);

            byte maxPriority = 10;

            advancedBus.QueueDeclareAsync(QueueNames.PriorityQueue, maxPriority: maxPriority)
                .ContinueWith(task =>
                {
                    if (!task.IsFaulted)
                    {
                        advancedBus.BindAsync(exchange, task.Result, "#");
                    }
                    else
                    {

                        Console.WriteLine(task.Exception);
                    }
                });

            var rnd = new Random();

            for (int i = 0; ; i++)
            {
                var priority = rnd.Next(0, maxPriority);

                advancedBus.Publish(
                    exchange, "#", false, false, new Message<PriorityMessage>(new PriorityMessage(i))
                    {
                        Properties = {Priority = (byte) priority}
                    });

                Console.Write("\rPublished: {0}", i);
            }
        }
    }
}
