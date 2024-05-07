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
    public FigureColors Color;
    public string CellId;
    public FigureNames Name;

    public Figure(FigureColors color, Cell cell, FigureNames name)
    {
        Color = color;
        CellId = cell.Id;
        Name = name;
    }
    
    public bool CanMove(Cell target, List<Cell>? cells, KingAttacker? kingAttacker)
    {
        if (kingAttacker!= null &&!kingAttacker.IntermCells.Contains(target))
        {
            return false;
        }
        return!(target.Figure!= null && target.Figure.Color == Color);
    }
}