using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Trademark")]
public partial class Trademark
{
    [Key]
    [Column("Trademark_Id")]
    public int TrademarkId { get; set; }

    [Column("Name_Trademark")]
    [StringLength(150)]
    public string NameTrademark { get; set; } = null!;

    [Column("Status_Trademark")]
    [StringLength(50)]
    public string? StatusTrademark { get; set; }

    [StringLength(255)]
    public string? Descriptions { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }


    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("Updated_At", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Trademark")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
