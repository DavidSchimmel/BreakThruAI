using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks.Sources;

namespace AgentLibrary
{
    public class AlphaBetaStandard : IAgent
    {
        private int playerNumber;
        private IEvaluationHeuristic evaluationHeuristic;
        private const int randomRange = 5;
        public int depth = 1;
        Random random = new Random();

        public AlphaBetaStandard(int playerNumber, IEvaluationHeuristic evaluationHeuristic)
        {
            this.playerNumber = playerNumber;
            this.evaluationHeuristic = evaluationHeuristic;
        }

        public (int, int) GetNextMove(Board.Board board)
        {
            LinkedList<(int, int)> moves = board.GetLegalMoves();
            (int, int) selectedMove = (-1, -1);
            int score = int.MinValue / 2;
            int value = score;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            foreach ((int, int) move in moves)
            {
                board.Move(move);

                if (board.remainingActions <= 1)
                {
                    value = AB(ref board, Math.Max(alpha, score), beta, depth - 1);
                }
                else
                {
                    value = -AB(ref board, -1 * beta, -1 * Math.Max(alpha, score), depth - 1);
                }

                if (value > score)
                {
                    score = value;
                    selectedMove = move;
                }

                board.Undo();

                if (score > beta)
                {
                    break;
                }

            }
            return selectedMove;
        }

        private int AB(ref Board.Board board, int alpha, int beta, int depth) // maybe can implement minimal search window and deep pruning by giving alpha and beta as reference
        {
            if (!(depth != 0 || board.CheckTerminalPosition() >= 0)) {
                return evaluationHeuristic.Evaluate(board, playerNumber) + random.Next(-randomRange, randomRange);
            }

            LinkedList<(int, int)> moves = board.GetLegalMoves();
            if (moves.Count == 0)
            {
                return evaluationHeuristic.Evaluate(board, playerNumber) + random.Next(-randomRange, randomRange);
            }

            int score = int.MinValue / 2;
            int value = score;

            foreach ((int, int) move in moves)
            {
                board.Move(move);
                
                if (board.remainingActions <= 1)
                {
                    value = AB(ref board, Math.Max(alpha, score), beta, depth);
                }
                else
                {
                    value = -AB(ref board, -1 * beta, -1 * Math.Max(alpha, score), depth - 1);
                }

                if (value > score)
                {
                    score = value;
                }

                board.Undo();
             
                if (score > beta)
                {
                    break;
                }

            }
            return score;
        }
    }
}
