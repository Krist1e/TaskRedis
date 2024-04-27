using Discussion.Dto.Request;
using Discussion.Dto.Response;
using Discussion.Models;
using Riok.Mapperly.Abstractions;
namespace Discussion.Mappers;

[Mapper]
public static partial class PostMapper
{
    public static partial Post ToEntity(this PostRequestTo postRequestTo);
    public static partial PostResponseTo ToResponse(this Post post);
    public static partial IEnumerable<PostResponseTo> ToResponse(this IEnumerable<Post> posts);

    public static Post UpdateEntity(this Post post, PostRequestTo updatePostRequestTo) => new()
    {
        Id = post.Id,
        TweetId = updatePostRequestTo.TweetId,
        Content = updatePostRequestTo.Content,
        Country = post.Country
    };
}
