using System.Text.Json.Serialization;
namespace Publisher.Clients.Discussion.Dto.Request;

public record DiscussionPostRequestTo(
    [property: JsonPropertyName("id")] 
    long Id,
    [property: JsonPropertyName("tweetId")]
    long TweetId,
    [property: JsonPropertyName("content")]
    string Content,
    [property: JsonPropertyName("country")]
    string Country
)
{
    public DiscussionPostRequestTo(long id) : this(id, 0, string.Empty, string.Empty)
    {}

    public DiscussionPostRequestTo() : this(0, 0, string.Empty, string.Empty)
    {}
}
