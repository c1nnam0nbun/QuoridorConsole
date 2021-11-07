using System;
using System.Collections.Generic;
using QC.States;
using QC.Structures;

namespace QC
{
    public static class Input
    {
        public static void ReadInput()
        {
            string move = Console.ReadLine();
            if (move == null) throw new Exception("Could not get input from console.");
            string[] tokens = move.Split(' ');

            switch (tokens.Length)
            {
                case 1: ProcessColor(tokens[0]); break;
                case 2:
                    if (tokens[0] == "wall") ProcessWall(tokens);
                    else ProcessMove(tokens);
                    break;
                default: throw new Exception("Unexpected number of parameters.");
            }
        }

        private static void ProcessColor(string color)
        {
            color = color.ToLower();
            switch (color)
            {
                case "black": 
                    GameManager.TurnState = new BlackTurnState(true);
                    GameManager.TurnState.ChangeTurn();
                    break;
                case "white":
                    GameManager.TurnState = new WhiteTurnState(true);
                    break;
                default: throw new Exception($"Undefined color {color}");
            }
        }

        private static void ProcessMove(IReadOnlyList<string> tokens)
        {
            string action = tokens[0].ToLower();
            if (action != "move" && action != "jump") throw new Exception($"Undefined move {tokens[0]}");
            Move move = new Move(action, GameManager.GetCellByLocation(tokens[1].ToLower()));
            GameManager.ProcessMove(move);
        }

        private static void ProcessWall(IReadOnlyList<string> tokens)
        {
            string action = tokens[0].ToLower();
            if (action != "wall")
                throw new Exception($"Undefined command {tokens[0]}");

            string param = tokens[1].ToLower();
            
            string wallLocation = param.Substring(0, 2);
            string wallDirection = param.Substring(2);

            Move move = new Move(action, GameManager.GetAnchorPointByLocation(wallLocation), wallDirection);
            GameManager.ProcessMove(move);
        }
    }
}