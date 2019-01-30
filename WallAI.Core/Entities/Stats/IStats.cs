namespace WallAI.Core.Entities.Stats
{
    public interface IStats : IReadOnlyStats
    {
        new bool Alive { get; set; }
        new uint Energy { get; set; }
        new uint Height { get; set; }
        new bool Opaque { get; set; }
        new byte VisionRadius { get; set; }
    }
}
