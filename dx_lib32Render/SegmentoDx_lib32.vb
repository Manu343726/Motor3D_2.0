Imports Motor3D.Escena.Renders.RenderDx_lib32
Imports Motor3D.Espacio2D
Imports dx_lib32
Imports System.Drawing

Namespace Motor3D.Escena.Renders.RenderDx_lib32
    Public Class SegmentoDx_lib32
        Inherits PrimitivaDx_lib32

        Private mSegmento As Segmento2D
        Private mColor As Color

        Public ReadOnly Property Segmento As Segmento2D
            Get
                Return mSegmento
            End Get
        End Property

        Public ReadOnly Property Color As Color
            Get
                Return mColor
            End Get
        End Property

        Public Sub New(ByVal Segmento As Segmento2D, ByVal Color As Color)
            mSegmento = Segmento
            mColor = Color
        End Sub

        Public Overrides Sub Redibujar(ByRef Graphics As dx_GFX_Class)
            Graphics.DRAW_Line(mSegmento.ExtremoInicial.X, mSegmento.ExtremoInicial.Y, mSegmento.ExtremoFinal.X, mSegmento.ExtremoFinal.Y, 0, Color.ToArgb)
        End Sub
    End Class
End Namespace

