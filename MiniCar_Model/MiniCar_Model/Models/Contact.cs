using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Contact")]
public partial class Contact
{
    [Key]
    [Column("Contact_Id")]
    public int ContactId { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(150)]
    public string? Subject { get; set; }

    [StringLength(500)]
    public string Message { get; set; } = null!;

    [Column("Status_Contact")]
    [StringLength(50)]
    public string? StatusContact { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
}
