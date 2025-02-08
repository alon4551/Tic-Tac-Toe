using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacTow
{
    static class View
    {
        public static void Color(char c)
        {
            switch (c)
            {
                case 'P':
                    Console.BackgroundColor = ConsoleColor.White; 
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 'X':
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 'O':
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        public static void Print(char[,] board,int X,int Y)
        {
            for (int i = 0; i < board.GetLength(0); i++,Console.WriteLine())
                for (int j = 0; j < board.GetLength(1); j++){
                    if (i == Y && j == X)
                        Color('P');
                    else
                        Color(board[i,j]);
                    Console.Write(board[i, j]);
                }
            Color('_');
            Console.WriteLine("Design By Alon Shraibman");
        }
    }
}
