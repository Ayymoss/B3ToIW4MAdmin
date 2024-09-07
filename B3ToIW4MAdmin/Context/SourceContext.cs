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
                .HasColumnType("int(11) unsigned")
                .HasColumnName("id");

            entity.Property(e => e.Connections)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("connections")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Guid)
                .HasColumnType("varchar(36)")
                .HasColumnName("guid")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Ip)
                .HasColumnType("varchar(16)")
                .HasColumnName("ip")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name")
                .HasDefaultValueSql("''");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("time_add")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeEdit)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("time_edit")
                .HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<Penalty>(entity =>
        {
            entity.ToTable("penalties");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");

            entity.Property(e => e.Type)
                .HasColumnType("text")
                .HasColumnName("type")
                .HasDefaultValueSql("'Ban'");

            entity.Property(e => e.ClientId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("client_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.AdminId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("admin_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Inactive)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("inactive")
                .HasDefaultValueSql("0");

            entity.Property(e => e.Reason)
                .HasColumnType("varchar(255)")
                .HasColumnName("reason")
                .HasDefaultValueSql("''");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("time_add")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeExpire)
                .HasColumnType("int(11)")
                .HasColumnName("time_expire")
                .HasDefaultValueSql("'0'");
        });

        base.OnModelCreating(modelBuilder);
    }
}
