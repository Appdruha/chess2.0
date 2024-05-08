import { ReactNode, useContext } from 'react'
import { Navigate, useParams } from 'react-router-dom'
import { WebsocketContext } from '../../websocket-context.ts'
import { useWebsocket } from '../../../hooks/use-websocket.ts'
import { MessageType } from '../../../models/Message.ts'

export const RoomDistributor = (props: { children: ReactNode }) => {
  const roomId = useParams().roomId
  const webSocketState = useContext(WebsocketContext)
  if (!webSocketState) {
    return <h2>Please reload page</h2>
  }
  const [message] = useWebsocket({
    webSocketState,
    onOpenMessage: { type: roomId ? MessageType.join : MessageType.create, params: null, roomId: roomId || '' },
  })

  if (!message) {
    return <h1>LOADING...</h1>
  }
  if (roomId) {
    return props.children
  }
  return <Navigate to={message.roomId} state={{isHost: true}}/>
}