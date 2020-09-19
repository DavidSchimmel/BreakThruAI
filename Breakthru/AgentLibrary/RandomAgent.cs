using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    class RandomAgent : IAgent
    {
        public (int, int) GetNextMove(Board.Board board)
        {
            Random random = new Random();
            var legalMoves = board.GetLegalMoves();

            int randomIndex = random.Next(legalMoves.Count);
            throw new NotImplementedException();
        }
    }
}
