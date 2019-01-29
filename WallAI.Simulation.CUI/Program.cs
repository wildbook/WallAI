using System;
using System.Threading;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Simulation.Ai;

namespace WallAI.Simulation.CUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 2 is minimum, or the console goes haywire
            var offsetInt = 2;

            Console.CursorVisible = false;

            var simulation = new DemoSimulation(new Point2D(25, 25), 1);

            var offset = new Point2D(offsetInt * 2, offsetInt);
            var windowRect = new Rectangle2D(offset, offset + offset + new Point2D(simulation.Size.X * 2, simulation.Size.Y) + offset + new Point2D(-1, 0));

            Console.SetWindowSize(windowRect.Width, windowRect.Height);
            Console.SetBufferSize(windowRect.Width, windowRect.Height);

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

                DrawRect(new Rectangle2D(offset, (simulation.Size.X) * 2 + 1, simulation.Size.Y + 1), ConsoleColor.DarkGray);
                Render(offset + new Point2D(1, 1), simulation);
                Thread.Sleep(250);
            }
        }

        public static void DrawRect(Rectangle2D rect, ConsoleColor color)
        {
            Console.SetCursorPosition(rect.X, rect.Y);

            var center = new string('═', rect.Width - 1);

            Console.Write($"╔{center}╗");

            for (var y = rect.Y + 1; y < rect.Y2; y++)
            {
                Console.SetCursorPosition(rect.X, y);
                Console.Write('║');

                Console.SetCursorPosition(rect.X2, y);
                Console.Write('║');
            }

            Console.SetCursorPosition(rect.X, rect.Y2);
            Console.Write($"╚{center}╝");
            Console.ResetColor();
        }

        private static void Render(Point2D offset, Simulation simulation)
        {
            var world = simulation.World;
            var size = simulation.Size;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(offset.X, offset.Y);

            for (var y = 0; y < size.Y; y++)
            {
                Console.CursorLeft = offset.X;

                for (var x = 0; x < size.X; x++)
                    PrintTile(world[x, y]);

                Console.CursorTop++;
            }
        }

        private static void PrintTile(ITile2D tile)
        {
            ConsoleColor color;

            if (tile.Entity == null)
                color = ConsoleColor.DarkYellow;
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
