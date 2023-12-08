using codelab_exam_server.Dtos.Exercise;
using Microsoft.AspNetCore.Mvc;
using codelab_exam_server.Services.ExerciseService;


namespace codelab_exam_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }
    
    // GET: api/exercises
    [HttpGet]
    public async Task<IEnumerable<ExerciseResponse>> GetAll()
    {
        return await _exerciseService.GetAllExercises();
    }
    
    // GET: api/exercises/5
    [HttpGet("{id}")]
    public async Task<ExerciseResponse> Get(int id)
    {
        return await _exerciseService.GetExerciseById(id);
    }
    
    // GET: api/exercises/topic/1
    [HttpGet("topic/{topicId}")]
    public async Task<IEnumerable<ExerciseResponse>> GetByTopicId(int topicId)
    {
        return await _exerciseService.GetAllExercisesByTopicId(topicId);
    }
    
    // GET: api/exercises/topic/
    [HttpGet("topic/{topicId}/{userId}")]
    public async Task<IEnumerable<ExerciseResponse>> GetAllByTopicIdAndUserId(int topicId, int userId)
    {
        return await _exerciseService.GetAllByTopicIdAndUserId(topicId, userId);
    }
    
    //PUT
    [HttpPut("{id}")]
    public async Task<ExerciseResponse> Put(int id, ExerciseRequest exerciseRequest)
    {
        return await _exerciseService.UpdateExercise(id, exerciseRequest);
    }
    
    // POST: api/exercises
    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> Post(ExerciseRequest exerciseRequest)
    {
        return await _exerciseService.CreateExercise(exerciseRequest);
    }
    
    // DELETE
    [HttpDelete("{id}")]
    public async Task<ExerciseResponse> Delete(int id)
    {
        return await _exerciseService.DeleteExercise(id);
    }
}