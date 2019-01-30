using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WallAI.Core.Ai;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Ai;
using WallAI.Core.World.Entities;

namespace WallAI.Simulation.CUI
{
    internal class Program
    {
        private static DemoSimulation _simulation;

        private static void Main()
        {
            // 2 is minimum, or the console goes haywire
            var offsetInt = 2;

            Console.CursorVisible = false;

            _simulation = new DemoSimulation(new Point2D(40, 40), 1);

            var offset = new Point2D(offsetInt * 2, offsetInt);
            var worldOffset = offset + new Point2D(1, 1);
            var windowRect = new Rectangle2D(offset, offset + offset + new Point2D(_simulation.Size.X * 2, _simulation.Size.Y) + offset + new Point2D(1, 1));

            Console.SetWindowSize(windowRect.Width, windowRect.Height);
            Console.SetBufferSize(windowRect.Width, windowRect.Height);

            while (true)
            {
                _simulation.Tick();

                DrawRect(new Rectangle2D(offset, (_simulation.Size.X) * 2 + 1, _simulation.Size.Y + 1), ConsoleColor.DarkGray);
                var visionMap = CalculateVisionMap(_simulation.World.Entities);
                Render(worldOffset, visionMap, _simulation);
                Thread.Sleep(100);
            }
        }

        private static HashSet<Point2D> CalculateVisionMap(IEnumerable<IWorld2DEntity> worldEntities)
        {
            var visionMap = new HashSet<Point2D>();

            foreach (var entity in worldEntities.Where(x => x.Stats.Alive).ToArray())
            {
                var core = new AiCore(new AiWorld2D(entity.World), entity, _simulation.CurrentTick);
                using (var map = core.GetVisibleWorld())
                {
                    var tiles = map.TilesInRange(new Circle2D(entity.Location, core.Stats.VisionRadius));

                    foreach (var tile in tiles)
                        visionMap.Add(tile.Location);
                }
            }

            return visionMap;
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

        private static void Render(Point2D offset, HashSet<Point2D> visionMap, Simulation simulation)
        {
            var world = simulation.World;
            var size = simulation.Size;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(offset.X, offset.Y);

            for (var y = 0; y < size.Y; y++)
            {
                Console.CursorLeft = offset.X;

                for (var x = 0; x < size.X; x++)
                {
                    var point = new Point2D(x, y);
                    PrintTile(world[point], visionMap.Contains(point));
                }

                Console.CursorTop++;
            }
        }

        private static void PrintTile(ITile2D tile, bool vision)
        {
            ConsoleColor color;

            if (tile.Entity == null)
                color = vision ? ConsoleColor.DarkGray : ConsoleColor.Black;
            else if (tile.Entity.Stats.Alive)
                color = vision ? ConsoleColor.White : ConsoleColor.Gray;
            else
                color = vision ? ConsoleColor.Red : ConsoleColor.DarkRed;

            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(tile.Entity == null ? "░░" : "  ");

            Console.ResetColor();
        }
    }
}
