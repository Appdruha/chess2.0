import { Button } from '../../../ui/button/Button.tsx'
import { Dispatch, SetStateAction } from 'react'
import { MessageToServer, MessageType } from '../../../models/Message.ts'
import styles from './select-mode.module.css'

interface SelectModeProps {
  sendCreateMessage: Dispatch<SetStateAction<MessageToServer | null>>
}

export const SelectMode = ({sendCreateMessage}: SelectModeProps) => {
  const message: MessageToServer = {
    type: MessageType.create,
    params: '',
    roomId: ''
  }

  const handleCommonChessClick = () => {
    sendCreateMessage(message)
  }

  const handleChess20Click = () => {
    message.params = 'chess20'
    sendCreateMessage(message)
  }

  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Выберите режим</h2>
      <div className={styles.buttonsContainer}>
        <Button onClick={handleCommonChessClick}>
          Классический
        </Button>
        <Button onClick={handleChess20Click}>
          Расширенный
        </Button>
      </div>
    </div>
  )
}