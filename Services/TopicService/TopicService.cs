using Azure.Core;
using codelab_exam_server.Data;
using codelab_exam_server.Dtos;
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
    
    public async Task<IEnumerable<TopicDto>> GetAllTopics()
    {
        var topics = await _dbContext.Topics.ToListAsync();
        
        return topics.Select(t => TopicToDto(t)).ToList();
    }

    public async Task<TopicDto> GetTopicById(int id)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        return TopicToDto(topic);
    }

    public async Task<TopicDto> CreateTopic(TopicDto topicDto)
    {
        var topic = ToEntity(topicDto);
        
        _dbContext.Topics.Add(topic);
        await _dbContext.SaveChangesAsync();

        return topicDto;
    }   

    public async Task<TopicDto> UpdateTopic(int id, TopicDto topicDto)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        topic.Name = topicDto.Name;

        await _dbContext.SaveChangesAsync();

        return topicDto;
    }

    public async Task<TopicDto> DeleteTopic(int id)
    {
        var topic = await _dbContext.Topics.FindAsync(id);
        if (topic == null)
        {
            throw new Exception("Topic with given id not found.");
        }

        _dbContext.Topics.Remove(topic);
        await _dbContext.SaveChangesAsync();

        return TopicToDto(topic);
    }

    private static TopicDto TopicToDto(Topic topic) =>
        new TopicDto()
        {
            Id = topic.Id,
            Name = topic.Name
        };

    private static Topic ToEntity(TopicDto topicDto) =>
        new Topic()
        {
            Id = topicDto.Id,
            Name = topicDto.Name
        };
}