﻿using Cassandra.Mapping.Attributes;
namespace Discussion.Models;

[Table("tbl_posts", Keyspace = "distcomp", AllowFiltering = true)]
public class Post
{
    [PartitionKey] public string Country { get; set; } = string.Empty;

    [ClusteringKey(0)] public long TweetId { get; set; }
    
    [ClusteringKey(1)] public long Id { get; set; }
    
    public string Content { get; set; } = string.Empty;
}
