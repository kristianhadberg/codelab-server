namespace codelab_exam_server.Dtos.Submission;

public class SubmissionRequest
{
    public string SubmittedCode { get; set; }
    public int UserId { get; set; }
    public int ExerciseId { get; set; }
}