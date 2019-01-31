using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConcurrentCollections;
using WallAI.Core.Ai;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Ai;
using WallAI.Core.Worlds.Entities;
using WallAI.Simulation.Ai;

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

            //NOTE: The GUIDs below are random, meaning the world is not deterministic.
            // Do not spawn in entities manually like below without controlling the seed.

            _simulation.World[new Point2D(10, 10)].Entity
                = new Entity<ObedientAi>(Guid.NewGuid(),
                    new Stats
                    {
                        Alive = true,
                        Energy = 25,
                        Opaque = true,
                        VisionRadius = 8
                    });

            _simulation.World[new Point2D(11, 10)].Entity
                = new Entity<Testing>(Guid.NewGuid(),
                    new Stats
                    {
                        Alive = false,
                        Opaque = true,
                    });

            while (true)
            {
                _simulation.Tick();

                var visionMap = CalculateVisionMap(_simulation.World.Entities);

                DrawRect(new Rectangle2D(offset, _simulation.Size.X * 2 + 1, _simulation.Size.Y + 1), ConsoleColor.DarkGray);
                Render(worldOffset, visionMap, _simulation);

                Thread.Sleep(100);
            }
        }

        private static ConcurrentHashSet<Point2D> CalculateVisionMap(IEnumerable<IWorld2DEntity> worldEntities)
        {
            var visionMap = new ConcurrentHashSet<Point2D>();
            var world = new AiWorld2D(_simulation.World);

            var entities = worldEntities.Where(x => x.Stats.Alive).ToArray();
            
            foreach (var entity in entities)
              TickEntity(entity);

            return visionMap;

            void TickEntity(IWorld2DEntity entity)
            {
                var core = new AiCore(world, entity, _simulation.CurrentTick);

                using (var map = core.GetVisibleWorld())
                {
                    var tiles = map.TilesInRange(new Circle2D(entity.Location, core.Stats.VisionRadius));

                    foreach (var tile in tiles)
                        visionMap.Add(tile.Location + entity.Location);
                }
            }
        }

        public static void DrawRect(Rectangle2D rect, ConsoleColor color)
        {
            Console.SetCursorPosition(rect.X1, rect.Y1);

            var center = new string('═', rect.Width - 1);

            Console.Write($"╔{center}╗");

            for (var y = rect.Y1 + 1; y < rect.Y2; y++)
            {
                Console.SetCursorPosition(rect.X1, y);
                Console.Write('║');

                Console.SetCursorPosition(rect.X2, y);
                Console.Write('║');
            }

            Console.SetCursorPosition(rect.X1, rect.Y2);
            Console.Write($"╚{center}╝");
            Console.ResetColor();
        }

        private static void Render(Point2D offset, ICollection<Point2D> visionMap, Simulation simulation)
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
