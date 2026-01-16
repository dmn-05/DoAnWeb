using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Comment")]
public partial class Comment
{
    [Key]
    [Column("Comment_Id")]
    public int CommentId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Variant_Id")]
    public int VariantId { get; set; }

    public double? Rating { get; set; }

    [StringLength(500)]
    public string? Content { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column("Status_Comment")]
    [StringLength(50)]
    public string? StatusComment { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Comments")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("Comments")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
