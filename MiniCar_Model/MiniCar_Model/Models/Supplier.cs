using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    [Column("Supplier_Id")]
    public int SupplierId { get; set; }

    [Column("Name_Supplier")]
    [StringLength(150)]
    public string NameSupplier { get; set; } = null!;

    [Column("Address_Supplier")]
    [StringLength(255)]
    public string? AddressSupplier { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }

    [Column("Phone_Number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("Status_Supplier")]
    [StringLength(50)]
    public string? StatusSupplier { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
