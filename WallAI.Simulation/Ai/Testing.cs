using WallAI.Core.Ai;
using WallAI.Core.Enums;
using WallAI.Core.Helpers.Extensions;

namespace WallAI.Simulation.Ai
{
    public class Testing : IAi
    {
        public string Name => nameof(Testing);

        public void Tick(IAiCore ai)
        {
            if (ai.Stats.Energy == 0)
            {
                ai.Kill();
                return;
            }

            while(!ai.Move(ai.Random.NextEnum<Direction>()).Success);
        }
    }
}
