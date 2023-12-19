namespace codelab_exam_server.Dtos.LearningPath;

public class LearningPathProgressResponse
{
    public int CompletedExercises { get; set; }
    public int TotalExercises { get; set; }
    public decimal PercentageCompleted  { get; set; }
}