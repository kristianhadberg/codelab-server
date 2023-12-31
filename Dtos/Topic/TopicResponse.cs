using codelab_exam_server.Dtos.Exercise;

namespace codelab_exam_server.Dtos.Topic;

public class TopicResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public List<ExerciseResponse> Exercises { get; set; }
    
}