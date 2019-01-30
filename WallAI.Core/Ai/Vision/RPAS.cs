using System;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Ai.Vision
{
    // Based on http://roguebasin.roguelikedevelopment.org/index.php?title=Restrictive_Precise_Angle_Shadowcasting
    public class RPAS
    {
        // Holds the three angles for each cell. Near is closest to the horizontal/vertical line, and far is furthest.
        //
        // Also used for obstructions; for the purposes of obstructions, the center variable is ignored.
        private class CellAngles
        {
            public float Near { get; set; }
            public float Center { get; }
            public float Far { get; set; }

            public CellAngles(float near, float center, float far)
            {
                Near = near;
                Center = center;
                Far = far;
            }

            public override string ToString() => $"{{ Near = {Near}, Center = {Center}, Far = {Far} }}";
        }

        public readonly struct Configuration
        {
            public static Configuration Default { get; } = new Configuration(1 / 3f, true, Visible.Center, true);

            public enum Visible
            {
                /// <summary>
                /// If you have a line to the near, center, or far, it will return as visible
                /// </summary>
                Partially,
                
                /// <summary>
                /// If you have a line to the center and at least one other corner it will return as visible
                /// </summary>
                Center,
                
                /// <summary>
                /// If you have a line to all the near, center, and far, it will return as visible
                /// </summary>
                Fully,
            }

            // Changing the radius-fudge changes how smooth the edges of the vision bubble are.
            //
            // RADIUS_FUDGE should always be a value between 0 and 1.
            public readonly float RadiusFudge;

            // If this is False, some cells will unexpectedly be visible.
            //
            // For example, let's say you you have obstructions blocking (0.0 - 0.25) and (.33 - 1.0).
            // A far off cell with (near=0.25, center=0.3125, far=0.375) will have both its near and center unblocked.
            //
            // On certain restrictiveness settings this will mean that it will be visible, but the blocks in front of it will
            // not, which is unexpected and probably not desired.
            //
            // Setting it to True, however, makes the algorithm more restrictive.
            public readonly bool NotVisibleBlocksVision;

            // Determines how restrictive the algorithm is.
            public readonly Visible Restrictiveness;

            // If VISIBLE_ON_EQUAL is False, an obstruction will obstruct its endpoints. If True, it will not.
            //
            // For example, if there is an obstruction (0.0 - 0.25) and a square at (0.25 - 0.5), the square's near angle will
            // be unobstructed in True, and obstructed on False.
            //
            // Setting this to False will make the algorithm more restrictive.
            public readonly bool VisibleOnEqual;

            public Configuration(float radiusFudge, bool notVisibleBlocksVision, Visible restrictiveness, bool visibleOnEqual)
            {
                RadiusFudge = radiusFudge;
                NotVisibleBlocksVision = notVisibleBlocksVision;
                Restrictiveness = restrictiveness;
                VisibleOnEqual = visibleOnEqual;
            }
        }

        public Configuration CurrentConfiguration { get; }

        public RPAS(Configuration configuration)
            => CurrentConfiguration = configuration;

        // Parameter func_transparent is a function with the sig: boolean func(x, y)
        // It should return True if the cell is transparent, and False otherwise.
        //
        // Returns a set with all (x, y) tuples visible from the centerpoint.
        public HashSet<Point2D> CalculateVisibleCells(Circle2D circle, Func<Point2D, bool> isTransparent)
        {
            var cells = VisibleCellsInQuadrantFrom(circle.X, circle.Y, 1, 1, circle.Radius, isTransparent);
            cells.UnionWith(VisibleCellsInQuadrantFrom(circle.X, circle.Y, 1, -1, circle.Radius, isTransparent));
            cells.UnionWith(VisibleCellsInQuadrantFrom(circle.X, circle.Y, -1, -1, circle.Radius, isTransparent));
            cells.UnionWith(VisibleCellsInQuadrantFrom(circle.X, circle.Y, -1, 1, circle.Radius, isTransparent));
            cells.Add(circle.Origin);
            return cells;
        }

        // Parameters quad_x, quad_y should only be 1 or -1. The combination of the two determines the quadrant.
        // Returns a set of (x, y) tuples.
        private HashSet<Point2D> VisibleCellsInQuadrantFrom(int xCenter, int yCenter, int quadX, int quadY, int radius, Func<Point2D, bool> funcTransparent)
        {
            var cells = VisibleCellsInOctantFrom(xCenter, yCenter, quadX, quadY, radius, funcTransparent, true);
            cells.UnionWith(VisibleCellsInOctantFrom(xCenter, yCenter, quadX, quadY, radius, funcTransparent, false));
            return cells;
        }

        // Returns a set of (x, y) typles.
        // Utilizes the NOT_VISIBLE_BLOCKS_VISION variable.
        private HashSet<Point2D> VisibleCellsInOctantFrom(int xCenter, int yCenter, int quadX, int quadY, int radius, Func<Point2D, bool> funcTransparent, bool isVertical)
        {
            var iteration = 1;
            var visibleCells = new HashSet<Point2D>();
            var obstructions = new List<CellAngles>();

            // End conditions:
            //   iteration > radius
            //   Full obstruction coverage (indicated by one object in the obstruction list covering the full angle from 0 to 1)
            while (iteration <= radius && !(obstructions.Count == 1 && obstructions[0].Near == 0.0 && obstructions[0].Far == 1.0))
            {
                var numCellsInRow = iteration + 1;
                var angleAllocation = 1.0f / numCellsInRow;

                // Start at the center (vertical or horizontal line) and step outwards
                for (var step = 0; step < iteration + 1; step++)
                {
                    var cell = CellAt(xCenter, yCenter, quadX, quadY, step, iteration, isVertical);

                    if (!CellInRadius(xCenter, yCenter, cell, radius))
                        continue;

                    var cellAngles = new CellAngles(
                        (step) * angleAllocation,
                        (step + .5f) * angleAllocation,
                        (step + 1) * angleAllocation);

                    if (CellIsVisible(cellAngles, obstructions))
                    {
                        visibleCells.Add(cell);
                        if (!funcTransparent(cell))
                            obstructions = AddObstruction(obstructions, cellAngles);
                    }
                    else
                    {
                        if (CurrentConfiguration.NotVisibleBlocksVision)
                            obstructions = AddObstruction(obstructions, cellAngles);
                    }
                }

                iteration += 1;
            }

            return visibleCells;
        }

        private Point2D CellAt(int xCenter, int yCenter, int quadX, int quadY, int step, int iteration, bool isVertical)
        {
            if (isVertical)
                return new Point2D(xCenter + step * quadX, yCenter + iteration * quadY);
            return new Point2D(xCenter + iteration * quadX, yCenter + step * quadY);
        }

        private bool CellInRadius(int xCenter, int yCenter, Point2D cell, int radius)
        {
            var cellDistance = System.Math.Sqrt((xCenter - cell.X) * (xCenter - cell.X) +
                                                 (yCenter - cell.Y) * (yCenter - cell.Y));
            return cellDistance <= radius + CurrentConfiguration.RadiusFudge;
        }

        private bool CellIsVisible(CellAngles cellAngles, IEnumerable<CellAngles> obstructions)
        {
            var nearVisible = true;
            var centerVisible = true;
            var farVisible = true;

            foreach (var obstruction in obstructions)
            {
                if (CurrentConfiguration.VisibleOnEqual)
                {
                    if (obstruction.Near < cellAngles.Near && cellAngles.Near < obstruction.Far)
                        nearVisible = false;
                    if (obstruction.Near < cellAngles.Center && cellAngles.Center < obstruction.Far)
                        centerVisible = false;
                    if (obstruction.Near < cellAngles.Far && cellAngles.Far < obstruction.Far)
                        farVisible = false;
                }
                else
                {
                    if (obstruction.Near <= cellAngles.Near && cellAngles.Near <= obstruction.Far)
                        nearVisible = false;
                    if (obstruction.Near <= cellAngles.Center && cellAngles.Center <= obstruction.Far)
                        centerVisible = false;
                    if (obstruction.Near <= cellAngles.Far && cellAngles.Far <= obstruction.Far)
                        farVisible = false;
                }
            }

            switch (CurrentConfiguration.Restrictiveness)
            {
                case Configuration.Visible.Partially:
                    return centerVisible || nearVisible || farVisible;
                case Configuration.Visible.Center:
                    return (centerVisible && nearVisible) || (centerVisible && farVisible);
                case Configuration.Visible.Fully:
                    return centerVisible && nearVisible && farVisible;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Generates a new list by combining all old obstructions with the new one (removing them if they are combined) and
        // adding the resulting obstruction to the list.
        //
        // Returns the generated list.
        private static List<CellAngles> AddObstruction(IEnumerable<CellAngles> obstructions, CellAngles newObstruction)
        {
            var newObject = new CellAngles(newObstruction.Near, newObstruction.Center, newObstruction.Far);
            var newList = obstructions.Where(x => !CombineObstructions(x, newObject)).ToList();
            newList.Add(newObject);
            return newList;
        }


        private static bool CombineObstructions(CellAngles old, CellAngles obs)
        {
            CellAngles low, high;

            if (old.Near < obs.Near)
            {
                low = old;
                high = obs;
            }
            else if (obs.Near < old.Near)
            {
                low = obs;
                high = old;
            }
            else
            {
                obs.Far = System.Math.Max(old.Far, obs.Far);
                return true;
            }

            if (low.Far >= high.Near)
            {
                obs.Near = System.Math.Min(low.Near, high.Near);
                obs.Far = System.Math.Max(low.Far, high.Far);
                return true;
            }

            return false;
        }
    }
}