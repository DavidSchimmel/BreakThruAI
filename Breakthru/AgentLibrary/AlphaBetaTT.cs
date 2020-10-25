using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AgentLibrary
{
    public class AlphaBetaTT : IAgent
    {
        private readonly ulong _zobristKeyLength = (ulong)Math.Pow(2, 27);

        private int playerNumber;
        private IEvaluationHeuristic evaluationHeuristic;
        private const int randomRange = 50;
        public int timer = 600;
        public int depth = 5;
        Random random = new Random();

        Dictionary<int, ulong> ttKeys;
        //Dictionary<ulong, (int, (int, int), int, int, int)> tt; // checksum, move, value, flag (0:=exact, 1:=lower, 2:=upper), search depth
        (int, (int, int), int, int, int)[] tt2;
        ulong zobristHash;

        public AlphaBetaTT(int playerNumber, Board.Board initialBoard, IEvaluationHeuristic evaluationHeuristic)
        {
            this.playerNumber = playerNumber - 1;
            this.evaluationHeuristic = evaluationHeuristic;
            InitTTKeys(initialBoard);
            //tt = new Dictionary<ulong, (int, (int, int), int, int, int)>();
            tt2 = new (int, (int, int), int, int, int)[(int) Math.Pow(2, 28)];
        }

        public void InitTTKeys(Board.Board board)
        {
            ttKeys = new Dictionary<int, ulong>();
            zobristHash = 0;

            for (int i = 0; i < board.width * board.height; i++)
            {
                ttKeys[i + board.width * board.height * 0] = (ulong)((random.NextDouble() * 2.0 - 1.0) * ulong.MaxValue);
                ttKeys[i + board.width * board.height * 1] = (ulong)((random.NextDouble() * 2.0 - 1.0) * ulong.MaxValue);
                ttKeys[i + board.width * board.height * 2] = (ulong)((random.NextDouble() * 2.0 - 1.0) * ulong.MaxValue);
                ttKeys[i + board.width * board.height * 4] = (ulong)((random.NextDouble() * 2.0 - 1.0) * ulong.MaxValue);
                zobristHash = zobristHash ^ ttKeys[i + board.width * board.height * board.board[i]];
            }
        }

        public void Rehash(Board.Board board, (int, int) move)
        {
            zobristHash = zobristHash ^ ttKeys[(move.Item1 + board.width * board.height * board.board[move.Item1])];
            zobristHash = zobristHash ^ ttKeys[(move.Item2 + board.width * board.height * board.board[move.Item2])];
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
                Rehash(board, move);

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
                Rehash(board, move);

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

            Console.WriteLine($"player {board.activePlayer}");
            Console.WriteLine($"score {score}");

            return selectedMove;
        }

        private int AB(ref Board.Board board, int alpha, int beta, int depth) 
        {
            (int, (int, int), int, int, int) storedMove;
            int oldAlpha = alpha;
            (int, int) bestMove = (-1, -1);

            //if (tt.TryGetValue(zobristHash % _zobristKeyLength, out storedMove))
            if (tt2[zobristHash % _zobristKeyLength] != (0, (0, 0), 0, 0, 0))
            {
                storedMove = tt2[zobristHash % _zobristKeyLength];
                if (storedMove.Item1 == (int)(zobristHash / _zobristKeyLength))
                {
                    if (storedMove.Item5 > depth && storedMove.Item4 == 0)
                    {
                        return storedMove.Item3;
                    }
                    else if (storedMove.Item5 > depth && storedMove.Item4 == 1) // lower bound
                    {
                        alpha = Math.Max(storedMove.Item3, alpha);
                    }
                    else if (storedMove.Item5 > depth && storedMove.Item4 == 2) // upper bound
                    {
                        beta = Math.Min(storedMove.Item3, beta);
                    }
                    if (alpha >= beta)
                    {
                        return storedMove.Item3;
                    }

                    bestMove = storedMove.Item2;
                }
            }

            if (board.CheckTerminalPosition() >= 0)
            {
                return evaluationHeuristic.Evaluate(board, board.activePlayer) + random.Next(-randomRange, randomRange);
            }

            if (depth == 0)
            {
                #region quiescence search
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
                        Rehash(board, move);

                        if (tt2[zobristHash % _zobristKeyLength] != (0, (0, 0), 0, 0, 0))
                        {
                            if (tt2[zobristHash % _zobristKeyLength].Item1 == (int)(zobristHash / _zobristKeyLength))
                            {
                                board.Undo();
                                Rehash(board, move);
                                return tt2[zobristHash % _zobristKeyLength].Item3;
                            }
                        }
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
                        Rehash(board, move);
                        tt2[zobristHash % _zobristKeyLength] = (((int)(zobristHash / _zobristKeyLength)), move, selectScore, 0, 0);

                        if (selectScore > beta)
                        {
                            break;
                        }
                    }
                    return selectScore;
                }
                #endregion quiescence search

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
                Rehash(board, move);

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
                    bestMove = move;
                }

                board.Undo();
                Rehash(board, move);

                if (score > beta)
                {
                    break;
                }
            }
            #region TT assignment
            int flag = 0;
            if (score <= oldAlpha)
            {
                flag = 2;
            }
            else if (score >= beta)
            {
                flag = 1;
            }
            tt2[zobristHash % _zobristKeyLength] = (((int) (zobristHash / _zobristKeyLength)), bestMove, score, flag, depth);
            #endregion

            return score;
        }
    }
}
