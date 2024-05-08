import { Dispatch, SetStateAction, useEffect, useState } from 'react'
import { Message, MessageParams, MessageType } from '../models/Message.ts'

interface UseWebsocket {
  webSocketState: {webSocket: WebSocket, isConnected: boolean}
  roomId?: string
  onOpenMessage?: Message
  onClose?: () => void
  onError?: () => void
}

const defaultCallback = (message: string) => {
  console.log(message)
}

const messageConstructor = (type: MessageType, params: MessageParams | null, roomId: string) => {
  const newMessage: Message = {
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
  }: UseWebsocket): [null | Omit<Message, 'type'>, Dispatch<SetStateAction<Message | null>>] => {
  const {webSocket, isConnected} = webSocketState

  const [lastMessage, setLastMessage] = useState<null | Omit<Message, 'type'>>(null)
  const [messageToServer, sendMessage] = useState<null | Message>(null)

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
      const { params, roomId, type } = JSON.parse(event.data) as Message
      const newMessage = { params, roomId }
      if (type === MessageType.create
        || type === MessageType.join
        || type === MessageType.init
        || type === MessageType.move
        || type === MessageType.start) {
        setLastMessage(newMessage)
      }
    }
  } catch (e) {
    setLastMessage(null)
    webSocket.close()
    throw new Error(`ошибка при чтении сообщения с сервера ${e}`)
  }

  return [lastMessage, sendMessage]
}