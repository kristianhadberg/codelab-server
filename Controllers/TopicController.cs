using codelab_exam_server.Dtos;
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
    public async Task<IEnumerable<TopicDto>> GetAll()
    {
        return await _topicService.GetAllTopics();
    }
    
    // GET: api/Topic/5
    [HttpGet("{id}")]
    public async Task<TopicDto> Get(int id)
    {
        return await _topicService.GetTopicById(id);
    }
    
    //PUT
    [HttpPut("{id}")]
    public async Task<TopicDto> Put(int id, TopicDto topicDto)
    {
        return await _topicService.UpdateTopic(id, topicDto);
    }
    
    // POST: api/Topic
    [HttpPost]
    public async Task<ActionResult<TopicDto>> Post(TopicDto topicDto)
    {
        return await _topicService.CreateTopic(topicDto);
    }
    
    // DELETE
    [HttpDelete("{id}")]
    public async Task<TopicDto> Delete(int id)
    {
        return await _topicService.DeleteTopic(id);
    }
    
}