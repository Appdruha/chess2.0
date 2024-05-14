import { Dispatch, SetStateAction, useEffect, useState } from 'react'
import { MessageFromServer, MessageToServer, MessageType } from '../models/Message.ts'

interface UseWebsocket {
  webSocketState: {webSocket: WebSocket, isConnected: boolean}
  onOpenMessage?: MessageToServer
  onClose?: () => void
  onError?: () => void
}

const defaultCallback = (message: string) => {
  console.log(message)
}

const messageConstructor = (type: MessageType, params: string, roomId: string) => {
  const newMessage: MessageToServer = {
    type,
    params,
    roomId,
  }
  return JSON.stringify(newMessage)
}

export const useWebsocket = (
  {
    webSocketState,
    onOpenMessage,
    onError = () => defaultCallback('socket error'),
    onClose = () => defaultCallback('socket closed'),
  }: UseWebsocket): [null | MessageFromServer, Dispatch<SetStateAction<MessageToServer | null>>] => {
  const {webSocket, isConnected} = webSocketState

  const [lastMessage, setLastMessage] = useState<null | MessageFromServer>(null)
  const [messageToServer, sendMessage] = useState<null | MessageToServer>(null)

  useEffect(() => {
    if (isConnected && messageToServer) {
      const { type, params, roomId } = messageToServer
      webSocket.send(messageConstructor(type, params, roomId))
    }
  }, [messageToServer])

  try {
    webSocket.onopen = () => {
      webSocketState.isConnected = true
      if (onOpenMessage) {
        webSocket.send(JSON.stringify(onOpenMessage))
      }
      console.log('socket connected')
    }
    webSocket.onclose = () => {
      webSocketState.isConnected = false
      setLastMessage(null)
      onClose()
    }
    webSocket.onerror = () => {
      webSocketState.isConnected = false
      setLastMessage(null)
      onError()
    }
    webSocket.onmessage = (event) => {
      const { params, roomId, type } = JSON.parse(event.data) as MessageFromServer
      if (type === MessageType.create
        || type === MessageType.join
        || type === MessageType.init
        || type === MessageType.nextTurn
        || type === MessageType.start
      ) {
        setLastMessage({ params, roomId, type })
      }
      if (type === MessageType.endGame) {
        setLastMessage({ params, roomId, type })
        if (params?.turn) {
          setTimeout(() => sendMessage({ params: '', type: MessageType.restart, roomId }), 8000)
        }
      }
      if (type === MessageType.changeFigure) {
        setLastMessage({...lastMessage, type} as MessageFromServer)
      }
    }
  } catch (e) {
    setLastMessage(null)
    webSocket.close()
    throw new Error(`ошибка при чтении сообщения с сервера ${e}`)
  }

  return [lastMessage, sendMessage]
}