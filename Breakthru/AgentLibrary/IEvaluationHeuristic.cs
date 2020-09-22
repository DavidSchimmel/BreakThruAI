using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    interface IEvaluationHeuristic
    {
        public int Evaluate(Board.Board board, int searchingPlayer);
    }
}
