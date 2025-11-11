using System;
using System.Collections.Generic;

namespace AuthLibrary.Models;

public partial class CinemaUser
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? FailedLoginAttempts { get; set; }

    public DateTime? UnlockDate { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<CinemaRolePrivilege> CinemaRolePrivileges { get; set; } = new List<CinemaRolePrivilege>();
}
