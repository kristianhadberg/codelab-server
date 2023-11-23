using codelab_exam_server.Dtos.Submission;
using codelab_exam_server.Models;
using codelab_exam_server.Services.SubmissionService;
using Microsoft.AspNetCore.Mvc;

namespace codelab_exam_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubmissionsController
{
    private readonly ISubmissionService _submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet]
    public async Task<IEnumerable<SubmissionResponse>> GetAll()
    {
        return await _submissionService.GetAllSubmissions();
    }
    
    [HttpGet("{id}")]
    public async Task<SubmissionResponse> Get(int id)
    {
        return await _submissionService.GetSubmissionById(id);
    }
    
    [HttpGet("exercise/{exerciseId}")]
    public async Task<IEnumerable<SubmissionResponse>> GetAllByExerciseId(int exerciseId)
    {
        return await _submissionService.GetAllSubmissionsByExerciseId(exerciseId);
    }

    [HttpPut("{id}")]
    public async Task<SubmissionResponse> Put(int id, SubmissionRequest submissionRequest)
    {
        return await _submissionService.UpdateSubmission(id, submissionRequest);
    }

    [HttpPost]
    public async Task<ActionResult<JudgeZeroSubmissionStatus>> Post(SubmissionRequest submissionRequest)
    {
        return await _submissionService.CreateSubmission(submissionRequest);
    }

    [HttpDelete("{id}")]
    public async Task<SubmissionResponse> Delete(int id)
    {
        return await _submissionService.DeleteSubmission(id);
    }
    
}