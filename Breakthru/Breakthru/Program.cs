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
        private const string DEFAULT_LOG_PATH = @"C:\Users\David Schimmel\Desktop\game1\logs\gamelog.txt";

        static void Main(string[] args)
        {
            Board.Board board = new Board.Board(11, 11);
            board.Initialize();
            board.Print();

            #region test
            IAgent player0 = new ConsolePlayer();//AlphaBetaTT(1, new WeightedEvaluation(200, 150, 70, 20, 4));//WeightedEvaluation());//ConsolePlayer();//RandomAgent();
            IAgent player1 = new AlphaBetaTT(2, new WeightedEvaluation(150, 200, 50, 20, 4));//EvaluationMaterialBalance());
            IAgent[] players = new IAgent[2];
            players[0] = player0;
            players[1] = player1;
            #endregion test 

            #region replay
            if (false)
            {
                ReplayLog(DEFAULT_LOG_PATH, board, true);

                Environment.Exit(0);
            }
            #endregion

            int player = 0;

            #region restore position from log
            if (false)
            {
                ReplayLog(DEFAULT_LOG_PATH, board, false);
            }
            #endregion

            while (player == 0 || player == 1)
            {
                (int, int) nextMove = players[player].GetNextMove(board);
                board.Move(nextMove);
                Console.WriteLine($"Next Move: {board.SerializeMove(board.log.Last.Value)}");
                board.Print();
                player = board.activePlayer;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(DEFAULT_LOG_PATH, true))
                {
                    file.WriteLine($"{board.SerializeMove(nextMove)}");
                }

                if (board.CheckTerminalPosition() == 0)
                {
                    player = -2;
                }
                else if (board.CheckTerminalPosition() == 1)
                {
                    player = -1;
                }
            }

            if (player == -1)
            {
                Console.WriteLine("Silver player won");
            }
            else if (player == -2)
            {
                Console.WriteLine("Gold player won");
            }

            LinkedList<(int, int)> possibleMoves = board.GetLegalMoves();

            Console.WriteLine("END!");
        }

        public static void ReplayLog(string path, Board.Board board, bool showcase)
        {
            try
            {
                string logString;
                using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                {
                    logString = file.ReadToEnd();
                }

                List<(int, int)> moveList = board.ParseLog(logString);
                board.Replay(moveList, showcase);

            }
            catch (Exception e)
            {
                Console.WriteLine("Recreading board position failed");
            }
        }
    }
}
