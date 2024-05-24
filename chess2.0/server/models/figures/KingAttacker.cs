namespace chess2._0.models.figures;

public class KingAttacker
{
    public Figure Figure { get; set; }
    public List<Cell> IntermCells { get; set; }

    public KingAttacker(Figure figure, List<Cell> intermCells)
    {
        Figure = figure;
        IntermCells = intermCells;
    }
}