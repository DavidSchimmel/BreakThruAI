using Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    interface IAgent
    {
        public (int, int) GetNextMove(Board.Board board);
    }
}
