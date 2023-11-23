namespace codelab_exam_server.Models;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TopicId { get; set; }
    public string SourceCode { get; set; }
    public string StarterCode { get; set; }
    public string ExpectedOutput { get; set; }
    public IList<Submission> Submissions { get; } = new List<Submission>();
}