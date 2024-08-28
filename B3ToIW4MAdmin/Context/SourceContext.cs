using B3ToIW4MAdmin.Entities;
using Microsoft.EntityFrameworkCore;

namespace B3ToIW4MAdmin.Context;

public class SourceContext : DbContext
{
    public SourceContext(DbContextOptions<SourceContext> options) : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Penalty> Penalties { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");

            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .HasColumnName("id");

            entity.Property(e => e.Connections)
                .HasColumnType("integer")
                .HasColumnName("connections");

            entity.Property(e => e.Guid)
                .HasColumnType("varchar(20)")
                .HasColumnName("guid");

            entity.Property(e => e.Ip)
                .HasColumnType("varchar(16)")
                .HasColumnName("ip");

            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("varchar(11)")
                .HasColumnName("time_add");

            entity.Property(e => e.TimeEdit)
                .HasColumnType("integer")
                .HasColumnName("time_edit");
        });

        modelBuilder.Entity<Penalty>(entity =>
        {
            entity.ToTable("penalties");

            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .HasColumnName("id");

            entity.Property(e => e.Type)
                .HasColumnType("text")
                .HasColumnName("type");

            entity.Property(e => e.ClientId)
                .HasColumnType("integer")
                .HasColumnName("client_id");

            entity.Property(e => e.AdminId)
                .HasColumnType("integer")
                .HasColumnName("admin_id");

            entity.Property(e => e.Reason)
                .HasColumnType("varchar(255)")
                .HasColumnName("reason");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("integer")
                .HasColumnName("time_add");

            entity.Property(e => e.TimeExpire)
                .HasColumnType("integer")
                .HasColumnName("time_expire");
        });

        base.OnModelCreating(modelBuilder);
    }
}
