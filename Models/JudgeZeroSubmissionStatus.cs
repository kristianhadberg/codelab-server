namespace codelab_exam_server.Models;

public class JudgeZeroSubmissionStatus
{
    public string Stdout { get; set; }
    public SubmitStatus Status {get; set; }
}

public class SubmitStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
}