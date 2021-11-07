using System.Collections.Generic;
using System.Linq;
using QC.Structures;

namespace QC.Utils
{
    public static class MoveGenerator
    {
        public static List<Move> GenerateMoves()
        {
            List<Move> moves = new List<Move>();

            Cell player = GameManager.TurnState.PlayerPosition;
            Cell opponent = GameManager.TurnState.OpponentPosition;

            foreach (Cell neighbour in player.Neighbours)
            {
                if (neighbour.Location == opponent.Location)
                {
                    if (player.Index.X == neighbour.Index.X - 1)
                    {
                        Cell curr = neighbour;
                        Cell next = neighbour.Neighbours.Find(n => n.Index.X == curr.Index.X + 1);
                        if (next != null) moves.Add(new Move("jump", next, null, null));
                    }

                    else if (player.Index.X == neighbour.Index.X + 1)
                    {
                        Cell curr = neighbour;
                        Cell next = neighbour.Neighbours.Find(n => n.Index.X == curr.Index.X - 1);
                        if (next != null) moves.Add(new Move("jump", next, null, null));
                    }

                    else if (player.Index.Y == neighbour.Index.Y - 1)
                    {
                        Cell curr = neighbour;
                        Cell next = neighbour.Neighbours.Find(n => n.Index.Y == curr.Index.Y + 1);
                        if (next != null) moves.Add(new Move("jump", next, null, null));
                    }

                    else if (player.Index.Y == neighbour.Index.Y + 1)
                    {
                        Cell curr = neighbour;
                        Cell next = neighbour.Neighbours.Find(n => n.Index.Y == curr.Index.Y - 1);
                        if (next != null) moves.Add(new Move("jump", next, null, null));
                    }
                }
                else moves.Add(new Move("move", neighbour, null, null));
            }

            if (GameManager.TurnState.WallCount > 0)
            {
                foreach (AnchorPoint point in GameManager.AnchorPoints)
                {
                    if (!GameManager.BannedPointsH.Contains(point) && point.AdjacentCells.Contains(GameManager.TurnState.OpponentPosition)) moves.Add(new Move("wall", null, point, "h"));
                    if (!GameManager.BannedPointsV.Contains(point) && point.AdjacentCells.Contains(GameManager.TurnState.OpponentPosition)) moves.Add(new Move("wall", null, point, "v"));
                }
            }

            moves = RemoveIllegalMoves(moves);
            
            return moves;
        }
        
        private static List<Move> RemoveIllegalMoves(List<Move> moves)
        {
            List<Move> legalMoves = new List<Move>();
            
            foreach (Move move in moves)
            {
                if (move.Action == "wall")
                {
                    switch (move.WallDirection)
                    {
                        case "h":
                        {
                            GameManager.TryRemoveNeighboursHorizontal(move.WallPoint.Location);
                            if (GameManager.PathExists()) legalMoves.Add(move);
                            GameManager.RestoreNeighboursHorizontal(move.WallPoint.Location);
                            break;
                        }
                        case "v":
                        {
                            GameManager.TryRemoveNeighboursVertical(move.WallPoint.Location);
                            if (GameManager.PathExists()) legalMoves.Add(move);
                            GameManager.RestoreNeighboursVertical(move.WallPoint.Location);
                            break;
                        }
                    }
                }
                else legalMoves.Add(move);
            }

            return legalMoves;
        }
        
    }
}