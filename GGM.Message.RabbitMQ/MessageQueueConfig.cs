namespace GGM.Message.RabbitMQ
{
    /// <summary>
    /// MessageQueue 초기화에 대한 정보를 가지고 있는 클래스입니다.
    /// </summary>
    public class MessageQueueConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Exchange { get; set; }
        public string ExchangeType { get; set; }
        // TODO: 추후 여러 Queue를 바인드 할 수 있게끔 수정될 예정입니다.
        public string QueueName { get; set; }
        public string RoutingKey { get; set; } = string.Empty;
    }
}
