namespace codelab_exam_server.Models;

public class UserExerciseProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    public bool IsCompleted { get; set; }
}