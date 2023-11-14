using codelab_exam_server.Dtos.Topic;
using codelab_exam_server.Services.TopicService;
using Microsoft.AspNetCore.Mvc;

namespace codelab_exam_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;
    
    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }
    
    // GET: api/Topic
    [HttpGet]
    public async Task<IEnumerable<TopicResponse>> GetAll()
    {
        return await _topicService.GetAllTopics();
    }
    
    // GET: api/Topic/5
    [HttpGet("{id}")]
    public async Task<TopicResponse> Get(int id)
    {
        return await _topicService.GetTopicById(id);
    }
    
    //PUT
    [HttpPut("{id}")]
    public async Task<TopicResponse> Put(int id, TopicRequest topicRequest)
    {
        return await _topicService.UpdateTopic(id, topicRequest);
    }
    
    // POST: api/Topic
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