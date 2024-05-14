import { Route, Routes } from 'react-router-dom'
import { Layout } from '../layout/Layout.tsx'
import {GamingRoom} from '../../pages/Gaming-room.tsx'
import { RoomDistributor } from '../../modules/room-distributor/Room-distributor.tsx'

export const RootRouter = () => {
  return (
    <Routes>
      <Route path='/' element={<Layout />}>
        <Route index path=':roomId?' element={<RoomDistributor><GamingRoom /></RoomDistributor>} />
      </Route>
    </Routes>
  )
}