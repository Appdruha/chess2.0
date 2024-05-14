import { FigureNames } from '../../../../models/Figure.ts'
import styles from './figure-box.module.css'

interface FigureBoxProps {
  figureIconSrc: string
  figureName: FigureNames
  handleClick: (figureName: FigureNames) => void
}

export const FigureBox = ({ figureIconSrc, figureName, handleClick }: FigureBoxProps) => {
  return (
    <div className={styles.container} onClick={() => handleClick(figureName)}>
      <img src={figureIconSrc} alt="figure" />
    </div>
  )
}