using Publisher.Clients.Discussion.Dto.Message;
using Publisher.Clients.Discussion.Dto.Request;
using Publisher.Clients.Discussion.Dto.Response;
using TaskRedis.ServiceDefaults.Kafka.SyncClient;
namespace Publisher.Clients.Discussion.KafkaClient;

public class KafkaDiscussionServiceClient(KafkaSyncClient<InTopicMessage, OutTopicMessage> client) : IDiscussionService
{
    public async Task<DiscussionPostResponseTo> GetPost(long id)
    {
        OutTopicMessage outMessage = await client.ProduceAsync(
            new InTopicMessage(OperationType.GetById, new DiscussionPostRequestTo(id)));

        if (!outMessage.IsSuccess)
            throw new Exception(outMessage.ErrorMessage);

        return outMessage.Result.First();
    }

    public async Task<IEnumerable<DiscussionPostResponseTo>> GetPosts()
    {
        OutTopicMessage outMessage = await client.ProduceAsync(
            new InTopicMessage(OperationType.GetAll, new DiscussionPostRequestTo()));

        if (!outMessage.IsSuccess)
            throw new Exception(outMessage.ErrorMessage);

        return outMessage.Result;
    }

    public async Task<DiscussionPostResponseTo> CreatePost(DiscussionPostRequestTo createPostRequestTo)
    {
        OutTopicMessage outMessage = await client.ProduceAsync(
            new InTopicMessage(OperationType.Create, createPostRequestTo));

        if (!outMessage.IsSuccess)
            throw new Exception(outMessage.ErrorMessage);

        return outMessage.Result.First();
    }

    public async Task DeletePost(long id)
    {
        OutTopicMessage outMessage = await client.ProduceAsync(
            new InTopicMessage(OperationType.Delete, new DiscussionPostRequestTo(id)));

        if (!outMessage.IsSuccess)
            throw new Exception(outMessage.ErrorMessage);
    }

    public async Task<DiscussionPostResponseTo> UpdatePost(DiscussionPostRequestTo postRequestTo)
    {
        OutTopicMessage outMessage = await client.ProduceAsync(
            new InTopicMessage(OperationType.Update, postRequestTo));

        if (!outMessage.IsSuccess)
            throw new Exception(outMessage.ErrorMessage);

        return outMessage.Result.First();
    }
}
