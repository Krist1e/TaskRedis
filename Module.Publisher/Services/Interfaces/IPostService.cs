using Publisher.Dto.Request.CreateTo;
using Publisher.Dto.Request.UpdateTo;
using Publisher.Dto.Response;
namespace Publisher.Services.Interfaces;

public interface IPostService
{
    Task<PostResponseTo> GetPostById(long id);
    Task<IEnumerable<PostResponseTo>> GetPosts();
    Task<PostResponseTo> CreatePost(CreatePostRequestTo createPostRequestTo, string country);
    Task DeletePost(long id);
    Task<PostResponseTo> UpdatePost(UpdatePostRequestTo modifiedPost, string country);
}