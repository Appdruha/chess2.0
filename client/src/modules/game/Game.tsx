import { useWebsocket } from '../../hooks/use-websocket.ts'
import { useContext, useEffect, useRef, useState } from 'react'
import { WebsocketContext } from '../../app/websocket-context.ts'
import { useLocation, useParams } from 'react-router-dom'
import { GameWinner, MessageToServer, MessageType } from '../../models/Message.ts'
import { Cell, CellColors } from '../../models/Cell.ts'
import { getFigureIcons } from './helpers/get-figure-icon.ts'
import { handleMouseMove } from './helpers/handle-mouse-move.ts'
import { handleClick } from './helpers/handle-click.ts'
import { chooseFigureIcon } from './helpers/choose-figure-icon.ts'
import { Modal } from '../../shared/modal/Modal.tsx'
import { SelectFigure } from './components/select-figure/Select-figure.tsx'
import { Button } from '../../ui/button/Button.tsx'
import styles from './game.module.css'

export const Game = () => {
  const roomId = useParams().roomId!
  const isHost = useLocation().state?.isHost
  const webSocketState = useContext(WebsocketContext)!
  const windowHeightRef = useRef(window.innerHeight);
  const chessBoardWidthInCellsRef = useRef(8)
  const cellSideSizeRef = useRef(windowHeightRef.current / chessBoardWidthInCellsRef.current)
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

  const [newFigureName, setNewFigureName] = useState<null | string>(null)
  const [isSelectFigureModalOpen, setIsSelectFigureModalOpen] = useState(false)
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
      chessBoardRef.current.width = windowHeightRef.current
      chessBoardRef.current.height = windowHeightRef.current
      requestRef.current = window.requestAnimationFrame(() => drawAll())
    }

    return () => {
      cancelAnimationFrame(requestRef.current as number)
    }
  }, [])

  useEffect(() => {
    chessBoardStateRef.current = message?.params?.chessBoardState
    if (message?.params) {
      selectedFigureIconRef.current = null
    }
    if (message && (message.type === MessageType.start || message.type === MessageType.init)) {
      chessBoardWidthInCellsRef.current = Math.sqrt(message.params!.chessBoardState.length)
      cellSideSizeRef.current = windowHeightRef.current / chessBoardWidthInCellsRef.current
    }
    if (message && message.type === MessageType.endGame) {
      const winner = message.params!.winner!
      if (winner === GameWinner.white) {
        alert('Белые победили!')
      } else if (winner === GameWinner.black) {
        alert('Черные победили!')
      } else {
        alert('Ничья!')
      }
    }
    if (message && message.type === MessageType.changeFigure) {
      setIsSelectFigureModalOpen(true)
    }
  }, [message])

  useEffect(() => {
    if (newFigureName) {
      sendMessage({ roomId, type: MessageType.changeFigure, params: newFigureName })
      setNewFigureName(null)
    }
  }, [newFigureName])

  const drawAll = () => {
    if (chessBoardRef.current && ctxRef.current && chessBoardStateRef.current) {
      const chessBoardState = chessBoardStateRef.current
      const ctx = ctxRef.current
      ctx.clearRect(0, 0, windowHeightRef.current, windowHeightRef.current)
      chessBoardState.forEach(cell => {
        if (cell.color === CellColors.white) {
          ctx.fillStyle = '#FFFFEB'
        } else if (cell.color === CellColors.black) {
          ctx.fillStyle = '#A1452E'
        } else {
          ctx.fillStyle = '#f84848'
        }
        const cellSideSize = cellSideSizeRef.current
        ctx.fillRect(cell.x * cellSideSize, cell.y * cellSideSize, cellSideSize, cellSideSize)
        if (cell.figure) {
          const icon = chooseFigureIcon({ figure: cell.figure, figureIcons: figureIconsRef.current })
          if (icon) {
            ctx.drawImage(icon, cell.x * cellSideSize, cell.y * cellSideSize, cellSideSize, cellSideSize)
          }
        }
        if (selectedFigureIconRef.current && clientPositionOnChessBoardRef.current) {
          const { x, y } = clientPositionOnChessBoardRef.current
          ctx.drawImage(selectedFigureIconRef.current, x - cellSideSize / 2, y - cellSideSize / 2, cellSideSize, cellSideSize)
        }
      })
    }
    requestRef.current = window.requestAnimationFrame(() => drawAll())
  }

  return (
    <div className={styles.container}>
      <div className={styles.sidebar}>
        <h2>Ход: {message?.params?.turn}</h2>
        <br/>
        <Button onClick={() => console.log('erg')}>Предложить ничью</Button>
        <br/>
        <br/>
        <Button onClick={() => console.log('erg')}>Сдаться</Button>
      </div>
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
          isMyTurn: message?.params?.isMyTurn,
          turnColor: message?.params?.color,
          selectedFigureIconRef,
          chessBoardState: chessBoardStateRef.current,
          chooseFigureIcon,
          clientPositionOnChessBoard: clientPositionOnChessBoardRef.current,
          figureIcons: figureIconsRef.current,
          cellSideSize: cellSideSizeRef.current,
          sendMessage,
          moveMessage,
          prevCellIdRef,
          isMate: message?.type === MessageType.endGame,
        })}
      ></canvas>
      {isSelectFigureModalOpen
        && <Modal>
          <SelectFigure
            playerColor={message!.params!.color!}
            setIsModalOpen={setIsSelectFigureModalOpen}
            setNewFigureName={setNewFigureName} />
        </Modal>
      }
    </div>
  )
}