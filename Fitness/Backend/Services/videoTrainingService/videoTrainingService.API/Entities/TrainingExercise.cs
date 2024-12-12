namespace videoTrainingService.API.Entities;

public class TrainingExercise
{
    public string TrainingId { get; set; }
    public string ExerciseId { get; set; }
    public int ExerciseReps { get; set; }
    public int Set { get; set; }
    public int SetReps { get; set; }
}