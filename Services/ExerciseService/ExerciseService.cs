using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Exercise;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;

namespace codelab_exam_server.Services.ExerciseService;

public class ExerciseService : IExerciseService
{
    private readonly DatabaseContext _dbContext;
    
    public ExerciseService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<ExerciseResponse>> GetAllExercises()
    {
        /*var exercises = await _dbContext.Exercises.ToListAsync();*/
        var exercises = await _dbContext.Exercises.Include(e => e.TestCases).ToListAsync();

        return exercises.Select(t => ExerciseToResponse(t)).ToList();
    }

    public async Task<ExerciseResponse> GetExerciseById(int id)
    {
        var exercise = await _dbContext.Exercises.Include(e => e.TestCases).FirstOrDefaultAsync(e => e.Id == id);
        /*var exercise = await _dbContext.Exercises.FindAsync(id);*/
        if (exercise == null)
        {
            throw new Exception("Exercise with given id not found.");
        }

        return ExerciseToResponse(exercise);
    }

    public async Task<IEnumerable<ExerciseResponse>> GetAllExercisesByTopicId(int topicId)
    {
        var exercises = await _dbContext.Exercises
            .Include(e => e.TestCases)
            .AsNoTracking()
            .Where(e => e.TopicId == topicId)
            .Select(e => ExerciseToResponse(e)).ToListAsync();
        
        return exercises;
    }

    public async Task<ExerciseResponse> CreateExercise(ExerciseRequest exerciseRequest)
    {
        var exercise = ToEntity(exerciseRequest);
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync();

        return ExerciseToResponse(exercise);
    }

    public Task<ExerciseResponse> UpdateExercise(int id, ExerciseRequest exerciseRequest)
    {
        throw new NotImplementedException();
    }

    public Task<ExerciseResponse> DeleteExercise(int id)
    {
        throw new NotImplementedException();
    }
    
    private static ExerciseResponse ExerciseToResponse(Exercise exercise) =>
        new ExerciseResponse()
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            TopicId = exercise.TopicId,
            StarterCode = exercise.StarterCode,
            ExpectedOutput = exercise.ExpectedOutput,
            TestCases = exercise.TestCases
        };

    private static Exercise ToEntity(ExerciseRequest exerciseRequest) =>
        new Exercise()
        {
            Name = exerciseRequest.Name,
            Description = exerciseRequest.Description,
            TopicId = exerciseRequest.TopicId,
            SourceCode = exerciseRequest.SourceCode,
            StarterCode = exerciseRequest.StarterCode,
            ExpectedOutput = exerciseRequest.ExpectedOutput
        };
}