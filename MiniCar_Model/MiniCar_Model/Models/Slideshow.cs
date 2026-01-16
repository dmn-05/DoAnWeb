using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

[Table("Slideshow")]
public partial class Slideshow
{
    [Key]
    [Column("Slideshow_Id")]
    public int SlideshowId { get; set; }

    [StringLength(150)]
    public string? Title { get; set; }

    [Column("Image_Url")]
    [StringLength(255)]
    public string ImageUrl { get; set; } = null!;

    [Column("Link_Type")]
    [StringLength(50)]
    public string? LinkType { get; set; }

    [Column("Link_Id")]
    public int? LinkId { get; set; }

    [Column("Link_Url")]
    [StringLength(255)]
    public string? LinkUrl { get; set; }

    [StringLength(50)]
    public string? Position { get; set; }

    [Column("Display_Order")]
    public int? DisplayOrder { get; set; }

    [Column("Start_Date", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("End_Date", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("Status_Slideshow")]
    [StringLength(50)]
    public string? StatusSlideshow { get; set; }

    [Column("Created_At", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("Updated_At", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }
}
