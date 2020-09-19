using System;
using System.Collections.Generic;
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

            LinkedList<(int, int)> possibleMoves = board.GetLegalMoves();
            
            Console.WriteLine("END!");
        }
    }
}
