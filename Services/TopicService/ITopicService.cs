
using codelab_exam_server.Dtos;
using codelab_exam_server.Dtos.Topic;

namespace codelab_exam_server.Services.TopicService;

public interface ITopicService
{
    Task<IEnumerable<TopicResponse>> GetAllTopics();
    Task<TopicResponse> GetTopicById(int id);
    Task<IEnumerable<TopicResponse>> GetTopicsByLearningPathId(int learningPathId);
    Task<TopicResponse> CreateTopic(TopicRequest topicRequest);
    Task<TopicResponse> UpdateTopic(int id, TopicRequest topicRequest);
    Task<TopicResponse> DeleteTopic(int id);
    
}