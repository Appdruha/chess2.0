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

export interface Figure {
  color: FigureColors
  cellId: string
  name: FigureNames
  isFirstStep?: boolean
}