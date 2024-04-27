using Discussion.Dto.Request;
using Discussion.Dto.Response;
namespace Discussion.Services.Interfaces;

public interface IPostService
{
    Task<PostResponseTo> GetPostById(long id);
    Task<IEnumerable<PostResponseTo>> GetPosts();
    Task<PostResponseTo> CreatePost(PostRequestTo postRequestTo);
    Task DeletePost(long id);
    Task<PostResponseTo> UpdatePost(PostRequestTo modifiedPost);
}