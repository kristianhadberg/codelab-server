namespace codelab_exam_server.Models;

public class LearningPath
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Topic> Topics { get; set; }
}