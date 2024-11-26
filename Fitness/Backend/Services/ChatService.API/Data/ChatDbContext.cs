using Microsoft.EntityFrameworkCore;

namespace ChatService.API.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<MessageEntity> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MessageEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("GETDATE()");
        });
    }
}