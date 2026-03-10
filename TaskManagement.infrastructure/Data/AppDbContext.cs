using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<TaskManage> TaskManages { get; set; }
    public DbSet<SubTaskManeg> SubTaskManegs { get; set; }
    public DbSet<projectModel> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TaskManage>()
             .HasOne(t => t.employeeModel)
             .WithMany(e => e.TaskManage)
             .HasForeignKey(t => t.employeeId)
             .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<projectModel>()
             .HasOne(p => p.employeeModel)
             .WithMany(e => e.ProjectModel)
             .HasForeignKey(p => p.employeeId)
             .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<TaskManage>()
            .HasOne(t => t.ProjectModel)
            .WithMany(p => p.TaskManages)
            .HasForeignKey(t => t.projectId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SubTaskManeg>()
              .HasOne(s => s.TaskManage)
              .WithMany(t => t.SubTasks)
              .HasForeignKey(s => s.TaskManageid)
              .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
