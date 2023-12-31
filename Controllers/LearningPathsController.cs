using codelab_exam_server.Dtos.LearningPath;
using codelab_exam_server.Services.LearningPathService;
using Microsoft.AspNetCore.Mvc;

namespace codelab_exam_server.Controllers;

[Route("api/learning-paths")]
[ApiController]
public class LearningPathsController
{
    private readonly ILearningPathService _learningPathService;

    public LearningPathsController(ILearningPathService learningPathService)
    {
        _learningPathService = learningPathService;
    }
    
    // GET: api/learning-paths
    [HttpGet]
    public async Task<IEnumerable<LearningPathResponse>> GetAll()
    {
        return await _learningPathService.GetAllLearningPaths();
    }
    
    // GET: api/learning-paths/1
    [HttpGet("{id}")]
    public async Task<LearningPathResponse> Get(int id)
    {
        return await _learningPathService.GetLearningPathById(id);
    }
    
    // GET: api/learning-paths/1/1
    [HttpGet("{id}/{userId}")]
    public async Task<LearningPathResponse> GetByIdAndUserId(int id, int userId)
    {
        return await _learningPathService.GetLearningPathByIdAndUserId(id, userId);
    }
    
    // GET: api/learning-paths/progress/1/1
    [HttpGet("progress/{id}/{userId}")]
    public async Task<LearningPathProgressResponse> GetProgressByIdAndUserId(int id, int userId)
    {
        return await _learningPathService.GetLearningPathProgressByIdAndUserId(id, userId);
    }
    
    // POST: api/exercises
    [HttpPost]
    public async Task<ActionResult<LearningPathResponse>> Post(LearningPathRequest learningPathRequest)
    {
        return await _learningPathService.CreateLearningPath(learningPathRequest);
    }
}