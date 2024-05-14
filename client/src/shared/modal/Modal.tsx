import { createPortal } from 'react-dom'
import styles from './modal.module.css'
import { ReactNode } from 'react'

const portal = document.getElementById('portal')!

interface ModalProps {
  children: ReactNode
}

export const Modal = ({children}: ModalProps) => {
  return (
    createPortal(
      <div className={styles.modalContainer}>
        <div className={styles.modal}>
          {children}
        </div>
      </div>,
      portal,
    )
  )
}