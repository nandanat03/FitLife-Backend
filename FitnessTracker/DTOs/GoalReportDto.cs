namespace FitnessTracker.Dtos
{
    public record GoalReportDto
    {
        public int GoalId { get; init; }
        public int UserId { get; init; }
        public string GoalType { get; init; } = string.Empty;
        public double GoalValue { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public double AchievedValue { get; init; }
        public double Remaining { get; init; }
    }
}
