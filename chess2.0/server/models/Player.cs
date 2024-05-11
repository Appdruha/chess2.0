using Fleck;

public class Player
{
    public IWebSocketConnection Connection { get; }
    public FigureColors Color { get; }
    public Cell KingCell { get; set; }

    public Player(FigureColors color, IWebSocketConnection connection)
    {
        Connection = connection;
        Color = color;
        KingCell = new Cell(-1, -1, CellCollors.RED, "00");
    }
}