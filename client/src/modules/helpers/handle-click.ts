import { Cell } from '../../models/Cell.ts'
import { Dispatch, MutableRefObject, SetStateAction } from 'react'
import { FigureColors, FigureNames } from '../../models/Figure.ts'
import { ChooseFigureIconArgs } from './choose-figure-icon.ts'
import { MessageToServer } from '../../models/Message.ts'

interface HandleClickArgs {
  isMyTurn?: boolean
  turnColor?: FigureColors | null
  chessBoardState: Cell[] | undefined
  selectedFigureIconRef: MutableRefObject<HTMLImageElement | null>
  clientPositionOnChessBoard: { x: number, y: number } | null
  cellSideSize: number
  chooseFigureIcon: (args: ChooseFigureIconArgs) => HTMLImageElement | undefined
  figureIcons: Record<FigureNames, {black: HTMLImageElement, white: HTMLImageElement} | null>
  sendMessage: Dispatch<SetStateAction<MessageToServer | null>>
  moveMessage: MessageToServer,
  prevCellIdRef: MutableRefObject<string | null>
}

const findCell = (clientPosition: { x: number, y: number }, cells: Cell[], cellSideSize: number) => {
  const halfSize = cellSideSize / 2
  return cells.find(cell => Math.abs(cell.x * cellSideSize + halfSize - clientPosition.x) <= halfSize
    && Math.abs(cell.y * cellSideSize + halfSize - clientPosition.y) <= halfSize)
}

export const handleClick = (
  {
    isMyTurn,
    turnColor,
    chessBoardState,
    selectedFigureIconRef,
    clientPositionOnChessBoard,
    cellSideSize,
    chooseFigureIcon,
    figureIcons,
    sendMessage,
    moveMessage,
    prevCellIdRef
  }: HandleClickArgs) => {
  if (isMyTurn && chessBoardState && clientPositionOnChessBoard) {
    const targetCell = findCell(clientPositionOnChessBoard, chessBoardState, cellSideSize)
    if (targetCell) {
      if (!selectedFigureIconRef.current && targetCell.figure && targetCell.figure.color === turnColor) {
        const icon = chooseFigureIcon({figure: targetCell.figure, figureIcons})
        if (icon) {
          selectedFigureIconRef.current = icon
          targetCell.figure = null
          prevCellIdRef.current = targetCell.id
        }
      } else {
        if (prevCellIdRef.current) {
          moveMessage.params = `${prevCellIdRef.current} ${targetCell.id}`
          sendMessage(moveMessage)
          prevCellIdRef.current = null
        }
      }
    }
  } else {
    throw new Error('handle click error')
  }
}