import { Cell } from './Cell.ts'
import { FigureColors } from './Figure.ts'

export enum MessageType {
  create,
  join,
  init,
  start,
  leave,
  move,
  nextTurn,
  endGame,
  changeFigure,
  restart,
  error,
}

export interface MessageParams {
  chessBoardState: Cell[]
  turn: FigureColors | null
}

export interface MessageFromServer {
  type: MessageType
  params: null | MessageParams
  roomId: string
}

export interface MessageToServer {
  type: MessageType
  params: string
  roomId: string
}