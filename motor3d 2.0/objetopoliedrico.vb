Imports Motor3D.Espacio3D

Namespace Primitivas3D
    Public Class ObjetoPrimitiva3D
        Inherits ObjetoGeometrico3D

        Public Shadows Event Modificado(ByRef Sender As ObjetoPrimitiva3D)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace

