using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class WeightedEvaluation : IEvaluationHeuristic
    {
        private const int GOLD_CAPTURES_WEIGHT = 150;
        private const int SILVER_CAPTURES_WEIGHT = 200;
        private const int SOUTH_BIAS_WEIGHT = 50;
        private const int FLAGSHIP_LIBERTY_WEIGHT = 20;
        private const int FLAFSHIP_LIBERTY_DEFAULT = 4;

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
            return liberties - FLAFSHIP_LIBERTY_DEFAULT;
        }
    }
}
