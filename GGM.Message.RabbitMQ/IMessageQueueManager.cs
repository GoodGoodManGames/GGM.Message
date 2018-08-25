using System;
using System.Collections.Generic;
using System.Text;

namespace GGM.Message.RabbitMQ
{
    public interface IMessageQueueManager
    {
        void AddConsumer(BaseConsumer baseConsumer);
        void Publish(Message message);
    }
}
