export enum FigureColors
{
  black,
  white,
}

export enum FigureNames
{
  king,
  knight,
  pawn,
  queen,
  rook,
  bishop,
}

export class Figure {
  public color: FigureColors
  public cellId: string
  public name: FigureNames

  constructor(color: FigureColors, cellId: string, name: FigureNames)
  {
    this.name = name
    this.color = color
    this.cellId = cellId
  }
}