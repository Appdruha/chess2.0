using Fleck;

public class Player
{
    public IWebSocketConnection Connection { get; }
    public FigureColors Color { get; }

    public Player(FigureColors color, IWebSocketConnection connection)
    {
        Connection = connection;
        Color = color;
    }
}