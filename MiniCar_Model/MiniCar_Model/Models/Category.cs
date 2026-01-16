using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("Category_Id")]
    public int CategoryId { get; set; }

    [Column("Parent_Id")]
    public int? ParentId { get; set; }

    [Column("Name_Category")]
    [StringLength(150)]
    public string NameCategory { get; set; } = null!;

    [Column("Status_Category")]
    [StringLength(50)]
    public string? StatusCategory { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("Updated_At", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Category? Parent { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
