namespace Play.Catalog.Serivce.RabbitMQ
{
    public interface IRabitMQProducer
    {
        public void SendMessage<T>(T message);
    }
}
