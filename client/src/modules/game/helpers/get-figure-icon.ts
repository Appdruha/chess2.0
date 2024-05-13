import { FigureNames } from '../../../models/Figure.ts'
import wB from '/public/alpha/wB.png'
import bB from '/public/alpha/bB.png'
import wP from '/public/alpha/wP.png'
import bP from '/public/alpha/bP.png'
import wQ from '/public/alpha/wQ.png'
import bQ from '/public/alpha/bQ.png'
import wN from '/public/alpha/wN.png'
import bN from '/public/alpha/bN.png'
import wR from '/public/alpha/wR.png'
import bR from '/public/alpha/bR.png'
import wK from '/public/alpha/wK.png'
import bK from '/public/alpha/bK.png'

export const getFigureIcons = () => {
  const icons: Record<FigureNames, { black: HTMLImageElement, white: HTMLImageElement } | null> = {
    [FigureNames.pawn]: null,
    [FigureNames.king]: null,
    [FigureNames.queen]: null,
    [FigureNames.bishop]: null,
    [FigureNames.knight]: null,
    [FigureNames.rook]: null,
  }

  const wp = new Image()
  wp.src = wP
  const bp = new Image()
  bp.src = bP
  icons[FigureNames.pawn] = { black: bp, white: wp }

  const wk = new Image()
  wk.src = wK
  const bk = new Image()
  bk.src = bK
  icons[FigureNames.king] = { black: bk, white: wk }

  const wq = new Image()
  wq.src = wQ
  const bq = new Image()
  bq.src = bQ
  icons[FigureNames.queen] = { black: bq, white: wq }

  const wb = new Image()
  wb.src = wB
  const bb = new Image()
  bb.src = bB
  icons[FigureNames.bishop] = { black: bb, white: wb }

  const wn = new Image()
  wn.src = wN
  const bn = new Image()
  bn.src = bN
  icons[FigureNames.knight] = { black: bn, white: wn }

  const wr = new Image()
  wr.src = wR
  const br = new Image()
  br.src = bR
  icons[FigureNames.rook] = { black: br, white: wr }

  return icons
}