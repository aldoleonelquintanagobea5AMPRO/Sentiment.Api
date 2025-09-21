using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sentiment.Api.Models;

public partial class AppDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    // Constructor que recibe opciones DI
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Crea la conexión sólo si aún no está configurado (i.e. no se pasó por AddDbContext)
        if (!optionsBuilder.IsConfigured)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("No configuration available to configure DbContext.");
            }

            var connStr = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");
            }
            optionsBuilder.UseSqlServer(connStr);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3213E83F533CD5ED");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Sentiment)
                .HasMaxLength(20)
                .HasColumnName("sentiment");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
