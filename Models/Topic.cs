namespace codelab_exam_server.Models;

public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<LearningPath> LearningPaths { get; set; }
    public IList<Exercise> Exercises { get; } = new List<Exercise>();

}