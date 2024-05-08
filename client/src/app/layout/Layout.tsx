import { Outlet } from 'react-router-dom'
import styles from './layout.module.css'

export const Layout = () => {
  return (
    <div className={styles.container}>
      <main className={styles.main}>
        <Outlet/>
      </main>
    </div>
  )
}