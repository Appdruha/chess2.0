export enum FigureColors
{
  black,
  white,
  none
}

export enum FigureNames
{
  king,
  knight,
  pawn,
  queen,
  rook,
  bishop,
  wall,
  ram
}

export interface Figure {
  color: FigureColors
  cellId: string
  name: FigureNames
  isFirstStep?: boolean
}