import { useWebsocket } from '../hooks/use-websocket.ts'
import { useContext, useEffect } from 'react'
import { WebsocketContext } from '../app/websocket-context.ts'
import { useLocation, useParams } from 'react-router-dom'
import { MessageType } from '../models/Message.ts'

export const Game = () => {
  const roomId = useParams().roomId!
  const isHost = useLocation().state?.isHost
  const webSocketState = useContext(WebsocketContext)!
  const [message, sendMessage] = useWebsocket({ webSocketState })
  useEffect(() => {
    if (isHost) {
      sendMessage({ roomId, type: MessageType.init, params: null })
    } else {
      sendMessage({ roomId, type: MessageType.start, params: null })
    }
  }, [])

  useEffect(() => {
    console.log(message)
  }, [message])

  return (
    <div>
      gameRoom
    </div>
  )
}