using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace GGM.Message.RabbitMQ
{
    /// <summary>
    /// MessageQueue를 관리하는 관리자클래스입니다. 사용자는 해당 클래스를 이용하여 메세지를 발행하거나 구독할 수 있습니다.
    /// </summary>
    public class MessageQueueManager : IMessageQueueManager
    {
        public MessageQueueManager(MessageQueueConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = Config.HostName,
                    UserName = Config.UserName,
                    Password = Config.Password
                };

                // 커넥션과 모델 생성
                _connection = factory.CreateConnection();
                _model = _connection.CreateModel();

                // Exchange 선언
                _model.ExchangeDeclare(
                    exchange: Config.Exchange,
                    type: Config.ExchangeType
                );

                // Queue 생성과 바인딩
                QueueName = _model.QueueDeclare(config.QueueName, exclusive: false, autoDelete: false).QueueName;
                _model.QueueBind(
                    queue: QueueName,
                    exchange: Config.Exchange,
                    routingKey: Config.RoutingKey
                );

                _connection.ConnectionShutdown += ConnectionShutdown;
            }
            catch (Exception e)
            {
                // 예외 발생 시 자원 정리
                _connection?.Dispose();
                _model?.Dispose();

                // re-throw
                throw;
            }
        }
        
        /// <summary>
        /// MessageQueueManager를 생성하는데 사용된 MessageQueueConfig객체입니다.
        /// </summary>
        public MessageQueueConfig Config { get; }

        /// <summary>
        /// 생성된 Queue의 이름입니다.
        /// </summary>
        public string QueueName { get; }

        private readonly IConnection _connection;
        private readonly IModel _model;

        /// <summary>
        /// MessageQueue가 Shutdown 될 때 호출되는 이벤트입니다.
        /// </summary>
        public EventHandler<ShutdownEventArgs> ConnectionShutdown { get; set; }

        /// <summary>
        /// Consumer를 등록하여 이벤트를 구독합니다.
        /// </summary>
        /// <param name="baseConsumer">이벤트를 구독할 Consumer</param>
        public void AddConsumer(BaseConsumer baseConsumer)
        {
            if (baseConsumer == null)
                throw new ArgumentNullException(nameof(baseConsumer));

            var basicConsumer = new EventingBasicConsumer(_model);
            basicConsumer.Received += baseConsumer.OnReceived;

            _model.BasicConsume(
                queue: QueueName,
                autoAck: true,
                consumer: basicConsumer
            );
        }

        /// <summary>
        /// Queue에 Message를 발행합니다.
        /// </summary>
        /// <param name="message">Queue에 발행할 메세지</param>
        public void Publish(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _model.BasicPublish(
                exchange: message.Exchange,
                routingKey: message.Route,
                basicProperties: null,
                body: message.Body
            );
        }
    }
}
