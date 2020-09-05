using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(11, 11);
            board.InitializeDefault();
            board.Print();
            Console.WriteLine("Hello World!");
        }
    }
}
