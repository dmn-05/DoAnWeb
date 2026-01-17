using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Role_Name")]
    [StringLength(100)]
    public string RoleName { get; set; } = null!;

    [StringLength(255)]
    public string? Descriptions { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
