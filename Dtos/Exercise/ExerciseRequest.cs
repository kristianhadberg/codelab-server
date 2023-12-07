namespace codelab_exam_server.Dtos.Exercise;

public class ExerciseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int TopicId { get; set; }
    public string? SourceCode { get; set; }
    public string StarterCode { get; set; }
    public string ExpectedOutput { get; set; }
    public bool IsLearningPathExercise { get; set; }
}