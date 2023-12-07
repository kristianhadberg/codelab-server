using codelab_exam_server.Data;
using codelab_exam_server.Dtos.LearningPath;
using codelab_exam_server.Dtos.Topic;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;

namespace codelab_exam_server.Services.LearningPathService;

public class LearningPathService : ILearningPathService
{
    private readonly DatabaseContext _dbContext;

    public LearningPathService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<LearningPathResponse>> GetAllLearningPaths()
    {
        var learningPaths = await _dbContext.LearningPaths
            .Include(lp => lp.Topics)
            .ToListAsync();

        return learningPaths.Select(lp => LearningPathToResponse(lp)).ToList();

    }
    
    public async Task<LearningPathResponse> GetLearningPathById(int id)
    {
        var learningPath = await _dbContext.LearningPaths
            .FirstOrDefaultAsync(lp => lp.Id == id);
        if (learningPath == null)
        {
            throw new Exception("Learning path with given id not found.");
        }

        return LearningPathToResponse(learningPath);
    }
    
    public async Task<LearningPathResponse> CreateLearningPath(LearningPathRequest learningPathRequest)
    {
        var learningPath = ToEntity(learningPathRequest);
        _dbContext.LearningPaths.Add(learningPath);
        await _dbContext.SaveChangesAsync();

        return LearningPathToResponse(learningPath);
    }

    private static LearningPathResponse LearningPathToResponse(LearningPath lp) =>
        new LearningPathResponse()
        {
            Id = lp.Id,
            Name = lp.Name,
            TopicResponses = lp.Topics.Select(topic=> TopicToResponse(topic)).ToList() ?? new List<TopicResponse>()
        };
    
    private static LearningPath ToEntity(LearningPathRequest learningPathRequest) =>
        new LearningPath()
        {
            Name = learningPathRequest.Name
        };
    
    private static TopicResponse TopicToResponse(Topic topic) =>
        new TopicResponse()
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description
        };
}