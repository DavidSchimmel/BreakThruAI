using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class EvaluationMaterialBalance : IEvaluationHeuristic
    {
        private const int MATERIAL_BALANCE_WEIGHT = 50;
        public int Evaluate(Board.Board board, int evaluatingPlayer)
        {
            int score = 0;
            int winningPositionFor = board.CheckTerminalPosition();
            int signum = evaluatingPlayer == 0 ? 1 : -1;

            if (winningPositionFor == 0)
            {
                score += 1000;
            }
            if (winningPositionFor == 1)
            {
                score -= 1000;
            }
            score =  score * signum;

            score += (JudgeMaterialBalance(board) * MATERIAL_BALANCE_WEIGHT * signum);


            return score;
        }

        private int JudgeMaterialBalance(Board.Board board)
        {
            int value = 0;
            if (board.captures.Count > 0)
            {
                foreach ((int, int, int) capture in board.captures)
                {
                    if (capture.Item3 == 1)
                    {
                        value += 1;
                    }
                    else
                    {
                        value -= 1;
                    }
                }
            }
            return value;
        }
    }
}
