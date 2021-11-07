using System;
using System.Collections.Generic;
using QC.States;
using QC.Structures;
using QC.Utils;

namespace QC
{
    public static class GameManager
    {
        public static Cell[,] Cells { get; } = new Cell[9, 9];
        public static AnchorPoint[,] AnchorPoints { get; } = new AnchorPoint[8, 8];

        private static Dictionary<string, Cell> LocationToCell { get; } = new Dictionary<string, Cell>();
        private static Dictionary<string, AnchorPoint> LocationToAnchorPoint { get; } = new Dictionary<string, AnchorPoint>();

        public static List<AnchorPoint> BannedPointsV { get; } = new List<AnchorPoint>();
        public static List<AnchorPoint> BannedPointsH { get; } = new List<AnchorPoint>();

        public static TurnState TurnState { get; set; }
        
        public static void Init()
        {
            CreateCells();
            CreateAnchorPoints();
        }

        public static void MakeMove()
        {
            TurnState saved = TurnState.Copy();
            Minimax.Search(2, true);
            TurnState = saved;
            Move move = Minimax.NextMove;
            ProcessMove(move);
            Console.WriteLine(move);
        }

        public static void ProcessMove(Move move)
        {
            if (move.Action == "move" || move.Action == "jump")
                TurnState.PlayerPosition = move.TargetCell;
            else
                ProcessWall(move);
            TurnState.ChangeTurn();
        }

        private static void ProcessWall(Move move)
        {
            switch (move.WallDirection)
            {
                case "h":
                {
                    TryRemoveNeighboursHorizontal(move.WallPoint.Location);
                    if (PathExists())
                    {
                        
                        BanPointsHorizontal(move.WallPoint.Location);
                        TurnState.WallCount--;
                        break;
                    }
                    RestoreNeighboursHorizontal(move.WallPoint.Location);
                    break;
                }
                case "v":
                {
                    TryRemoveNeighboursVertical(move.WallPoint.Location);
                    if (PathExists())
                    {
                        BanPointsVertical(move.WallPoint.Location);
                        TurnState.WallCount--;
                        break;
                    }
                    RestoreNeighboursVertical(move.WallPoint.Location);
                    break;
                }
            }
        }

        public static bool PathExists()
        {
            bool playerOneHasPath = false;
            bool playerTwoHasPath = false;
            
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                List<Cell> path = Pathfinder.FindPath(TurnState.PlayerPosition, new Point(i, TurnState.WinIndex));
                if (path == null || path.Count <= 0) continue;
                playerOneHasPath = true;
                break;
            }
            
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                List<Cell> path = Pathfinder.FindPath(TurnState.OpponentPosition, new Point(i, TurnState.LoseIndex));
                if (path == null || path.Count <= 0) continue;
                playerTwoHasPath = true;
                break;
            }

