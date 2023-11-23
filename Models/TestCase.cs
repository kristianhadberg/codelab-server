namespace codelab_exam_server.Models;

public class TestCase
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string Input { get; set; }
    public string ExpectedOutput { get; set; }
}