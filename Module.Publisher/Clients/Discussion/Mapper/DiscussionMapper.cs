using Publisher.Clients.Discussion.Dto.Request;
using Publisher.Clients.Discussion.Dto.Response;
using Publisher.Dto.Request.CreateTo;
using Publisher.Dto.Request.UpdateTo;
using Publisher.Dto.Response;
using Riok.Mapperly.Abstractions;
namespace Publisher.Clients.Discussion.Mapper;

[Mapper]
public static partial class DiscussionMapper
{
    public static DiscussionPostRequestTo ToDiscussionRequest(this CreatePostRequestTo requestTo, long id, string country) =>
        new(id, requestTo.TweetId, requestTo.Content, country);
    public static DiscussionPostRequestTo ToDiscussionRequest(this UpdatePostRequestTo requestTo, string country) =>
        new(requestTo.Id, requestTo.TweetId, requestTo.Content, country);

    public static partial PostResponseTo ToResponse(this DiscussionPostResponseTo responseTo);

    public static partial IEnumerable<PostResponseTo> ToResponse(this IEnumerable<DiscussionPostResponseTo> responseTo);
}
