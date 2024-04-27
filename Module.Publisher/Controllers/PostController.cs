using System.Globalization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Publisher.Constants;
using Publisher.Dto.Request.CreateTo;
using Publisher.Dto.Request.UpdateTo;
using Publisher.Dto.Response;
using Publisher.Services.Interfaces;
namespace Publisher.Controllers;

[Route("api/v{version:apiVersion}/posts")]
[ApiVersion("1.0")]
[ApiController]
public class PostController(IPostService service, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostResponseTo>> GetPost(long id)
    {
        return Ok(await service.GetPostById(id));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponseTo>>> GetPosts()
    {
        return Ok(await service.GetPosts());
    }

    [HttpPost]
    public async Task<ActionResult<PostResponseTo>> CreatePost(CreatePostRequestTo createPostRequestTo)
    {
        var country = GetCountryFromRequest();
        PostResponseTo addedPost = await service.CreatePost(createPostRequestTo, country);
        await cacheStore.EvictByTagAsync(CacheTags.Posts, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetPost), new { id = addedPost.Id }, addedPost);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePost(long id)
    {
        await service.DeletePost(id);
        await cacheStore.EvictByTagAsync(CacheTags.Posts, HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult<PostResponseTo>> UpdatePost(UpdatePostRequestTo updatePostRequestTo)
    {
        var country = GetCountryFromRequest();
        PostResponseTo updatePost = await service.UpdatePost(updatePostRequestTo, country);
        await cacheStore.EvictByTagAsync(CacheTags.Posts, HttpContext.RequestAborted);
        return Ok(updatePost);
    }

    private string GetCountryFromRequest()
    {
        var lang = Request.GetTypedHeaders().AcceptLanguage
            .MaxBy(x => x.Quality ?? 1)
            ?.Value.ToString();

        return string.IsNullOrEmpty(lang) ? "Unknown" : new RegionInfo(lang).TwoLetterISORegionName;
    }
}
