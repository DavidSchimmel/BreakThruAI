using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class ConsolePlayer : IAgent
    {
        public (int, int) GetNextMove(Board.Board board)
        {
            string moveString = Console.ReadLine();

            try
            {
                return board.ParseMove(moveString);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return (-1, -1);
        }
    }
}
