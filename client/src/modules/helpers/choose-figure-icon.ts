import { Figure, FigureColors, FigureNames } from '../../models/Figure.ts'

export interface ChooseFigureIconArgs {
  figure: Figure
  figureIcons:  Record<FigureNames, {black: HTMLImageElement, white: HTMLImageElement} | null>
}

export const chooseFigureIcon = ({figure, figureIcons}: ChooseFigureIconArgs) => {
  return figure.color === FigureColors.white
    ? figureIcons[figure.name]?.white
    : figureIcons[figure.name]?.black
}