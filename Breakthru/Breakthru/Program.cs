using System;
using System.Collections.Generic;
using AgentLibrary;
using Board;


/*
 First terminated game: 
alphabetastandard with weightedevaluation with parameters:
        private const int GOLD_CAPTURES_WEIGHT = 150;
        private const int SILVER_CAPTURES_WEIGHT = 200;
        private const int SOUTH_BIAS_WEIGHT = 50;
        private const int FLAGSHIP_LIBERTY_WEIGHT = 20;
        private const int FLAFSHIP_LIBERTY_DEFAULT = 4;
4 ply deep search
~ 7 minutes for the whole game, silver won
 */

namespace Breakthru
{
    class Program
    {
        static void Main(string[] args)
        {
            Board.Board board = new Board.Board(11, 11);
            board.Initialize();
            board.Print();

            #region test
            IAgent player0 = new AlphaBetaStandard(1, new WeightedEvaluation());//WeightedEvaluation());//ConsolePlayer();//RandomAgent();
            IAgent player1 = new AlphaBetaStandard(2, new WeightedEvaluation());//EvaluationMaterialBalance());
            IAgent[] players = new IAgent[2];
            players[0] = player0;
            players[1] = player1;
            #endregion test 

            int player = 0;
            while (player == 0 || player == 1)
            {
                (int, int) nextMove = players[player].GetNextMove(board);
                board.Move(nextMove);
                Console.WriteLine($"Next Move: {nextMove.Item1}->{nextMove.Item2}");
                board.Print();
                player = board.activePlayer;
            }

            if (player == -1)
            {
                Console.WriteLine("Silver player won");
            } else if (player == -2)
            {
                Console.WriteLine("Gold player won");
            }

            LinkedList<(int, int)> possibleMoves = board.GetLegalMoves();
            
            Console.WriteLine("END!");
        }
    }
}
