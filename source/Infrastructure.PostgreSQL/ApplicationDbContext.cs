using DirectoryService.Domain.DepartmentsContext;
using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PostgreSQL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация Location
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id).HasConversion(
                id => id.Value,
                value => LocationId.Create(value));
            
            entity.OwnsOne(l => l.Name, name =>
            {
                name.Property(n => n.Value).HasColumnName("Name");
            });
            
            entity.OwnsOne(l => l.Address, address =>
            {
                address.Property(a => a.Value).HasColumnName("Address");
            });
            
            entity.OwnsOne(l => l.TimeZone, timeZone =>
            {
                timeZone.Property(t => t.Value).HasColumnName("TimeZone");
            });
            
            // entity.OwnsOne(l => l.LifeTime, lifeTime =>
            // {
            //     lifeTime.Property(lt => lt.CreatedAt).HasColumnName("CreatedAt");
            //     lifeTime.Property(lt => lt.DeletedAt).HasColumnName("DeletedAt");
            // });
        });

        // Конфигурация Position
        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasConversion(
                id => id.Value,
                value => PositionId.Create(value));
            
            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Value).HasColumnName("Name");
            });
            
            entity.OwnsOne(p => p.Description, description =>
            {
                description.Property(d => d.Value).HasColumnName("Description");
            });
            
            // entity.OwnsOne(p => p.LifeTime, lifeTime =>
            // {
            //     lifeTime.Property(lt => lt.CreatedAt).HasColumnName("CreatedAt");
            //     lifeTime.Property(lt => lt.DeletedAt).HasColumnName("DeletedAt");
            // });
        });

        // Конфигурация Department
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).HasConversion(
                id => id.Value,
                value => DepartmentId.Create(value));
            
            entity.OwnsOne(d => d.Name, name =>
            {
                name.Property(n => n.Value).HasColumnName("Name");
            });
            
            entity.OwnsOne(d => d.Identifier, identifier =>
            {
                identifier.Property(i => i.Value).HasColumnName("Identifier");
            });
            
            entity.OwnsOne(d => d.Path, path =>
            {
                path.Property(p => p.Value).HasColumnName("Path");
            });
            
            entity.OwnsOne(d => d.Depth, depth =>
            {
                depth.Property(d => d.Value).HasColumnName("Depth");
            });
            
            // entity.OwnsOne(d => d.LifeTime, lifeTime =>
            // {
            //     lifeTime.Property(lt => lt.CreatedAt).HasColumnName("CreatedAt");
            //     lifeTime.Property(lt => lt.DeletedAt).HasColumnName("DeletedAt");
            // });
            
            entity.Property(d => d.ParentId).HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                value => value == null ? null : DepartmentId.Create(value.Value)
                );                
        });
    }
}