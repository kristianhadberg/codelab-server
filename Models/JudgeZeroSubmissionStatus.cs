namespace codelab_exam_server.Models;

public class JudgeZeroSubmissionStatus
{
    public SubmitStatus Status {get; set; }
}

public class SubmitStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
}