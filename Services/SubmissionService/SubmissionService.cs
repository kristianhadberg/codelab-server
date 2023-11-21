using System.Text;
using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Submission;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace codelab_exam_server.Services.SubmissionService;

public class SubmissionService : ISubmissionService
{
    private readonly DatabaseContext _dbContext;
    private readonly JudgeZeroSubmissionHandler _judgeZeroSubmissionHandler;

    public SubmissionService(DatabaseContext dbContext, JudgeZeroSubmissionHandler judgeZeroSubmissionHandler)
    {
        _dbContext = dbContext;
        _judgeZeroSubmissionHandler = judgeZeroSubmissionHandler;
    }
    
    public async Task<IEnumerable<SubmissionResponse>> GetAllSubmissions()
    {
        var submissions = await _dbContext.Submissions.ToListAsync();

        return submissions.Select(s => SubmissionToResponse(s)).ToList();
    }

    public async Task<SubmissionResponse> GetSubmissionById(int id)
    {
        var submission = await _dbContext.Submissions.FindAsync(id);
        if (submission == null)
        {
            throw new Exception("Exercise with given id not found.");
        }

        return SubmissionToResponse(submission);
    }

    public async Task<IEnumerable<SubmissionResponse>> GetAllSubmissionsByExerciseId(int exerciseId)
    {
        var submissions = await _dbContext.Submissions
            .AsNoTracking()
            .Where(s => s.ExerciseId == exerciseId)
            .Select(s => SubmissionToResponse(s)).ToListAsync();

        return submissions;
    }

    public async Task<SubmissionResponse> CreateSubmission(SubmissionRequest submissionRequest)
    {
        var submitJson = await _judgeZeroSubmissionHandler.JudgeSubmission(submissionRequest);
        if (submitJson.Status.Description != "Accepted")
        {
            throw new Exception("Submission not accepted");
        }
        
        var submission = ToEntity(submissionRequest);
        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();

        return SubmissionToResponse(submission);
    }

    public Task<SubmissionResponse> UpdateSubmission(int id, SubmissionRequest submissionRequest)
    {
        throw new NotImplementedException();
    }

    public Task<SubmissionResponse> DeleteSubmission(int id)
    {
        throw new NotImplementedException();
    }

    /*public async Task<SubmitJson> TestRequest()
    {
        string url = "http://localhost:2358/submissions/?base64_encoded=true&wait=true&fields=status";
        var body = new
        {
            source_code =
                "cHVibGljIGNsYXNzIE1haW57CiAgICAgcHVibGljIHN0YXRpYyB2b2lkIG1haW4oU3RyaW5nIFtdYXJncyl7CiAgICAgICAgU3lzdGVtLm91dC5wcmludGxuKCJIZWxsbywgV29ybGQhIik7CiAgICAgfQp9",
            language_id = 62,
            expected_output = "SGVsbG8sIFdvcmxkIQ=="
        };
        
        // serialize to json
        string jsonContent = JsonConvert.SerializeObject(body);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        // deserialize to SubmitJson object
        var submitJson = await response.Content.ReadFromJsonAsync<SubmitJson>();
        
        return submitJson;
    }*/

    private static SubmissionResponse SubmissionToResponse(Submission submission) =>
        new SubmissionResponse()
        {
            Id = submission.Id,
            SubmittedCode = submission.SubmittedCode,
            SubmissionDate = submission.SubmissionDate,
            ExerciseId = submission.ExerciseId
        };

    private static Submission ToEntity(SubmissionRequest submissionRequest) =>
        new Submission()
        {
            SubmittedCode = submissionRequest.SubmittedCode,
            ExerciseId = submissionRequest.ExerciseId
        };
}