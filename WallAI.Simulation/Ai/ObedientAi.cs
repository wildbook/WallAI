using System;
using WallAI.Core.Ai;
using WallAI.Core.Enums;

namespace WallAI.Simulation.Ai
{
    public class ObedientAi : IAi
    {
        public string Name => nameof(ObedientAi);

        public void Tick(IAiCore ai)
        {
            if (ai.Stats.Energy == 0)
            {
                ai.Kill();
                return;
            }

            ConsoleKey? key = null;
            while (Console.KeyAvailable)
                key = Console.ReadKey(false).Key;

            if (key != null)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        ai.Move(Direction.North);
                        break;
                    case ConsoleKey.DownArrow:
                        ai.Move(Direction.South);
                        break;
                    case ConsoleKey.LeftArrow:
                        ai.Move(Direction.West);
                        break;
                    case ConsoleKey.RightArrow:
                        ai.Move(Direction.East);
                        break;
                }
            }
        }
    }
}
