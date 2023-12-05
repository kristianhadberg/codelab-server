using codelab_exam_server.Dtos.Topic;
using codelab_exam_server.Services.TopicService;
using Microsoft.AspNetCore.Mvc;

namespace codelab_exam_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;
    
    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }
    
    // GET: api/topic
    [HttpGet]
    public async Task<IEnumerable<TopicResponse>> GetAll()
    {
        return await _topicService.GetAllTopics();
    }
    
    // GET: api/topic/5
    [HttpGet("{id}")]
    public async Task<TopicResponse> Get(int id)
    {
        return await _topicService.GetTopicById(id);
    }
    
    // GET: api/topics/learning-path/1
    [HttpGet("learning-path/{learningPathId}")]
    public async Task<IEnumerable<TopicResponse>> GetAllByLearningPathId(int learningPathId)
    {
        return await _topicService.GetTopicsByLearningPathId(learningPathId);
    }
    
    //PUT
    [HttpPut("{id}")]
    public async Task<TopicResponse> Put(int id, TopicRequest topicRequest)
    {
        return await _topicService.UpdateTopic(id, topicRequest);
    }
    
    // POST: api/topic
    [HttpPost]
    public async Task<ActionResult<TopicResponse>> Post(TopicRequest topicRequest)
    {
        return await _topicService.CreateTopic(topicRequest);
    }
    
    // DELETE
    [HttpDelete("{id}")]
    public async Task<TopicResponse> Delete(int id)
    {
        return await _topicService.DeleteTopic(id);
    }
    
}