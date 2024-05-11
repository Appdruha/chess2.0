using Newtonsoft.Json;

public enum FigureColors
{
    BLACK,
    WHITE
}

public enum FigureNames
{
    KING,
    KNIGHT,
    PAWN,
    QUEEN,
    ROOK,
    BISHOP
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
        return!(target.Figure != null && target.Figure.Color == Color);
    }
}