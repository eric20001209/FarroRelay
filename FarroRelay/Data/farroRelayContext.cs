using FarroRelay.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarroRelay.Data
{
	public partial class farroRelayContext : DbContext
	{
		public farroRelayContext(DbContextOptions<farroRelayContext> options)
		: base(options)
		{
		}
		public virtual DbSet<Branch> Branch { get; set; }
		public virtual DbSet<Card> Card { get; set; }
		public virtual DbSet<EnumTable> EnumTable{ get; set; }
		public virtual DbSet<Purchase> Purchase { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
		
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
			modelBuilder.Entity<Branch>(entity =>
			{
				entity.ToTable("branch");
				entity.Property(e => e.Id).HasColumnName("id");
				entity.Property(e => e.Name).HasColumnName("name");
				entity.Property(e => e.Active).HasColumnName("activated").HasDefaultValue(true);

			});
			modelBuilder.Entity<Card>(entity =>
			{
				entity.ToTable("card");
				entity.HasIndex(e => e.Id, "IDX_card_id")
				.IsUnique()
				.IsClustered();
				entity.Property(e => e.Id).HasColumnName("id");
				entity.Property(e => e.Corp_Number).HasColumnName("corp_number");
				entity.Property(e => e.Company).HasColumnName("company");
			});
			modelBuilder.Entity<EnumTable>(entity =>
			{
				entity.ToTable("enum");
				entity.Property(e => e.Class).HasColumnName("class");
				entity.Property(e => e.Id).HasColumnName("id");
				entity.Property(e => e.Name).HasColumnName("name");
			});
			modelBuilder.Entity<Purchase>(entity =>
			{
				entity.ToTable("purchase");
				entity.Property(e => e.Id).HasColumnName("id");
				entity.Property(e => e.Branch_Id).HasColumnName("branch_id").HasDefaultValueSql("(1)");
				entity.Property(e => e.Supplier_Id).HasColumnName("supplier_id");
				entity.Property(e => e.PO_Number).HasColumnName("po_number");
				entity.Property(e => e.Inv_Number).HasColumnName("inv_number");
				entity.Property(e => e.Date_Create).HasColumnName("date_create").HasDefaultValueSql("getdate()");
				entity.Property(e => e.Type).HasColumnName("type");
				entity.Property(e => e.Date_Invoiced).HasColumnName("date_invoiced");
				entity.Property(e => e.Status).HasColumnName("status");
				entity.Property(e => e.Payment_Status).HasColumnName("payment_status").HasDefaultValueSql("(1)");
				entity.Property(e => e.Total_Amount).HasColumnName("total_amount").HasDefaultValueSql("(0)");
			});

		}
	}
}
