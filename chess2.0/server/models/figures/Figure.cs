using Newtonsoft.Json;

public enum FigureColors
{
    BLACK,
    WHITE,
    NONE
}

public enum FigureNames
{
    KING,
    KNIGHT,
    PAWN,
    QUEEN,
    ROOK,
    BISHOP,
    WALL,
    RAM
}

public class Figure
{
    [JsonProperty("color")]
    public FigureColors Color;
    [JsonProperty("cellId")]
    public string CellId;
    [JsonProperty("name")]
    public FigureNames Name;

    public Figure(FigureColors color, Cell cell, FigureNames name)
    {
        Color = color;
        CellId = cell.Id;
        Name = name;
    }
    
    public virtual bool CanMove(Cell target, List<Cell>? cells, KingAttacker? kingAttacker)
    {
        if (kingAttacker != null && !kingAttacker.IntermCells.Contains(target))
        {
            return false;
        }
        if (target.Figure != null && target.Figure.Color == Color)
        {
            return false;
        }

        if (target.Figure != null && target.Figure.Name == FigureNames.WALL && Name != FigureNames.RAM)
        {
            return false;
        }

        if (target.Color == CellCollors.RED)
        {
            return false;
        }

        return true;
    }
}