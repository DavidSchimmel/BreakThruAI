using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class Board
    {

        private int[][] board;
        public static int[][] DEFAULT_POSITION  =  { new int[] { 1, 3, 4 }, new int[] { 1, 3, 5 }, new int[] { 1, 3, 6 }, //Gold player escorts
                                                     new int[] { 1, 4, 3 }, new int[] { 1, 5, 3 }, new int[] { 1, 6, 3 },
                                                     new int[] { 1, 7, 4 }, new int[] { 1, 7, 5 }, new int[] { 1, 7, 6 },
                                                     new int[] { 1, 4, 7 }, new int[] { 1, 5, 7 }, new int[] { 1, 6, 7 },
                                                     new int[] { 3, 5, 5 }, //flagship
                                                     new int[] { 2, 1, 3 }, new int[] { 2, 1, 4 }, new int[] { 2, 1, 5 }, new int[] { 2, 1, 6 }, new int[] { 2, 1, 7 }, //silver player pieces
                                                     new int[] { 2, 3, 1 }, new int[] { 2, 4, 1 }, new int[] { 2, 5, 1 }, new int[] { 2, 6, 1 }, new int[] { 2, 7, 1 },
                                                     new int[] { 2, 9, 3 }, new int[] { 2, 9, 4 }, new int[] { 2, 9, 5 }, new int[] { 2, 9, 6 }, new int[] { 2, 9, 7 },
                                                     new int[] { 2, 3, 9 }, new int[] { 2, 4, 9 }, new int[] { 2, 5, 9 }, new int[] { 2, 6, 9 }, new int[] { 2, 7, 9 }
                                                  };
        private static char[] BOARD_ICONS = { '.', 'G', 'S', 'F' };
        private int activePlayer = 0; // active player 0 is gold, 1 is silver

        public Board(int width, int height)
        {
            board = new int[height][];
            for (int i=0; i<board.Length; i++)
            {
                board[i] = new int[width];
            }
        }

        public void InitializeDefault()
        {
            foreach (int[] piece in DEFAULT_POSITION)
            {
                int column = piece[1];
                int row = piece[2];
                int value = piece[0];
                board[row][column] = value;
            }
        }

        public void MakeMove(int x1, int y1, int x2, int y2)
        {
            if (!CheckLegalMove(x1, y1, x2, y2)) {
                throw new Exception($"Illegal Move (X{x1 + 1}), try again!");
            }

            board[y2][x2] = board[y1][x1];
            board[y1][x1] = 0;

        }

        public bool CheckLegalMove(int x1, int y1, int x2, int y2)
        {
            //check if piece belongs to active player
            //check if there is a piece on the origin location
            //check if there is either no piece on the target location or
            //check if there is a opposing capturable position is on target location and enough actions for capture remain
            //check if the piece has already been moved (might remember the last target position and check if the new source position is the same)
            //check for no move (origin = source)

            //or just make a legal move dictionary that gets used by the search already.
            return true;
        }

        public (int, int) ParseLocationString(string input)
        {
            int x, y;
            var inputArray = input.ToCharArray();

            if (inputArray.Length != 2)
            {
                throw new Exception($"Invalid move format: {input}");
            }

            x = (int)(char)(inputArray[0]);
            x -= 97;
            y = (int)Char.GetNumericValue(inputArray[1]);
            y -= 1;
            return (x, y);
        }

        public List<List<int>> GenerateLegalMoves()
        {
            List<List<int>> legalMoves = new List<List<int>>();
            
            if (activePlayer == 0) //  gold's turn
            {

            }
            else if (activePlayer == 1) // silber's turn
            {

            }
            return legalMoves;
        }

        private List<int> AddPossibleCaptures(int x, int y)
        {
            List<int> possibleCaptures = new List<int>();

            if (x-1 >= 0 && y-1 >= 0)
            {
                if (checkHostileTarget(x, y, x-1, y-1))
                {
                    possibleCaptures.Add(x - 1);
                    possibleCaptures.Add(y - 1);
                }
            }

            if (x - 1 >= 0 && y + 1 < board[0].Length)
            {
                if (checkHostileTarget(x, y, x - 1, y + 1))
                {
                    possibleCaptures.Add(x - 1);
                    possibleCaptures.Add(y + 1);
                }
            }

            if (x + 1 < board[0].Length && y - 1 >= 0)
            {
                if (checkHostileTarget(x, y, x + 1, y - 1))
                {
                    possibleCaptures.Add(x + 1);
                    possibleCaptures.Add(y - 1);
                }
            }

            if (x + 1 < board[0].Length && y + 1 < board[0].Length)
            {
                if (checkHostileTarget(x, y, x + 1, y + 1))
                {
                    possibleCaptures.Add(x + 1);
                    possibleCaptures.Add(y + 1);
                }
            }

            return possibleCaptures;
        }

        private bool checkHostileTarget(int x1, int y1, int x2, int y2)
        {

        }

        private List<int> AddPossibleMovements()
        {
            List<int> possibleMovements = new List<int>();
        }

        public string GetLocationString(int x, int y)
        {
            x += 97;
            y +=  1;
            return $"{Convert.ToChar(x)}{y.ToString()}";
        }

        public string GetString()
        {
            string representation = "\r\n";

            for (int i = board.Length -1; i >= 0; --i)
            {
                representation += $"{(i+1):00}|";

                string rowString = "";
                foreach (var tile in board[i])
                {
                    representation += tile;
                }
                rowString = rowString.Replace('0', BOARD_ICONS[0]).Replace('1', BOARD_ICONS[1]).Replace('2', BOARD_ICONS[2]).Replace('3', BOARD_ICONS[3]);
                representation += rowString + "\r\n";
            }

            representation += "--+";
            for (int i = 0; i < board[0].Length; i++)
            {
                representation += "-";
            }

            representation += "\r\n  |";
            for (int i = 0; i < board[0].Length; i++)
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
