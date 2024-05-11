import { useWebsocket } from '../hooks/use-websocket.ts'
import { useContext, useEffect, useRef } from 'react'
import { WebsocketContext } from '../app/websocket-context.ts'
import { useLocation, useParams } from 'react-router-dom'
import { MessageToServer, MessageType } from '../models/Message.ts'
import { cellSideSize, chessBoardSideSize } from './consts/chess-board-size.ts'
import { Cell, CellColors } from '../models/Cell.ts'
import { getFigureIcons } from './helpers/get-figure-icon.ts'
import { handleMouseMove } from './helpers/handle-mouse-move.ts'
import { handleClick } from './helpers/handle-click.ts'
import { chooseFigureIcon } from './helpers/choose-figure-icon.ts'
import styles from './game.module.css'

export const Game = () => {
  const roomId = useParams().roomId!
  const isHost = useLocation().state?.isHost
  const webSocketState = useContext(WebsocketContext)!
  const chessBoardRef = useRef<null | HTMLCanvasElement>(null)
  const ctxRef = useRef<null | CanvasRenderingContext2D>(null)
  const requestRef = useRef<undefined | number>(undefined)
  const chessBoardStateRef = useRef<undefined | Cell[]>(undefined)
  const selectedFigureIconRef = useRef<null | HTMLImageElement>(null)
  const clientPositionOnChessBoardRef = useRef<{ x: number, y: number } | null>(null)
  const chessBoardPositionRef = useRef<{ x: number, y: number } | null>(null)
  const prevCellIdRef = useRef<string | null>(null)
  const figureIconsRef = useRef(getFigureIcons())

  const moveMessage: MessageToServer = {
    type: MessageType.move,
    params: '',
    roomId,
  }

  const [message, sendMessage] = useWebsocket({ webSocketState })

  useEffect(() => {
    if (isHost) {
      sendMessage({ roomId, type: MessageType.init, params: '' })
    } else {
      sendMessage({ roomId, type: MessageType.start, params: '' })
    }
    if (chessBoardRef.current) {
      const { x, y } = chessBoardRef.current.getBoundingClientRect()
      chessBoardPositionRef.current = { x, y }
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
    console.log(chessBoardStateRef.current)
    if (message?.params) {
      selectedFigureIconRef.current = null
    }
  }, [message])

  const drawAll = () => {
    if (chessBoardRef.current && ctxRef.current && chessBoardStateRef.current) {
      const chessBoardState = chessBoardStateRef.current
      const ctx = ctxRef.current
      ctx.clearRect(0, 0, chessBoardSideSize, chessBoardSideSize)
      chessBoardState.forEach(cell => {
        ctx.fillStyle = cell.color === CellColors.black ? '#A1452E' : '#FFFFEB'
        ctx.fillRect(cell.x * cellSideSize, cell.y * cellSideSize, cellSideSize, cellSideSize)
        if (cell.figure) {
          const icon = chooseFigureIcon({ figure: cell.figure, figureIcons: figureIconsRef.current })
          if (icon) {
            ctx.drawImage(icon, cell.x * cellSideSize, cell.y * cellSideSize)
          }
        }
        if (selectedFigureIconRef.current && clientPositionOnChessBoardRef.current) {
          const { x, y } = clientPositionOnChessBoardRef.current
          ctx.drawImage(selectedFigureIconRef.current, x - cellSideSize / 2, y - cellSideSize / 2)
        }
      })
    }
    requestRef.current = window.requestAnimationFrame(() => drawAll())
  }

  return (
    <div className={styles.chessBoardContainer}>
      <canvas
        className={styles.chessBoard}
        ref={chessBoardRef}
        onMouseMove={(event) => handleMouseMove({
          event,
          clientPositionOnChessBoardRef,
          chessBoardPosition: chessBoardPositionRef.current,
        })}
        onMouseOut={() => clientPositionOnChessBoardRef.current = null}
        onClick={() => handleClick({
          isMyTurn: true,
          selectedFigureIconRef,
          chessBoardState: chessBoardStateRef.current,
          chooseFigureIcon,
          clientPositionOnChessBoard: clientPositionOnChessBoardRef.current,
          figureIcons: figureIconsRef.current,
          cellSideSize,
          sendMessage,
          moveMessage,
          prevCellIdRef
        })}
      ></canvas>
    </div>
  )
}