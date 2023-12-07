
using codelab_exam_server.Dtos.Topic;

namespace codelab_exam_server.Dtos.LearningPath;

public class LearningPathResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TopicResponse> TopicResponses { get; set; }
}