namespace WallAI.Core.Entities.Stats
{
    public interface IReadOnlyStats
    {
        bool Alive { get; }
        uint Energy { get; }
    }
}
