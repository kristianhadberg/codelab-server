using System.Text;
using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Submission;
using codelab_exam_server.Models;
using Newtonsoft.Json;

namespace codelab_exam_server.Services;

public class JudgeZeroSubmissionHandler
{
    private readonly HttpClient _httpClient;
    private readonly DatabaseContext _dbContext;

    public JudgeZeroSubmissionHandler(HttpClient httpClient, DatabaseContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }
    
    public async Task<JudgeZeroSubmissionStatus> JudgeSubmission(SubmissionRequest submissionRequest)
    {
        string url = "http://localhost:2358/submissions/?base64_encoded=true&wait=true&fields=status";
        
        // BASE64 encode the submission & the expected output
        string submittedCode = submissionRequest.SubmittedCode;
        byte[] submittedBytes = Encoding.UTF8.GetBytes(submittedCode);
        string base64SubmittedString = Convert.ToBase64String(submittedBytes);

        var exercise = await _dbContext.Exercises.FindAsync(submissionRequest.ExerciseId);
        byte[] expectedBytes = Encoding.UTF8.GetBytes(exercise.ExpectedOutput);
        string base64ExpectedString = Convert.ToBase64String(expectedBytes);
        
        var body = new
        {
            source_code =
                base64SubmittedString,
            language_id = 62, // Judge zero language id for Java
            expected_output = base64ExpectedString
        };
        
        // serialize to json
        string jsonContent = JsonConvert.SerializeObject(body);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        // deserialize to SubmitJson object
        var submitJson = await response.Content.ReadFromJsonAsync<JudgeZeroSubmissionStatus>();
        
        return submitJson;
    }
}