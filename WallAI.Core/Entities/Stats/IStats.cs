namespace WallAI.Core.Entities.Stats
{
    public interface IStats : IReadOnlyStats
    {
        new bool Alive { get; set; }
        new uint Energy { get; set; }
        new int VisionRadius { get; set; }
    }
}
