namespace WallAI.Core.Entities.Stats
{
    public interface IStats
    {
        bool Alive { get; set; }
        uint Energy { get; set; }
    }
}
