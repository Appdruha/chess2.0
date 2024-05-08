import { Dispatch, SetStateAction, useEffect, useState } from 'react'
import { Message, MessageParams, MessageType } from '../models/Message.ts'

interface UseWebsocket {
  webSocket: WebSocket
  roomId?: string
  onOpen?: () => void
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
    webSocket,
    roomId,
    onOpen = () => defaultCallback('socket opened'),
    onError = () => defaultCallback('socket error'),
    onClose = () => defaultCallback('socket closed'),
  }: UseWebsocket): [null | Omit<Message, 'type'>, Dispatch<SetStateAction<Omit<Message, "type"> | null>>] => {
  const [lastMessage, setLastMessage] = useState<null | Omit<Message, 'type'>>(null)
  const [messageToServer, sendMessage] = useState<null | Omit<Message, 'type'>>(null)
  const [isConnected, setIsConnected] = useState(false)

  useEffect(() => {
    if (isConnected) {
      if (roomId) {
        webSocket.send(messageConstructor(MessageType.join, null, roomId))
      }
      webSocket.send(messageConstructor(MessageType.create, null, ''))
    }
  }, [roomId, isConnected])

  try {
    webSocket.onopen = () => {
      setIsConnected(true)
      onOpen()
    }
    webSocket.onclose = () => {
      setIsConnected(false)
      setLastMessage(null)
      onClose()
    }
    webSocket.onerror = () => {
      setIsConnected(false)
      setLastMessage(null)
      onError()
    }
    webSocket.onmessage = (event) => {
      const { params, roomId, type } = JSON.parse(event.data) as Message
      const newMessage = { params, roomId }
      if (type === MessageType.create || type === MessageType.join || type === MessageType.move || type === MessageType.start) {
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