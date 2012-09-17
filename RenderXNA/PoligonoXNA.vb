Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Motor3D.Espacio2D

Namespace Motor3D.Escena.Renders.RenderXNA
    Public Class PoligonoXNA
        Inherits ObjetoEscena

        Private mNumeroVertices As Long
        Private mColor As Color
        Private mVertices() As VertexPositionColor
        Private mIndices() As Short

        Public ReadOnly Property NumeroVertices As Long
            Get
                Return mNumeroVertices
            End Get
        End Property

        Public ReadOnly Property Vertices() As VertexPositionColor()
            Get
                Return mVertices
            End Get
        End Property

        Public ReadOnly Property Indices() As Short()
            Get
                Return mIndices
            End Get
        End Property

        Public Sub New(ByVal Poligono As Poligono2D, ByVal AnchoPantalla As Integer, ByVal AltoPantalla As Integer)
            Dim j As Integer = 0
            Dim x, y As Integer
            If AnchoPantalla <> 0 Then x = AnchoPantalla Else x = 1
            If AltoPantalla <> 0 Then y = AltoPantalla Else y = 1

            mNumeroVertices = Poligono.NumeroLados
            mColor = New Color(Poligono.Color.R, Poligono.Color.G, Poligono.Color.B, 1)
            ReDim mVertices(mNumeroVertices - 1)
            ReDim mIndices(((mNumeroVertices - 2) * 3) - 1)

            For i As Integer = 0 To mNumeroVertices - 1
                mVertices(i).Color = mColor
                mVertices(i).Position = New Vector3(Poligono.Vertices(i).X / x, Poligono.Vertices(i).Y / y, 0)

                If i >= 2 Then
                    mIndices(j) = 0
                    mIndices(j + 1) = i - 1
                    mIndices(j + 2) = i
                    j += 3
                End If
            Next
        End Sub
    End Class
End Namespace

