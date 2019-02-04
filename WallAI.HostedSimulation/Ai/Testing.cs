using System.Runtime.Serialization;
using WallAI.Core.Ai;
using WallAI.Core.Enums;
using WallAI.Core.Helpers.Extensions;

namespace WallAI.HostedSimulation.Ai
{
    public class Testing : IAi
    {
        [DataMember(Name = nameof(Name))]
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
