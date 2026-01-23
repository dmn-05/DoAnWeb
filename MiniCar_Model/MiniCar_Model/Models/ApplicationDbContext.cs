using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniCar_Model.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillInfo> BillInfos { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

    public virtual DbSet<CompanyPolicy> CompanyPolicies { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductPost> ProductPosts { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShippingAddress> ShippingAddresses { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Slideshow> Slideshows { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<Trademark> Trademarks { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__B19E45E929B5123C");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534C81DEABE").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Account__C9F28456FBCA89E5").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.AddressAccount)
                .HasMaxLength(255)
                .HasColumnName("Address_Account");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.NameAccount)
                .HasMaxLength(150)
                .HasColumnName("Name_Account");
            entity.Property(e => e.PasswordAccount)
                .HasMaxLength(255)
                .HasColumnName("Password_Account");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.StatusAccount)
                .HasMaxLength(50)
                .HasColumnName("Status_Account");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvertisementId).HasName("PK__Advertis__4B7F25D8D9ED91DC");

            entity.ToTable("Advertisement");

            entity.Property(e => e.AdvertisementId).HasColumnName("Advertisement_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.ImageAdvertisement)
                .HasMaxLength(255)
                .HasColumnName("Image_Advertisement");
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(255)
                .HasColumnName("Link_Url");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.StatusAdvertisement)
                .HasMaxLength(50)
                .HasColumnName("Status_Advertisement");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bill__CF6E7DA3BB3DD632");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId).HasColumnName("Bill_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_At");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("Payment_Date");
            entity.Property(e => e.StatusBill)
                .HasMaxLength(50)
                .HasColumnName("Status_Bill");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("Total_Price");

            entity.HasOne(d => d.Account).WithMany(p => p.Bills)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_Account");
        });

        modelBuilder.Entity<BillInfo>(entity =>
        {
            entity.HasKey(e => new { e.BillId, e.VariantId });

            entity.ToTable("BillInfo");

            entity.Property(e => e.BillId).HasColumnName("Bill_Id");
            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("Unit_Price");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillInfo_Bill");

            entity.HasOne(d => d.Variant).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillInfo_Variant");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogPost__3214EC07B81F2DC2");

            entity.ToTable("BlogPost");

            entity.HasIndex(e => e.Slug, "UQ__BlogPost__BC7B5FB6908D3010").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.Thumbnail).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__D6AB4759D9301ABA");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.AccountId, "UQ__Cart__B19E45E8EF0B163B").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("Cart_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.StatusCart)
                .HasMaxLength(50)
                .HasColumnName("Status_Cart");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");

            entity.HasOne(d => d.Account).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Account");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__7B65150136746869");

            entity.ToTable("CartItem");

            entity.HasIndex(e => new { e.CartId, e.VariantId }, "UQ_Cart_Variant").IsUnique();

            entity.Property(e => e.CartItemId).HasColumnName("CartItem_Id");
            entity.Property(e => e.CartId).HasColumnName("Cart_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_Variant");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__6DB38D6E688D8CDB");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(150)
                .HasColumnName("Name_Category");
            entity.Property(e => e.ParentId).HasColumnName("Parent_Id");
            entity.Property(e => e.StatusCategory)
                .HasMaxLength(50)
                .HasColumnName("Status_Category");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Category_Parent");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Color__795F1D54330106E8");

            entity.ToTable("Color");

            entity.Property(e => e.ColorId).HasColumnName("Color_Id");
            entity.Property(e => e.ColorCode)
                .HasMaxLength(20)
                .HasColumnName("Color_Code");
            entity.Property(e => e.ColorName)
                .HasMaxLength(50)
                .HasColumnName("Color_Name");
            entity.Property(e => e.StatusColor)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE")
                .HasColumnName("Status_Color");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__99FC14DBF605C54D");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("Comment_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.StatusComment)
                .HasMaxLength(50)
                .HasColumnName("Status_Comment");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Account");

            entity.HasOne(d => d.Variant).WithMany(p => p.Comments)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Product");
        });

        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyI__3214EC07D225A272");

            entity.ToTable("CompanyInfo");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.BusinessField).HasMaxLength(200);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Hotline).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
        });

        modelBuilder.Entity<CompanyPolicy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyP__3214EC07B935CF00");

            entity.ToTable("CompanyPolicy");

            entity.HasIndex(e => e.Code, "UQ__CompanyP__A25C5AA7D78E879A").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__82ACC1ED99191E2C");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("Contact_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_At");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.StatusContact)
                .HasMaxLength(50)
                .HasDefaultValue("NEW")
                .HasColumnName("Status_Contact");
            entity.Property(e => e.Subject).HasMaxLength(150);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__9834FBBAA20E40A3");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(150)
                .HasColumnName("Name_Product");
            entity.Property(e => e.PromotionId).HasColumnName("Promotion_Id");
            entity.Property(e => e.StatusProduct)
                .HasMaxLength(50)
                .HasColumnName("Status_Product");
            entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");
            entity.Property(e => e.TrademarkId).HasColumnName("Trademark_Id");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Products)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_Product_Promotion");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Product_Supplier");

            entity.HasOne(d => d.Trademark).WithMany(p => p.Products)
                .HasForeignKey(d => d.TrademarkId)
                .HasConstraintName("FK_Product_Trademark");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__3CAB4D59D9699B05");

            entity.ToTable("ProductImage");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.IsMain)
                .HasDefaultValue(false)
                .HasColumnName("Is_Main");
            entity.Property(e => e.UrlImage)
                .HasMaxLength(255)
                .HasColumnName("Url_Image");
            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");

            entity.HasOne(d => d.Variant).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK_Image_Variant");
        });

        modelBuilder.Entity<ProductPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC078D26C8C2");

            entity.ToTable("ProductPost");

            entity.HasIndex(e => e.Slug, "UQ__ProductP__BC7B5FB61DF0BF62").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Slug).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.Thumbnail).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__ProductV__E19D76CC09943E02");

            entity.ToTable("ProductVariant");

            entity.HasIndex(e => new { e.ProductId, e.SizeId, e.ColorId }, "UQ_Product_Size_Color").IsUnique();

            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");
            entity.Property(e => e.ColorId).HasColumnName("Color_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.SizeId).HasColumnName("Size_Id");
            entity.Property(e => e.StatusVariant)
                .HasMaxLength(50)
                .HasDefaultValue("ACTIVE")
                .HasColumnName("Status_Variant");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Variant_Color");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Variant_Product");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Variant_Size");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__DAF79ADBD4C99F35");

            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId).HasColumnName("Promotion_Id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.DescriptionPromotion)
                .HasMaxLength(255)
                .HasColumnName("Description_Promotion");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(10)
                .HasColumnName("Discount_Type");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Discount_Value");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.StatusPromotion)
                .HasMaxLength(50)
                .HasColumnName("Status_Promotion");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49B106308CC");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Descriptions).HasMaxLength(255);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<ShippingAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Shipping__03BDEBBAB1FB2951");

            entity.ToTable("ShippingAddress");

            entity.Property(e => e.AddressId).HasColumnName("Address_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(255)
                .HasColumnName("Address_Line");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("Is_Default");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.ReceiverName)
                .HasMaxLength(150)
                .HasColumnName("Receiver_Name");

            entity.HasOne(d => d.Account).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShippingAddress_Account");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Size__0BC32560C797B6AF");

            entity.ToTable("Size");

            entity.Property(e => e.SizeId).HasColumnName("Size_Id");
            entity.Property(e => e.Descriptions).HasMaxLength(255);
            entity.Property(e => e.Scale).HasMaxLength(50);
            entity.Property(e => e.StatusSize)
                .HasMaxLength(50)
                .HasColumnName("Status_Size");
        });

        modelBuilder.Entity<Slideshow>(entity =>
        {
            entity.HasKey(e => e.SlideshowId).HasName("PK__Slidesho__27DE308DE10F66F5");

            entity.ToTable("Slideshow");

            entity.Property(e => e.SlideshowId).HasColumnName("Slideshow_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("Image_Url");
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(255)
                .HasColumnName("Link_Url");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.StatusSlideshow)
                .HasMaxLength(50)
                .HasDefaultValue("Active")
                .HasColumnName("Status_Slideshow");
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
            entity.Property(e => e.VariantId).HasColumnName("Variant_Id");

            entity.HasOne(d => d.Variant).WithMany(p => p.Slideshows)
                .HasForeignKey(d => d.VariantId)
                .HasConstraintName("FK_Slideshow_VariantId");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__83918DB84589DDE8");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");
            entity.Property(e => e.AddressSupplier)
                .HasMaxLength(255)
                .HasColumnName("Address_Supplier");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.NameSupplier)
                .HasMaxLength(150)
                .HasColumnName("Name_Supplier");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.StatusSupplier)
                .HasMaxLength(50)
                .HasColumnName("Status_Supplier");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__Token__AA16D4A07EC6EE54");

            entity.ToTable("Token");

            entity.Property(e => e.TokenId).HasColumnName("Token_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("datetime")
                .HasColumnName("Expired_At");
            entity.Property(e => e.Token1)
                .HasMaxLength(500)
                .HasColumnName("Token");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Account");
        });

        modelBuilder.Entity<Trademark>(entity =>
        {
            entity.HasKey(e => e.TrademarkId).HasName("PK__Trademar__FECFFA0795A9135A");

            entity.ToTable("Trademark");

            entity.Property(e => e.TrademarkId).HasColumnName("Trademark_Id");
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Descriptions).HasMaxLength(255);
            entity.Property(e => e.NameTrademark)
                .HasMaxLength(150)
                .HasColumnName("Name_Trademark");
            entity.Property(e => e.StatusTrademark)
                .HasMaxLength(50)
                .HasColumnName("Status_Trademark");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__C65247E3ED2E3C4A");

            entity.ToTable("Wishlist");

            entity.HasIndex(e => new { e.AccountId, e.ProductVariantId }, "UQ_Wishlist").IsUnique();

            entity.Property(e => e.WishlistId).HasColumnName("Wishlist_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.ProductVariantId).HasColumnName("ProductVariant_Id");

            entity.HasOne(d => d.Account).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wishlist_Account");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wishlist_ProductVariant");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
