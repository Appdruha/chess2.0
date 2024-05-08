import { RootRouter } from './routes/Root-router.tsx'
import {WebsocketContext} from './websocket-context.ts'

export const App = () => {
  return (
    <WebsocketContext.Provider value={{webSocket: new WebSocket('ws://localhost:8181'), isConnected: false}}>
      <RootRouter />
    </WebsocketContext.Provider>
  )
}
