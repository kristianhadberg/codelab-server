using codelab_exam_server.Dtos.Exercise;
using codelab_exam_server.Dtos.LearningPath;

namespace codelab_exam_server.Services.LearningPathService;

public interface ILearningPathService
{
    Task<IEnumerable<LearningPathResponse>> GetAllLearningPaths();
    Task<LearningPathResponse> GetLearningPathById(int id);
    Task<LearningPathResponse> CreateLearningPath(LearningPathRequest learningPathRequest);
    /*Task<ExerciseResponse> UpdateExercise(int id, ExerciseRequest exerciseRequest);
    Task<ExerciseResponse> DeleteExercise(int id);#1#*/
}