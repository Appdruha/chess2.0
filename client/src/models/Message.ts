import { Cell } from './Cell.ts'
import { FigureColors } from './Figure.ts'

export enum MessageType {
  create,
  join,
  init,
  start,
  leave,
  move,
  endGame,
  changeFigure,
  restart,
  error,
}

export interface MessageParams {
  chessBoardState: Cell[]
  turn: FigureColors | null
}

export interface Message {
  type: MessageType
  params: null | MessageParams
  roomId: string
}