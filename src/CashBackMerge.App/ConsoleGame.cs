using System.Text;

namespace CashBackMerge.App;

public class ConsoleGame
{
    public static void PrintBoard(GameSession session)
    {
        int columns;
        var rows = columns = session.BoardSize;
        var border = BuildBorder(rows);

        for (var row = 0; row < rows; row++)
        {
            Console.WriteLine(border);

            for (var column = 0; column < columns; column++)
            {
                Console.Write($"| {session.Board[row, column]} ");
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(border);
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