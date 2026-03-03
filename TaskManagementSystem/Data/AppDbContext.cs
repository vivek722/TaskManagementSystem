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
        // Configure relationships and constraints if needed


        modelBuilder.Entity<TaskManage>()
            .HasOne(e => e.employeeModel)
            .WithMany(t => t.TaskManage)
            .HasForeignKey(e => e.employeeId);
        modelBuilder.Entity<projectModel>()
            .HasOne(e => e.employeeModel)
            .WithMany(p => p.ProjectModel)
            .HasForeignKey(e => e.employeeId);
        modelBuilder.Entity<TaskManage>()
           .HasOne(e => e.ProjectModel)
           .WithMany(p => p.TaskManages)
           .HasForeignKey(e => e.projectId);
        modelBuilder.Entity<SubTaskManeg>()
            .HasOne(s => s.TaskManage)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(s => s.TaskManageid);
    }
}
