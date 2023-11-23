namespace codelab_exam_server.Models;

public class Submission
{
    public int Id { get; set; }
    public string SubmittedCode { get; set; }
    public DateTime SubmissionDate { get; } = DateTime.Today;
    public int ExerciseId { get; set; }
}