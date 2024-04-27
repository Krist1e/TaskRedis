﻿using TaskRedis.ServiceDefaults.Kafka.Consumer;
namespace TaskRedis.ServiceDefaults.Kafka.SyncClient;

public sealed class SyncClientHandler<TInput, TOutput>(KafkaSyncClient<TInput, TOutput> client) : IKafkaHandler<string, TOutput>
    where TInput : class 
    where TOutput : class
{
    public Task HandleAsync(string key, TOutput value)
    {
        client.Handle(key, value);
        return Task.CompletedTask;
    }
}
