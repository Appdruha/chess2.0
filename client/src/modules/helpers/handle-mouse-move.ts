import { MouseEvent, MutableRefObject } from 'react'

interface HandleMouseMoveParams {
  event: MouseEvent<HTMLCanvasElement>
  clientPositionOnChessBoardRef: MutableRefObject<{ x: number, y: number } | null>
  chessBoardPosition: {x: number, y: number} | null
}

export const handleMouseMove = ({ event, clientPositionOnChessBoardRef, chessBoardPosition }: HandleMouseMoveParams) => {
  if (!chessBoardPosition) {
    throw new Error('handleMouseMove Error')
  }
  clientPositionOnChessBoardRef.current = {
    x: event.clientX - chessBoardPosition.x,
    y: event.clientY - chessBoardPosition.y
  }
}