using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    class RandomAgent : IAgent
    {
        public (int, int) GetNextMove(Board.Board board)
        {
            var positions = board.GetLegalMoves();

            throw new NotImplementedException();
        }
    }
}
