using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WallAI.Core.Ai;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Ai;
using WallAI.Core.World.Entities;
using WallAI.Simulation.Ai;

namespace WallAI.Simulation.CUI
{
    internal class Program
    {
        private static DemoSimulation _simulation;

        private static void Main(string[] args)
        {
            // 2 is minimum, or the console goes haywire
            var offsetInt = 2;

            Console.CursorVisible = false;

            _simulation = new DemoSimulation(new Point2D(25, 25), 1);

            var offset = new Point2D(offsetInt * 2, offsetInt);
            var worldOffset = offset + new Point2D(1, 1);
            var windowRect = new Rectangle2D(offset, offset + offset + new Point2D(_simulation.Size.X * 2, _simulation.Size.Y) + offset + new Point2D(1, 1));

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

            _simulation.World[new Point2D(12, 12)].Entity = new Entity<Testing>(stats, maxStats);

            while (true)
            {
                _simulation.Tick();

                DrawRect(new Rectangle2D(offset, (_simulation.Size.X) * 2 + 1, _simulation.Size.Y + 1), ConsoleColor.DarkGray);
                Render(worldOffset, _simulation);
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
                    PrintTile(world[new Point2D(x, y)]);

                Console.CursorTop++;
            }
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
            Console.ForegroundColor = ConsoleColor.DarkGray;

            if (color == ConsoleColor.Black)
                Console.Write("░░");
            else
                Console.Write("  ");

            Console.ResetColor();
        }
    }
}
