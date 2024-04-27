using Microsoft.EntityFrameworkCore;
using Publisher.Clients.Discussion;
using Publisher.Clients.Discussion.Dto.Response;
using Publisher.Clients.Discussion.Mapper;
using Publisher.Dto.Request.CreateTo;
using Publisher.Dto.Request.UpdateTo;
using Publisher.Dto.Response;
using Publisher.Exceptions;
using Publisher.Services.Interfaces;
using Publisher.Storage;
namespace Publisher.Services.Implementations;

public sealed class PostService(IDiscussionService discussionService, AppDbContext dbContext) : IPostService
{
    private readonly Random _random = new();

    public async Task<PostResponseTo> GetPostById(long id) =>
        (await discussionService.GetPost(id)).ToResponse();

    public async Task<IEnumerable<PostResponseTo>> GetPosts() =>
        (await discussionService.GetPosts()).ToResponse();

    public async Task<PostResponseTo> CreatePost(CreatePostRequestTo createPostRequestTo, string country)
    {
        var isTweetExist = await dbContext.Tweets.AnyAsync(t => t.Id == createPostRequestTo.TweetId);
        if (!isTweetExist)
            throw new EntityNotFoundException("Tweet not found");

        DiscussionPostResponseTo discussionPostResponseTo = (await discussionService.CreatePost(createPostRequestTo.ToDiscussionRequest(CreateId(), country)));
        return discussionPostResponseTo.ToResponse();
    }

    public async Task DeletePost(long id)
    {
        await discussionService.DeletePost(id);
    }

    public async Task<PostResponseTo> UpdatePost(UpdatePostRequestTo modifiedPost, string country)
    {
        var isTweetExist = await dbContext.Tweets.AnyAsync(t => t.Id == modifiedPost.TweetId);
        if (!isTweetExist)
            throw new EntityNotFoundException("Tweet not found");

        return (await discussionService.UpdatePost(modifiedPost.ToDiscussionRequest(country))).ToResponse();
    }

    private long CreateId() => (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() << 24 | (uint)_random.Next(0, 1 << 24)) & long.MaxValue;
}
