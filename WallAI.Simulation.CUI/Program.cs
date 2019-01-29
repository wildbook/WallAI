using System;
using System.Threading;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Tiles;
using WallAI.Simulation.Ai;

namespace WallAI.Simulation.CUI
{
    internal class Program
    { 
        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.ReadLine();

            var simulation = new DemoSimulation();

            var stats = new Stats
            {
                Alive = true,
                Energy = 25,
            };

            var maxStats = new Stats(stats)
            {
                Energy = 30,
            };

            simulation.World[12, 12].Entity = new Entity<Testing>(stats, maxStats);

            while (true)
            {
                simulation.Tick();
                Render(simulation);
                Thread.Sleep(250);
            }
        }

        private static void Render(Simulation simulation)
        {
            var world = simulation.World;
            var size = simulation.Size;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < size.Y; y++)
                Console.WriteLine(new string(' ', size.X * 2));
            Console.SetCursorPosition(0, 0);

            for (var y = 0; y < size.Y; y++)
            {
                for (var x = 0; x < size.X; x++)
                {
                    var tile = world[x, y];
                    PrintTile(tile);
                }

                Console.CursorTop++;
                Console.CursorLeft = 0;
            }

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(size.X * 2, 0);
            for (var y = 0; y <= size.Y; y++)
            {
                Console.Write("  ");
                Console.CursorTop++;
                Console.CursorLeft -= 2;
            }

            Console.SetCursorPosition(0, size.Y);
            Console.WriteLine(new string(' ', size.X * 2));
            Console.ResetColor();
        }
        private static void PrintTile(ITile2D tile)
        {
            ConsoleColor color;

            if (tile.Entity == null)
                color = ConsoleColor.Black;
            else if (tile.Entity.Stats.Alive)
                color = ConsoleColor.White;
            else
                color = ConsoleColor.DarkRed;

            Console.BackgroundColor = color;
            Console.Write("  ");
            Console.ResetColor();
        }
    }
}
