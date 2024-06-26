using chess2._0.models.figures;
using Fleck;

namespace chess2._0.models;

public class Player
{
    public IWebSocketConnection Connection { get; }
    public FigureColors Color { get; set; }
    public Cell KingCell { get; set; }

    public Player(FigureColors color, IWebSocketConnection connection)
    {
        Connection = connection;
        Color = color;
        KingCell = new Cell(-1, -1, CellCollors.RED, "00");
    }
}