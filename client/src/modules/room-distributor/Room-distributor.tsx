import { ReactNode, useContext } from 'react'
import { Navigate, useParams } from 'react-router-dom'
import { WebsocketContext } from '../../app/websocket-context.ts'
import { useWebsocket } from '../../hooks/use-websocket.ts'
import { Modal } from '../../shared/modal/Modal.tsx'
import { SelectMode } from './components/Select-mode.tsx'
import { MessageType } from '../../models/Message.ts'
import styles from './room-distributor.module.css'

export const RoomDistributor = (props: { children: ReactNode }) => {
  const roomId = useParams().roomId
  const webSocketState = useContext(WebsocketContext)
  if (!webSocketState) {
    return <h2>Please reload page</h2>
  }
  const [message, sendMessage] = useWebsocket({
    webSocketState,
    onOpenMessage: roomId ? { type: MessageType.join, params: '', roomId } : undefined
  })

  if (!message && !roomId) {
    return (<Modal>
      <SelectMode sendCreateMessage={sendMessage}/>
    </Modal>)
  }

  if (!message) {
    return (
      <div className={styles.container}>
        <span className={styles.loader}></span>
      </div>
    )
  }

  if (roomId) {
    return props.children
  }

  return <Navigate to={message.roomId} state={{isHost: true}}/>
}