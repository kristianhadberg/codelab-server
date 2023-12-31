using codelab_exam_server.Dtos.Exercise;

namespace codelab_exam_server.Services.ExerciseService;

public interface IExerciseService
{
    Task<IEnumerable<ExerciseResponse>> GetAllExercises();
    Task<ExerciseResponse> GetExerciseById(int id);
    Task<IEnumerable<ExerciseResponse>> GetAllExercisesByTopicId(int topicId);
    Task<ExerciseResponse> CreateExercise(ExerciseRequest exerciseRequest);
    Task<ExerciseResponse> UpdateExercise(int id, ExerciseRequest exerciseRequest);
    Task<ExerciseResponse> DeleteExercise(int id);
    Task<IEnumerable<ExerciseResponse>> GetAllByTopicIdAndUserId(int topicId, int userId);
}