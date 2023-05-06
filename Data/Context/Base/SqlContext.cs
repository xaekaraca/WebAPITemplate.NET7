using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context.Base;

public class SqlContext : DbContext
{
    public SqlContext () { }
    
    public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //implement seed data method and create method outside this class as SqlContextSeedDataCreator or something like that
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder().Build();

        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlContext"));
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaveChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        OnBeforeSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        //User id will be setting from session

        OnBeforeAddedEntities();
        OnBeforeModifiedEntities();
    }

    private void OnBeforeAddedEntities()
    {
        var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        foreach (var item in addedEntities)
        {
            if (item.Entity.GetType().GetProperty("CreatedAt") != null)
                item.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            if (item.Entity.GetType().GetProperty("ChangedAt") != null)
                item.Property("ChangedAt").CurrentValue = item.Property("CreatedAt").CurrentValue;
        }
    }

    private void OnBeforeModifiedEntities()
    {
        var editedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        foreach (var item in editedEntities)
        {
            if (item.Entity.GetType().GetProperty("CreatedAt") != null)
                item.Property("CreatedAt").CurrentValue = (DateTime)(item.Property("CreatedAt").OriginalValue ?? DateTime.MinValue) == DateTime.MinValue ? DateTime.UtcNow : item.Property("CreatedAt").OriginalValue;
            if (item.Entity.GetType().GetProperty("ChangedAt") != null)
                item.Property("ChangedAt").CurrentValue = DateTime.UtcNow;
        }
    }
}