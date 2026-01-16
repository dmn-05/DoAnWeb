using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("ShippingAddress")]
public partial class ShippingAddress
{
    [Key]
    [Column("Address_Id")]
    public int AddressId { get; set; }

    [Column("Account_Id")]
    public int AccountId { get; set; }

    [Column("Receiver_Name")]
    [StringLength(150)]
    public string? ReceiverName { get; set; }

    [Column("Phone_Number")]
    [StringLength(10)]
    public string? PhoneNumber { get; set; }

    [Column("Address_Line")]
    [StringLength(255)]
    public string? AddressLine { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Province { get; set; }

    [Column("Is_Default")]
    public bool? IsDefault { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("ShippingAddresses")]
    public virtual Account Account { get; set; } = null!;
}
