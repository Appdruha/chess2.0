import { Dispatch, SetStateAction } from 'react'
import { MessageToServer, MessageType } from '../../../../models/Message.ts'
import { Button } from '../../../../ui/button/Button.tsx'
import styles from './confirm-draw.module.css'

interface ConfirmDrawProps {
  sendConfirmDrawMessage: Dispatch<SetStateAction<MessageToServer | null>>
  setIsModalOpen: Dispatch<SetStateAction<boolean>>
  roomId: string
}

export const ConfirmDraw = ({sendConfirmDrawMessage, roomId, setIsModalOpen}: ConfirmDrawProps) => {
  const message: MessageToServer = {
    type: MessageType.confirmDraw,
    params: '',
    roomId: roomId
  }

  const handleConfirmClick = () => {
    sendConfirmDrawMessage(message)
    setIsModalOpen(false)
  }

  const handleRejectClick = () => {
    setIsModalOpen(false)
  }

  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Вам предлагают ничью</h2>
      <div className={styles.buttonsContainer}>
        <Button onClick={handleConfirmClick}>
          Принять
        </Button>
        <Button onClick={handleRejectClick}>
          Отклонить
        </Button>
      </div>
    </div>
  )
}