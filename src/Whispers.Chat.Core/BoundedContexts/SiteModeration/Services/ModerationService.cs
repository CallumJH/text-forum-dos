using System;
using System.Threading.Tasks;
using Whispers.Chat.Core.BoundedContexts.Posts.AggregateRoots;
using Whispers.Chat.Core.BoundedContexts.SiteModeration.AggregateRoots;
using Whispers.Chat.Core.BoundedContexts.SiteModeration.Events;
using Whispers.Chat.Core.Interfaces;

namespace Whispers.Chat.Core.BoundedContexts.SiteModeration.Services;

public class ModerationService : IDomainService
{
  public async Task FlagPost(Post post, Moderator moderator, string reason)
  {
    if (!moderator.IsActive)
    {
      throw new DomainException("Inactive moderators cannot flag posts");
    }

    post.AddDomainEvent(new PostFlaggedEvent(post.Id, moderator.Id, reason));
  }

  public async Task<Moderator> AppointModerator(Guid userId)
  {
    var moderator = new Moderator(userId);
    return moderator;
  }

  public async Task DeactivateModerator(Moderator moderator)
  {
    moderator.Deactivate();
  }
} 