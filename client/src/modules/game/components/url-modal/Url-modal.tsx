import styles from './url-modal.module.css'

interface UrlModalProps {
  url: string
}

export const UrlModal = ({url}: UrlModalProps) => {
  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Ссылка для подключения</h2>
      <p>{url}</p>
    </div>
  )
}