using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgentLibrary
{
    public class RandomAgent : IAgent
    {
        public (int, int) GetNextMove(Board.Board board)
        {
            Random random = new Random();
            var legalMoves = board.GetLegalMoves();

            int randomIndex = random.Next(legalMoves.Count);
            return legalMoves.ToArray()[randomIndex];
            throw new NotImplementedException();
        }
    }
}
