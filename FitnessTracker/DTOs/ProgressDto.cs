namespace FitnessTracker.Dtos
{
    public record ProgressDto(string[] Labels, object[] Data);

    public record DailySummaryDto(string Label, double Calories, float Distance, float Duration);
}

