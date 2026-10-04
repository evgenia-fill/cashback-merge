namespace CashBackMerge.App;

public class ConsoleGame
{
    private GameSession Session { get; set; }

    public ConsoleGame(GameSession session)
    {
        Session = session;
    }

    public static void PrintField(int[,] field)
    {
        int columns;
        var rows = columns = GameSession.FieldSize;

        for (var row = 0; row < rows; row++)
        {
            Console.WriteLine("+---+---+---+---+");

            for (var column = 0; column < columns; column++)
            {
                Console.Write($"| {field[row, column]} ");
            }

            Console.WriteLine("|");
        }

        Console.WriteLine("+---+---+---+---+");
    }
}