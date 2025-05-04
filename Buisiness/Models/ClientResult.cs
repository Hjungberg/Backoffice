

using Domain.Models;

namespace Buisiness.Models;

public class ClientResult : ServiceResult
{
    public IEnumerable<Client>? Result { get; set; }
}
