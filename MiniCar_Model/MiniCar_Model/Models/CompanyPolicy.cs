using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class CompanyPolicy
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Code { get; set; }

    public string? Icon { get; set; }

    public int? DisplayOrder { get; set; }

    public string? Status { get; set; }
}
