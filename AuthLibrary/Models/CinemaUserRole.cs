using System;
using System.Collections.Generic;

namespace AuthLibrary.Models;

public partial class CinemaUserRole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<CinemaRolePrivilege> CinemaRolePrivileges { get; set; } = new List<CinemaRolePrivilege>();
}
