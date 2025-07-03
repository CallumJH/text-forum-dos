using System;
using System.Threading.Tasks;
using Whispers.Chat.Core.BoundedContexts.Posts.AggregateRoots;
using Whispers.Chat.Core.BoundedContexts.Posts.Events;
using Whispers.Chat.Core.Interfaces;

namespace Whispers.Chat.Core.BoundedContexts.Posts.Services;

public class PostService : IDomainService
{
  public async Task<Post> CreatePost(string content, Guid authorId)
  {
    var post = new Post(content, authorId);
    return post;
  }

  public async Task LikePost(Post post, Guid userId)
  {
    if (post.AuthorId == userId)
    {
      throw new DomainException("Users cannot like their own posts");
    }

    if (post.HasUserLiked(userId))
    {
      throw new DomainException("User has already liked this post");
    }

    post.AddLike(userId);
  }

  public async Task AddComment(Post post, string content, Guid userId)
  {
    post.AddComment(content, userId);
  }
} 