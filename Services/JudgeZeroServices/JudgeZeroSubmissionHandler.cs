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
        string judge0Url = "http://localhost:2358/submissions/?base64_encoded=true&wait=true&fields=status,stdout";
        var exercise = await _dbContext.Exercises.FindAsync(submissionRequest.ExerciseId);

        string sourceCode = exercise.SourceCode;
        string modifiedCode = "";
        if (sourceCode == null)
        {
            // if there is not source code to append user code to,
            // run all the user code
            // this is for cases where the user writes their own Main class (i.e, writes the entire program)
            modifiedCode = submissionRequest.SubmittedCode;
        }
        else
        {
            // Appends the users submitted code to the source code which
            // runs the source code together with the users code.
             modifiedCode = AppendBeforeLastCharacter(sourceCode, submissionRequest.SubmittedCode);
        }
        

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
        
        if (lastIndex != -1)
        {
            StringBuilder stringBuilder = new StringBuilder(original);
            stringBuilder.Insert(lastIndex, toAppend);
            return stringBuilder.ToString();
        }
        else
        {
            return original;
        }
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
            source_code = sourceCode,
            language_id = 62, // Judge0 language id for Java
            expected_output = expectedOutput
        };
    }
}