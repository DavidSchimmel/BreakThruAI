using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class Board
    {
        private int[][] board;
        public static int[][] DEFAULT_POSITION =  { new int[] { 1, 3, 4 }, new int[] { 1, 3, 5 }, new int[] { 1, 3, 6 }, //Gold player escorts
                                                    new int[] { 1, 4, 3 }, new int[] { 1, 5, 3 }, new int[] { 1, 6, 3 },
                                                    new int[] { 1, 7, 4 }, new int[] { 1, 7, 5 }, new int[] { 1, 7, 6 },
                                                    new int[] { 1, 4, 7 }, new int[] { 1, 5, 7 }, new int[] { 1, 6, 7 },
                                                    new int[] { 3, 5, 5 }, //flagship
                                                    new int[] { 2, 1, 3 }, new int[] { 2, 1, 4 }, new int[] { 2, 1, 5 }, new int[] { 2, 1, 6 }, new int[] { 2, 1, 7 }, //silver player pieces
                                                    new int[] { 2, 3, 1 }, new int[] { 2, 4, 1 }, new int[] { 2, 5, 1 }, new int[] { 2, 6, 1 }, new int[] { 2, 7, 1 },
                                                    new int[] { 2, 9, 3 }, new int[] { 2, 9, 4 }, new int[] { 2, 9, 5 }, new int[] { 2, 9, 6 }, new int[] { 2, 9, 7 },
                                                    new int[] { 2, 3, 9 }, new int[] { 2, 4, 9 }, new int[] { 2, 5, 9 }, new int[] { 2, 6, 9 }, new int[] { 2, 7, 9 }
                                                  };

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

        public string GetString()
        {
            string representation = "";
            foreach (var row in board)
            {
                foreach (var tile in row)
                {
                    representation += tile;
                }
                representation += "\r\n";
            }

            return representation;
        }
        public void Print()
        {
            Console.WriteLine(GetString());
        }
    }
}
