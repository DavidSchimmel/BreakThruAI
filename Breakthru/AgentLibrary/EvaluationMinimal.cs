using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class EvaluationMinimal : IEvaluationHeuristic
    {
        public int Evaluate(Board.Board board, int searchingPlayer)
        {
            int score = 0;
            int winningPositionFor = board.CheckTerminalPosition();

            if (winningPositionFor == 0)
            {
                score += 1000;
            }
            if (winningPositionFor == 1)
            {
                score -= 1000;
            }

            score = searchingPlayer == 0 ? score : (score * -1);

            return score;
        }
    }
}
