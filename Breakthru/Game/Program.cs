using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(11, 11);
            board.InitializeDefault();
            Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World!");

            string input = ""; 
            while (input != "exit")
            {
                board.Print();
                Console.WriteLine("Enter move coordinates: (x1y1, x2y2)");
                input = Console.ReadLine();
                string[] inputArray = input.Split(",");
                var origin = board.ParseLocationString(inputArray[0]);
                var target = board.ParseLocationString(inputArray[1]);

                board.MakeMove(origin.Item1, origin.Item2, target.Item1, target.Item2);
            }
        }
    }
}
