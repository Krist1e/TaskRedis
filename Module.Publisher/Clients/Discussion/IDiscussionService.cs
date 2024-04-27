using Publisher.Clients.Discussion.Dto.Request;
using Publisher.Clients.Discussion.Dto.Response;
namespace Publisher.Clients.Discussion;

public interface IDiscussionService
{
    Task<DiscussionPostResponseTo> GetPost(long id);
    
    Task<IEnumerable<DiscussionPostResponseTo>> GetPosts();
    
    Task<DiscussionPostResponseTo> CreatePost(DiscussionPostRequestTo createPostRequestTo);
    
    Task DeletePost(long id);
    
    Task<DiscussionPostResponseTo> UpdatePost(DiscussionPostRequestTo postRequestTo);
}