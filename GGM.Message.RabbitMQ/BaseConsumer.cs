using RabbitMQ.Client.Events;

namespace GGM.Message.RabbitMQ
{
    /// <summary>
    /// MessageQueue를 구독할 Consumer입니다.
    /// MessageQueue에서 메세지를 구독하고 싶다면 해당 클래스를 상속받아 구현한 뒤 MessageQueueManager에 AddConsumer합니다.
    /// </summary>
    public abstract class BaseConsumer
    {
        internal void OnReceived(object sender, BasicDeliverEventArgs args)
        {
            var message = new Message
            {
                Exchange = args.Exchange,
                Route = args.RoutingKey,
                Body = args.Body
            };
            OnReceived(message);
        }

        /// <summary>
        /// MessageQueue가 Received 이벤트를 받았을때 호출되는 이벤트입니다.
        /// </summary>
        /// <param name="message">MessageQueue로 부터 받아들여온 객체</param>
        public abstract void OnReceived(Message message);
    }
}
