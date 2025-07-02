using Whispers.Chat.Core.Generated.ContributorAggregate;

namespace Whispers.Chat.Core.Generated.ContributorAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(int contributorId) =>
    Query
        .Where(contributor => contributor.Id == contributorId);
}
