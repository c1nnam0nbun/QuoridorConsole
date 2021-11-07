using System;
using System.Collections.Generic;
using QC.States;
using QC.Structures;

namespace QC.Utils
{
    public static class Minimax
    {
        public static Move NextMove { get; private set; }

        public static int Search(int depth, bool maximizingPlayer)
        {
            if (depth == 0) return Evaluate(maximizingPlayer);

            List<Move> moves = MoveGenerator.GenerateMoves();

            if (maximizingPlayer)
            {
                int bestValue = int.MinValue;
                foreach (Move move in moves)
                {
                    if (move.TargetCell != null && move.TargetCell.Index.Y == GameManager.TurnState.WinIndex)
                    {
                        NextMove = move;
                        return int.MaxValue;
                    }
                    TurnState saved = GameManager.TurnState.Copy();
                    GameManager.TryMove(move);
                    if (!GameManager.PathExists())
                    {
                        GameManager.UndoMove(move);
                        GameManager.TurnState = saved;
                        continue;
                    }
                    int value = -Search(depth - 1, false);
                    int prevBest = bestValue;
                    bestValue = Math.Max(bestValue, value);
                    if (prevBest < bestValue) NextMove = move;
                    GameManager.UndoMove(move);
                    GameManager.TurnState = saved;
                }

                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;
                foreach (Move move in moves)
                {
                    if (move.TargetCell != null && move.TargetCell.Index.Y == GameManager.TurnState.WinIndex)
                    {
                        return int.MaxValue;
                    }
                    TurnState saved = GameManager.TurnState.Copy();
                    GameManager.TryMove(move);
                    if (!GameManager.PathExists()) 
                    {
                        GameManager.UndoMove(move);
                        GameManager.TurnState = saved;
                        continue;
                    }
                    int value = -Search(depth - 1, true);
                    bestValue = Math.Min(bestValue, value);
                    GameManager.UndoMove(move);
                    GameManager.TurnState = saved;
                }

                return bestValue;
            }
            
        }

        private static int Evaluate(bool maximizingPlayer)
        {
            Cell cell = maximizingPlayer
                ? GameManager.TurnState.PlayerPosition
                : GameManager.TurnState.OpponentPosition;
            int winIndex = maximizingPlayer
                ? GameManager.TurnState.WinIndex
                : GameManager.TurnState.LoseIndex;
            
            Cell opponent = maximizingPlayer
                ? GameManager.TurnState.OpponentPosition
                : GameManager.TurnState.PlayerPosition;
            int loseIndex = maximizingPlayer
                ? GameManager.TurnState.LoseIndex
                : GameManager.TurnState.WinIndex;
            
            int opponentShortest = int.MaxValue;
            
            for (int i = 0; i < GameManager.Cells.GetLength(0); i++)
            {
                List<Cell> path = Pathfinder.FindPath(opponent ,new Point(i, loseIndex));
                if (path == null) continue;
                if (path.Count < opponentShortest) opponentShortest = path.Count;
            }

            int shortest = int.MaxValue;
            for (int i = 0; i < GameManager.Cells.GetLength(0); i++)
            {
                List<Cell> path = Pathfinder.FindPath(cell ,new Point(i, winIndex));
                if (path == null) continue;
                if (path.Count < shortest) shortest = path.Count;
            }

            return maximizingPlayer ? (opponentShortest - shortest) : -(opponentShortest - shortest);
        }
    }
}