using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TokenAuthentication.Models;

namespace TokenAuthentication.Database;

public partial class TokenDbContext : DbContext
{
    public TokenDbContext()
    {
    }

    public TokenDbContext(DbContextOptions<TokenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBook> TblBooks { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Open-Close Principle [O in SOLID]
    {
        modelBuilder.Entity<TblBook>(entity =>    /* Fluent Api */
        {
            entity.HasKey(e => e.BookId);      /* Fluent Api */

            entity.ToTable("TblBook");          /* Fluent Api */

            entity.Property(e => e.BookId).HasColumnName("BookID");       /* Fluent Api */
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)                                  /* Fluent Api */
                .IsUnicode(false);
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);                              /* Fluent Api */
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("TblUser");

            entity.Property(e => e.UserId).HasColumnName("UserID");              /* Fluent Api */
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(128);
            entity.Property(e => e.Salt).HasMaxLength(128);                       /* Fluent Api */
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
