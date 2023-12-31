using codelab_exam_server.Data;
using codelab_exam_server.Dtos.Submission;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<JudgeZeroSubmissionStatus> CreateSubmission(SubmissionRequest submissionRequest)
    {
        var submissionStatus = await _judgeZeroSubmissionHandler.JudgeSubmission(submissionRequest);
        
        if (submissionStatus.Status.Description == "Accepted")
        {
            var submission = ToEntity(submissionRequest);
            _dbContext.Submissions.Add(submission);
            await _dbContext.SaveChangesAsync();
            await UpdateExerciseProgress(submissionRequest.UserId, submissionRequest.ExerciseId);
            submissionStatus.SubmissionResponse = SubmissionToResponse(submission);
        }
        
        return submissionStatus;
    }

    public async Task UpdateExerciseProgress(int userId, int exerciseId)
    {
        var userExerciseProgress =
            await _dbContext.UserExerciseProgresses.SingleOrDefaultAsync(uep =>
                uep.UserId == userId && uep.ExerciseId == exerciseId);

        if (userExerciseProgress == null)
        {
            userExerciseProgress = new UserExerciseProgress
            {
                UserId = userId,
                ExerciseId = exerciseId,
                IsCompleted = true
            };
            _dbContext.UserExerciseProgresses.Add(userExerciseProgress);
        }

        await _dbContext.SaveChangesAsync();
    }

    public Task<SubmissionResponse> UpdateSubmission(int id, SubmissionRequest submissionRequest)
    {
        throw new NotImplementedException();
    }

    public Task<SubmissionResponse> DeleteSubmission(int id)
    {
        throw new NotImplementedException();
    }
    
    private static SubmissionResponse SubmissionToResponse(Submission submission) =>
        new SubmissionResponse()
        {
            Id = submission.Id,
            UserId = submission.UserId,
            SubmittedCode = submission.SubmittedCode,
            SubmissionDate = submission.SubmissionDate,
            ExerciseId = submission.ExerciseId
        };

    private static Submission ToEntity(SubmissionRequest submissionRequest) =>
        new Submission()
        {
            SubmittedCode = submissionRequest.SubmittedCode,
            ExerciseId = submissionRequest.ExerciseId,
            UserId = submissionRequest.UserId
        };
}