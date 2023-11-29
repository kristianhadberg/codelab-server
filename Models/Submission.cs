namespace codelab_exam_server.Models;

public class Submission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SubmittedCode { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
    public int ExerciseId { get; set; }
}