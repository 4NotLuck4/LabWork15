using System;
using System.Collections.Generic;

namespace AuthLibrary.Models;

public partial class CinemaPrivilege
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CinemaRolePrivilege> CinemaRolePrivileges { get; set; } = new List<CinemaRolePrivilege>();
}
