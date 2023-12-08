using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;

namespace codelab_exam_server.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LearningPath> LearningPaths { get; set; }
    public DbSet<UserExerciseProgress> UserExerciseProgresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LearningPath>()
            .HasMany(e => e.Topics)
            .WithMany(e => e.LearningPaths)
            .UsingEntity(j => j.ToTable("LearningPathTopic"));
    }
    
}