using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add volunteer models
    public virtual DbSet<VolunteerShift> VolunteerShifts { get; set; }
    public virtual DbSet<VolunteerRequest> VolunteerRequests { get; set; }



    public virtual DbSet<AdminReport> AdminReports { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public DbSet<Donation> Donations { get; set; }
    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<StaticPage> StaticPages { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<VisitItem> VisitItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AdminReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__AdminRep__D5BD48054847CF54");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK__Campuses__FD598DD605EAD8FF");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B3F5E458C9");

            entity.Property(e => e.LastUpdated).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Campus).WithMany(p => p.Inventories).HasConstraintName("FK__Inventory__Campu__5535A963");

            entity.HasOne(d => d.Item).WithMany(p => p.Inventories).HasConstraintName("FK__Inventory__ItemI__5441852A");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838BB912C500");

            entity.Property(e => e.Points).HasDefaultValue(0);

            entity.HasOne(d => d.ItemCategory).WithMany(p => p.Items).HasConstraintName("FK__Items__ItemCateg__5070F446");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.ItemCategoryId).HasName("PK__ItemCate__C24A2925B47D7986");
        });

        modelBuilder.Entity<StaticPage>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("PK__StaticPa__C565B104383F3B0F");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visits__4D3AA1DEE3517811");

            entity.HasOne(d => d.Campus).WithMany(p => p.Visits).HasConstraintName("FK__Visits__CampusId__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.Visits).HasConstraintName("FK__Visits__UserId__59063A47");
        });

        modelBuilder.Entity<VisitItem>(entity =>
        {
            entity.HasKey(e => e.VisitItemId).HasName("PK__VisitIte__4B99F797008F7E62");

            entity.HasOne(d => d.Item).WithMany(p => p.VisitItems).HasConstraintName("FK__VisitItem__ItemI__6754599E");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitItems).HasConstraintName("FK__VisitItem__Visit__66603565");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
