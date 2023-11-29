namespace codelab_exam_server.Dtos.Submission;

public class SubmissionResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SubmittedCode { get; set; }
    public DateTime SubmissionDate { get; set; }
    public int ExerciseId { get; set; }
}