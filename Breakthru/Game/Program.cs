using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(11, 11);
            board.InitializeDefault();
            string input = "";

            //agent select
            Console.WriteLine("Available Agenst:");
            string agentSelection = "\r\nHUMAN\r\nRANDOM\r\n"; // TODO: replace with some sort of dict or struct once the agent interface is in place kek
            Console.WriteLine(agentSelection);
            Console.WriteLine("Select Agent for player one!");
            string agent1 = Console.ReadLine(); // TODO: Also replace with the key selection mechanism
            Console.WriteLine("Select Agent for player 2!");
            string agent2 = Console.ReadLine(); // TODO: see above 



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
