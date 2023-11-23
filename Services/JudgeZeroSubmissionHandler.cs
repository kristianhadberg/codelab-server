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
    
    /*public async Task<JudgeZeroSubmissionStatus> JudgeSubmission(SubmissionRequest submissionRequest)
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
    }*/
    
    public async Task<JudgeZeroSubmissionStatus> JudgeSubmission(SubmissionRequest submissionRequest)
    {
        string judge0Url = "http://localhost:2358/submissions/?base64_encoded=true&wait=true&fields=status,stdout";
        var exercise = await _dbContext.Exercises.FindAsync(submissionRequest.ExerciseId);

        string sourceCode = exercise.SourceCode;

        // Appends the users submitted code to the source code which
        // runs the source code together with the users code.
        string modifiedCode = AppendBeforeLastCharacter(sourceCode, submissionRequest.SubmittedCode);

        string base64EncodeSubmissionCode = Base64Encode(modifiedCode);
        string base64EncodeExpectedOutput = Base64Encode(exercise.ExpectedOutput);

        var requestBody = CreateRequestBody(base64EncodeSubmissionCode, base64EncodeExpectedOutput);
        
        // serialize to json
        string jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(judge0Url, content);
        response.EnsureSuccessStatusCode();

        // deserialize to SubmitJson object
        var submitJson = await response.Content.ReadFromJsonAsync<JudgeZeroSubmissionStatus>();
        
        // base64 decode output of result
        submitJson.Stdout = Base64Decode(submitJson.Stdout);
        
        return submitJson;
    }
    
    private static string AppendBeforeLastCharacter(string original, string toAppend)
    {
        int lastIndex = original.LastIndexOf('}');
        
        StringBuilder stringBuilder = new StringBuilder(original);
        stringBuilder.Insert(lastIndex, toAppend);
        return stringBuilder.ToString();
    }

    private static string Base64Encode(string stringToEncode)
    {
        byte[] submittedBytes = Encoding.UTF8.GetBytes(stringToEncode);
        return Convert.ToBase64String(submittedBytes);
    }

    private static string Base64Decode(string stringToDecode)
    {
        byte[] decodedBytes = Convert.FromBase64String(stringToDecode);
        return Encoding.UTF8.GetString(decodedBytes);
    }

    private static object CreateRequestBody(string sourceCode, string expectedOutput)
    {
        return new
        {
            source_code =
                sourceCode,
            language_id = 62, // Judge0 language id for Java
            expected_output = expectedOutput
        };
    }
}