            return playerOneHasPath && playerTwoHasPath;
        }
        
        private static void BanPointsHorizontal(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            BannedPointsH.Add(point);
            BannedPointsV.Add(point);
            if (point.Index.X > 0) BannedPointsH.Add(AnchorPoints[point.Index.X - 1, point.Index.Y]);
            if (point.Index.X < 7) BannedPointsH.Add(AnchorPoints[point.Index.X + 1, point.Index.Y]);
        }
        
        private static void BanPointsVertical(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            BannedPointsH.Add(point);
            BannedPointsV.Add(point);
            if (point.Index.Y > 0) BannedPointsV.Add(AnchorPoints[point.Index.X, point.Index.Y - 1]);
            if (point.Index.Y < 7) BannedPointsV.Add(AnchorPoints[point.Index.X , point.Index.Y + 1]);
        }

        public static void RestoreNeighboursHorizontal(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            Cell[] adj = point.AdjacentCells;
            
            if (!adj[0].HasNeighbour(adj[1])) adj[0].AddNeighbour(adj[1]);
            if (!adj[1].HasNeighbour(adj[0])) adj[1].AddNeighbour(adj[0]);
            if (!adj[2].HasNeighbour(adj[3])) adj[2].AddNeighbour(adj[3]);
            if (!adj[3].HasNeighbour(adj[2])) adj[3].AddNeighbour(adj[2]);
        }

        public static void RestoreNeighboursVertical(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            Cell[] adj = point.AdjacentCells;

            if (!adj[0].HasNeighbour(adj[2])) adj[0].AddNeighbour(adj[2]);
            if (!adj[2].HasNeighbour(adj[0])) adj[2].AddNeighbour(adj[0]);
            if (!adj[1].HasNeighbour(adj[3])) adj[1].AddNeighbour(adj[3]);
            if (!adj[3].HasNeighbour(adj[1])) adj[3].AddNeighbour(adj[1]);
        }

        public static void TryRemoveNeighboursHorizontal(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            Cell[] adj = point.AdjacentCells;

            adj[0].RemoveNeighbour(adj[1]);
            adj[1].RemoveNeighbour(adj[0]);
            adj[2].RemoveNeighbour(adj[3]);
            adj[3].RemoveNeighbour(adj[2]);
        }

        public static void TryRemoveNeighboursVertical(string location)
        {
            AnchorPoint point = GetAnchorPointByLocation(location);
            Cell[] adj = point.AdjacentCells;

            adj[0].RemoveNeighbour(adj[2]);
            adj[2].RemoveNeighbour(adj[0]);
            adj[1].RemoveNeighbour(adj[3]);
            adj[3].RemoveNeighbour(adj[1]);
        }

        private static void CreateCells()
        {
            string[] cellLetters = { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
            
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    string location = cellLetters[i] + (j + 1);
                    Cells[i, j] = new Cell(new Point(i, j), location);
                    LocationToCell.Add(location, Cells[i,j]);
                }
            }
            
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cell cell = Cells[i, j];
                    if (i > 0) cell.AddNeighbour(Cells[i - 1, j]);
                    if (i < 8) cell.AddNeighbour(Cells[i + 1, j]);
                    if (j > 0) cell.AddNeighbour(Cells[i, j - 1]);
                    if (j < 8) cell.AddNeighbour(Cells[i, j + 1]);
                }
            }
        }

        public static Cell GetCellByLocation(string location)
        {
            if (LocationToCell.ContainsKey(location)) return LocationToCell[location];
            throw new Exception($"Could not get cell with location {location}");
        }
        
        private static void CreateAnchorPoints()
        {
            string[] pointLetters = { "s", "t", "u", "v", "w", "x", "y", "z" };
            
            for (int i = 0; i < AnchorPoints.GetLength(0); i++)
            {
                for (int j = 0; j < AnchorPoints.GetLength(1); j++)
                {
                    string location = pointLetters[i] + (j + 1);
                    AnchorPoints[i, j] = new AnchorPoint(new Point(i, j), location);
                    AnchorPoints[i, j].AddAdjacentCells(Cells[i, j], Cells[i, j + 1], Cells[i + 1, j], Cells[i + 1, j + 1]);
                    LocationToAnchorPoint.Add(location, AnchorPoints[i,j]);
                }
            }
        }

        public static AnchorPoint GetAnchorPointByLocation(string location)
        {
            if (LocationToAnchorPoint.ContainsKey(location)) return LocationToAnchorPoint[location];
            throw new Exception($"Could not get anchor point with location {location}");
        }

        public static void TryMove(Move move)
        {
            if (move.Action == "wall")
            {
                if (move.WallDirection == "h") TryRemoveNeighboursHorizontal(move.WallPoint.Location);
                else TryRemoveNeighboursVertical(move.WallPoint.Location);
            }
            else
            {
                Cell cell = GetCellByLocation(move.TargetCell.Location);
                TurnState.PlayerPosition = cell;
            }
            TurnState.ChangeTurn();
        }
        
        public static void UndoMove(Move move)
        {
            if (move.Action == "wall")
            {
                if (move.WallDirection == "h") RestoreNeighboursHorizontal(move.WallPoint.Location);
                else RestoreNeighboursVertical(move.WallPoint.Location);
            }
            
            TurnState.ChangeTurn();
        }
    }
}