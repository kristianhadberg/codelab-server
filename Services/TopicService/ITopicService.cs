
using codelab_exam_server.Dtos;

namespace codelab_exam_server.Services.TopicService;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetAllTopics();
    Task<TopicDto> GetTopicById(int id);
    Task<TopicDto> CreateTopic(TopicDto topicDto);
    Task<TopicDto> UpdateTopic(int id, TopicDto topicDto);
    Task<TopicDto> DeleteTopic(int id);
    
}