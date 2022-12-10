using Confluent.Kafka;

namespace TelegramBot.Controllers;

public static class MyProducers
{
    private static ClientConfig Config { get; set; } = null!;

    private static Dictionary<string, IProducer<string, string>> Producers { get; set; } = null!;

    public static void StartProducers()
    {
        Config = new ProducerConfig{BootstrapServers = "localhost:9092"};
        Producers = new Dictionary<string, IProducer<string, string>>
        {
            ["command-counter-input"] = new ProducerBuilder<string, string>(Config).Build(),
            ["people-activity-input"] = new ProducerBuilder<string, string>(Config).Build(),
            ["avg-input"] = new ProducerBuilder<string, string>(Config).Build(),
            ["trials-input"] = new ProducerBuilder<string, string>(Config).Build(),
        };
    }

    public static void Produce(string topic, string key, string value)
    {
        Producers[topic].Produce(topic, new Message<string, string> { Key = key, Value = value},
            deliveryReport =>
            {
                Console.WriteLine(deliveryReport.Error.Code != ErrorCode.NoError
                    ? $"Failed to deliver message: {deliveryReport.Error.Reason}"
                    : $"Produced message to: {deliveryReport.TopicPartitionOffset}");
            });
    }
}