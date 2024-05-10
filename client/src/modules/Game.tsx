import { useWebsocket } from '../hooks/use-websocket.ts'
import { useContext, useEffect, useRef } from 'react'
import { WebsocketContext } from '../app/websocket-context.ts'
import { useLocation, useParams } from 'react-router-dom'
import { MessageType } from '../models/Message.ts'
import { cellSideSize, chessBoardSideSize } from './consts/chess-board-size.ts'
import { Cell, CellColors } from '../models/Cell.ts'
import styles from './game.module.css'
import { getFigureIcons } from './helpers/get-figure-icon.ts'
import { FigureColors } from '../models/Figure.ts'

export const Game = () => {
  const roomId = useParams().roomId!
  const isHost = useLocation().state?.isHost
  const webSocketState = useContext(WebsocketContext)!
  const chessBoardRef = useRef<null | HTMLCanvasElement>(null)
  const ctxRef = useRef<null | CanvasRenderingContext2D>(null)
  const requestRef = useRef<undefined | number>(undefined)
  const chessBoardStateRef = useRef<undefined | Cell[]>(undefined)
  const figureIconsRef = useRef(getFigureIcons())

  const [message, sendMessage] = useWebsocket({ webSocketState })

  useEffect(() => {
    if (isHost) {
      sendMessage({ roomId, type: MessageType.init, params: null })
    } else {
      sendMessage({ roomId, type: MessageType.start, params: null })
    }
    if (chessBoardRef.current) {
      ctxRef.current = chessBoardRef.current.getContext('2d')
      chessBoardRef.current.width = chessBoardSideSize
      chessBoardRef.current.height = chessBoardSideSize
      requestRef.current = window.requestAnimationFrame(() => drawAll())
    }
    return () => {
      cancelAnimationFrame(requestRef.current as number)
    }
  }, [])

  useEffect(() => {
    chessBoardStateRef.current = message?.params?.chessBoardState
  }, [message])

  const drawAll = () => {
    if (chessBoardRef.current && ctxRef.current && chessBoardStateRef.current) {
      const chessBoardState = chessBoardStateRef.current
      const ctx = ctxRef.current
      ctx.clearRect(0, 0, chessBoardSideSize, chessBoardSideSize)
      const cells = chessBoardState
      cells.forEach(cell => {
        ctx.fillStyle = cell.color === CellColors.black ? '#A1452E' : '#FFFFEB'
        ctx.fillRect(cell.x * cellSideSize, cell.y * cellSideSize, cellSideSize, cellSideSize)
        if (cell.figure) {
          const figure = cell.figure
          const figureIcons = figureIconsRef.current
          const icon = figure.color === FigureColors.white
            ? figureIcons[figure.name]?.white
            : figureIcons[figure.name]?.black
          if (icon) {
            ctx.drawImage(icon, cell.x * cellSideSize, cell.y * cellSideSize)
          }
        }
      })
    }
    requestRef.current = window.requestAnimationFrame(() => drawAll())
  }

  return (
    <div className={styles.chessBoardContainer}>
      <canvas className={styles.chessBoard} ref={chessBoardRef}></canvas>
    </div>
  )
}