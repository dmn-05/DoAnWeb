using System;
using System.Collections.Generic;

namespace MiniCar_Model.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int RoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string? NameAccount { get; set; }

    public string Email { get; set; } = null!;

    public string? AddressAccount { get; set; }

    public string? PhoneNumber { get; set; }

    public string PasswordAccount { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? StatusAccount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
