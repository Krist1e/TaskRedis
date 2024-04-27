﻿using System.Text.Json.Serialization;
namespace Publisher.Clients.Discussion.Dto.Response;

public record DiscussionPostResponseTo(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("tweetId")]
    long TweetId,
    [property: JsonPropertyName("content")]
    string Content
);