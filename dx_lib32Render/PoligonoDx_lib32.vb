Imports dx_lib32
Imports Motor3D.Espacio2D
Imports System.Drawing

Namespace Motor3D.Escena.Renders.RenderDx_lib32
    Public Class PoligonoDx_lib32
        Inherits PrimitivaDx_lib32

        Private mSubPoligonos As List(Of SubPoligonoDx_lib32)
        Private mSegmentos As List(Of SegmentoDx_lib32)
        Private mRelleno As Boolean

        Public ReadOnly Property SubPoligonos As SubPoligonoDx_lib32()
            Get
                Return mSubPoligonos.ToArray
            End Get
        End Property

        Public ReadOnly Property Segmentos As SegmentoDx_lib32()
            Get
                Return mSegmentos.ToArray
            End Get
        End Property

        Public ReadOnly Property Relleno As Boolean
            Get
                Return mRelleno
            End Get
        End Property

        Public Sub New(ByVal Poligono As Poligono2D, Optional ByVal Relleno As Boolean = True)
            Dim Verts() As Vertex
            mSubPoligonos = New List(Of SubPoligonoDx_lib32)
            mSegmentos = New List(Of SegmentoDx_lib32)

            mRelleno = Relleno

            If Relleno Then
                If Poligono.NumeroLados = 4 Then
                    ReDim Verts(3)

                    Verts(0).X = Poligono.Vertices(0).X
                    Verts(0).Y = Poligono.Vertices(0).Y
                    Verts(0).Color = Poligono.Color.ToArgb

                    Verts(1).X = Poligono.Vertices(1).X
                    Verts(1).Y = Poligono.Vertices(1).Y
                    Verts(1).Color = Poligono.Color.ToArgb

                    Verts(3).X = Poligono.Vertices(2).X
                    Verts(3).Y = Poligono.Vertices(2).Y
                    Verts(3).Color = Poligono.Color.ToArgb

                    Verts(2).X = Poligono.Vertices(3).X
                    Verts(2).Y = Poligono.Vertices(3).Y
                    Verts(2).Color = Poligono.Color.ToArgb

                    mSubPoligonos.Add(New SubPoligonoDx_lib32(Verts))
                End If
            Else
                For Each Lado As Segmento2D In Poligono.Lados
                    mSegmentos.Add(New SegmentoDx_lib32(Lado, Poligono.Color))
                Next
            End If
        End Sub

        Public Overrides Sub Redibujar(ByRef Graphics As dx_GFX_Class)
            If mRelleno Then
                For Each SubPoligono As SubPoligonoDx_lib32 In mSubPoligonos
                    SubPoligono.Redibujar(Graphics)
                Next
            Else
                For Each Segmento As SegmentoDx_lib32 In mSegmentos
                    Segmento.Redibujar(Graphics)
                Next
            End If
        End Sub
    End Class
End Namespace


