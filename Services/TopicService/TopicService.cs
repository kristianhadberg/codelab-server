using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Topic;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;

namespace codelab_exam_server.Services.TopicService;

public class TopicService : ITopicService
{
    private readonly DatabaseContext _dbContext;
    
    public TopicService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<TopicResponse>> GetAllTopics()
    {
        var topics = await _dbContext.Topics.ToListAsync();
        
        return topics.Select(t => TopicToResponse(t)).ToList();
    }

    public async Task<TopicResponse> GetTopicById(int id)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        return TopicToResponse(topic);
    }

    public async Task<TopicResponse> CreateTopic(TopicRequest topicRequest)
    {
        var topic = ToEntity(topicRequest);
        
        _dbContext.Topics.Add(topic);
        await _dbContext.SaveChangesAsync();

        return TopicToResponse(topic);
    }   

    public async Task<TopicResponse> UpdateTopic(int id, TopicRequest topicRequest)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        topic.Name = topicRequest.Name;

        await _dbContext.SaveChangesAsync();

        return TopicToResponse(topic);
    }

    public async Task<TopicResponse> DeleteTopic(int id)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        _dbContext.Topics.Remove(topic);
        await _dbContext.SaveChangesAsync();

        return TopicToResponse(topic);
    }

    private static TopicResponse TopicToResponse(Topic topic) =>
        new TopicResponse()
        {
            Id = topic.Id,
            Name = topic.Name
        };

    private static Topic ToEntity(TopicRequest topicRequest) =>
        new Topic()
        {
            Name = topicRequest.Name
        };
}