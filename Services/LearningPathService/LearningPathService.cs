using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Exercise;
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
                .ThenInclude(t => t.Exercises)
            .ToListAsync();

        return learningPaths.Select(lp => LearningPathToResponse(lp)).ToList();

    }
    
    public async Task<LearningPathResponse> GetLearningPathById(int id)
    {
        var learningPath = await _dbContext.LearningPaths
            .Include(lp => lp.Topics)
                .ThenInclude(t => t.Exercises.Where(e => e.IsLearningPathExercise))
            .FirstOrDefaultAsync(lp => lp.Id == id);
        if (learningPath == null)
        {
            throw new Exception("Learning path with given id not found.");
        }

        return LearningPathToResponse(learningPath);
    }
    
    public async Task<LearningPathResponse> GetLearningPathByIdAndUserId(int id, int userId)
    {
        var learningPath = await _dbContext.LearningPaths
            .Include(lp => lp.Topics)
            .ThenInclude(t => t.Exercises.Where(e => e.IsLearningPathExercise))
            .ThenInclude(e => e.UserExerciseProgresses.Where(uep => uep.UserId == userId))
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

    /*
     * Mapping methods
     */
    private static LearningPathResponse LearningPathToResponse(LearningPath lp) =>
        new LearningPathResponse()
        {
            Id = lp.Id,
            Name = lp.Name,
            Topics = lp.Topics.Select(topic => TopicToResponse(topic)).ToList() ?? new List<TopicResponse>()
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
            Description = topic.Description,
            Exercises = topic.Exercises.Select(e => ExerciseToResponse(e)).ToList()
        };
    
    private static ExerciseResponse ExerciseToResponse(Exercise exercise)
    {
        var isCompleted = exercise.UserExerciseProgresses?.Any(uep => uep.ExerciseId == exercise.Id) == true;
    
        return new ExerciseResponse()
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            TopicId = exercise.TopicId,
            StarterCode = exercise.StarterCode,
            ExpectedOutput = exercise.ExpectedOutput,
            TestCases = exercise.TestCases,
            SubmissionCount = exercise.Submissions.Count,
            IsLearningPathExercise = exercise.IsLearningPathExercise,
            IsCompleted = isCompleted
        };
    }
}