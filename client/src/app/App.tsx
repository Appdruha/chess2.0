import { RootRouter } from './routes/Root-router.tsx'
import {WebsocketContext} from './websocket-context.ts'

export const App = () => {
  return (
    <WebsocketContext.Provider value={{webSocket: new WebSocket(import.meta.env.VITE_SERVER), isConnected: false}}>
      <RootRouter />
    </WebsocketContext.Provider>
  )
}
