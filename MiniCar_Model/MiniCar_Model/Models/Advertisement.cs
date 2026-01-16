using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Advertisement")]
public partial class Advertisement
{
    [Key]
    [Column("Advertisement_Id")]
    public int AdvertisementId { get; set; }

    [Column("Image_Advertisement")]
    [StringLength(255)]
    public string? ImageAdvertisement { get; set; }

    [Column("Link_Url")]
    [StringLength(255)]
    public string? LinkUrl { get; set; }

    [Column("Start_Date", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("End_Date", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("Status_Advertisement")]
    [StringLength(50)]
    public string? StatusAdvertisement { get; set; }

    [Column("Create_At", TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column("Update_At", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }
}
