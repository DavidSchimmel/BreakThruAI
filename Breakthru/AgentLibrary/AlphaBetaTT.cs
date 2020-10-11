using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public class AlphaBetaTT : IAgent
    {
        private int playerNumber;
        private IEvaluationHeuristic evaluationHeuristic;
        private const int randomRange = 50;
        public int timer = 600;
        public int depth = 4;
        Random random = new Random();

        public AlphaBetaTT(int playerNumber, IEvaluationHeuristic evaluationHeuristic)
        {
            this.playerNumber = playerNumber - 1;
            this.evaluationHeuristic = evaluationHeuristic;
        }

        public (int, int) GetNextMove(Board.Board board)
        {
            DateTime startTime = DateTime.Now;
            LinkedList<(int, int)> moves = board.GetLegalMoves();
            (int, int) selectedMove = (-1, -1);
            int score = int.MinValue / 2;
            int value = score;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            // treat no valid move options
            if (moves.Count == 0)
            {

            }

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

            DateTime endTime = DateTime.Now;
            var timeDelta = endTime - startTime;
            timer = timer - (int) timeDelta.TotalSeconds;
            Console.WriteLine($"{timer} seconds remaining");
            if (timer < 180)
            {
                depth = 3;
            }
            else if (timer < 45)
            {
                depth = 2;
            }

            return selectedMove;
        }

        private int AB(ref Board.Board board, int alpha, int beta, int depth) // maybe can implement minimal search window and deep pruning by giving alpha and beta as reference
        {
            if (depth == 0)
            {
                //selective deepening in case of captures, might want to extend to other more or less forcing moves ("check", looming escpae) 
                // right now seems to slow down the calculations considerably, if tt doesnt help, might consider also to simplify it to only search recapturing sequences
                if (board.captures.Count > 0 && board.captures.Last.Value.Item1 == (board.turnCounter - 1))
                {
                    int selectScore = int.MinValue / 2;
                    int selectValue = selectScore;

                    LinkedList<(int, int)> forcingMoves = board.GetForcingMoves();

                    foreach ((int, int) move in forcingMoves)
                    {
                        board.Move(move);

                        if (board.remainingActions <= 1)
                        {
                            selectValue = AB(ref board, Math.Max(alpha, selectScore), beta, 0);
                        }
                        else
                        {
                            selectValue = -AB(ref board, -1 * beta, -1 * Math.Max(alpha, selectScore), 0);
                        }

                        if (selectValue > selectScore)
                        {
                            selectScore = selectValue;
                        }

                        board.Undo();

                        if (selectScore > beta)
                        {
                            break;
                        }

                    }
                    return selectScore;
                }

                return evaluationHeuristic.Evaluate(board, board.activePlayer) + random.Next(-randomRange, randomRange);
            }

            if (depth == 0 || board.CheckTerminalPosition() >= 0)
            {
                return evaluationHeuristic.Evaluate(board, board.activePlayer) + random.Next(-randomRange, randomRange);
            }

            LinkedList<(int, int)> moves = board.GetLegalMoves();
            if (moves.Count == 0)
            {
                return evaluationHeuristic.Evaluate(board, board.activePlayer) + random.Next(-randomRange, randomRange);
            }

            int score = int.MinValue / 2;
            int value = score;

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
