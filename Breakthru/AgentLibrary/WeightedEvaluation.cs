using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class WeightedEvaluation : IEvaluationHeuristic
    {
        private readonly int GOLD_CAPTURES_WEIGHT = 150;
        private readonly int SILVER_CAPTURES_WEIGHT = 200;
        private readonly int SOUTH_BIAS_WEIGHT = 50;
        private readonly int FLAGSHIP_LIBERTY_WEIGHT = 20;
        private readonly int FLAGSHIP_LIBERTY_DEFAULT = 4;

        public WeightedEvaluation() { }

        public WeightedEvaluation(int goldCapturesWeight, int silverCapturesWeight, int SouthBiasWeight, int flagshipLibertyWeight, int flagshipLibertyDefault)
        {
            GOLD_CAPTURES_WEIGHT = goldCapturesWeight;
            SILVER_CAPTURES_WEIGHT = silverCapturesWeight;
            SOUTH_BIAS_WEIGHT = SouthBiasWeight;
            FLAGSHIP_LIBERTY_WEIGHT = flagshipLibertyWeight;
            FLAGSHIP_LIBERTY_DEFAULT = flagshipLibertyDefault;
        }

        public int Evaluate(Board.Board board, int evaluatingPlayer)
        {
            int score = 0;
            int winningPositionFor = board.CheckTerminalPosition();
            int signum = evaluatingPlayer == 0 ? 1 : -1;

            if (winningPositionFor == 0)
            {
                 return 10000 * signum;
            }
            if (winningPositionFor == 1)
            {
                return -10000 * signum;
            }
            score = score * signum;

            score = score +
                    CountGoldCaptures(board) * GOLD_CAPTURES_WEIGHT * signum +
                    CountSilverCaptures(board) * SILVER_CAPTURES_WEIGHT * signum + 
                    CalculateSouthBias(board) * SOUTH_BIAS_WEIGHT / board.width * signum + 
                    CalculateFlagshipLiberties(board) * FLAGSHIP_LIBERTY_WEIGHT * signum;


            return score;
        }

        private int CountGoldCaptures(Board.Board board)
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
                }
            }
            return value;
        }

        private int CountSilverCaptures(Board.Board board)
        {
            int value = 0;
            if (board.captures.Count > 0)
            {
                foreach ((int, int, int) capture in board.captures)
                {
                    if (capture.Item3 != 1)
                    {
                        value -= 1;
                    }
                }
            }
            return value;
        }

        private int CalculateSouthBias(Board.Board board)
        {
            int value = 0;
            for (int i = 0; i < board.width * board.height; i++)
            {
                if (board.board[i] == 4)
                {
                    value += (i / board.width);
                }
            }
            return board.width - value;
        }

        private int CalculateFlagshipLiberties(Board.Board board)
        {
            int liberties = 0;
            for (int i = 0; i < board.board.Length; i++)
            {
                if (board.board[i] == 4)
                {
                    int target = i + 1;
                    while (target >= ((int)(i / board.width) * board.width) && board.board[target] == 0)
                    {
                        liberties++;
                        target--;
                    }

                    target = i + 1;
                    while (target < ((int)(i / board.width) + 1) * board.width && board.board[target] == 0)
                    {
                        liberties++;
                        target++;
                    }

                    target = i - board.width;
                    while (target >= 0 && board.board[target] == 0)
                    {
                        liberties++;
                        target -= board.width;
                    }

                    target = i + board.width;
                    while (target < (board.width * board.height) && board.board[target] == 0)
                    {
                        liberties++;
                        target += board.width;
                    }
                }
            }
            return liberties - FLAGSHIP_LIBERTY_DEFAULT;
        }
    }
}
