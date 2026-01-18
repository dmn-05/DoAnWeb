using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public string? ImageAdvertisement { get; set; }

    public string? LinkUrl { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? StatusAdvertisement { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
