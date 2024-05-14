import { FigureBox } from '../../ui/figure-box/Figure-box.tsx'
import { Dispatch, SetStateAction } from 'react'
import wB from '/alpha/wB.png'
import bB from '/alpha/bB.png'
import wQ from '/alpha/wQ.png'
import bQ from '/alpha/bQ.png'
import wN from '/alpha/wN.png'
import bN from '/alpha/bN.png'
import wR from '/alpha/wR.png'
import bR from '/alpha/bR.png'
import { FigureColors, FigureNames } from '../../../../models/Figure.ts'
import styles from './select-figure.module.css'

interface ChooseFigureProps {
  playerColor: FigureColors
  setIsModalOpen: Dispatch<SetStateAction<boolean>>
  setNewFigureName: Dispatch<SetStateAction<null | string>>
}

export const SelectFigure = (
  {
    playerColor,
    setIsModalOpen,
    setNewFigureName,
  }: ChooseFigureProps) => {
  const handleClick = (figureName: FigureNames) => {
    if (figureName === FigureNames.bishop) {
      setNewFigureName('bishop')
    } else if (figureName === FigureNames.queen) {
      setNewFigureName('queen')
    } else if (figureName === FigureNames.rook) {
      setNewFigureName('rook')
    } else {
      setNewFigureName('knight')
    }
    setIsModalOpen(false)
  }

  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Выберите фигуру</h2>
      <div className={styles.figuresBlock}>
        <FigureBox handleClick={handleClick} figureIconSrc={playerColor === FigureColors.white ? wQ : bQ}
                   figureName={FigureNames.queen} />
        <FigureBox handleClick={handleClick} figureIconSrc={playerColor === FigureColors.white ? wR : bR}
                   figureName={FigureNames.rook} />
        <FigureBox handleClick={handleClick} figureIconSrc={playerColor === FigureColors.white ? wN : bN}
                   figureName={FigureNames.knight} />
        <FigureBox handleClick={handleClick} figureIconSrc={playerColor === FigureColors.white ? wB : bB}
                   figureName={FigureNames.bishop} />
      </div>
    </div>
  )
}