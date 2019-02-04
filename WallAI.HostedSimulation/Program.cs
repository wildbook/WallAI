using NLog;
using System;
using System.Linq;
using System.Timers;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Helpers;
using WallAI.Core.Helpers.Extensions;
using WallAI.Core.Math.Geometry;
using WallAI.HostedSimulation.Ai;
using WallAI.HostedSimulation.Servers;
using WallAI.HostedSimulation.Simulations;

namespace WallAI.HostedSimulation
{
    public class Program
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            var random = new LoggingRandom(0);

            Log.Info("Creating simulation.");
            var sim = new Simulation(Demo.World2DMethods.Instance, random.Next(), 25, 25);

            Log.Info("Creating game server.");
            var game = new GameServer(sim);

            Log.Info("Creating http server.");
            var html = new HtmlServer();

            Log.Info("Starting servers.");
            {
                html.Start();
                Log.Info("Started HTTP.");

                game.Start();
                Log.Info("Started GS.");
            }

            Log.Info("Starting simulation.");
            //sim.Start();

            var stats = new Stats
            {
                Alive = true,
                Energy = uint.MaxValue,
                Opaque = true,
                VisionRadius = 4,
            };

            sim.World[new Point2D(random.Next(), random.Next())].Entity = new Entity<Testing>(random.NextGuid(), stats);

            var timer = new Timer
            {
                AutoReset = true,
                Interval = 250,
                Enabled = true,
            };

            timer.Elapsed += (sender, args) => sim.Tick();

            Console.ReadLine();

            Log.Info("Shutting down simulation.");
            //sim.Stop();

            Log.Info("Shutting down servers.");
            {
                html.Stop();
                Log.Info("Shut down HTTP.");

                game.Stop();
                Log.Info("Shut down GS.");
            }

            Log.Info("Shut down. Hit enter to close.");
            Console.ReadLine();
        }
    }
}
