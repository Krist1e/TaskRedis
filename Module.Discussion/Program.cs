using Asp.Versioning;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Discussion;
using Discussion.Dto.Message;
using Discussion.ExceptionHandlers;
using Discussion.Extensions;
using Discussion.Services.Implementations;
using Discussion.Services.Interfaces;
using Discussion.Services.Kafka;
using Discussion.Storage;
using TaskRedis.ServiceDefaults;
using TaskRedis.ServiceDefaults.Kafka;
using TaskRedis.ServiceDefaults.Kafka.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.IncludeFields = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

builder.AddKafkaMessageProducer<string, OutTopicMessage>("kafka",
    settings =>
    {
        settings.Topic = "out-echo";
        settings.KeySerializer = new KafkaStringSerializer();
        settings.ValueSerializer = new KafkaJsonSerializer<OutTopicMessage>();
    });

builder.AddKafkaMessageHandler<string, InTopicMessage, KafkaPostsHandler>("kafka",
    settings =>
    {
        settings.Topic = "in-echo";
        settings.KeyDeserializer = new KafkaStringDeserializer();
        settings.ValueDeserializer = new KafkaJsonDeserializer<InTopicMessage>();
    },
    consumerSettings =>
    {
        consumerSettings.Config.GroupId = "echo-group";
    });

builder.AddCassandraDbContext("distcomp-discussion");
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();
