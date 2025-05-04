

using Domain.Models;

namespace Buisiness.Models;

public class UserResult : ServiceResult
{
    public IEnumerable<User>? Result { get; set; }
}
