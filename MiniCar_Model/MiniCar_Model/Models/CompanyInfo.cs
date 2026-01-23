using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class CompanyInfo
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public string? BusinessField { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Hotline { get; set; }

    public string? Description { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
