using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Token")]
public partial class Token
{
    [Key]
    [Column("Token_Id")]
    public int TokenId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Token")]
    [StringLength(500)]
    public string Token1 { get; set; } = null!;

    [StringLength(50)]
    public string? Type { get; set; }

    [Column("Expired_At", TypeName = "datetime")]
    public DateTime ExpiredAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Tokens")]
    public virtual Account Account { get; set; } = null!;
}
