﻿using System.Linq;
using WallAI.Core.Ai;
using WallAI.Core.Math.Geometry;
using WallAI.Core.World;
using WallAI.Core.World.Ai;

namespace WallAI.Simulation
{
    public class Simulation
    {
        public Point2D Size => new Point2D(_xSize, _ySize);

        private readonly int _xSize;
        private readonly int _ySize;
        public int CurrentTick { get; protected set; }

        public readonly IWorld2D World;

        public Simulation(IWorld2DMethods methods, int seed, int xSize, int ySize)
        {
            CurrentTick = 0;
            _xSize = xSize;
            _ySize = ySize;
            World = WrappingWorld2D.Create(methods, new Point2D(xSize, ySize), seed);
        }

        public void Tick()
        {
            var aiWorld = new AiWorld2D(World);

            foreach (var entity in World.Entities.Where(x => x.Stats.Alive).ToArray())
                entity.Ai.Tick(new AiCore(aiWorld, entity, CurrentTick));

            CurrentTick++;
        }
    }
}
