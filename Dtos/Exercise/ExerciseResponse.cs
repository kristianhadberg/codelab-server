namespace codelab_exam_server.Dtos.Exercise;

public class ExerciseResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TopicId { get; set; }
    public string StarterCode { get; set; }
    public string ExpectedOutput { get; set; }
}