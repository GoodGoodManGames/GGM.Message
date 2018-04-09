namespace GGM.Message.RabbitMQ
{
    /// <summary>
    /// Messqueue에 보내질 Message클래스입니다.
    /// </summary>
    public class Message
    {
        public string Exchange { get; set; }
        public string Route { get; set; }
        public byte[] Body { get; set; }
    }
}
