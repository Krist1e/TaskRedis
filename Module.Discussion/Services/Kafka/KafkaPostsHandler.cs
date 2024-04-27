using Discussion.Dto.Message;
using Discussion.Dto.Response;
using Discussion.Services.Interfaces;
using TaskRedis.ServiceDefaults.Kafka.Consumer;
using TaskRedis.ServiceDefaults.Kafka.Producer;
namespace Discussion.Services.Kafka;

public sealed class KafkaPostsHandler(
    KafkaMessageProducer<string, OutTopicMessage> producer,
    IPostService postService) : IKafkaHandler<string, InTopicMessage>
{
    public async Task HandleAsync(string key, InTopicMessage value)
    {
        OutTopicMessage response;
        try
        {
            response = await HandleOperation(value);
        }
        catch (Exception e)
        {
            response = new OutTopicMessage(value.OperationType, [], e.Message);
        }

        await producer.ProduceAsync(key, response);
    }

    private async Task<OutTopicMessage> HandleOperation(InTopicMessage value) => value.OperationType switch
    {
        OperationType.GetById => await HandleGetById(value),
        OperationType.GetAll => await HandleGetAll(),
        OperationType.Create => await HandleCreate(value),
        OperationType.Delete => await HandleDelete(value),
        OperationType.Update => await HandleUpdate(value),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    private async Task<OutTopicMessage> HandleGetById(InTopicMessage value)
    {
        PostResponseTo post = await postService.GetPostById(value.Message.Id);
        return new OutTopicMessage(value.OperationType, [post]);
    }
    
    private async Task<OutTopicMessage> HandleGetAll()
    {
        var posts = await postService.GetPosts();
        return new OutTopicMessage(OperationType.GetAll, posts.ToList());
    }
    
    private async Task<OutTopicMessage> HandleCreate(InTopicMessage value)
    {
        PostResponseTo post = await postService.CreatePost(value.Message);
        return new OutTopicMessage(value.OperationType, [post]);
    }
    
    private async Task<OutTopicMessage> HandleDelete(InTopicMessage value)
    {
        await postService.DeletePost(value.Message.Id);
        return new OutTopicMessage(value.OperationType, []);
    }
    
    private async Task<OutTopicMessage> HandleUpdate(InTopicMessage value)
    {
        PostResponseTo post = await postService.UpdatePost(value.Message);
        return new OutTopicMessage(value.OperationType, [post]);
    }
}
