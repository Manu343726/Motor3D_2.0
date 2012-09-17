Imports Motor3D.Espacio2D
Imports dx_lib32

Namespace Motor3D.Escena.Renders.RenderDx_lib32
    Public Class SubPoligonoDx_lib32
        Inherits PrimitivaDx_lib32

        Private mVertices(3) As Vertex
        Private mTextura As Integer

        Public ReadOnly Property Vertices As Vertex()
            Get
                Return mVertices
            End Get
        End Property

        Public Property Textura As Integer
            Get
                Return mTextura
            End Get
            Set(value As Integer)
                mTextura = value
            End Set
        End Property

        Public Sub New(ByVal Vertices() As Vertex, Optional ByVal Textura As Integer = -1)
            mTextura = Textura
            mVertices = Vertices
        End Sub

        Public Overrides Sub Redibujar(ByRef Graphics As dx_GFX_Class)
            If mTextura > -1 Then
                Graphics.DRAW_VertexMap(mTextura, mVertices, 0, Specular, Blit_Alpha.Blendop_Color, Blit_Mirror.Mirror_None, Blit_Filter.Filter_Trilinear)
            Else
                Graphics.DRAW_Trapezoid(mVertices)
            End If
        End Sub
    End Class
End Namespace
