using codelab_exam_server.Dtos.Submission;

namespace codelab_exam_server.Services.SubmissionService;

public interface ISubmissionService
{
    Task<IEnumerable<SubmissionResponse>> GetAllSubmissions();
    Task<SubmissionResponse> GetSubmissionById(int id);
    Task<IEnumerable<SubmissionResponse>> GetAllSubmissionsByExerciseId(int exerciseId);
    Task<SubmissionResponse> CreateSubmission(SubmissionRequest submissionRequest);
    Task<SubmissionResponse> UpdateSubmission(int id, SubmissionRequest submissionRequest);
    Task<SubmissionResponse> DeleteSubmission(int id);
}