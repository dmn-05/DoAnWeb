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

	public virtual DbSet<Cart> Carts { get; set; }

	public virtual DbSet<CartItem> CartItems { get; set; }

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Color> Colors { get; set; }

	public virtual DbSet<Comment> Comments { get; set; }

	public virtual DbSet<Contact> Contacts { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<ProductImage> ProductImages { get; set; }

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
	public DbSet<CompanyInfo> CompanyInfos { get; set; }
	public DbSet<CompanyPolicy> CompanyPolicies { get; set; }
	public DbSet<BlogPost> BlogPosts { get; set; }
	public DbSet<ProductPost> ProductPosts { get; set; }



	//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
	//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MINICARS_MODEL;Integrated Security=True;Trust Server Certificate=True");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Account>(entity =>
		{
			entity.HasKey(e => e.AccountId).HasName("PK__Account__B19E45E9731F749F");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Account_Role");
		});

		modelBuilder.Entity<Advertisement>(entity =>
		{
			entity.HasKey(e => e.AdvertisementId).HasName("PK__Advertis__4B7F25D894D62921");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
		});

		modelBuilder.Entity<Bill>(entity =>
		{
			entity.HasKey(e => e.BillId).HasName("PK__Bill__CF6E7DA380B3FAC3");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Account).WithMany(p => p.Bills)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Bill_Account");
		});

		modelBuilder.Entity<BillInfo>(entity =>
		{
			entity.HasOne(d => d.Bill).WithMany(p => p.BillInfos)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_BillInfo_Bill");

			entity.HasOne(d => d.Variant).WithMany(p => p.BillInfos)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_BillInfo_Variant");
		});

		modelBuilder.Entity<Cart>(entity =>
		{
			entity.HasKey(e => e.CartId).HasName("PK__Cart__D6AB47593C9754B7");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Account).WithOne(p => p.Cart)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Cart_Account");
		});

		modelBuilder.Entity<CartItem>(entity =>
		{
			entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__7B6515015751607D");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_CartItem_Cart");

			entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_CartItem_Variant");
		});

		modelBuilder.Entity<Category>(entity =>
		{
			entity.HasKey(e => e.CategoryId).HasName("PK__Category__6DB38D6E0AB1D4B4");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Category_Parent");
		});

		modelBuilder.Entity<Color>(entity =>
		{
			entity.HasKey(e => e.ColorId).HasName("PK__Color__795F1D546CBCDC30");

			entity.Property(e => e.StatusColor).HasDefaultValue("ACTIVE");
		});

		modelBuilder.Entity<Comment>(entity =>
		{
			entity.HasKey(e => e.CommentId).HasName("PK__Comment__99FC14DBF735A565");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Account).WithMany(p => p.Comments)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Comment_Account");

			entity.HasOne(d => d.Variant).WithMany(p => p.Comments)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Comment_Product");
		});

		modelBuilder.Entity<Contact>(entity =>
		{
			entity.HasKey(e => e.ContactId).HasName("PK__Contact__82ACC1ED6254707C");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
			entity.Property(e => e.StatusContact).HasDefaultValue("NEW");
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(e => e.ProductId).HasName("PK__Product__9834FBBA5A6C4EF1");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Product_Category");

			entity.HasOne(d => d.Promotion).WithMany(p => p.Products).HasConstraintName("FK_Product_Promotion");

			entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Product_Supplier");

			entity.HasOne(d => d.Trademark).WithMany(p => p.Products).HasConstraintName("FK_Product_Trademark");
		});

		modelBuilder.Entity<ProductImage>(entity =>
		{
			entity.HasKey(e => e.ImageId).HasName("PK__ProductI__3CAB4D59DBC2BE52");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
			entity.Property(e => e.IsMain).HasDefaultValue(false);

			entity.HasOne(d => d.Variant).WithMany(p => p.ProductImages).HasConstraintName("FK_Image_Variant");
		});

		modelBuilder.Entity<ProductVariant>(entity =>
		{
			entity.HasKey(e => e.VariantId).HasName("PK__ProductV__E19D76CC53E6E6B5");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
			entity.Property(e => e.StatusVariant).HasDefaultValue("ACTIVE");

			entity.HasOne(d => d.Color).WithMany(p => p.ProductVariants)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Variant_Color");

			entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Variant_Product");

			entity.HasOne(d => d.Size).WithMany(p => p.ProductVariants)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Variant_Size");
		});

		modelBuilder.Entity<Promotion>(entity =>
		{
			entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__DAF79ADB2A0CA205");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49B0F6E9838");
		});

		modelBuilder.Entity<ShippingAddress>(entity =>
		{
			entity.HasKey(e => e.AddressId).HasName("PK__Shipping__03BDEBBA5D19BFD3");

			entity.Property(e => e.IsDefault).HasDefaultValue(false);

			entity.HasOne(d => d.Account).WithMany(p => p.ShippingAddresses)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_ShippingAddress_Account");
		});

		modelBuilder.Entity<Size>(entity =>
		{
			entity.HasKey(e => e.SizeId).HasName("PK__Size__0BC32560ADFEAC86");
		});

		modelBuilder.Entity<Slideshow>(entity =>
		{
			entity.HasKey(e => e.SlideshowId).HasName("PK__Slidesho__27DE308DC9D1EBA5");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
			entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
			entity.Property(e => e.StatusSlideshow).HasDefaultValue("ACTIVE");
		});

		modelBuilder.Entity<Supplier>(entity =>
		{
			entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__83918DB80AB237D2");

			entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
		});

		modelBuilder.Entity<Token>(entity =>
		{
			entity.HasKey(e => e.TokenId).HasName("PK__Token__AA16D4A0E5278945");

			entity.HasOne(d => d.Account).WithMany(p => p.Tokens)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Token_Account");
		});

		modelBuilder.Entity<Trademark>(entity =>
		{
			entity.HasKey(e => e.TrademarkId).HasName("PK__Trademar__FECFFA0776D4CA97");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
		});

		modelBuilder.Entity<Wishlist>(entity =>
		{
			entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__C65247E316DFB51A");

			entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Account).WithMany(p => p.Wishlists)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Wishlist_Account");

			entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("FK_Wishlist_Product");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
