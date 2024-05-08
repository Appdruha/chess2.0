import { createContext } from 'react'

export const WebsocketContext = createContext<null | {webSocket: WebSocket, isConnected: boolean}>(null)