Imports Motor3D.Escena
Imports Motor3D.Espacio2D
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Namespace Motor3D.Escena.Renders.RenderXNA
    Public Class RenderXNA
        Inherits Render

        Private mPoligonos As List(Of PoligonoXNA)

        Public Shadows Event Actualizado(ByRef sender As RenderXNA)

        Public ReadOnly Property Poligonos As PoligonoXNA()
            Get
                Return mPoligonos.ToArray
            End Get
        End Property

        Public Sub New(ByRef Motor As Motor3D)
            MyBase.New(Motor)
            mPoligonos = New List(Of PoligonoXNA)
            AddHandler mMotor.ZBuffer.Modificado, AddressOf Actualizar
        End Sub

        Protected Overrides Sub Actualizar(ByRef ZBuffer As ZBuffer)
            mPoligonos.Clear()

            If Not ZBuffer.Vacio Then
                For Each Poligono As Poligono2D In ZBuffer.Represenatciones
                    mPoligonos.Add(New PoligonoXNA(Poligono, mAncho, mAlto))
                Next
            End If

            RaiseEvent Actualizado(Me)
        End Sub
    End Class
End Namespace

