using System.Text;

namespace CashBackMerge.App.Console;

public class ConsoleGame
{
    public static void PrintBoard(int[,] board)
    {
        var border = BuildBorder(board.GetLength(0));

        for (var row = 0; row < board.GetLength(0); row++)
        {
            System.Console.WriteLine(border);

            for (var column = 0; column < board.GetLength(1); column++)
            {
                System.Console.Write($"| {board[row, column]} ");
            }

            System.Console.WriteLine("|");
        }

        System.Console.WriteLine(border);
    }

    private static string BuildBorder(int size)
    {
        var border = new StringBuilder();
        for (var i = 0; i < size; i++)
        {
            border.Append("+---");
        }

        border.Append("+");

        return border.ToString();
    }
}