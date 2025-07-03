using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

namespace Whispers.Chat.Core.Specifications;

public class UserByUsernameSpec : Specification<User>
{
  public UserByUsernameSpec(string username) =>
    Query.Where(user => string.Equals(user.UserName, username, StringComparison.CurrentCulture));
}

public class UserByEmailSpec : Specification<User>
{
  public UserByEmailSpec(string email) =>
    Query.Where(user => string.Equals(user.Email, email, StringComparison.CurrentCulture));
}

public class UserByUsernameOrEmailSpec : Specification<User>
{
  public UserByUsernameOrEmailSpec(string usernameOrEmail) =>
    Query.Where(user => string.Equals(user.UserName, usernameOrEmail, StringComparison.CurrentCulture) ||
                string.Equals(user.Email, usernameOrEmail, StringComparison.CurrentCulture));
}
