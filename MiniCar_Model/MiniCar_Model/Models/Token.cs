using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Token
{
    public int TokenId { get; set; }

    public int AccountId { get; set; }

    public string Token1 { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime ExpiredAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
