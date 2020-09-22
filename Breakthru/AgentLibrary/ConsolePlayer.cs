using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    class ConsolePlayer : IAgent
    {
        public (int, int) GetNextMove(Board.Board board)
        {
            Console.ReadLine(); 
        }
    }
}
