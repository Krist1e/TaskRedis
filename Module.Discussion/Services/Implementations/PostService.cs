using Cassandra.Data.Linq;
using Discussion.Dto.Request;
using Discussion.Dto.Response;
using Discussion.Exceptions;
using Discussion.Mappers;
using Discussion.Models;
using Discussion.Services.Interfaces;
using Discussion.Storage;
namespace Discussion.Services.Implementations;

public sealed class PostService(CassandraDbContext context, ILogger<PostService> logger) : IPostService
{
    public async Task<PostResponseTo> GetPostById(long id)
    {
        Post? post = await context.Posts.FirstOrDefault(p => p.Id == id).ExecuteAsync();
        if (post == null) throw new EntityNotFoundException($"Post with id = {id} doesn't exist.");

        logger.LogInformation("Post with id = {Id} was found", id);
        return post.ToResponse();
    }

    public async Task<IEnumerable<PostResponseTo>> GetPosts()
    {
        logger.LogInformation("Getting all posts");
        return (await context.Posts.ExecuteAsync()).ToResponse();
    }

    public async Task<PostResponseTo> CreatePost(PostRequestTo postRequestTo)
    {
        Post post = postRequestTo.ToEntity();
        await context.Posts.Insert(post).ExecuteAsync();
        logger.LogInformation("Post with id = {Id} was created", post.Id);
        return post.ToResponse();
    }

    public async Task DeletePost(long id)
    {
        Post? post = await context.Posts.FirstOrDefault(p => p.Id == id).ExecuteAsync();
        if (post == null) throw new EntityNotFoundException($"Post with id = {id} doesn't exist.");

        await context.Posts.Where(p => p.Country == post.Country && p.TweetId == post.TweetId && p.Id == post.Id)
            .Delete()
            .ExecuteAsync();
        logger.LogInformation("Post with id = {Id} was deleted", id);
    }

    public async Task<PostResponseTo> UpdatePost(PostRequestTo modifiedPost)
    {
        Post? post = await context.Posts.FirstOrDefault(p => p.Id == modifiedPost.Id).ExecuteAsync();
        if (post == null) throw new EntityNotFoundException($"Post with id = {modifiedPost.Id} doesn't exist.");

        await context.Posts.Where(p => p.Country == post.Country && p.TweetId == post.TweetId && p.Id == post.Id)
            .Select(p => new Post
            {
                Content = modifiedPost.Content
            })
            .Update()
            .ExecuteAsync();

        return modifiedPost.ToEntity().ToResponse();
    }
}
