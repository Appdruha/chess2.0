import { Figure } from './Figure.ts'

export enum CellColors {
  black,
  white,
  red,
}

export interface Cell {
  readonly x: number
  readonly y: number
  readonly color: CellColors
  readonly id: string
  figure: Figure | null
}