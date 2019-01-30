using System;
using WallAI.Core.Ai;
using WallAI.Core.Enums;
using WallAI.Core.Helpers.Extensions;

namespace WallAI.Simulation.Ai
{
    public class Testing : IAi
    {
        public void Tick(IAiCore ai)
        {
            var rand = new Random();
            
            using (var world = ai.GetVisibleWorld())
            {
                var tiles = world.TilesInRange(ai.Vision);

            }

            if (ai.Stats.Energy == 0)
            {
                ai.Kill();
                return;
            }

            ai.Move(rand.NextEnum<Direction>());
        }
    }
}
