import { ReactNode, useContext } from 'react'
import { Navigate, useParams } from 'react-router-dom'
import { WebsocketContext } from '../../websocket-context.ts'
import { useWebsocket } from '../../../hooks/use-websocket.ts'

export const RoomDistributor = (props: { children: ReactNode }) => {
  const roomId = useParams().roomId
  const webSocket = useContext(WebsocketContext)
  if (!webSocket) {
    return <h2>Please reload page</h2>
  }
  const [message] = useWebsocket({webSocket})

  if (!message) {
    return <h1>LOADING...</h1>
  }
  if (roomId) {
    return props.children
  }
  return <Navigate to={message.roomId} />
}