

using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
