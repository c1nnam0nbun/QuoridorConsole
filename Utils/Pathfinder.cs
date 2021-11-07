using System;
using System.Collections.Generic;
using QC.Structures;

namespace QC.Utils
{
    public static class Pathfinder
    {
        private static List<Cell> _openSet;

        private static Dictionary<Cell, int> _gScore;
        private static  Dictionary<Cell, int> _fScore;

        private static Dictionary<Cell, Cell> _path;

        public static List<Cell> FindPath(Cell start, Point goal)
        {
            _openSet = new List<Cell>();
            _gScore = new Dictionary<Cell, int>();
            _fScore = new Dictionary<Cell, int>();
            _path = new Dictionary<Cell, Cell>();
            
            return AStarPathfinder(start, goal);
        }

        private static List<Cell> AStarPathfinder(Cell start, Point goal)
        {
            _openSet.Add(start);
            _gScore.Add(start, 0);
            _fScore.Add(start, Heuristic(start.Index, goal));

            while (_openSet.Count > 0)
            {
                if (start.Index == goal) break;
                int lowest = 0;
                for (int i = 0; i < _openSet.Count; i++)
                {
                    if (!_fScore.ContainsKey(_openSet[lowest])) _fScore.Add(_openSet[lowest], int.MaxValue);
                    if (_fScore[_openSet[i]] < _fScore[_openSet[lowest]]) lowest = i;
                }

                Cell current = _openSet[lowest];
                if (current.Index == goal)
                {
                    List<Cell> res = new List<Cell>();
                    while (_path.ContainsKey(current))
                    {
                        current = _path[current];
                        res.Add(current);
                    }

                    return res;
                }

                _openSet.Remove(current);
                foreach (Cell neighbour in current.Neighbours)
                {
                    int tempG = _gScore[current] + 1;
                    if (!_gScore.ContainsKey(neighbour)) _gScore.Add(neighbour, int.MaxValue);
                    if (tempG >= _gScore[neighbour]) continue;
                    _path[neighbour] = current;
                    _gScore[neighbour] = tempG;
                    _fScore[neighbour] = _gScore[neighbour] + Heuristic(neighbour.Index, goal);
                    if (!_openSet.Contains(neighbour)) _openSet.Add(neighbour);
                }
            }

            return null;
        }

        private static int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}