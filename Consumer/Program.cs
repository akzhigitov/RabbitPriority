using System;
using Contract;
using EasyNetQ;

namespace Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var advancedBus = RabbitHutch.CreateBus().Advanced;
            var queue = advancedBus.QueueDeclare(QueueNames.PriorityQueue, maxPriority: 10);

            advancedBus.Consume<PriorityMessage>(
                queue,
                (m, i) => Console.WriteLine("Priority: {0} Number: {1}", m.Properties.Priority, m.Body.Number));
        }
    }
}
