import CopyToClipboard from 'react-copy-to-clipboard'
import styles from './url-modal.module.css'
import { useState } from 'react'

interface UrlModalProps {
  url: string
}

export const UrlModal = ({url}: UrlModalProps) => {
  const [isCopied, setIsCopied] = useState(false)

  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Ссылка для подключения</h2>
      <CopyToClipboard text={url} onCopy={() => setIsCopied(true)}>
        <input className={styles.input} value={url}></input>
      </CopyToClipboard>
      <CopyToClipboard text={url} onCopy={() => setIsCopied(true)}>
        <div className={styles.copy}>{isCopied ? 'Скопировано' : 'Скопировать'}</div>
      </CopyToClipboard>
    </div>
  )
}