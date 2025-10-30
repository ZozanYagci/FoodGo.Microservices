using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly RabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;

        public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider)
            : base(config, serviceProvider)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = config.Connection?.HostName ?? "localhost",
                UserName = config.Connection?.UserName ?? "guest",
                Password = config.Connection?.Password ?? "guest",
                Port = config.Connection?.Port ?? 5672,
                DispatchConsumersAsync = true
            };

            _persistentConnection = new RabbitMQPersistentConnection(_connectionFactory, config.ConnectionRetryCount);
            _consumerChannel = CreateConsumerChannel();

            subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            _consumerChannel.QueueUnbind(
                queue: GetSubName(eventName),
                exchange: _config.DefaultTopicName,
                routingKey: eventName);

            if (subsManager.IsEmpty)
                _consumerChannel.Close();
        }

        public override async Task Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(
                    _config.ConnectionRetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );

            var eventName = ProcessEventName(@event.GetType().Name);
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                using var channel = _persistentConnection.CreateModel();
                channel.ExchangeDeclare(_config.DefaultTopicName, type: "direct");

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                var queueName = GetSubName(eventName);
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicPublish(
                    exchange: _config.DefaultTopicName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public override void Subscribe<T, TH>(string? queue = null)
        {
            var eventName = ProcessEventName(typeof(T).Name);
            var queueName = queue ?? GetSubName(eventName);

            if (!subsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_persistentConnection.IsConnected)
                    _persistentConnection.TryConnect();

                _consumerChannel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _consumerChannel.QueueBind(queue: queueName, exchange: _config.DefaultTopicName, routingKey: eventName);
            }

            subsManager.AddSubscription<T, TH>();
            StartBasicConsume(queueName);
        }

        public override void UnSubscribe<T, TH>(string? queueName = null)
        {
            subsManager.RemoveSubscription<T, TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(_config.DefaultTopicName, type: "direct");
            return channel;
        }

        private void StartBasicConsume(string queueName)
        {
            if (_consumerChannel == null || _consumerChannel.IsClosed)
                _consumerChannel = CreateConsumerChannel();

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
            consumer.Received += async (sender, ea) => await Consumer_Received(sender, ea);

            _consumerChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = ProcessEventName(eventArgs.RoutingKey);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception)
            {
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        }
    }
}