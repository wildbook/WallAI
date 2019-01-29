namespace WallAI.Core.Entities.Stats
{
    public class Stats : IStats
    {
        public bool Alive { get; set; }
        public uint Energy { get; set; }

        public Stats() { }
        public Stats(IStats stats)
        {
            Alive = stats.Alive;
            Energy = stats.Energy;
        }
    }
}
