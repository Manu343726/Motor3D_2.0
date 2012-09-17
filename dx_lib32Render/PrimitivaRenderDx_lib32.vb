Imports dx_lib32
Imports Motor3D

Namespace Motor3D.Escena.Renders.RenderDx_lib32
    Public MustInherit Class PrimitivaDx_lib32
        Protected Shared Specular(3) As Integer
        Protected Shared mFiltro As Blit_Filter

        Public Shared Property Filtro As Blit_Filter
            Get
                Return mFiltro
            End Get
            Set(value As Blit_Filter)
                mFiltro = value
            End Set
        End Property

        Public MustOverride Sub Redibujar(ByRef Graphics As dx_GFX_Class)
    End Class
End Namespace

