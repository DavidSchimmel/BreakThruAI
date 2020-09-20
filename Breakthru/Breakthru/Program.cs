using System;
using System.Collections.Generic;
using AgentLibrary;
using Board;

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
            IAgent player0 = new RandomAgent();
            IAgent player1 = new RandomAgent();
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
                string test = Console.ReadLine();
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
