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

            entity.HasIndex(e => e.Guid, "IX_clients_guid")
                .IsUnique();

            entity.HasIndex(e => e.GroupBits, "idx_clients_group_bits");

            entity.HasIndex(e => e.Name, "idx_clients_name");

            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .HasColumnName("id");

            entity.Property(e => e.AutoLogin)
                .HasColumnType("integer")
                .HasColumnName("auto_login")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.ChatMute)
                .HasColumnType("integer")
                .HasColumnName("chat_mute")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Connections)
                .HasColumnType("integer")
                .HasColumnName("connections")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Greeting)
                .HasColumnType("varchar(128)")
                .HasColumnName("greeting")
                .HasDefaultValueSql("''");

            entity.Property(e => e.GroupBits)
                .HasColumnType("integer")
                .HasColumnName("group_bits")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Guid)
                .HasColumnType("varchar(20)")
                .HasColumnName("guid");

            entity.Property(e => e.Ip)
                .HasColumnType("varchar(16)")
                .HasColumnName("ip")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Login)
                .HasColumnType("varchar(16)")
                .HasColumnName("login")
                .HasDefaultValueSql("''");

            entity.Property(e => e.MaskLevel)
                .HasColumnType("integer")
                .HasColumnName("mask_level")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Name)
                .HasColumnType("varchar(32)")
                .HasColumnName("name")
                .HasDefaultValueSql("''");

            entity.Property(e => e.OnlyWithClient)
                .HasColumnType("integer")
                .HasColumnName("only_with_client")
                .HasDefaultValueSql("'1'");

            entity.Property(e => e.Password)
                .HasColumnType("varchar(32)")
                .HasColumnName("password")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Pbid)
                .HasColumnType("varchar(32)")
                .HasColumnName("pbid")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Regular)
                .HasColumnType("integer")
                .HasColumnName("regular")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Show)
                .HasColumnType("integer")
                .HasColumnName("show")
                .HasDefaultValueSql("'1'");

            entity.Property(e => e.Spammer)
                .HasColumnType("integer")
                .HasColumnName("spammer")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("varchar(11)")
                .HasColumnName("time_add")
                .HasDefaultValueSql("''");

            entity.Property(e => e.TimeEdit)
                .HasColumnType("integer")
                .HasColumnName("time_edit")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TrgAdmin)
                .HasColumnType("integer")
                .HasColumnName("trg_admin")
                .HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<Penalty>(entity =>
        {
            entity.ToTable("penalties");

            entity.HasIndex(e => e.AdminId, "idx_penalties_admin_id");

            entity.HasIndex(e => e.ClientId, "idx_penalties_client_id");

            entity.HasIndex(e => e.Inactive, "idx_penalties_inactive");

            entity.HasIndex(e => e.Keyword, "idx_penalties_keyword");

            entity.HasIndex(e => e.TimeAdd, "idx_penalties_time_add");

            entity.HasIndex(e => e.TimeExpire, "idx_penalties_time_expire");

            entity.HasIndex(e => e.Type, "idx_penalties_type");

            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .HasColumnName("id");

            entity.Property(e => e.AdminId)
                .HasColumnType("integer")
                .HasColumnName("admin_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.ClientId)
                .HasColumnType("integer")
                .HasColumnName("client_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Data)
                .HasColumnType("varchar(255)")
                .HasColumnName("data")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Duration)
                .HasColumnType("integer")
                .HasColumnName("duration")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.EditAdminId)
                .HasColumnType("integer")
                .HasColumnName("edit_admin_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.EditReason)
                .HasColumnType("varchar(255)")
                .HasColumnName("edit_reason")
                .HasDefaultValueSql("''");

            entity.Property(e => e.EditTime)
                .HasColumnType("integer")
                .HasColumnName("edit_time")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Inactive)
                .HasColumnType("integer")
                .HasColumnName("inactive")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.InactiveAdminId)
                .HasColumnType("integer")
                .HasColumnName("inactive_admin_id")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Keyword)
                .HasColumnType("varchar(16)")
                .HasColumnName("keyword")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Reason)
                .HasColumnType("varchar(255)")
                .HasColumnName("reason")
                .HasDefaultValueSql("''");

            entity.Property(e => e.Server)
                .HasColumnType("varchar(12)")
                .HasColumnName("server")
                .HasDefaultValueSql("'N/A'");

            entity.Property(e => e.TimeAdd)
                .HasColumnType("integer")
                .HasColumnName("time_add")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeEdit)
                .HasColumnType("integer")
                .HasColumnName("time_edit")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeExpire)
                .HasColumnType("integer")
                .HasColumnName("time_expire")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.TimeInactive)
                .HasColumnType("integer")
                .HasColumnName("time_inactive")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.Type)
                .HasColumnType("text")
                .HasColumnName("type")
                .HasDefaultValueSql("'Ban'");
        });

        base.OnModelCreating(modelBuilder);
    }
}
