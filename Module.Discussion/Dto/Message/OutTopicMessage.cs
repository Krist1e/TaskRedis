using Discussion.Dto.Response;
namespace Discussion.Dto.Message;

public record OutTopicMessage(
    OperationType OperationType,
    List<PostResponseTo> Result,
    string? ErrorMessage = null)
{
    public bool IsSuccess => ErrorMessage == null;
}
