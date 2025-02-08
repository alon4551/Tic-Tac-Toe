
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;
using TicTacTow;

char[,] board = new char[3, 3] ;
int X=0, Y=0;
char Player = 'X',Winner;
ConsoleKey key;
Random random = new Random();
board = Reset(board);

char [,] Reset (char[,] board)
{
    for (int i = 0; i < board.GetLength(0); i++)
        for (int j = 0; j < board.GetLength(1); j++)
            board[i, j] = '_';
    return board;
}
char CheckWinner(char[,] board)
{
    int Axis = 0, ReverseAxis = 0,Row,Col,len = board.GetLength(0)-1,free_spaces=0;
    for (int i=0; i < board.GetLength(0); i++)
    {
        Row = 0;
        Col = 0;
        if (board[i, i] == 'X')
            Axis++;
        else if (board[i, i] == 'O')
            Axis--;
        if (board[i, len - i] == 'X')
            ReverseAxis++;
        else if (board[i, len - i] == 'O')
            ReverseAxis--;
        for(int j = 0;j<board.GetLength(1); j++)
        {
            if (board[i, j] == 'X')
                Row++;
            else if (board[i, j] == 'O')
                Row--;
            if (board[j, i] == 'X')
                Col++;
            else if (board[j,i]=='O')
                Col--;
        }
        if (Row == 3 || Col == 3)
            return 'X';
        if (Col == -3 || Row == -3)
            return 'O';

    }
    if (Axis == 3 || ReverseAxis == 3)
        return 'X';
    if (ReverseAxis == -3 || Axis == -3)
        return 'O';
    foreach (char c in board)
        if (c =='_')
            free_spaces++;
    if (free_spaces != 0)
        return '_';
    else
        return 'N';
}
void SetRandomPlace()
{
    int x, y;
    do
    {
        x = random.Next(0, board.GetLength(1));
        y = random.Next(0, board.GetLength(0));
        Winner = CheckWinner(board);
    } while (Winner == '_' && board[y, x] != '_');
    if (Winner == '_')
        board[y, x] = getOpponentSign(Player);
}
int [] getEmptyAxisPoint(bool reverse)
{
    int[] arr = [-1,-1];
    if (reverse)
    {
        for (int i = 0, j = board.GetLength(0) - 1; i < board.GetLength(0); i++, j--)
            if (board[i, j] == '_')
                return [i, j];
    }
    else
    {
        for (int i = 0; i < board.GetLength(0); i++)
            if (board[i, i] == '_')
                return[i, i];
    }
    return null;    
}
bool SmartPlacementLevel(char sign,int countSign)
{
    int Row, Col, Axis=0, ReverseAxis = 0,len = board.GetLength(0)-1,empty;
    for (int i = 0; i < board.GetLength(0); i++)
    {
        Row = 0;
        Col=0;
        if (board[i, i] == sign)
            Axis++;
        if (board[i, len - i] == sign)
            ReverseAxis++;
        empty = 0;
        for (int j = 0; j < board.GetLength(1); j++)
        {
            if (board[i, j] == '_')
                empty = j;
            if (board[i, j] == sign)
                Row++;
            if (board[j, i] == sign)
                Col++;
        }
        if (Row == countSign && board[i, empty] == '_') {
            board[i, empty] = getOpponentSign(Player);
            return true;
        }
        if ( Col == countSign && board[empty,i] =='_')
        {
            board[empty,i] = getOpponentSign(Player);
            return true;
        }
    }
    if (Axis == countSign)
    {
        int[] emptyslot = getEmptyAxisPoint(false);
        if (emptyslot != null)
        {
            board[emptyslot[0], emptyslot[1]] = getOpponentSign(Player);
            return true;
        }
    }
    else if (ReverseAxis == countSign)
    {
        int[] emptyslot = getEmptyAxisPoint(true);
        if (emptyslot != null)
        {
            board[emptyslot[0], emptyslot[1]] = getOpponentSign(Player);
            return true;
        }
    }
    return false;
}
void KeyHandler(ConsoleKey key,bool bot,int level)
{
    switch (key)
    {
        case ConsoleKey.UpArrow:
            if (Y - 1 >= 0)
                Y--;
            break;
        case ConsoleKey.DownArrow:
            if (Y + 1 < board.GetLength(0))
                Y++;
            break;
        case ConsoleKey.LeftArrow:
            if (X - 1 >= 0)
                X--;
            break;
        case ConsoleKey.RightArrow:
            if (X + 1 < board.GetLength(1))
                X++;
            break;
        case ConsoleKey.Enter:
            if (board[Y, X] == '_')
            {
                board[Y, X] = Player; 
                if (bot)
                {
                    int x, y;
                    Console.Clear();
                    View.Print(board, X, Y);
                    Thread.Sleep(500);
                    switch (level)
                    {
                        case 1:
                            SetRandomPlace();
                            break;
                        case 2:
                            if (SmartPlacementLevel(getOpponentSign(Player),2))
                            {
                                break;
                            }
                            else if (SmartPlacementLevel(Player,2))
                            {
                                break;
                            }
                            else
                                SetRandomPlace();

                            break;
                        case 3:
                            if (SmartPlacementLevel(getOpponentSign(Player), 2))
                                break;
                            else if (SmartPlacementLevel(Player, 2))
                                break;
                            else if (SmartPlacementLevel(getOpponentSign(Player), 2))
                                break;
                            else
                                SetRandomPlace();
                            break;

                    }
                }
            }
            break;
    }
}
void SwitchPlayer()
{
    if (Player == 'X')
        Player = 'O';
    else if (Player == 'O')
        Player ='X';
}

