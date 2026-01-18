using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class ShippingAddress
{
    public int AddressId { get; set; }

    public int AccountId { get; set; }

    public string? ReceiverName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public bool? IsDefault { get; set; }

    public virtual Account Account { get; set; } = null!;
}
