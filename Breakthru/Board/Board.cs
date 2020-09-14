using System;

namespace Board
{
    public class Board
    {
        private static char[] BOARD_ICONS = { '.', 'G', 'S', 'F' };
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
        //int[] position;
        int[] board;
        int width;
        int height;

        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.board = new int[(width * height)];
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

        public string GetString()
        {
            string representation = "\r\n";

            for (int i = width*(height-1); i >= 0; i -= width)
            {
                representation += $"{((i/width + 1)):00}|";

                string rowString = "";
                for (int j = 0; j<width; j++)
                {
                    representation += board[i+j];
                }

                rowString = rowString.Replace('0', BOARD_ICONS[0]).Replace('1', BOARD_ICONS[1]).Replace('2', BOARD_ICONS[2]).Replace('3', BOARD_ICONS[3]);
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
