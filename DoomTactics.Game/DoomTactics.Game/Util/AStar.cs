using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public static class AStar
    {
        private const int MaximumSearchDistance = 50;

        public static IList<Tile> CalculateAStarPath(Tile start, Tile goal, Level level, ActorBase actor)
        {
            if (start == null || goal == null)
                return null;
            if (goal.ActorInTile != null)
                return null;

            var path = new List<Tile>();
            var nodes = new Node[level.Length,level.Width];
            for (int x = 0; x < level.Length; x++)
            {
                for (int y = 0; y < level.Width; y++)
                {
                    nodes[x, y] = new Node(x, y);
                }
            }

            return CalculatePath(start, goal, level, nodes, actor);
        }

        private static IList<Tile> CalculatePath(Tile start, Tile goal, Level level, Node[,] nodes, ActorBase actor)
        {
            nodes[start.XCoord, start.YCoord].Cost = 0;
            nodes[start.XCoord, start.YCoord].Depth = 0;
            var open = new List<Node>();
            var closed = new List<Node>();
            var path = new List<Tile>();
            int depth = 0;

            open.Add(nodes[start.XCoord, start.YCoord]);            

            while (depth < MaximumSearchDistance && (open.Count != 0))
            {
                Node current = open[0];
                if (current == nodes[goal.XCoord, goal.YCoord])
                {
                    break;
                }

                open.RemoveAt(0);
                closed.Add(current);
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (x != 0 && y != 0) continue;

                        int candidateX = x + current.X;
                        int candidateY = y + current.Y;

                        if (CanMoveToTile(actor, level, start.XCoord, start.YCoord, current.X, current.Y, candidateX, candidateY))
                        {
                            float nextStepCost = current.Cost +
                                                 CalculateMovementCost(actor, level, current.X, current.Y, candidateX,
                                                                       candidateY);
                            Node neighbor = nodes[candidateX, candidateY];
                            if (nextStepCost < neighbor.Cost)
                            {
                                if (open.Contains(neighbor))
                                {
                                    open.Remove(neighbor);
                                }
                                if (closed.Contains(neighbor))
                                {
                                    closed.Remove(neighbor);
                                }
                            }

                            if (!open.Contains(neighbor) && !closed.Contains(neighbor))
                            {
                                neighbor.Cost = nextStepCost;
                                neighbor.Heuristic = GetHeuristicCost(actor, level.GetTileAt(candidateX, candidateY),
                                                                      goal);
                                depth = Math.Max(depth, neighbor.SetParent(current));
                                open.Add(neighbor);
                                open = open.OrderBy(o => o.TotalCost()).ToList();
                            }

                        }
                    }
                }
            }

            if (nodes[goal.XCoord, goal.YCoord].Parent == null) return null;            
            Node target = nodes[goal.XCoord, goal.YCoord];
            while (target != nodes[start.XCoord, start.YCoord])
            {
                path.Insert(0, level.GetTileAt(target.X, target.Y));
                target = target.Parent;
            }
            path.Insert(0, start);
            if (path.Count > actor.MovementRange + 1) return null;

            return path;
        }


        private static bool CanMoveToTile(ActorBase actor, Level level, int startX, int startY, int currentX, int currentY, int candidateX, int candidateY)
        {
            if (candidateX < 0 || candidateY < 0 || candidateX >= level.Length || candidateY >= level.Width || (candidateX == startX && candidateY == startY))
            {
                return false;
            }

            var candidateTile = level.GetTileAt(candidateX, candidateY);

            if (candidateTile.ActorInTile != null)
                return false;

            return true;
        }

        private static float CalculateMovementCost(ActorBase actor, Level level, int currentX, int currentY, int candidateX, int candidateY)
        {
            var currentTile = level.GetTileAt(currentX, currentY);
            var candidateTile = level.GetTileAt(candidateX, candidateY);
            return (float)Math.Abs(currentTile.GameHeight - candidateTile.GameHeight);
        }

        private static float GetHeuristicCost(ActorBase actor, Tile tile, Tile goal)
        {
            return Math.Abs(tile.XCoord - goal.XCoord) + Math.Abs(tile.YCoord - goal.YCoord);
        }

    }

    public class Node
    {
        public int X;
        public int Y;
        public float Cost;
        public int Depth;
        public Node Parent;
        public float Heuristic;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int SetParent(Node parent)
        {
            Depth = parent.Depth + 1;
            Parent = parent;

            return Depth;
        }

        public float TotalCost()
        {
            return Heuristic + Cost;
        }
    }
}
