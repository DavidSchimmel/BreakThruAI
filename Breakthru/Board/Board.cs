using System;
using System.Collections.Generic;
using System.Linq;

namespace Board
{
    public class Board
    {
        private static char[] BOARD_ICONS = { '.', 'G', 'S', ' ', 'F' };
        private static int[] DEFAULT_POSITION = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0,
                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                 0, 1, 0, 0, 2, 2, 2, 0, 0, 1, 0,
                                                 0, 1, 0, 2, 0, 0, 0, 2, 0, 1, 0,
                                                 0, 1, 0, 2, 0, 4, 0, 2, 0, 1, 0,
                                                 0, 1, 0, 2, 0, 0, 0, 2, 0, 1, 0,
                                                 0, 1, 0, 0, 2, 2, 2, 0, 0, 1, 0,
                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0,
                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        int activePlayer;
        int remainingActions;
        int flagShipPos;
        int turnCounter;
        LinkedList<(int, int)> log;
        LinkedList<(int, int, int)> captures; // turnnumber, position, player
        Queue<(int, int, int)> moves = new Queue<(int, int, int)> ();

        //int[] position;
        int[] board;
        int width;
        int height;

        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.board = new int[(width * height)];
            this.turnCounter = 0;
            this.log = new LinkedList<(int, int)>();
            this.captures = new LinkedList<(int, int, int)>();
            this.activePlayer = 0;
            this.remainingActions = 2;
        }

        public int Move((int, int) move)
        {
            //debug query, remove this in the competetive Version
            if (!GetLegalMoves().Contains(move))
            {
                throw new Exception("Attempted an illegal move!");
            }

            log.AddLast(move);

            if(board[move.Item2] != 0)
            {
                captures.AddLast((turnCounter, move.Item2, ((activePlayer + 1) % 2)));
            }

            if (SetRemainingActions(move) == 0)
            {
                activePlayer = (activePlayer + 1) % 2;
                remainingActions = 2;
            }

            turnCounter++;

            board[move.Item2] = board[move.Item1];
            board[move.Item1] = 0;

            return activePlayer;
        }

        public int SetRemainingActions((int, int) move)
        {
            if (board[move.Item2] != 0 || board[move.Item1] == 4)
            {
                remainingActions--;
            }
            remainingActions--;

            return remainingActions;
        }

        public bool Initialize(int[] initialPosition = null)
        {
            try
            {
                if (initialPosition != null)
                {
                    board = initialPosition;
                    return true;
                }
                else
                {
                    board = DEFAULT_POSITION;
                    width = 11;
                    height = 11;
                    return true;
                }
            } 
            catch (Exception e)
            {
                return false;
            }   
        }

        public LinkedList<(int, int)> GetLegalMoves()
        {
            LinkedList<(int, int)> legalMoves = new LinkedList<(int, int)>();

            for (int tile = 0; tile < width * height; tile++)
            {
                if (board[tile] != 0 && board[tile] % 2 == activePlayer)
                {
                    // check captures
                    if (remainingActions >= 1)
                    {
                        legalMoves = GetPossibleCaptures(legalMoves, tile);
                    }

                    // check moves
                    legalMoves = GetPossibleMovements(legalMoves, tile);

                    if (remainingActions <= 1)
                    {
                        var flagShipMoves = legalMoves.Where((move) => board[move.Item1] == 4).ToList();
                        foreach ((int, int) flagShipMove in flagShipMoves) {
                            legalMoves.Remove(flagShipMove);
                        }
                        var lastMovedPieceMoves = legalMoves.Where((move) => move.Item1 == log.Last.Value.Item2).ToList();
                        foreach ((int, int) lastMovedPieceMove in lastMovedPieceMoves)
                        {
                            legalMoves.Remove(lastMovedPieceMove);
                        }
                    }
                }
            }

            // get flagship moves
            // get captures
            // get other moves

            return legalMoves;
        }

        public LinkedList<(int, int)> GetPossibleCaptures(LinkedList<(int, int)> moveList, int source)
        {
            int upLeft = source - width - 1;
            int upRight = source - width + 1;
            int downLeft = source + width - 1;
            int downRight = source + width + 1;
            
            if (upLeft % width < source % width && 
                                 upLeft >= 0 && 
                                 board[upLeft] != 0 && 
                                 board[upLeft] % 2 != board[source] % 2)
            {
                moveList.AddFirst((source, upLeft));
            }

            if (upRight % width > source % width && 
                                  upRight >= 0 && 
                                  board[upRight] != 0 && 
                                  board[upRight] % 2 != board[source] % 2)
            {
                moveList.AddFirst((source, upRight));
            }

            if (downLeft % width < source % width && 
                                   downLeft < width * height && 
                                   board[downLeft] != 0 && 
                                   board[downLeft] % 2 != board[source] % 2)
            {
                moveList.AddFirst((source, downLeft));
            }

            if (downRight % width > source % width && 
                                    downRight < width * height && 
                                    board[downRight] != 0 && 
                                    board[downRight] % 2 != board[source] % 2)
            {
                moveList.AddFirst((source, downRight));
            }

            return moveList;
        }

        public LinkedList<(int, int)> GetPossibleMovements(LinkedList<(int, int)> moveList, int source)
        {
            int target = source - 1;
            while (target >= ((int)(source / width) * width) && board[target] == 0)
            {
                moveList.AddLast((source, target));
                target--;
            }
            
            target = source + 1;
            while (target < ((int)(source / width) + 1) * width && board[target] == 0)
            {
                moveList.AddLast((source, target));
                target++;
            }

            target = source - width;
            while (target >= 0 && board[target] == 0)
            {
                moveList.AddLast((source, target));
                target -= width;
            }

            target = source + width;
            while (target < (width * height) && board[target] == 0)
            {
                moveList.AddLast((source, target));
                target += width;
            }

            return moveList;
        }

        public bool CheckTerminalPosition()
        {
            return false;
        }

        public string GetString()
        {
            string representation = "\r\n";

            for (int i = width*(height-1); i >= 0; i -= width)
            {
                representation += $"{((i/width + 1)):00}|";

                string rowString = "";
                for (int j = 0; j<width; j++)
                {
                    rowString += board[i+j];
                }

                rowString = rowString.Replace('0', BOARD_ICONS[0]).Replace('1', BOARD_ICONS[1]).Replace('2', BOARD_ICONS[2]).Replace('4', BOARD_ICONS[4]);
                representation += rowString + "\r\n";
            }

            representation += "--+";
            for (int i = 0; i < width; i++)
            {
                representation += "-";
            }

            representation += "\r\n  |";
            for (int i = 0; i < width; i++)
            {
                representation += Convert.ToChar(i + 97);
            }

            return representation;
        }
        public void Print()
        {
            Console.WriteLine(GetString());
        }

    }
}
