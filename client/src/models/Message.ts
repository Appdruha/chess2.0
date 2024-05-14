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

export enum GameWinner
{
  white,
  black,
  draw
}

export interface MessageParams {
  chessBoardState: Cell[]
  isMyTurn: boolean
  turn: number
  winner: GameWinner | null
  color: FigureColors | null
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