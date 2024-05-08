import { Figure } from './Figure.ts'

export enum CellColors {
  black,
  white,
  red,
}

export class Cell {
  readonly x: number
  readonly y: number
  readonly color: CellColors
  readonly id: string
  _figure: Figure | null

  constructor(x: number, y: number, color: CellColors, id: string, cellSideSize: number) {
    this.x = x * cellSideSize
    this.y = y * cellSideSize
    this.color = color
    this.id = id
    this._figure = null
  }
}