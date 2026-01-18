using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Descriptions { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