char getOpponentSign(char Player)
{
    if (Player == 'X')
        return 'O';
    else
        return 'X';
}

bool Bot(int level)
{
    Reset(board);
    do
    {
        Console.WriteLine($"Player vs Bot level {level}");
        View.Print(board, X, Y);
        key = Console.ReadKey().Key;
        KeyHandler(key,true,level);
        Winner = CheckWinner(board);
        Console.Clear();
    } while (Winner == '_' && key != ConsoleKey.Escape);
    View.Print(board,X,Y);
    if (Winner != 'N')
        Console.WriteLine($"Player {Winner} have Won");
    else if (Winner =='N')
        Console.WriteLine("Draw");
    else if(Winner =='_')
        return false;
    return true;
    
}


bool TwoPlayer()
{
    Reset(board);
    do
    {
        Console.WriteLine($"Player {Player} turn");
        View.Print(board, X, Y);
        key = Console.ReadKey().Key;
        KeyHandler(key,false,0);
        if(key==ConsoleKey.Enter)
            SwitchPlayer();
        Winner = CheckWinner(board);
        Console.Clear();
    } while (Winner == '_' && key != ConsoleKey.Escape);
    View.Print(board, X, Y);
    if (Winner != 'N')
        Console.WriteLine($"Player {Winner} have Won");
    else if (Winner == 'N')
        Console.WriteLine("Draw");
    else if (Winner == '_')
        return false;
    return true;
}

void TicTacTow()
{
    bool result;
    do
    {
        Console.WriteLine("Hello To the Tic Tac Tow game, which option would you like to play");
        Console.WriteLine("A. 2 player game\nB.Bot");
        key = Console.ReadKey().Key;

        if (key == ConsoleKey.A || key == ConsoleKey.B)
        {
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.A:
                     TwoPlayer();
                    break;
                case ConsoleKey.B:
                    do
                    {
                        Console.WriteLine("Which level would you like the bot\n1 Easy\n2 Medium\n3 Hard");
                        key= Console.ReadKey().Key;
                    }while(key != ConsoleKey.NumPad1&&key!=ConsoleKey.NumPad2&&key!=ConsoleKey.NumPad3);
                    Console.Clear();
                    switch (key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            Bot(1);
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                             Bot(2);
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            Bot(3);
                            break;
                    }
                    break;
            }
            Console.WriteLine("Game Over, press exit to end the program, press any key to Restart");
            key = Console.ReadKey().Key;
        }
        Console.Clear();
    } while (key != ConsoleKey.Escape);
}
TicTacTow();