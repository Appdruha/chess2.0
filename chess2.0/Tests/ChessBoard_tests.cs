using chess2._0.models;
using chess2._0.models.figures;
using chess2._0.models.gameRoom;

namespace Tests;

[TestFixture]
public class ChessBoardTestsForChess20
{
    private ChessBoard _chessBoard;

    [SetUp]
    public void SetUp()
    {
        _chessBoard = new ChessBoard(GameMode.Chess20);
    }

    [Test]
    public void ChessBoard_Return_100_Cells()
    {
        Assert.That(_chessBoard.ChessBoardState.Count, Is.EqualTo(100), "Wrong cells count in Chess20 mode");
    }

    [TestCase(0)]
    [TestCase(1)]
    public void ChessBoard_Return_Draw_After_Second_Narrowing(int narrowingsCount)
    {
        _chessBoard.InitFigures(GameMode.Chess20);
        var gameWinner = _chessBoard.DoNarrowing(narrowingsCount);
        if (narrowingsCount == 1)
        {
            Assert.That(gameWinner, Is.EqualTo(GameWinner.Draw), "Wrong winner after Narrowing");
        }
        else
        {
            Assert.That(gameWinner, Is.EqualTo(null), "Unexpected winner after Narrowing");
        }
    }
    
    [Test]
    public void ChessBoard_Return_Figures_On_Correct_Cells()
    {
        _chessBoard.InitFigures(GameMode.Chess20);
        var incorrectCellIds = new List<string>();

        foreach (var cell in _chessBoard.ChessBoardState)
        {
            if (cell.Id.Contains('3') && !cell.Id.Contains('A') && !cell.Id.Contains('J'))
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.PAWN, FigureColors.WHITE))
                {
                   incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "F2")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KING, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "E2")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.QUEEN, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "G2" || cell.Id == "D2")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.BISHOP, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "B2" || cell.Id == "I2")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.ROOK, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "C2" || cell.Id == "H2")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KNIGHT, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id.Contains('8') && !cell.Id.Contains('A') && !cell.Id.Contains('J'))
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.PAWN, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "F9")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KING, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "E9")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.QUEEN, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "G9" || cell.Id == "D9")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.BISHOP, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "B9" || cell.Id == "I9")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.ROOK, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "C9" || cell.Id == "H9")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KNIGHT, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "A3" || cell.Id == "J3")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.RAM, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "A8" || cell.Id == "J8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.RAM, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
        }
        
        Assert.That(incorrectCellIds.Count, Is.EqualTo(0), string.Format("Incorrect Figures on cells: {0} in Chess20 mode", String.Join(",", incorrectCellIds)));
    }
}

[TestFixture]
public class ChessBoardTestsForCommonChess
{
    private ChessBoard _chessBoard;

    [SetUp]
    public void SetUp()
    {
        _chessBoard = new ChessBoard(GameMode.CommonChess);
    }

    [Test]
    public void ChessBoard_Return_64_Cells()
    {
        Assert.That(_chessBoard.ChessBoardState.Count, Is.EqualTo(64), "Wrong cells count in Common mode");
    }

    [Test]
    public void ChessBoard_Return_Figures_On_Correct_Cells()
    {
        _chessBoard.InitFigures(GameMode.CommonChess);
        var incorrectCellIds = new List<string>();

        foreach (var cell in _chessBoard.ChessBoardState)
        {
            if (cell.Id.Contains('2'))
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.PAWN, FigureColors.WHITE))
                {
                   incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "E1")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KING, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "D1")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.QUEEN, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "F1" || cell.Id == "C1")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.BISHOP, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "A1" || cell.Id == "H1")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.ROOK, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "B1" || cell.Id == "G1")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KNIGHT, FigureColors.WHITE))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id.Contains('7'))
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.PAWN, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "E8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KING, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "D8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.QUEEN, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "F8" || cell.Id == "C8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.BISHOP, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "A8" || cell.Id == "H8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.ROOK, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
            else if (cell.Id == "B8" || cell.Id == "G8")
            {
                if (!Helper.CheckIsCorrectFigure(cell, FigureNames.KNIGHT, FigureColors.BLACK))
                {
                    incorrectCellIds.Add(cell.Id); 
                }
            }
        }
        
        Assert.That(incorrectCellIds.Count, Is.EqualTo(0), string.Format("Incorrect Figures on cells: {0} in common mode", String.Join(",", incorrectCellIds)));
    }
}

public class Helper
{
    public static bool CheckIsCorrectFigure(Cell cell, FigureNames figureName, FigureColors expectedColor)
    {
        var figure = cell.Figure;
        
        if (figure == null)
        {
            return false;
        } 
        
        if (figure.Name != figureName)
        {
            return false;
        }

        if (figure.Color != expectedColor)
        {
            return false;
        }

        return true;
    }
}